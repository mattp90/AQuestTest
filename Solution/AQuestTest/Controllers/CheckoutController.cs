using AQuestTest.Database;
using AQuestTest.Models;
using Database;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AQuestTest.Controllers
{
    public class CheckoutController : BaseController
    {
        private PayPal.Api.Payment payment;

        public ActionResult Index(int id)
        {
            CheckoutModel checkout = new CheckoutModel()
            {
                Order = LoadOrder(id)
            };
            checkout.User = LoadUserInfo(checkout.Order.UserInfoId.Value);

            return View(checkout);
        }

        /// <summary>
        /// Submit method checkout view.
        /// </summary>
        /// <param name="checkout">The model of checkout.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckoutOrder(CheckoutModel checkout) 
        {
            if (Request.Form["Back"] != null)
            {
                return BackToUserInfo(checkout.Order.Id);
            }
            else if (Request.Form["PayWithPaypal"] != null)
            {
                return PaymentWithPaypal(checkout.Order.Id);
            }
            else if (Request.Form["PayWithStrype"] != null)
            {
                // to do
            }

            return View();
        }

        /// <summary>
        /// Method called When user click on "Back" button.
        /// </summary>
        /// <param name="idOrder">Order id</param>
        /// <returns></returns>
        public ActionResult BackToUserInfo(int idOrder)
        {
            return RedirectToAction("Index", "UserInfo", new { id = idOrder });
        }

        #region Payment
        public ActionResult PaymentWithPaypal(int? idOrder = null, string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            if (!string.IsNullOrEmpty(Cancel) && bool.Parse(Cancel))
            {
                return RedirectToAction("Index", "Checkout", new { id = idOrder.Value});
            }

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];
                
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string url = $"{@Url.Content("~")}Checkout/PaymentWithPayPal?idOrder={idOrder}&";
                    string baseURI = $"{Request.Url.Scheme}://{Request.Url.Authority}{url}";
                    
                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));
                    
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = CreatePayment(idOrder.Value, apiContext, baseURI + "guid=" + guid);
                    
                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Errore durante il pagamento.";
                return RedirectToAction("Index", "Home");
            }

            // On successful payment, show home page page to user.
            TempData["PaymentExecuted"] = "Pagamento effettuato correttamente! Provvederemo al più presto a processare l'ordine richesto, grazie!";
            OrdersQueries.CloseOrder(idOrder.Value);

            return RedirectToAction("Index", "Home");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        
        private Payment CreatePayment(int idOrder, APIContext apiContext, string redirectUrl)
        {
            using (AQuestContext db = new AQuestContext())
            {
                Orders order = db.Orders.First(x => x.Id == idOrder);

                decimal _taxes = Math.Round((order.TotalPrice / 100) * 22, 2);
                decimal _subtotal = Math.Round(order.TotalPrice - _taxes, 2);

                //create itemlist and add item objects to it
                var itemList = new ItemList() { items = new List<Item>() };

                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = $"Order #{order.Id}",
                    currency = "EUR",
                    price = _subtotal.ToString().Replace(",", "."),
                    quantity = "1",
                    sku = "sku"
                });
                var payer = new Payer() { payment_method = "paypal" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&Cancel=true",
                    return_url = redirectUrl
                };

                // Adding Tax, shipping and Subtotal details
                
                var details = new Details()
                {
                    tax = _taxes.ToString().Replace(",", "."),
                    shipping = "0",
                    subtotal = _subtotal.ToString().Replace(",", ".")
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "EUR",
                    total = order.TotalPrice.ToString().Replace(",", "."), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();

                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {
                    description = $"Transaction {Guid.NewGuid().ToString()} for Order #{order.Id}",
                    invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }
            
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        #endregion
    }
}
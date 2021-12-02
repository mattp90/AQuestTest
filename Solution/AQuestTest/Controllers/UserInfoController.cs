using AQuestTest.Database;
using AQuestTest.Models;
using Database;
using System.Web.Mvc;

namespace AQuestTest.Controllers
{
    public class UserInfoController : BaseController
    {
        public ActionResult Index(int id)
        {
            UserInfoModel userInfo = new UserInfoModel()
            {
                IdOrder = id
            };

            Orders order = OrdersQueries.OrderById(id);

            if (order.UserInfosId != null)
            {
                UserInfos userInfoDb = UserInfosQueries.UserInfoById(order.UserInfosId.Value);
                userInfo = new UserInfoModel()
                {
                    IdOrder = userInfo.IdOrder,
                    Name = userInfoDb.Name,
                    Surname = userInfoDb.Surname,
                    Email = userInfoDb.Email,
                    Nationality = userInfoDb.Nationality,
                    SubscribedToNewsletter = userInfoDb.SubscribedToNewsletter,
                    RequestInvoice = userInfoDb.RequestInvoice,
                    TaxCode = userInfoDb.TaxCode,
                    Vat = userInfoDb.VAT
                };
            }

            return View(userInfo);
        }

        /// <summary>
        /// Submit method from user info view. User data will be save on database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertUserInfoData(UserInfoModel user)
        {
            if (Request.Form["Back"] != null)
            {
                return BackToOrderDetail(user.IdOrder);
            }
            else if (Request.Form["Continue"] != null)
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", user);
                }

                Orders order = OrdersQueries.OrderWithUserInfoById(user.IdOrder);

                if (order.UserInfos == null)
                {
                    order.UserInfos = new UserInfos()
                    {
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Nationality = user.Nationality,
                        SubscribedToNewsletter = user.SubscribedToNewsletter,
                        RequestInvoice = user.RequestInvoice,
                        VAT = user.Vat,
                        TaxCode = user.TaxCode
                    };

                    order.UserInfosId = UserInfosQueries.Add(order.UserInfos).Id;
                    OrdersQueries.Update(order);
                }
                else
                {
                    order.UserInfos.Name = user.Name;
                    order.UserInfos.Surname = user.Surname;
                    order.UserInfos.Email = user.Email;
                    order.UserInfos.Nationality = user.Nationality;
                    order.UserInfos.SubscribedToNewsletter = user.SubscribedToNewsletter;
                    order.UserInfos.RequestInvoice = user.RequestInvoice;
                    order.UserInfos.VAT = user.Vat;
                    order.UserInfos.TaxCode = user.TaxCode;

                    UserInfosQueries.Update(order.UserInfos);
                }

                // OrdersQueries.Update(order);
                
                return RedirectToAction("Index", "Checkout", new { id = user.IdOrder });
            }

            // Go to user info page
            return View();
        }

        /// <summary>
        /// Methow called when user click on "back" button.
        /// </summary>
        /// <param name="idOrder">Order id.</param>
        /// <returns></returns>
        public ActionResult BackToOrderDetail(int idOrder)
        {
            return RedirectToAction("Index", "OrderDetail", new { id = idOrder });
        }
    }
}
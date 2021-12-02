using AQuestTest.Database;
using AQuestTest.Models;
using Database;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AQuestTest.Controllers
{
    public class OrderDetailController : BaseController
    {
        public ActionResult Index(string id)
        {
            var model = LoadOrder(int.Parse(id));
            return View(model);
        }

        /// <summary>
        /// Submit method from order detail view.
        /// </summary>
        /// <param name="order">The model of the Order.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmOrder(OrderModel order)
        {
            // Go to user info page
            return RedirectToAction("Index", "UserInfo", new { id = order.Id });
        }

        /// <summary>
        /// Method called by sjax when user adds a single item on the cart.
        /// </summary>
        /// <param name="idProduct">Product id to add.</param>
        /// <param name="idOrder">Order id.</param>
        /// <param name="newQuantity">The new quantity of product.</param>
        /// <returns></returns>
        public JsonResult AddProductToCart(int idProduct, int idOrder, int newQuantity)
        {
            if (OrdersQueries.isQuantityInStock(idProduct, newQuantity))
            {
                Orders order = OrdersQueries.UpdateInfoProductOrder(idProduct, idOrder, newQuantity);

                decimal _newTotalPriceItem = order.Orders_Products.First(x => x.ProductsId == idProduct).Quantity *
                    order.Orders_Products.First(x => x.ProductsId == idProduct).Products.Price;

                return Json(new
                {
                    result = "success",
                    newTotalPriceItem = _newTotalPriceItem,
                    newTotalPriceOrder = order.TotalPrice,
                    newTotalPriceWithoutDiscount = order.TotalPriceWithoutDiscount
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = "error"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Method called by ajax when user removes a single item from the cart.
        /// </summary>
        /// <param name="idProduct">Product Id to add.</param>
        /// <param name="idOrder">Order Id.</param>
        /// <param name="newQuantity">The new quantity of product.</param>
        /// <returns></returns>
        public JsonResult RemoveProductToCart(int idProduct, int idOrder, int newQuantity)
        {
            Orders order = OrdersQueries.UpdateInfoProductOrder(idProduct, idOrder, -newQuantity);

            decimal _newTotalPriceItem = order.Orders_Products.First(x => x.ProductsId == idProduct).Quantity *
                    order.Orders_Products.First(x => x.ProductsId == idProduct).Products.Price;
            
            return Json(new
            {
                result = "success",
                newTotalPriceItem = _newTotalPriceItem,
                newTotalPriceOrder = order.TotalPrice
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method called by Ajax when user removes all quantities of the item from the cart.
        /// </summary>
        /// <param name="idProduct">Product Id to add.</param>
        /// <param name="idOrder">Order Id.</param>
        /// <returns></returns>
        public JsonResult RemoveProductFromOrder(int idProduct, int idOrder)
        {
            Orders order = OrdersQueries.RemoveProductOrder(idProduct, idOrder);

            return Json(new
            {
                result = "success",
                newTotalPriceOrder = order.TotalPrice
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method called by ajax when there isn't any item in the cart. The order will be deleted logically.
        /// </summary>
        /// <param name="idOrder">Order Id</param>
        /// <returns></returns>
        public JsonResult DeleteOrder(int idOrder)
        {
            OrdersQueries.DeleteOrder(idOrder);

            TempData["OrderDeleted"] = $"Conferma di avvenuta cancellazione dell'Ordine #{idOrder}.";

            return Json(new
            {
                result = "success"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks if there is in stock the quantity requested by user.
        /// </summary>
        /// <param name="idOrder">Order Id</param>
        /// <param name="idProduct">Product Id</param>
        /// <param name="quantityRequested">Quantity requested</param>
        /// <returns></returns>
        public JsonResult CheckQuantityForPurchase(int idOrder)
        {
            return Json(OrdersQueries.CheckQuantitiesOrder(idOrder), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method called by ajax for apply a coupon.
        /// </summary>
        /// <param name="idOrder"></param>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public JsonResult ApplyCoupon(int idOrder, string coupon)
        {
            Coupons c = CouponsQueries.CouponByCode(coupon);

            if (c == null)
            {
                return Json(new
                {
                    result = "error",
                    message = "Coupon disattivo o inesistente"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Orders order = OrdersQueries.OrderById(idOrder);
                order.CouponsId = c.Id;

                // Calculate new total price
                order.TotalPrice = Math.Round(order.TotalPrice - ((order.TotalPrice / 100) * c.PercentageDiscount), 2);

                OrdersQueries.Update(order);

                return Json(new
                {
                    result = "success",
                    coupon = c,
                    newTotalPriceOrder = order.TotalPrice
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
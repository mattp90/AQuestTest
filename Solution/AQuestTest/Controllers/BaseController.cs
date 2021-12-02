using AQuestTest.Database;
using AQuestTest.Models;
using Database;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AQuestTest.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Mapper from Orders entity to OrderModel
        /// </summary>
        /// <param name="idOrder"></param>
        /// <returns></returns>
        public OrderModel LoadOrder(int idOrder)
        {
            Orders order = OrdersQueries.OrderDetailById(idOrder);

            OrderModel orderModel = new OrderModel();
            orderModel.Id = order.Id;
            orderModel.OrderState = order.OrderStatesId;
            orderModel.UserInfoId = order.UserInfosId;
            orderModel.TotalPriceOrder = order.TotalPrice;
            orderModel.TotalPriceWithoutDiscount = order.TotalPriceWithoutDiscount;
            orderModel.Products = new List<ProductModel>();
            orderModel.IdCoupon = order.CouponsId;
            orderModel.Coupon = (order.Coupons == null) ? "": order.Coupons.Code;
            orderModel.PercentageDiscount = (order.Coupons == null) ? (int?)null : order.Coupons.PercentageDiscount;

            foreach (var op in order.Orders_Products)
            {
                orderModel.Products.Add(new ProductModel()
                {
                    Id = op.Products.Id,
                    Quantity = op.Quantity,
                    Name = op.Products.Name,
                    Link = "",
                    Price = op.Products.Price,
                    Stock = op.Products.StockQty,
                    TotalPriceItem = op.Products.Price * op.Quantity
                });
            }

            return orderModel;
        }

        /// <summary>
        /// Mapper from UserInfos entity to OrderModel
        /// </summary>
        /// <param name="idUserInfo"></param>
        /// <returns></returns>
        public UserInfoModel LoadUserInfo(int idUserInfo)
        {
            UserInfos userInfo = UserInfosQueries.UserInfoById(idUserInfo);
            
            UserInfoModel userInfoModel = new UserInfoModel()
            {
                Email = userInfo.Email,
                Name = userInfo.Name,
                Surname = userInfo.Surname,
                TaxCode = userInfo.TaxCode,
                Vat = userInfo.VAT
            };
            
            return userInfoModel;
        }
    }
}
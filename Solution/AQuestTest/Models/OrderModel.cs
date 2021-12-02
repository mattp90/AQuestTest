using System.Collections.Generic;

namespace AQuestTest.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int OrderState { get; set; }
        public int? UserInfoId { get; set; }
        public List<ProductModel> Products { get; set; }
        public decimal TotalPriceOrder { get; set; }
        public decimal TotalPriceWithoutDiscount { get; set; }
        public int? IdCoupon { get; set; }
        public string Coupon { get; set; }
        public int? PercentageDiscount { get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string LinkImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal TotalPriceItem { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace AQuestTest.Database
{
    public class Coupons
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public decimal MinPrice { get; set; }
        public Nullable<decimal> MaxPrice { get; set; }
        public string AssociatedProductIds { get; set; }
    }
}

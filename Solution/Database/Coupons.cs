using System;

namespace AQuestTest.Database
{
    public partial class Coupons
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public int PercentageDiscount { get; set; }
        public DateTime? DateOfUse { get; set; }
    }
}

namespace AQuestTest.Models
{
    public class CheckoutModel
    {
        public OrderModel Order { get; set; }
        public UserInfoModel User { get; set; }

        public bool GeneralConditionsReaded { get; set; }
    }
}
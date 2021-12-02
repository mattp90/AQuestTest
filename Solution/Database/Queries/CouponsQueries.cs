using AQuestTest.Database;
using System.Linq;

namespace Database
{
    public class CouponsQueries
    {
        public static Coupons CouponByCode(string code)
        {
            using (AQuestContext db = new AQuestContext())
            {
                return db.Coupons.FirstOrDefault(x => x.Code == code && x.Active);
            }
        }
    }
}

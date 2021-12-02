using AQuestTest.Database;
using System.Linq;

namespace Database
{
    public class UserInfosQueries : BaseQueries
    {
        public static UserInfos UserInfoById(int id)
        {
            using (AQuestContext db = new AQuestContext())
            {
                return db.UserInfos.FirstOrDefault(x => x.Id == id);
            }
        }
    }
}

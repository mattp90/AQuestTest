using AQuestTest.Database;
using System.Configuration;
using System.Data.Entity;

namespace Database
{
    public class BaseQueries
    {
        public static T Add<T>(T newEntity) where T : class
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                db.Set<T>().Add(newEntity);
                db.SaveChanges();
                return newEntity;
            }
        }

        public static T Update<T>(T entityToUpdate) where T : class
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                db.Entry(entityToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return entityToUpdate;
            }
        }

        public static void Delete<T>(T entityToDelete) where T : class
        {
            using (AQuestContext db = new AQuestContext(ConfigurationManager.AppSettings["DatabaseName"]))
            {
                db.Entry(entityToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}

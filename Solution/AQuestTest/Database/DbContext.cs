using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AQuestTest.Database
{
    public class AQuestContext : DbContext
    {
        public AQuestContext()
            : base("name=test")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Coupons> Coupons { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Orders_Products> Orders_Products { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<UserInfos> UserInfos { get; set; }
        public virtual DbSet<OrderStates> OrderStates { get; set; }
    }
}

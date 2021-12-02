using System.Data.Entity;

namespace AQuestTest.Database
{
    public class AQuestContext : DbContext
    {
        public AQuestContext()
            : base("name=test")
        {
        }

        public AQuestContext(string databaseName)
            : base($"name={databaseName}")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }

        public virtual DbSet<Coupons> Coupons { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Orders_Products> Orders_Products { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<UserInfos> UserInfos { get; set; }
        public virtual DbSet<OrderStates> OrderStates { get; set; }
    }
}

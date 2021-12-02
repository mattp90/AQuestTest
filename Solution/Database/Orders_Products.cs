using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AQuestTest.Database
{
    public partial class Orders_Products
    {
        [Key, Column(Order = 0)]
        public int OrdersId { get; set; }
        [Key, Column(Order = 1)]
        public int ProductsId { get; set; }
        public int Quantity { get; set; }
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}

using System.Collections.Generic;

namespace AQuestTest.Database
{
    public class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.Orders_Products = new HashSet<Orders_Products>();
        }
    
        public int Id { get; set; }
        public int IdUser { get; set; }
        public double TotalPrice { get; set; }
        public System.DateTime DateOrder { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Products> Orders_Products { get; set; }

        public virtual Users Users { get; set; }
    }
}

using System.Collections.Generic;

namespace AQuestTest.Database
{
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.Orders_Products = new HashSet<Orders_Products>();
        }
    
        public int Id { get; set; }
        public int? UserInfosId { get; set; }
        public int? CouponsId { get; set; }
        public int OrderStatesId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceWithoutDiscount { get; set; }
        public System.DateTime? DateOrder { get; set; }

        public virtual UserInfos UserInfos { get; set; }
        public virtual Coupons Coupons { get; set; }
        public virtual OrderStates OrderStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Products> Orders_Products { get; set; }
    }
}

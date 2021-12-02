using System.Collections.Generic;

namespace AQuestTest.Database
{
    public partial class UserInfos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserInfos()
        {
            this.Orders = new HashSet<Orders>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public bool SubscribedToNewsletter { get; set; }
        public bool RequestInvoice { get; set; }
        public string VAT { get; set; }
        public string TaxCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}

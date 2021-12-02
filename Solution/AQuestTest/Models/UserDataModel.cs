using System.ComponentModel.DataAnnotations;

namespace AQuestTest.Models
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        
        public int IdOrder { get; set; }

        [Required(ErrorMessage ="Campo obbligatorio")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Campo obbligatorio")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "E-mail non valida")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Nationality { get; set; }
        
        public bool SubscribedToNewsletter { get; set; }
        
        public bool RequestInvoice { get; set; }
        
        public string Vat { get; set; }
        
        public string TaxCode { get; set; }
        public bool PrivacyPolicyReaded { get; set; }
    }
}
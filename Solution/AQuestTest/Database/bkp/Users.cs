using System;
using System.Collections.Generic;

namespace AQuestTest.Database
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool SubscribedToNewsletter { get; set; }
        public bool RequestInvoice { get; set; }
        public string VAT { get; set; }
        public string TaxCode { get; set; }
    }
}

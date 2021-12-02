using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AQuestTest.Database
{
    public class Orders_Products
    {
        [Key, Column(Order = 0)]
        public int IdOrder { get; set; }
        [Key, Column(Order = 1)]
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual Products Products { get; set; }
    }
}

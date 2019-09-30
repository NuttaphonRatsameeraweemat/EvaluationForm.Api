using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PurchaseOrg
    {
        [Column("PurchaseOrg")]
        [StringLength(4)]
        public string PurchaseOrg1 { get; set; }
        [StringLength(200)]
        public string PurchaseName { get; set; }
    }
}

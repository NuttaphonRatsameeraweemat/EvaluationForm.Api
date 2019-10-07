using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PurchaseOrgItem
    {
        [StringLength(4)]
        public string PuchaseOrg { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
        [StringLength(10)]
        public string Type { get; set; }
    }
}

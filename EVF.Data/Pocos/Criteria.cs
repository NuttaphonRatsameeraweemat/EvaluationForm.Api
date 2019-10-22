using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Criteria
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string CriteriaName { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsUse { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        [StringLength(4)]
        public string CreateByPurchaseOrg { get; set; }
    }
}

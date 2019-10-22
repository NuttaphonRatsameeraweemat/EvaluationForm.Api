using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class KpiGroup
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string KpiGroupNameTh { get; set; }
        [StringLength(200)]
        public string KpiGroupNameEn { get; set; }
        [StringLength(40)]
        public string KpiGroupShortTextTh { get; set; }
        [StringLength(40)]
        public string KpiGroupShortTextEn { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool? IsUse { get; set; }
        [Column("SapFieldsID")]
        public int? SapFieldsId { get; set; }
        [StringLength(4)]
        public string CreateByPurchaseOrg { get; set; }
    }
}

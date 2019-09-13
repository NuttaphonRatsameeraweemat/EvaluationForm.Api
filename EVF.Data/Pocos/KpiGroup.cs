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
        [StringLength(255)]
        public string KpiGroupNameTh { get; set; }
        [StringLength(255)]
        public string KpiGroupNameEn { get; set; }
        [StringLength(20)]
        public string KpiGroupShortTextTh { get; set; }
        [StringLength(20)]
        public string KpiGroupShortTextEn { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool? IsUse { get; set; }
        [StringLength(100)]
        public string SapScoreField { get; set; }
    }
}

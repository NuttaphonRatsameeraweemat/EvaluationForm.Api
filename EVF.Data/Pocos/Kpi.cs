using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Kpi
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(255)]
        public string KpiNameTh { get; set; }
        [StringLength(255)]
        public string KpiNameEn { get; set; }
        [StringLength(20)]
        public string KpiShortTextTh { get; set; }
        [StringLength(20)]
        public string KpiShortTextEn { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool? IsUse { get; set; }
    }
}

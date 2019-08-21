using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PerformanceGroup
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(255)]
        public string PerformanceGroupNameTh { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(255)]
        public string LastModifyBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastModifyDate { get; set; }
        [StringLength(255)]
        public string PerformanceGroupNameEn { get; set; }
        [StringLength(100)]
        public string SapScoreField { get; set; }
    }
}

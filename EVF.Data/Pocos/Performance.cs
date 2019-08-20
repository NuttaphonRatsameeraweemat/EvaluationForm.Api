using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Performance
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(255)]
        public string PerformanceNameTh { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(255)]
        public string LastModifyBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastModifyDate { get; set; }
        [StringLength(255)]
        public string PerformanceNameEn { get; set; }
    }
}

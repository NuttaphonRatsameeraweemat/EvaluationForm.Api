using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluatorGroup
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(250)]
        public string EvaluatorGroupName { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        [Column("PeriodItemID")]
        public int? PeriodItemId { get; set; }
        [Column("PeriodID")]
        public int? PeriodId { get; set; }
    }
}

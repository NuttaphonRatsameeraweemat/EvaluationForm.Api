using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluationLogItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("EvaluationLogID")]
        public int? EvaluationLogId { get; set; }
        [Column("PerformanceGroupID")]
        public int? PerformanceGroupId { get; set; }
        [Column("PerformanceID")]
        public int? PerformanceId { get; set; }
        public int? Score { get; set; }
        [StringLength(255)]
        public string Reason { get; set; }
    }
}

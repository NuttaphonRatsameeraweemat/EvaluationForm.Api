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
        [Column("KpiGroupID")]
        public int? KpiGroupId { get; set; }
        [Column("KpiID")]
        public int? KpiId { get; set; }
        public int? Score { get; set; }
        public int? LevelPoint { get; set; }
        [StringLength(255)]
        public string Reason { get; set; }
        public int? RawScore { get; set; }
    }
}

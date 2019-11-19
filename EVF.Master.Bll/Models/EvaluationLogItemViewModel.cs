using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class EvaluationLogItemViewModel
    {
        public int Id { get; set; }
        public int? KpiGroupId { get; set; }
        public int? KpiId { get; set; }
        public int? Score { get; set; }
        public int? LevelPoint { get; set; }
        public string Reason { get; set; }
        public int MaxScore { get; set; }
        public double RawScore { get; set; }
    }
}

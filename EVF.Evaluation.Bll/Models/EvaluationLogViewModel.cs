using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class EvaluationLogViewModel
    {
        public EvaluationLogViewModel()
        {
            EvaluationLogs = new List<EvaluationLogItemViewModel>();
        }

        public DateTime? ActionDate { get; set; }
        public List<EvaluationLogItemViewModel> EvaluationLogs { get; set; }
    }

    public class EvaluationLogItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int? PerformanceGroupId { get; set; }
        [Required]
        public int? PerformanceId { get; set; }
        [Required]
        public int? Score { get; set; }
        [Required]
        public int? LevelPoint { get; set; }
        [MaxLength(255)]
        public string Reason { get; set; }
    }

}

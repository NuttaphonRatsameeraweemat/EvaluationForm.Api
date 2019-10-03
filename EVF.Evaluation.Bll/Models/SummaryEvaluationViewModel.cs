using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class SummaryEvaluationViewModel
    {
        public SummaryEvaluationViewModel()
        {
            Summarys = new List<SummaryEvaluationDetailViewModel>();
            UserLists = new List<UserEvaluationViewModel>();
        }

        public string VendorName { get; set; }
        public string WeightingKey { get; set; }
        public string PurchasingOrgName { get; set; }
        public int Total { get; set; }
        public string GradeName { get; set; }
        public List<SummaryEvaluationDetailViewModel> Summarys { get; set; }
        public List<UserEvaluationViewModel> UserLists { get; set; }
    }

    public class SummaryEvaluationDetailViewModel
    {
        public int KpiGroupId { get; set; }
        public int? KpiId { get; set; }
        public int Sequence { get; set; }
        public double Score { get; set; }
    }

    public class UserEvaluationViewModel
    {
        public UserEvaluationViewModel()
        {
            EvaluationLogs = new List<UserEvaluationDetailViewModel>();
        }

        public string EmpNo { get; set; }
        public string AdUser { get; set; }
        public string UserType { get; set; }
        public bool IsAction { get; set; }
        public bool IsReject { get; set; }
        public string ReasonReject { get; set; }
        public string FullName { get; set; }
        public List<UserEvaluationDetailViewModel> EvaluationLogs { get; set; }
    }

    public class UserEvaluationDetailViewModel
    {
        public UserEvaluationDetailViewModel()
        {
            EvaluationLogs = new List<EvaluationLogItemViewModel>();
        }

        public DateTime? ActionDate { get; set; }
        public List<EvaluationLogItemViewModel> EvaluationLogs { get; set; }
    }

}

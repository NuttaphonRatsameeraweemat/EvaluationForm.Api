using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class EvaluationAssignViewModel
    {

        public const string RoleForManageData = "Role_MA_SummaryEvaluation";
        public const string RoleForDisplayData = "Role_DS_SummaryEvaluation";

        public int Id { get; set; }
        public int? EvaluationId { get; set; }
        public string EmpNo { get; set; }
        public string AdUser { get; set; }
        public string UserType { get; set; }
        public bool IsReject { get; set; }
        public bool IsAction { get; set; }
        public string ReasonReject { get; set; }

        public string FullName { get; set; }
    }

    public class EvaluationAssignRequestViewModel
    {
        public int Id { get; set; }
        public int EvaluationId { get; set; }
        public string ToAdUser { get; set; }
    }

    public class EvaluationRejectViewModel
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }

}

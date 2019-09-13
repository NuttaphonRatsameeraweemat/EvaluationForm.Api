using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class EvaluationAssignViewModel
    {
        public int Id { get; set; }
        public int? EvaluationId { get; set; }
        public string EmpNo { get; set; }
        public string AdUser { get; set; }
        public string UserType { get; set; }
        public bool IsReject { get; set; }
        public bool IsAction { get; set; }


        public string FullName { get; set; }
    }
}

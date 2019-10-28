using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Models
{
    public class EvaluationSummaryReportRequestModel
    {
        public int PeriodItemId { get; set; }
        public string ComCode { get; set; }
        public string PurchaseOrg { get; set; }
        public string WeightingKey { get; set; }
    }
}

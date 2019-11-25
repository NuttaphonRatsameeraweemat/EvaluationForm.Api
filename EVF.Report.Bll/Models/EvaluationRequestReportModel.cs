using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Models
{
    public class EvaluationCompareReportRequestModel
    {
        public int[] Year { get; set; }
        public int[] PeriodItemId { get; set; }
        public string ComCode { get; set; }
        public string PurchaseOrg { get; set; }
        public string WeightingKey { get; set; }
    }

    public class VendorEvaluationStatusReportRequestModel
    {
        public int[] Year { get; set; }
        public int[] PeriodItemId { get; set; }
        public string ComCode { get; set; }
        public string PurchaseOrg { get; set; }
        public string WeightingKey { get; set; }
    }

    public class InvestigateEvaluationReportRequestModel
    {
        public int[] Year { get; set; }
        public int[] PeriodItemId { get; set; }
        public string ComCode { get; set; }
        public string PurchaseOrg { get; set; }
        public string WeightingKey { get; set; }
        public string[] Status { get; set; }
    }

}

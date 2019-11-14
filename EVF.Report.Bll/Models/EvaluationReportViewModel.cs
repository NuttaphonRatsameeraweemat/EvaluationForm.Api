using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Models
{
    public class EvaluationReportViewModel
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string CompanyName { get; set; }
        public string PurchaseOrgName { get; set; }
        public string WeightingKey { get; set; }
        public int TotalScore { get; set; }
        public string GradeName { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsPrintReport { get; set; }
        public string SendEmailDate { get; set; }
    }
}

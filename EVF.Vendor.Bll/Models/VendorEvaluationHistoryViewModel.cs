using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorEvaluationHistoryViewModel
    {
        public string PeriodName { get; set; }
        public string WeightingKey { get; set; }
        public string WeightingKeyName { get; set; }
        public int TotalScore { get; set; }
        public string GradeName { get; set; }
    }
}

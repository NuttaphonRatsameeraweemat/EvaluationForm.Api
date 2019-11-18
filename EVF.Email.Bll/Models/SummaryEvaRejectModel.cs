using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Models
{
    public class SummaryEvaRejectModel
    {
        public string DocNo { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }
        public string WeightingKeyName { get; set; }
        public string PeriodItemName { get; set; }
        public int TotalScore { get; set; }
        public string GradeName { get; set; }
    }
}

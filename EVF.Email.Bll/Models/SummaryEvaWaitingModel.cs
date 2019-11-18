using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Email.Bll.Models
{
    public class SummaryEvaWaitingModel
    {
        public string DocNo { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }
        public string WeightingKeyName { get; set; }
        public string StartEvaDate { get; set; }
        public string EndEvaDate { get; set; }

    }
}

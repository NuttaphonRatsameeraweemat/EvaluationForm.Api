using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Report.Bll.Models
{
    public class VendorEvaluationRequestModel
    {
        public VendorEvaluationRequestModel()
        {
            KpiGroups = new List<VendorEvaluationRequestItemModel>();
        }

        public string CompanyNameTh { get; set; }
        public string CompanyNameEn { get; set; }
        public string DocNo { get; set; }
        public string PrintDate { get; set; }
        public string PeriodName { get; set; }
        public string VendorName { get; set; }
        public int TotalScore { get; set; }
        public int MaxTotalScore { get; set; }
        public string GradeName { get; set; }
        public string ApproveBy { get; set; }
        public string PositionName { get; set; }
        public string ContentHeader { get; set; }
        public string ContentFooter { get; set; }
        public List<VendorEvaluationRequestItemModel> KpiGroups { get; set; }
    }

    public class VendorEvaluationRequestItemModel
    {
        public string KpiGroupName { get; set; }
        public int MaxScore { get; set; }
        public int Score { get; set; }
    }
}

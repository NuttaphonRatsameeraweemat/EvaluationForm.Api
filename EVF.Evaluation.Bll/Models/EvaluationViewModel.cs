using EVF.Helper.Components;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class EvaluationViewModel
    {
        public const string RoleForManageData = "Role_MA_Evaluation";
        public const string RoleForDisplayData = "Role_DS_Evaluation";

        public int Id { get; set; }
        public string DocNo { get; set; }
        public int EvaluationTemplateId { get; set; }
        public string VendorNo { get; set; }
        public string WeightingKey { get; set; }
        public string ComCode { get; set; }
        public string PurchasingOrg { get; set; }
        public int PeriodItemId { get; set; }
        public string Status { get; set; }

        //Display
        public string VendorName { get; set; }
        public string EvaluationTemplateName { get; set; }
        public string CompanyName { get; set; }
        public string PurchasingOrgName { get; set; }
        public string StartEvaDateString { get; set; }
        public string EndEvaDateString { get; set; }
        public string StatusName { get; set; }
        public string[] Categorys { get; set; }
        public string Remark { get; set; }
    }

    public class EvaluationRequestViewModel
    {
        public EvaluationRequestViewModel()
        {
            ImageList = new List<ImageViewModel>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedEvaluationTemplate)]
        public int? EvaluationTemplateId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedVendor)]
        public string VendorNo { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedWeightingKey)]
        public string WeightingKey { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedCompany)]
        public string ComCode { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPurchaseOrg)]
        public string PurchasingOrg { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPeriod)]
        public int? PeriodItemId { get; set; }

        public int EvaluatorGroup { get; set; }
        public string[] EvaluatorList { get; set; }
        public string EvaluatorPurchasing { get; set; }

        public string[] Categorys { get; set; }
        public string Remark { get; set; }

        public List<ImageViewModel> ImageList { get; set; }
    }
}

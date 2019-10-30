using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorFilterViewModel
    {
        public const string RoleForManageData = "Role_MA_VendorFilter";
        public const string RoleForDisplayData = "Role_DS_VendorFilter";

        public int Id { get; set; }
        public int? PeriodItemId { get; set; }
        public string PeriodItemName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string PurchasingOrg { get; set; }
        public string PurchasingName { get; set; }
        public string WeightingKey { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string AssignTo { get; set; }
        public string AssignToName { get; set; }
        public bool IsSending { get; set; }
        public string SendingStatus { get; set; }
        public DateTime? SendEvaluationDate { get; set; }
    }

    public class VendorFilterCriteriaViewModel
    {
        [Required(ErrorMessage = MessageValue.PleaseSelectedPeriod)]
        public int? PeriodItemId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedCompany)]
        public string CompanyCode { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPurchaseOrg)]
        public string PurchasingOrg { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedWeightingKey)]
        public string WeightingKey { get; set; }
    }

    public class VendorFilterSearchViewModel
    {
        [Required(ErrorMessage = MessageValue.PleaseSelectedPeriod)]
        public int PeriodItemId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedStartDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedEndDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndDate { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedCompany)]
        public string ComCode { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPurchaseOrg)]
        public string PurchaseOrg { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedWeightingKey)]
        public string WeightingKey { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedCondition)]
        public string Condition { get; set; }
        public double TotalSales { get; set; }
    }

    public class VendorFilterResponseViewModel
    {
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public double TotalSales { get; set; }
        public string TotalSalesText
        {
            get
            {
                return TotalSales.ToString("n0");
            }
        }
    }

    public class VendorFilterRequestViewModel
    {
        public int Id { get; set; }
        [Required]
        public int? PeriodItemId { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string PurchasingOrg { get; set; }
        [Required]
        public string WeightingKey { get; set; }
        [Required]
        public string VendorNo { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedAssignTo)]
        public string AssignTo { get; set; }
    }

    public class VendorFilterCollectionRequestViewModel
    {

        public VendorFilterCollectionRequestViewModel()
        {
            VendorFilterItems = new List<VendorFilterItemRequestViewModel>();
        }

        [Required]
        public int? PeriodItemId { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string PurchasingOrg { get; set; }
        [Required]
        public string WeightingKey { get; set; }
        public List<VendorFilterItemRequestViewModel> VendorFilterItems { get; set; }
    }

    public class VendorFilterItemRequestViewModel
    {
        [Required]
        public string VendorNo { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedAssignTo)]
        public string AssignTo { get; set; }
    }

    public class VendorFilterEditRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedAssignTo)]
        public string AssignTo { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorFilterViewModel
    {
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
        [Required]
        public int? PeriodItemId { get; set; }
        [Required]
        public string CompanyCode { get; set; }
        [Required]
        public string PurchasingOrg { get; set; }
        [Required]
        public string WeightingKey { get; set; }
    }

    public class VendorFilterRequestViewModel
    {
        public VendorFilterRequestViewModel()
        {
            IsSending = false;
        }

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
        [Required]
        public string AssignTo { get; set; }
        public bool IsSending { get; set; }
        public DateTime? SendEvaluationDate { get; set; }
    }
}

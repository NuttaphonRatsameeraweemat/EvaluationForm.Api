using EVF.Helper;
using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorTransectionViewModel
    {
        public const string RoleForManageData = "Role_MA_VendorTransection";
        public const string RoleForDisplayData = "Role_DS_VendorTransection";

        public int Id { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public string ReceiptDateString
        {
            get
            {
                return UtilityService.DateTimeToString(ReceiptDate.Value, ConstantValue.DateTimeFormat);
            }
        }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialGrp { get; set; }
        public string UnitCode { get; set; }
        public double? QuantityReceived { get; set; }
        public double? TotalRecieved { get; set; }
        public string CompanyCode { get; set; }
        public string DocNumber { get; set; }
        public string DocType { get; set; }
        public string LineId { get; set; }
        public string K2Key { get; set; }
        public string Datatype { get; set; }
        public string Free { get; set; }
        public string Intercomp { get; set; }
        public string PurorgCode { get; set; }
        public string PurorgName { get; set; }
        public string Condition { get; set; }
        public string PurgropCode { get; set; }
        public DateTime? DataUpdateDate { get; set; }
        public string MarkWeightingKey { get; set; }
    }

    public class VendorTransectionSearchViewModel
    {
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
        [Required]
        public string PurGroup { get; set; }
    }

    public class VendorTransectionRequestViewModel
    {
        public int Id { get; set; }
        public string WeightingKey { get; set; }
    }

}

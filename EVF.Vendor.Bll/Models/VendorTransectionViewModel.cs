using EVF.Helper;
using EVF.Helper.Components;
using Nest;
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
        public decimal? Gjahr { get; set; }
        public string Belnr { get; set; }
        public decimal? Buzei { get; set; }
        public int? Para { get; set; }
        public string ShortText { get; set; }
        public string PurchDoc { get; set; }
        public DateTime? DataUpdateDate { get; set; }
        public string MarkWeightingKey { get; set; }
    }

    public class VendorTransectionElasticSearchModel
    {
        [Number(Name = "id")]
        public int Id { get; set; }
        [Text(Name = "receiptdate")]
        public DateTime? ReceiptDate { get; set; }
        [Text(Name = "receiptdatestring")]
        public string ReceiptDateString
        {
            get
            {
                return UtilityService.DateTimeToString(ReceiptDate.Value, ConstantValue.DateTimeFormat);
            }
        }
        [Text(Name = "vendor")]
        public string Vendor { get; set; }
        [Text(Name = "vendorname")]
        public string VendorName { get; set; }
        [Text(Name = "materialcode")]
        public string MaterialCode { get; set; }
        [Text(Name = "materialgrp")]
        public string MaterialGrp { get; set; }
        [Text(Name = "unitcode")]
        public string UnitCode { get; set; }
        [Text(Name = "quantityreceived")]
        public double? QuantityReceived { get; set; }
        [Text(Name = "totalrecieved")]
        public double? TotalRecieved { get; set; }
        [Text(Name = "companycode")]
        public string CompanyCode { get; set; }
        [Text(Name = "docnumber")]
        public string DocNumber { get; set; }
        [Text(Name = "doctype")]
        public string DocType { get; set; }
        [Text(Name = "lineid")]
        public string LineId { get; set; }
        [Text(Name = "k2key")]
        public string K2Key { get; set; }
        [Text(Name = "datatype")]
        public string Datatype { get; set; }
        [Text(Name = "free")]
        public string Free { get; set; }
        [Text(Name = "intercomp")]
        public string Intercomp { get; set; }
        [Text(Name = "purorgcode")]
        public string PurorgCode { get; set; }
        [Text(Name = "purorgname")]
        public string PurorgName { get; set; }
        [Text(Name = "condition")]
        public string Condition { get; set; }
        [Text(Name = "purgropcode")]
        public string PurgropCode { get; set; }
        [Text(Name = "gjahr")]
        public decimal? Gjahr { get; set; }
        [Text(Name = "belnr")]
        public string Belnr { get; set; }
        [Text(Name = "buzei")]
        public decimal? Buzei { get; set; }
        [Text(Name = "para")]
        public int? Para { get; set; }
        [Text(Name = "shorttext")]
        public string ShortText { get; set; }
        [Text(Name = "purchdoc")]
        public string PurchDoc { get; set; }
        [Text(Name = "dataupdatedate")]
        public DateTime? DataUpdateDate { get; set; }
        [Text(Name = "markweightingkey")]
        public string MarkWeightingKey { get; set; }
    }

    public class VendorTransectionSearchViewModel
    {
        [Required(ErrorMessage = MessageValue.PleaseSelectedStartDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string StartDate { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedEndDate)]
        [RegularExpression(ConstantValue.RegexDateFormat, ErrorMessage = ConstantValue.DateIncorrectFormat)]
        public string EndDate { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPurGroup)]
        public string PurGroup { get; set; }
    }

    public class VendorTransectionRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedWeightingKey)]
        [MaxLength(2)]
        public string WeightingKey { get; set; }
    }

}

using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.CentralSetting.Bll.Models
{
    public class PurchasingOrgViewModel
    {

        public PurchasingOrgViewModel()
        {
            PurchasingItems = new List<PurchasingOrgItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_PurchaseOrg";
        public const string RoleForDisplayData = "Role_DS_PurchaseOrg";

        [Required(ErrorMessage = MessageValue.PleaseFillPurchaseOrgCode)]
        [MaxLength(4, ErrorMessage = MessageValue.PurchaseOrgCodeOverLength)]
        public string PurchaseOrg1 { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillPurchaseOrgName)]
        [MaxLength(200, ErrorMessage = MessageValue.PurchaseOrgNameOverLength)]
        public string PurchaseName { get; set; }
        public List<PurchasingOrgItemViewModel> PurchasingItems { get; set; }
    }

    public class PurchasingOrgItemViewModel
    {
        [Required]
        [MaxLength(100)]
        public string AdUser { get; set; }
        [Required]
        [MaxLength(10)]
        public string Type { get; set; }
    }

}

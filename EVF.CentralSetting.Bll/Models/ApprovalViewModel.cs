using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.CentralSetting.Bll.Models
{
    public class ApprovalViewModel
    {
        public ApprovalViewModel()
        {
            ApprovalList = new List<ApprovalItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_Approval";
        public const string RoleForDisplayData = "Role_DS_Approval";

        public int Id { get; set; }
        [Required]
        public string ComCode { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public List<ApprovalItemViewModel> ApprovalList { get; set; }
    }

    public class ApprovalItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public string AdUser { get; set; }
        [Required]
        public int Step { get; set; }
    }

}

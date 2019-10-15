using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class CriteriaViewModel
    {
        public CriteriaViewModel()
        {
            CriteriaGroups = new List<CriteriaGroupViewModel>();
        }

        public const string RoleForManageData = "Role_MA_Criteria";
        public const string RoleForDisplayData = "Role_DS_Criteria";

        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillCriteriaName)]
        [MaxLength(200, ErrorMessage = MessageValue.CriteriaNameOverLength)]
        public string CriteriaName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUse { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedKpiGroup)]
        public List<CriteriaGroupViewModel> CriteriaGroups { get; set; }
    }
}

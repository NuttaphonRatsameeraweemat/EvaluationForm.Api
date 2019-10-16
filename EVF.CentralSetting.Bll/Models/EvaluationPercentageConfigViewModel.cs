using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.CentralSetting.Bll.Models
{
    public class EvaluationPercentageConfigViewModel
    {
        public const string RoleForManageData = "Role_MA_EvaluationPercentageConfig";
        public const string RoleForDisplayData = "Role_DS_EvaluationPercentageConfig";

        public int Id { get; set; }
        public int PurchasePercentage { get; set; }
        public int UserPercentage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class EvaluationPercentageConfigRequestModel
    {
        public int Id { get; set; }
        [Required]
        public int PurchasePercentage { get; set; }
        [Required]
        public int UserPercentage { get; set; }
    }

}

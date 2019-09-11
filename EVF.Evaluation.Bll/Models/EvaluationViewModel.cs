using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Evaluation.Bll.Models
{
    public class EvaluationViewModel
    {

        public EvaluationViewModel()
        {

        }

        public const string RoleForManageData = "Role_MA_Evaluation";
        public const string RoleForDisplayData = "Role_DS_Evaluation";

        public int Id { get; set; }
        [Required]
        public int EvaluationTemplateId { get; set; }
        public string VendorNo { get; set; }
        [Required]
        public string WeightingKey { get; set; }
        [Required]
        public string ComCode { get; set; }
        [Required]
        public string PurchasingOrg { get; set; }
        [Required]
        public int PeriodId { get; set; }

        public string[] VendorList { get; set; }
        public string[] EvaluatorList { get; set; }
        public string PurchasingEmpNo { get; set; }

        //Display
        public string VendorName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.CentralSetting.Bll.Models
{
    public class EvaluatorGroupViewModel
    {
        public const string RoleForManageData = "Role_MA_EvaluatorGroup";
        public const string RoleForDisplayData = "Role_DS_EvaluatorGroup";

        public int Id { get; set; }
        [Required]
        public string EvaluatorGroupName { get; set; }
        public string[] AdUserList { get; set; }
    }
}

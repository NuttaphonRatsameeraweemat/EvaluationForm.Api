using EVF.Helper.Components;
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
        [Required(ErrorMessage = MessageValue.PleaseFillEvaluatorGroupName)]
        public string EvaluatorGroupName { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPeriod)]
        public int? PeriodId { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedPeriod)]
        public int? PeriodItemId { get; set; }
        public string PeriodItemName { get; set; }
        public string[] AdUserList { get; set; }
    }
}

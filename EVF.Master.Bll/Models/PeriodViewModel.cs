using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class PeriodViewModel
    {
        public PeriodViewModel()
        {
            PeriodItems = new List<PeriodItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_Period";
        public const string RoleForDisplayData = "Role_DS_Period";

        public int Id { get; set; }
        [Required]
        [RegularExpression(ConstantValue.RegexYearFormat, ErrorMessage = ConstantValue.YearIncorrectFormat)]
        public string Year { get; set; }
        public List<PeriodItemViewModel> PeriodItems { get; set; }
    }
}

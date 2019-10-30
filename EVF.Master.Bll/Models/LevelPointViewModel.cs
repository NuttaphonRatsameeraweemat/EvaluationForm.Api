using EVF.Helper.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class LevelPointViewModel
    {
        public LevelPointViewModel()
        {
            LevelPointItems = new List<LevelPointItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_LevelPoint";
        public const string RoleForDisplayData = "Role_DS_LevelPoint";

        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillLevelPointName)]
        [MaxLength(100, ErrorMessage = MessageValue.LevelPointNameOverLength)]
        public string Name { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseSelectedLevelPointCalculate)]
        public string WeightingKey { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUse { get; set; }
        public List<LevelPointItemViewModel> LevelPointItems { get; set; }
    }
}

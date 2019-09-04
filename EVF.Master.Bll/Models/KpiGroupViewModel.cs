using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class KpiGroupViewModel
    {
        public KpiGroupViewModel()
        {
            KpiGroupItems = new List<KpiGroupItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_PerformanceGroup";
        public const string RoleForDisplayData = "Role_DS_PerformanceGroup";

        public int Id { get; set; }
        [Required]
        public string KpiGroupNameTh { get; set; }
        [Required]
        public string KpiGroupNameEn { get; set; }
        [Required]
        public string SapScoreField { get; set; }
        public bool IsUse { get; set; }
        public List<KpiGroupItemViewModel> KpiGroupItems { get; set; }
    }
}

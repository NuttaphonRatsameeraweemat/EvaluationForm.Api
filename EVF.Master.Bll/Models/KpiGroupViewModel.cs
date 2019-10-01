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

        public const string RoleForManageData = "Role_MA_KpiGroup";
        public const string RoleForDisplayData = "Role_DS_KpiGroup";

        public int Id { get; set; }
        [Required]
        public string KpiGroupNameTh { get; set; }
        [Required]
        public string KpiGroupNameEn { get; set; }
        [Required]
        [MaxLength(20)]
        public string KpiGroupShortTextTh { get; set; }
        [Required]
        [MaxLength(20)]
        public string KpiGroupShortTextEn { get; set; }
        public string SapScoreField { get; set; }
        public bool IsUse { get; set; }
        public List<KpiGroupItemViewModel> KpiGroupItems { get; set; }
    }
}

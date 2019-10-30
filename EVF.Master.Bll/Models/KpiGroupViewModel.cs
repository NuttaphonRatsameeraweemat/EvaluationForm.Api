using EVF.Helper.Components;
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
        [Required(ErrorMessage = MessageValue.PleaseFillKpiGroupNameTh)]
        [MaxLength(200, ErrorMessage = MessageValue.KpiGroupNameOverLength)]
        public string KpiGroupNameTh { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiGroupNameEn)]
        [MaxLength(200, ErrorMessage = MessageValue.KpiGroupNameOverLength)]
        public string KpiGroupNameEn { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiGroupShortTextTh)]
        [MaxLength(40, ErrorMessage = MessageValue.KpiGroupShortTextOverLength)]
        public string KpiGroupShortTextTh { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiGroupShortTextEn)]
        [MaxLength(40, ErrorMessage = MessageValue.KpiGroupShortTextOverLength)]
        public string KpiGroupShortTextEn { get; set; }
        public string SapScoreField { get; set; }
        public bool IsUse { get; set; }
        public List<KpiGroupItemViewModel> KpiGroupItems { get; set; }
    }
}

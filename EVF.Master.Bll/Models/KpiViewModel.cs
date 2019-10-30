using EVF.Helper.Components;
using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class KpiViewModel
    {

        public const string RoleForManageData = "Role_MA_Kpi";
        public const string RoleForDisplayData = "Role_DS_Kpi";

        public int Id { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiNameTh)]
        [MaxLength(200, ErrorMessage = MessageValue.KpiNameOverLength)]
        public string KpiNameTh { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiNameEn)]
        [MaxLength(200, ErrorMessage = MessageValue.KpiNameOverLength)]
        public string KpiNameEn { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiShortTextTh)]
        [MaxLength(40, ErrorMessage = MessageValue.KpiShortTextOverLength)]
        public string KpiShortTextTh { get; set; }
        [Required(ErrorMessage = MessageValue.PleaseFillKpiShortTextEn)]
        [MaxLength(40, ErrorMessage = MessageValue.KpiShortTextOverLength)]
        public string KpiShortTextEn { get; set; }
        public bool IsUse { get; set; }
    }
}

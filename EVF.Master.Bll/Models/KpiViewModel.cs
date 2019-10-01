using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class KpiViewModel
    {

        public const string RoleForManageData = "Role_MA_Kpi";
        public const string RoleForDisplayData = "Role_DS_Kpi";

        public int Id { get; set; }
        [Required]
        public string KpiNameTh { get; set; }
        [Required]
        public string KpiNameEn { get; set; }
        [Required]
        [MaxLength(20)]
        public string KpiShortTextTh { get; set; }
        [Required]
        [MaxLength(20)]
        public string KpiShortTextEn { get; set; }
        public bool IsUse { get; set; }
    }
}

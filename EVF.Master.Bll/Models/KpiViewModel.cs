using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class KpiViewModel
    {

        public const string RoleForManageData = "Role_MA_Performance";
        public const string RoleForDisplayData = "Role_DS_Performance";

        public int Id { get; set; }
        [Required]
        public string KpiNameTh { get; set; }
        [Required]
        public string KpiNameEn { get; set; }
        public bool IsUse { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class PerformanceViewModel
    {

        public const string RoleForManageData = "Role_MA_Performance";
        public const string RoleForDisplayData = "Role_DS_Performance";

        public int Id { get; set; }
        [Required]
        public string PerformanceNameTh { get; set; }
        [Required]
        public string PerformanceNameEn { get; set; }
    }
}

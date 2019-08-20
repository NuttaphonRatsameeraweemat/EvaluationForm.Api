using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EVF.Master.Bll.Models
{
    public class PerformanceGroupViewModel
    {
        public PerformanceGroupViewModel()
        {
            PerformanceGroupItems = new List<int>();
        }

        public const string RoleForManageData = "Role_MA_PerformanceGroup";
        public const string RoleForDisplayData = "Role_DS_PerformanceGroup";

        public int Id { get; set; }
        [Required]
        public string PerformanceGroupNameTh { get; set; }
        [Required]
        public string PerformanceGroupNameEn { get; set; }
        public List<int> PerformanceGroupItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Bll.Models
{
    public class PerformanceViewModel
    {

        public const string RoleForManageData = "Role_MA_Performance";
        public const string RoleForDisplayData = "Role_DS_Performance";

        public int Id { get; set; }
        [Required]
        public string PerformanceName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Master.Bll.Models
{
    public class GradeViewModel
    {
        public GradeViewModel()
        {
            GradeItems = new List<GradeItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_Grade";
        public const string RoleForDisplayData = "Role_DS_Grade";

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUse { get; set; }
        public List<GradeItemViewModel> GradeItems { get; set; }
    }
}

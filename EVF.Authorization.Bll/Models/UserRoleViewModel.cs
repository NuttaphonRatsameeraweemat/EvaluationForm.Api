using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Authorization.Bll.Models
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            RoleList = new List<int>();
        }

        public const string RoleForManageData = "Role_MA_UserRole";
        public const string RoleForDisplayData = "Role_DS_UserRole";

        public string EmpNo { get; set; }
        public string FirstnameTH { get; set; }
        public string LastnameTH { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        [Required]
        public string AdUser { get; set; }
        public string RoleDisplay { get; set; }
        [Required]
        public List<int> RoleList { get; set; }

    }
}

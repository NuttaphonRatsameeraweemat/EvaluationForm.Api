using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Models
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
        public string FirstNameTh { get; set; }
        public string LastNameTh { get; set; }
        public string OrgName { get; set; }
        public string AdUser { get; set; }
        public string RoleDisplay { get; set; }
        public List<int> RoleList { get; set; }

    }
}

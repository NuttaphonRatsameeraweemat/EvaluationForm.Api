using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Authorization.Bll.Models
{
    public class AuthorityCompanyViewModel
    {
        public const string RoleForManageData = "Role_MA_AuthorityCompany";
        public const string RoleForDisplayData = "Role_DS_AuthorityCompany";

        [Required]
        public string AdUser { get; set; }
        [Required]
        public List<string> ComCode { get; set; }
    }
}

using EVF.Bll.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EVF.Bll.Models
{
    public class RoleItemViewModel
    {
        public RoleItemViewModel()
        {
            ParentMenu = new List<RoleItemViewModel>();
        }

        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsManage { get; set; }
        public string RoleDisplayName
        {
            get
            {
                if (IsDisplay)
                {
                    return ConstantValue.RoleDisplay + MenuCode;
                }
                else return null;
            }
        }
        public string RoleManageName
        {
            get
            {
                if (IsManage)
                {
                    return ConstantValue.RoleManage + MenuCode;
                }
                else return null;
            }
        }
        public List<RoleItemViewModel> ParentMenu { get; set; }
    }
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            RoleItem = new List<RoleItemViewModel>();
        }

        public const string RoleForManageData = "Role_MA_RoleManagement";
        public const string RoleForDisplayData = "Role_DS_RoleManagement";

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        public List<RoleItemViewModel> RoleItem { get; set; }
    }
}

using EVF.Helper.Components;
using System.Collections.Generic;

namespace EVF.Authorization.Bll.Models
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
}

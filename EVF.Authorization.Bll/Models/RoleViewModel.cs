using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EVF.Authorization.Bll.Models
{
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class AppMenu
    {
        [StringLength(100)]
        public string MenuCode { get; set; }
        [StringLength(200)]
        public string MenuName { get; set; }
        [StringLength(20)]
        public string MenuType { get; set; }
        [StringLength(100)]
        public string ParentMenuCode { get; set; }
        public int? Sequence { get; set; }
        [StringLength(100)]
        public string RoleForManage { get; set; }
        [StringLength(100)]
        public string RoleForDisplay { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
    }
}

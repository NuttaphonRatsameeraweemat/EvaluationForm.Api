using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class AppCompositeRoleItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("CompositeRoleID")]
        public int? CompositeRoleId { get; set; }
        [StringLength(255)]
        public string RoleMenu { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class UserRoles
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
        [Column("CompositeRoleID")]
        public int? CompositeRoleId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastModifyDate { get; set; }
        [StringLength(255)]
        public string LastModifyBy { get; set; }
    }
}

﻿using System;
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
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
    }
}

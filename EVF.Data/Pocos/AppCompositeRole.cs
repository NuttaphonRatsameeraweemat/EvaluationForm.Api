﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class AppCompositeRole
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool? Active { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    [Table("HRPosition")]
    public partial class Hrposition
    {
        [Column("PosID")]
        [StringLength(8)]
        public string PosId { get; set; }
        [StringLength(40)]
        public string PosName { get; set; }
        [StringLength(6)]
        public string PosLevel { get; set; }
        public DateTime? LastInterface { get; set; }
    }
}

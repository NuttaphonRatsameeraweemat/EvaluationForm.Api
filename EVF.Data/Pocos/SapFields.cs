using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class SapFields
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("SapFields")]
        [StringLength(20)]
        public string SapFields1 { get; set; }
        public bool IsUse { get; set; }
    }
}

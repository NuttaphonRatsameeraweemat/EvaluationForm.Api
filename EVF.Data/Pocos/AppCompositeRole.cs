using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace EVF.Data.Pocos
{
    public partial class AppCompositeRole
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "bit(1)")]
        public BitArray Active { get; set; }
        [StringLength(255)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(255)]
        public string ModifyBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifyDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class ApprovalItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("ApprovalID")]
        public int? ApprovalId { get; set; }
        [StringLength(255)]
        public string AdUser { get; set; }
        public int? Step { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluatorGroupItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("EvaluatorGroupID")]
        public int? EvaluatorGroupId { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
    }
}

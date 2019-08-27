using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class GradeItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("GradeID")]
        public int? GradeId { get; set; }
        public int? StartPoint { get; set; }
        public int? EndPoint { get; set; }
        [StringLength(100)]
        public string GradeName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class LevelPointItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("LevelPointID")]
        public int? LevelPointId { get; set; }
        [StringLength(100)]
        public string LevelPointName { get; set; }
        public int? PercentPoint { get; set; }
        public int? Sequence { get; set; }
    }
}

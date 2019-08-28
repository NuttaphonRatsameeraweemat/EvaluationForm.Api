using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PerformanceGroupItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("PerformanceGroupID")]
        public int? PerformanceGroupId { get; set; }
        [Column("PerformanceItemID")]
        public int? PerformanceItemId { get; set; }
        public int? Sequence { get; set; }
    }
}

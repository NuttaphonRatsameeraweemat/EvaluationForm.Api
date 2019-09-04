using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class KpiGroupItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("KpiGroupID")]
        public int? KpiGroupId { get; set; }
        [Column("KpiID")]
        public int? KpiId { get; set; }
        public int? Sequence { get; set; }
    }
}

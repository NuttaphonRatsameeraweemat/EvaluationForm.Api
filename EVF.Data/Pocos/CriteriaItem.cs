using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class CriteriaItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("CriteriaGroupID")]
        public int? CriteriaGroupId { get; set; }
        [Column("PerformanceID")]
        public int? PerformanceId { get; set; }
        public int? Score { get; set; }
    }
}

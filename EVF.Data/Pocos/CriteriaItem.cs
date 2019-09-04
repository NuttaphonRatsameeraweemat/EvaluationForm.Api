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
        [Column("KpiID")]
        public int? KpiId { get; set; }
        public int? MaxScore { get; set; }
    }
}

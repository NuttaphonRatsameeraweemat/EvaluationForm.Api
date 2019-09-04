using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class CriteriaGroup
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("CriteriaID")]
        public int? CriteriaId { get; set; }
        [Column("KpiGroupID")]
        public int? KpiGroupId { get; set; }
        public int? Sequence { get; set; }
        public int? MaxScore { get; set; }
    }
}

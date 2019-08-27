using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PeriodItem
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("PeriodID")]
        public int? PeriodId { get; set; }
        public DateTime? StartEvaDate { get; set; }
        public DateTime? EndEvaDate { get; set; }
        [StringLength(100)]
        public string PeriodName { get; set; }
    }
}

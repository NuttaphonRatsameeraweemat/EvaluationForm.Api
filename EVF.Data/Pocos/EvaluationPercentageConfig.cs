using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluationPercentageConfig
    {
        [Column("ID")]
        public int Id { get; set; }
        public int PurchasePercentage { get; set; }
        public int UserPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
    }
}

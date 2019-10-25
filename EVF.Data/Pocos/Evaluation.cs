using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Evaluation
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("EvaluationTemplateID")]
        public int? EvaluationTemplateId { get; set; }
        [StringLength(10)]
        public string VendorNo { get; set; }
        [StringLength(10)]
        public string WeightingKey { get; set; }
        [StringLength(10)]
        public string ComCode { get; set; }
        [StringLength(8)]
        public string PurchasingOrg { get; set; }
        [Column("PeriodItemID")]
        public int? PeriodItemId { get; set; }
        [StringLength(30)]
        public string Status { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? TotalScore { get; set; }
        [StringLength(20)]
        public string DocNo { get; set; }
        [Column("EvaPercentageID")]
        public int? EvaPercentageId { get; set; }
        [StringLength(255)]
        public string Category { get; set; }
        public string Remark { get; set; }
        [StringLength(20)]
        public string SendToEvaluationSapResultStatus { get; set; }
    }
}

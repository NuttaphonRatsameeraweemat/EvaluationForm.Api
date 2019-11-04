using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluationTemplate
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string EvaluationTemplateName { get; set; }
        [Column("CriteriaID")]
        public int? CriteriaId { get; set; }
        [Column("GradeID")]
        public int? GradeId { get; set; }
        [Column("LevelPointID")]
        public int? LevelPointId { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public bool? IsUse { get; set; }
        [StringLength(4)]
        public string CreateByPurchaseOrg { get; set; }
        public string ForPurchaseOrg { get; set; }
    }
}

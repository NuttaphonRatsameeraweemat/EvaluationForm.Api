using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluationAssign
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("EvaluationID")]
        public int? EvaluationId { get; set; }
        [StringLength(11)]
        public string EmpNo { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
        [StringLength(1)]
        public string UserType { get; set; }
        public bool? IsReject { get; set; }
        public bool? IsAction { get; set; }
        public string ReasonReject { get; set; }
    }
}

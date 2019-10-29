using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class EvaluationLog
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("EvaluationID")]
        public int? EvaluationId { get; set; }
        [StringLength(11)]
        public string EmpNo { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
        public DateTime? ActionDate { get; set; }
        [StringLength(100)]
        public string ActionBy { get; set; }
        [StringLength(11)]
        public string ActionByUserCode { get; set; }
    }
}

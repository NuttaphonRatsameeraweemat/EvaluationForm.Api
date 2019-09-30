using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class PurGroupWeightingKey
    {
        [Column("Pur_Group")]
        [StringLength(3)]
        public string PurGroup { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(3)]
        public string WeightingKey { get; set; }
        public bool? EvaStatus { get; set; }
    }
}

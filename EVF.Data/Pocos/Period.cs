using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Period
    {
        [Column("ID")]
        public int Id { get; set; }
        public int Year { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        public DateTime? LastModifyDate { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(4)]
        public string CreateByPurchaseOrg { get; set; }
    }
}

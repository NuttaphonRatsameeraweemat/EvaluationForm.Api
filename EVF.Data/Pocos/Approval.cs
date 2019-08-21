using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Approval
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string ComCode { get; set; }
        [Column("OrgID")]
        [StringLength(8)]
        public string OrgId { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
        [StringLength(11)]
        public string LastModifyBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastModifyDate { get; set; }
    }
}

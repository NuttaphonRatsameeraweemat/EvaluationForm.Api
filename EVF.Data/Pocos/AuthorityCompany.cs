using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class AuthorityCompany
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string AdUser { get; set; }
        [StringLength(10)]
        public string ComCode { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }
    }
}

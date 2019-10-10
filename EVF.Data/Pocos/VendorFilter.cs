using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class VendorFilter
    {
        [Column("ID")]
        public int Id { get; set; }
        public int? PeriodItemId { get; set; }
        [StringLength(10)]
        public string CompanyCode { get; set; }
        [StringLength(10)]
        public string PurchasingOrg { get; set; }
        [StringLength(4)]
        public string WeightingKey { get; set; }
        [StringLength(10)]
        public string VendorNo { get; set; }
        [StringLength(100)]
        public string AssignTo { get; set; }
        public bool? IsSending { get; set; }
        [StringLength(11)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? SendingEvaDate { get; set; }
        [StringLength(11)]
        public string SendingBy { get; set; }
    }
}

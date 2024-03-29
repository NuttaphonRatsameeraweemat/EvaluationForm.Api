﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Vendor
    {
        [StringLength(10)]
        public string VendorNo { get; set; }
        [StringLength(140)]
        public string VendorName { get; set; }
        [StringLength(20)]
        public string SearchTerm1 { get; set; }
        [StringLength(18)]
        public string TaxNo3 { get; set; }
        [StringLength(20)]
        public string VatRegNo { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(3)]
        public string CountyKey { get; set; }
        [StringLength(15)]
        public string CountyDesc { get; set; }
        [StringLength(6)]
        public string TimeZone { get; set; }
        [StringLength(20)]
        public string TrZone { get; set; }
        [StringLength(100)]
        public string TrZoneDesc { get; set; }
        [StringLength(30)]
        public string TelNo { get; set; }
        [StringLength(10)]
        public string TelExt { get; set; }
        [StringLength(30)]
        public string MobileNo { get; set; }
        [StringLength(30)]
        public string FaxNo { get; set; }
        [StringLength(10)]
        public string FaxExt { get; set; }
        [StringLength(241)]
        public string Email { get; set; }
        [StringLength(4)]
        public string VendorAccGrp { get; set; }
        [StringLength(30)]
        public string VendAccGrpName { get; set; }
        [StringLength(10)]
        public string CustNo { get; set; }
        [StringLength(1)]
        public string DelFlag { get; set; }
        [StringLength(1)]
        public string PostBlock { get; set; }
        [StringLength(1)]
        public string PurBlock { get; set; }
        [StringLength(1)]
        public string LanguageKey { get; set; }
        [StringLength(1)]
        public string OneTimeInd { get; set; }
        [StringLength(1)]
        public string NoDel { get; set; }
        [StringLength(12)]
        public string CreateBy { get; set; }
        [StringLength(10)]
        public string CreateDate { get; set; }
        [StringLength(8)]
        public string CreateTime { get; set; }
    }
}

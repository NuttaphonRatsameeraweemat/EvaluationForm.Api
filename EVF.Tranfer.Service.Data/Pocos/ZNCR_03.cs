using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Tranfer.Service.Data.Pocos
{
    public partial class ZNCR_03
    {
        [Key]
        [Column("VENDOR_NO")]
        [StringLength(10)]
        public string VendorNo { get; set; }
        [Column("VENDOR_NAME")]
        [StringLength(140)]
        public string VendorName { get; set; }
        [Column("SEARCH_TERM1")]
        [StringLength(20)]
        public string SearchTerm1 { get; set; }
        [Column("TAX_NO3")]
        [StringLength(18)]
        public string TaxNo3 { get; set; }
        [Column("VAT_REG_NO")]
        [StringLength(20)]
        public string VatRegNo { get; set; }
        [Column("ADDRESS")]
        [StringLength(200)]
        public string Address { get; set; }
        [Column("COUNTY_KEY")]
        [StringLength(3)]
        public string CountyKey { get; set; }
        [Column("COUNTY_DESC")]
        [StringLength(15)]
        public string CountyDesc { get; set; }
        [Column("TIME_ZONE")]
        [StringLength(6)]
        public string TimeZone { get; set; }
        [Column("TR_ZONE")]
        [StringLength(10)]
        public string TrZone { get; set; }
        [Column("TR_ZONE_DESC")]
        [StringLength(20)]
        public string TrZoneDesc { get; set; }
        [Column("TEL_NO")]
        [StringLength(30)]
        public string TelNo { get; set; }
        [Column("TEL_EXT")]
        [StringLength(10)]
        public string TelExt { get; set; }
        [Column("MOBILE_NO")]
        [StringLength(30)]
        public string MobileNo { get; set; }
        [Column("FAX_NO")]
        [StringLength(30)]
        public string FaxNo { get; set; }
        [Column("FAX_EXT")]
        [StringLength(10)]
        public string FaxExt { get; set; }
        [Column("EMAIL")]
        [StringLength(241)]
        public string Email { get; set; }
        [Column("VENDOR_ACC_GRP")]
        [StringLength(4)]
        public string VendorAccGrp { get; set; }
        [Column("VEND_ACC_GRP_NAME")]
        [StringLength(30)]
        public string VendAccGrpName { get; set; }
        [Column("CUST_NO")]
        [StringLength(10)]
        public string CustNo { get; set; }
        [Column("DEL_FLAG")]
        [StringLength(1)]
        public string DelFlag { get; set; }
        [Column("POST_BLOCK")]
        [StringLength(1)]
        public string PostBlock { get; set; }
        [Column("PUR_BLOCK")]
        [StringLength(1)]
        public string PurBlock { get; set; }
        [Column("LANGUAGE_KEY")]
        [StringLength(1)]
        public string LanguageKey { get; set; }
        [Column("ONE_TIME_IND")]
        [StringLength(1)]
        public string OneTimeInd { get; set; }
        [Column("NO_DEL")]
        [StringLength(1)]
        public string NoDel { get; set; }
        [Required]
        [Column("CREATE_DATE")]
        [StringLength(10)]
        public string CreateDate { get; set; }
        [Column("CREATE_TIME")]
        [StringLength(8)]
        public string CreateTime { get; set; }
        [Column("CREATE_BY")]
        [StringLength(12)]
        public string CreateBy { get; set; }
    }
}

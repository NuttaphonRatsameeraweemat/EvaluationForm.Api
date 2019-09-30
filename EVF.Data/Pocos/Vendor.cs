using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    public partial class Vendor
    {
        [StringLength(10)]
        public string VendorNo { get; set; }
        [StringLength(250)]
        public string VendorName { get; set; }
        [StringLength(150)]
        public string SearchTerm1 { get; set; }
        [StringLength(50)]
        public string TaxNumber3 { get; set; }
        [Column("VATRegNo")]
        [StringLength(50)]
        public string VatregNo { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(20)]
        public string CountryKey { get; set; }
        [StringLength(100)]
        public string CountryDescription { get; set; }
        public DateTime? AddressTimeZone { get; set; }
        [StringLength(20)]
        public string TransportationZone { get; set; }
        [StringLength(100)]
        public string TransportationZoneDescription { get; set; }
        [StringLength(20)]
        public string TelephoneNo { get; set; }
        [StringLength(20)]
        public string FaxNo { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [Column("MobilePhoneNo.")]
        [StringLength(20)]
        public string MobilePhoneNo { get; set; }
    }
}

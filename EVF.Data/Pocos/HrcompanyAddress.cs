using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVF.Data.Pocos
{
    [Table("HRCompanyAddress")]
    public partial class HrcompanyAddress
    {
        [StringLength(4)]
        public string SapComCode { get; set; }
        [StringLength(25)]
        public string AddressNumber { get; set; }
        [StringLength(100)]
        public string SearchTermTh { get; set; }
        [StringLength(100)]
        public string SearchTermEn { get; set; }
        [StringLength(100)]
        public string NameTh { get; set; }
        [StringLength(100)]
        public string NameEn { get; set; }
        [StringLength(100)]
        public string Address1Th { get; set; }
        [StringLength(100)]
        public string Address1En { get; set; }
        [StringLength(100)]
        public string Address2Th { get; set; }
        [StringLength(100)]
        public string Address2En { get; set; }
        [StringLength(100)]
        public string Address3Th { get; set; }
        [StringLength(100)]
        public string Address3En { get; set; }
        [StringLength(100)]
        public string DistrictTh { get; set; }
        [StringLength(100)]
        public string DistrictEn { get; set; }
        [StringLength(100)]
        public string CityTh { get; set; }
        [StringLength(100)]
        public string CityEn { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(20)]
        public string Telephone { get; set; }
        [StringLength(10)]
        public string TelephoneExt { get; set; }
        [StringLength(20)]
        public string Fax { get; set; }
    }
}

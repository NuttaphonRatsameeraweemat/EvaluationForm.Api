using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Models
{
    public class VendorViewModel
    {
        public const string RoleForManageData = "Role_MA_VendorProfile";
        public const string RoleForDisplayData = "Role_DS_VendorProfile";

        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string SearchTerm1 { get; set; }
        public string TaxNumber3 { get; set; }
        public string VatregNo { get; set; }
        public string Address { get; set; }
        public string CountryKey { get; set; }
        public string CountryDescription { get; set; }
        public DateTime? AddressTimeZone { get; set; }
        public string TransportationZone { get; set; }
        public string TransportationZoneDescription { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string MobilePhoneNo { get; set; }
    }

    public class VendorRequestViewModel
    {
        public string VendorNo { get; set; }
        public string Email { get; set; }
        public string TelephoneNo { get; set; }
    }

}

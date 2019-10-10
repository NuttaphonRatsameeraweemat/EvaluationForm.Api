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
        public string TaxNo3 { get; set; }
        public string VatRegNo { get; set; }
        public string Address { get; set; }
        public string CountryKey { get; set; }
        public string CountyDesc { get; set; }
        public string TimeZone { get; set; }
        public string TrZone { get; set; }
        public string TrZoneDesc { get; set; }
        public string TelNo { get; set; }
        public string TelExt { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string FaxExt { get; set; }
        public string Email { get; set; }
    }

    public class VendorRequestViewModel
    {
        public string VendorNo { get; set; }
        public string Email { get; set; }
        public string TelNo { get; set; }
        public string TelExt { get; set; }
        public string MobileNo { get; set; }
    }

}

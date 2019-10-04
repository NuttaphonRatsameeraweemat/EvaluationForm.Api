using EVF.Helper.Models;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Interfaces
{
    public interface IVendorBll
    {
        /// <summary>
        /// Get Vendor list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorViewModel> GetList();
        /// <summary>
        /// Get Detail of Vendor.
        /// </summary>
        /// <param name="vendorNo">The identity Vendor.</param>
        /// <returns></returns>
        VendorViewModel GetDetail(string vendorNo);
        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        ResultViewModel UpdateVendorContact(VendorRequestViewModel model);
    }
}

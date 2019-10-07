using EVF.Data.Pocos;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Interfaces
{
    public interface IVendorFilterBll
    {
        /// <summary>
        /// Get VendorFilter list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorFilterViewModel> GetList(int periodItemId);
        /// <summary>
        /// Get VendorFilter list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorFilterViewModel> GetVendorFilters(VendorFilterCriteriaViewModel model);
        /// <summary>
        /// Filter Vendor by condition and total sales.
        /// </summary>
        /// <param name="model">The criteria vendor filter.</param>
        /// <returns></returns>
        IEnumerable<VendorFilterResponseViewModel> SearchVendor(VendorFilterSearchViewModel model);
        /// <summary>
        /// Get Detail of Vendor.
        /// </summary>
        /// <param name="vendorNo">The identity Vendor.</param>
        /// <returns></returns>
        VendorFilterViewModel GetDetail(int id);
        /// <summary>
        /// Create VendorFilter.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        ResultViewModel Save(VendorFilterRequestViewModel model);
        /// <summary>
        /// Change Assign to VendorFilter.
        /// </summary>
        /// <param name="model">The Vendor filter assign to information.</param>
        /// <returns></returns>
        ResultViewModel ChangeAssignTo(VendorFilterEditRequestViewModel model);
        /// <summary>
        /// Delete VendorFilter.
        /// </summary>
        /// <param name="id">The Vendor filter identity.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Update sending status and log timestamp.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        ResultViewModel UpdateStatus(VendorFilter model);
    }
}

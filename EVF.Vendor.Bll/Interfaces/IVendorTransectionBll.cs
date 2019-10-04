using EVF.Helper.Models;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Vendor.Bll.Interfaces
{
    public interface IVendorTransectionBll
    {
        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorTransectionViewModel> GetList();
        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorTransectionViewModel> GetListSearch(VendorTransectionSearchViewModel model);
        /// <summary>
        /// Get Detail of VendorTransection.
        /// </summary>
        /// <param name="id">The identity VendorTranection.</param>
        /// <returns></returns>
        VendorTransectionViewModel GetDetail(int id);
        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        ResultViewModel MarkWeightingKey(VendorTransectionRequestViewModel model);
    }
}

using EVF.Data.Pocos;
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
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VendorTransectionElasticSearchModel> GetListSearchElastic(VendorTransectionSearchViewModel model);
        /// <summary>
        /// Get Vendor transection.
        /// </summary>
        /// <param name="startDateString">The start transection date.</param>
        /// <param name="endDateString">The end transection date.</param>
        /// <param name="purGroup">The purGroup code.</param>
        /// <returns></returns>
        IEnumerable<VendorTransaction> GetTransections(string startDateString, string endDateString, string[] purGroup);
        /// <summary>
        /// Get Transection list by condition.
        /// </summary>
        /// <param name="startDateString">The start transection date.</param>
        /// <param name="endDateString">The end transection date.</param>
        /// <param name="purGroup">The purGroup code.</param>
        /// <param name="comCode">The company code.</param>
        /// <param name="purOrg">The purchase org.</param>
        /// <returns></returns>
        IEnumerable<VendorTransectionElasticSearchModel> GetTransections(string startDateString, string endDateString, string[] purGroup, string comCode, string purOrg);
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
        /// <summary>
        /// Bulk vendor transaction to elastic.
        /// </summary>
        /// <returns></returns>
        string BulkVendorTransaction();
        /// <summary>
        /// Bulk vendor transaction status update to elastic.
        /// </summary>
        /// <returns></returns>
        string BulkUpdateVendorTransaction();
        /// <summary>
        /// Re import to elastic by transaction id.
        /// </summary>
        /// <param name="id">The transaction identity.</param>
        /// <returns></returns>
        string ReImportTransactionById(int id);
        /// <summary>
        /// Re import all vendor transaction to elastic.
        /// </summary>
        /// <returns></returns>
        string ReImportTransaction();
    }
}

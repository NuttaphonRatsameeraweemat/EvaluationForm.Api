using EVF.Helper.Models;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Bll.Interfaces
{
    public interface IVendorTranferBll
    {
        /// <summary>
        /// Tranfer data vendor master in zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        ResultViewModel TranferVendorData();
        /// <summary>
        /// Update data vendor master from zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        ResultViewModel UpdateVendorDataFromZncr03(string vendorNo);
        /// <summary>
        /// Insert new data vendor master from zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        ResultViewModel AddNewVendorDataFromZncr03(string vendorNo);
        /// <summary>
        /// Try to connect zncr 03 table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ZNCR_03> TryToConnect();
    }
}

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
        /// Tranfer data vendor transection in brb util to spe database.
        /// </summary>
        /// <returns></returns>
        ResultViewModel TranferVendorTransection();
        /// <summary>
        /// Try to connect brb util db.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SPE_TRANSAC_PO_QA> TryToConnect();
    }
}

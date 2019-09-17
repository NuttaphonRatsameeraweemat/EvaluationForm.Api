using EVF.Helper.Models;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Bll.Interfaces
{
    public interface ITranferBll
    {
        /// <summary>
        /// Tranfer data in evaluation sap result to zncr 02.
        /// </summary>
        /// <returns></returns>
        ResultViewModel TranferToDataMart();
        /// <summary>
        /// Try to connect zncr db.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ZNCR_02> TryToConnect();
    }
}

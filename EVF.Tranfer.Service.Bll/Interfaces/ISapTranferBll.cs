using EVF.Data.Pocos;
using EVF.Helper.Models;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Tranfer.Service.Bll.Interfaces
{
    public interface ISapTranferBll
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
        IEnumerable<ZSPE_02> TryToConnect();
        /// <summary>
        /// Try to insert sap result score to evf db.
        /// </summary>
        /// <returns></returns>
        ResultViewModel TryToInsertSapResult(IEnumerable<EvaluationSapResult> model);
        /// <summary>
        /// Get Evaluation Sap Result Score.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationSapResult> GetEvaluationSapResult();
    }
}

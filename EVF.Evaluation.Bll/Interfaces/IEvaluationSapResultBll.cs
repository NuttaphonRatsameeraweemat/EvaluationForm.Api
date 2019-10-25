using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface IEvaluationSapResultBll
    {
        /// <summary>
        /// Save evaluation result score to sap result table and upadte status flag.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        void Save(int evaluationId);
        /// <summary>
        /// Execute evaluation send to sap result status failed again.
        /// </summary>
        ResultViewModel ExecuteFailedProcess();
    }
}

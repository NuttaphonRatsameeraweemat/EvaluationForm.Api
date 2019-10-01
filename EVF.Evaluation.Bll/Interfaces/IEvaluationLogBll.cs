using EVF.Evaluation.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface IEvaluationLogBll
    {
        /// <summary>
        /// Get Evaluation Log.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationLogViewModel> GetEvaluationLog(int evaluationId);
        /// <summary>
        /// Get Evaluation Log.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationLogViewModel> GetListEvaluationLogs(int evaluationId);
        /// <summary>
        /// Insert new evaluation log.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        ResultViewModel Save(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model);
    }
}

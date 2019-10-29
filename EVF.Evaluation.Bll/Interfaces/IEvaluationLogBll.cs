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
        /// Get Evaluation Log with id.
        /// </summary>
        /// <param name="id">The evaluation log identity.</param>
        /// <returns></returns>
        IEnumerable<EvaluationLogViewModel> GetEvaluationLogById(int id);
        /// <summary>
        /// Validate Evaluation value before save.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        /// <returns></returns>
        ResultViewModel ValidateData(IEnumerable<EvaluationLogItemViewModel> model);
        /// <summary>
        /// Insert new evaluation log.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        ResultViewModel Save(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model);
        /// <summary>
        /// Insert new evaluation log when purchase user edit score.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        ResultViewModel SavePurchaseEditScore(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model);
        /// <summary>
        /// Inital model for post method save.
        /// </summary>
        /// <param name="evaluationTemplateId">The evaluation template identity.</param>
        /// <returns></returns>
        IEnumerable<EvaluationLogItemViewModel> GetModelEvaluation(int evaluationTemplateId);
    }
}

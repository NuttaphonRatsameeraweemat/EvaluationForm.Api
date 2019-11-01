using EVF.Evaluation.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface IEvaluationAssignBll
    {
        /// <summary>
        /// Get EvaluationAssign.
        /// </summary>
        /// <param name="evaluationAssignId">The identity evaluation and assign.</param>
        /// <returns></returns>
        EvaluationAssignViewModel GetEvaluator(int evaluationAssignId);
        /// <summary>
        /// Get EvaluationAssign list.
        /// </summary>
        /// <param name="evaluationId">The identity of evaluation.</param>
        /// <returns></returns>
        IEnumerable<EvaluationAssignViewModel> GetEvaluators(int evaluationId);
        /// <summary>
        /// Insert new evaluation assign list.
        /// </summary>
        /// <param name="evaluationId">The evaluation id.</param>
        /// <param name="purchasingAdUser">The purchasing aduser.</param>
        /// <param name="userList">The evaluator user list.</param>
        void SaveList(int evaluationId, string purchasingAdUser, string[] userList);
        /// <summary>
        /// Validate evaluation save and edit before add data.
        /// </summary>
        /// <param name="model">The evaluation assign value.</param>
        /// <returns></returns>
        ResultViewModel ValidateData(EvaluationAssignRequestViewModel model);
        /// <summary>
        /// Add new evaluator to evaluation form task.
        /// </summary>
        /// <param name="model">The information value evaluator.</param>
        /// <returns></returns>
        ResultViewModel Save(EvaluationAssignRequestViewModel model);
        /// <summary>
        /// Edit new evaluator to evaluation form task.
        /// </summary>
        /// <param name="model">The information value evaluator.</param>
        /// <returns></returns>
        ResultViewModel Edit(EvaluationAssignRequestViewModel model);
        /// <summary>
        /// Remove evaluation assign task.
        /// </summary>
        /// <param name="id">The evaluation assign identity.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

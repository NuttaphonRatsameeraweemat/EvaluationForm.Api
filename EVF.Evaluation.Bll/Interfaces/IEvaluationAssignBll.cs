﻿using EVF.Evaluation.Bll.Models;
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
        void Save(int evaluationId, string purchasingAdUser, string[] userList);
    }
}

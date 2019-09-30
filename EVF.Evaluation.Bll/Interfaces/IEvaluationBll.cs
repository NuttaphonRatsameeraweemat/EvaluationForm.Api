using EVF.Evaluation.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface IEvaluationBll
    {
        /// <summary>
        /// Get Evaluation List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationViewModel> GetEvaluator();
        /// <summary>
        /// Insert new evaluation.
        /// </summary>
        /// <param name="model">The evaluation information value.</param>
        ResultViewModel Save(EvaluationViewModel model);
    }
}

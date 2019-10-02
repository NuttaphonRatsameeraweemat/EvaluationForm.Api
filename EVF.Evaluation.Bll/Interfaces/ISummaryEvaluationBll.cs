using EVF.Evaluation.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface ISummaryEvaluationBll
    {
        /// <summary>
        /// Get Detail summary evaluation.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        SummaryEvaluationViewModel GetDetail(int id);
        /// <summary>
        /// Get Evaluation waiting List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationViewModel> GetList();
    }
}

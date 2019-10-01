﻿using EVF.Evaluation.Bll.Models;
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
        IEnumerable<EvaluationViewModel> GetList();
        /// <summary>
        /// Insert new evaluation.
        /// </summary>
        /// <param name="model">The evaluation information value.</param>
        ResultViewModel Save(EvaluationRequestViewModel model);
        /// <summary>
        /// Reject evaluation task.
        /// </summary>
        /// <param name="model">The evaluation reject information.</param>
        /// <returns></returns>
        ResultViewModel Reject(EvaluationRejectViewModel model);
    }
}

﻿using EVF.Evaluation.Bll.Models;
using EVF.Helper.Models;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Evaluation.Bll.Interfaces
{
    public interface ISummaryEvaluationBll
    {
        /// <summary>
        /// Get Evaluation waiting List.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationViewModel> GetList();
        /// <summary>
        /// Get Summary Evaluation List by period item id.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        IEnumerable<EvaluationViewModel> GetListSearch(int periodItemId);
        /// <summary>
        /// Get Detail summary evaluation.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        SummaryEvaluationViewModel GetDetail(int id);
        /// <summary>
        /// Send evaluation approve.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        ResultViewModel SendApprove(int id);
        /// <summary>
        /// Submit action task approve or reject.
        /// </summary>
        /// <param name="model">The evaluation task information.</param>
        /// <returns></returns>
        ResultViewModel SubmitAction(WorkflowViewModel model);
        /// <summary>
        /// Validate Status before send approve.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        ResultViewModel ValidateStatus(int id);
    }
}

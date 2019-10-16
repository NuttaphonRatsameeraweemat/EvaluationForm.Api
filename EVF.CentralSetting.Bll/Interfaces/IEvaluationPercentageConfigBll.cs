using EVF.CentralSetting.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.CentralSetting.Bll.Interfaces
{
    public interface IEvaluationPercentageConfigBll
    {
        /// <summary>
        /// Get Evaluation Percentage Config list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluationPercentageConfigViewModel> GetList();
        /// <summary>
        /// Get Detail of Evaluation Percentage Config.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        EvaluationPercentageConfigViewModel GetDetail(int id);
        /// <summary>
        /// Insert new Evaluation Percentage Config.
        /// </summary>
        /// <param name="model">The Evaluation Percentage Config information value.</param>
        /// <returns></returns>
        ResultViewModel Save(EvaluationPercentageConfigRequestModel model);
        /// <summary>
        /// Update Evaluation Percentage Config.
        /// </summary>
        /// <param name="model">The Evaluation Percentage Config information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(EvaluationPercentageConfigRequestModel model);
        /// <summary>
        /// Remove Evaluation Percentage Config.
        /// </summary>
        /// <param name="id">The identity Evaluation Percentage Config.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

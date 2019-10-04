using EVF.CentralSetting.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.CentralSetting.Bll.Interfaces
{
    public interface IEvaluatorGroupBll
    {
        /// <summary>
        /// Get EvaluatorGroup list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EvaluatorGroupViewModel> GetList();
        /// <summary>
        /// Get Detail of EvaluatorGroup.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        EvaluatorGroupViewModel GetDetail(int id);
        /// <summary>
        /// Insert new EvaluatorGroup list.
        /// </summary>
        /// <param name="model">The EvaluatorGroup information value.</param>
        /// <returns></returns>
        ResultViewModel Save(EvaluatorGroupViewModel model);
        /// <summary>
        /// Update EvaluatorGroup.
        /// </summary>
        /// <param name="model">The EvaluatorGroup information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(EvaluatorGroupViewModel model);
        /// <summary>
        /// Remove EvaluatorGroup.
        /// </summary>
        /// <param name="id">The identity EvaluatorGroup.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

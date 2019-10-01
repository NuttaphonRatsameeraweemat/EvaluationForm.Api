using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface ICriteriaBll
    {
        /// <summary>
        /// Get Criteria list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CriteriaViewModel> GetList();
        /// <summary>
        /// Get Detail of criteria.
        /// </summary>
        /// <param name="id">The identity of criteria group.</param>
        /// <returns></returns>
        CriteriaViewModel GetDetail(int id);
        /// <summary>
        /// Validate information value in criteria logic.
        /// </summary>
        /// <param name="model">The criteria information value.</param>
        /// <returns></returns>
        ResultViewModel ValidateData(CriteriaViewModel model);
        /// <summary>
        /// Insert new criteria group.
        /// </summary>
        /// <param name="model">The criteria information value.</param>
        /// <returns></returns>
        ResultViewModel Save(CriteriaViewModel model);
        /// <summary>
        /// Update grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(CriteriaViewModel model);
        /// <summary>
        /// Remove criteria.
        /// </summary>
        /// <param name="id">The identity of criteria.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Validate criteria is using in evaluation template or not.
        /// </summary>
        /// <param name="id">The criteria identity.</param>
        /// <returns></returns>
        bool IsUse(int id);
        /// <summary>
        /// Set flag is use in criteria.
        /// </summary>
        /// <param name="ids">The criteria identity.</param>
        /// <param name="isUse">The flag is using.</param>
        void SetIsUse(int id, bool isUse);
    }
}

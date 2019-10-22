using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System.Collections.Generic;

namespace EVF.Master.Bll.Interfaces
{
    public interface IKpiGroupBll
    {
        /// <summary>
        /// Get Kpi Group list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KpiGroupViewModel> GetList();
        /// <summary>
        /// Get Detail of Kpi group.
        /// </summary>
        /// <param name="id">The identity of Kpi group.</param>
        /// <returns></returns>
        KpiGroupViewModel GetDetail(int id);
        /// <summary>
        /// Get Kpi group item for display on criteria.
        /// </summary>
        /// <param name="kpiGroupId">The identity Kpi group</param>
        /// <returns></returns>
        IEnumerable<CriteriaItemViewModel> GetKpiItemDisplayCriteria(int kpiGroupId);
        /// <summary>
        /// Validate Data before insert and update kpi group.
        /// </summary>
        /// <returns></returns>
        ResultViewModel ValidateData(KpiGroupViewModel model);
        /// <summary>
        /// Validate kpi group any duplicate item.
        /// </summary>
        /// <param name="model">The kpi group information.</param>
        /// <returns></returns>
        ResultViewModel ValidateDuplicatesItems(KpiGroupViewModel model);
        /// <summary>
        /// Insert new Kpi group.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        ResultViewModel Save(KpiGroupViewModel model);
        /// <summary>
        /// Update Kpi group.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(KpiGroupViewModel model);
        /// <summary>
        /// Remove Kpi group.
        /// </summary>
        /// <param name="id">The identity of Kpi group.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Set flag is use in kpi group.
        /// </summary>
        /// <param name="ids">The kpi group identity list.</param>
        /// <param name="isUse">The flag is using.</param>
        void SetIsUse(int[] ids, bool isUse);
        /// <summary>
        /// Validate kpi group is using in criteria or not.
        /// </summary>
        /// <param name="id">The kpi group identity.</param>
        /// <returns></returns>
        bool IsUse(int id);
    }
}

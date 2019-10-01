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
        ResultViewModel ValidateData();
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
    }
}

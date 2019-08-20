using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System.Collections.Generic;

namespace EVF.Master.Bll.Interfaces
{
    public interface IPerformanceGroupBll
    {
        /// <summary>
        /// Get Performance Group list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PerformanceGroupViewModel> GetList();
        /// <summary>
        /// Get Detail of performance group.
        /// </summary>
        /// <param name="id">The identity of performance group.</param>
        /// <returns></returns>
        PerformanceGroupViewModel GetDetail(int id);
        /// <summary>
        /// Insert new performance group.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        ResultViewModel Save(PerformanceGroupViewModel model);
        /// <summary>
        /// Update performance group.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(PerformanceGroupViewModel model);
        /// <summary>
        /// Remove performance group.
        /// </summary>
        /// <param name="id">The identity of performance group.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

using EVF.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
{
    public interface IPerformanceBll
    {

        /// <summary>
        /// Get Performance item list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PerformanceViewModel> GetList();
        /// <summary>
        /// Get Detail of performance item.
        /// </summary>
        /// <param name="id">The identity of performance.</param>
        /// <returns></returns>
        PerformanceViewModel GetDetail(int id);
        /// <summary>
        /// Insert new performance item.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        ResultViewModel Save(PerformanceViewModel model);
        /// <summary>
        /// Update performance item.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(PerformanceViewModel model);
        /// <summary>
        /// Remove performance item.
        /// </summary>
        /// <param name="id">The identity of performance.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

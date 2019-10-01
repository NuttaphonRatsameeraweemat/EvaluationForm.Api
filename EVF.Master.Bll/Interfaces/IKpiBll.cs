using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface IKpiBll
    {

        /// <summary>
        /// Get Kpi item list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<KpiViewModel> GetList();
        /// <summary>
        /// Get Detail of Kpi item.
        /// </summary>
        /// <param name="id">The identity of Kpi.</param>
        /// <returns></returns>
        KpiViewModel GetDetail(int id);
        /// <summary>
        /// Insert new Kpi item.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        ResultViewModel Save(KpiViewModel model);
        /// <summary>
        /// Update Kpi item.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(KpiViewModel model);
        /// <summary>
        /// Remove Kpi item.
        /// </summary>
        /// <param name="id">The identity of Kpi.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Set flag is use in kpi.
        /// </summary>
        /// <param name="ids">The kpi identity list.</param>
        /// <param name="isUse">The flag is using.</param>
        void SetIsUse(int[] ids, bool isUse);
        /// <summary>
        /// Validate kpi is using in kpi group or not.
        /// </summary>
        /// <param name="id">The kpi identity.</param>
        /// <returns></returns>
        bool IsUse(int id);
    }
}

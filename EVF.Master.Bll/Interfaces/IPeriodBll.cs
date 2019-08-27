using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface IPeriodBll
    {
        /// <summary>
        /// Get Period list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PeriodViewModel> GetList();
        /// <summary>
        /// Get Detail of period.
        /// </summary>
        /// <param name="id">The identity of period group.</param>
        /// <returns></returns>
        PeriodViewModel GetDetail(int id);
        /// <summary>
        /// Insert new period group.
        /// </summary>
        /// <param name="model">The period information value.</param>
        /// <returns></returns>
        ResultViewModel Save(PeriodViewModel model);
        /// <summary>
        /// Update period group.
        /// </summary>
        /// <param name="model">The period information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(PeriodViewModel model);
        /// <summary>
        /// Remove period group.
        /// </summary>
        /// <param name="id">The identity of period group.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

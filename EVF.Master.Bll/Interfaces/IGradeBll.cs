using EVF.Helper.Models;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Master.Bll.Interfaces
{
    public interface IGradeBll
    {
        /// <summary>
        /// Get Grade list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<GradeViewModel> GetList();
        /// <summary>
        /// Get Detail of grade.
        /// </summary>
        /// <param name="id">The identity of grade group.</param>
        /// <returns></returns>
        GradeViewModel GetDetail(int id);
        /// <summary>
        /// Insert new grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        ResultViewModel Save(GradeViewModel model);
        /// <summary>
        /// Update grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(GradeViewModel model);
        /// <summary>
        /// Remove grade group.
        /// </summary>
        /// <param name="id">The identity of grade group.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
        /// <summary>
        /// Validate information value in grade logic.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        ResultViewModel ValidateData(GradeViewModel model);
    }
}

using EVF.CentralSetting.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.CentralSetting.Bll.Interfaces
{
    public interface IApprovalBll
    {
        /// <summary>
        /// Get Approval list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ApprovalViewModel> GetList();
        /// <summary>
        /// Get Detail of approval.
        /// </summary>
        /// <param name="id">The identity approval.</param>
        /// <returns></returns>
        ApprovalViewModel GetDetail(int id);
        /// <summary>
        /// Insert new approval list.
        /// </summary>
        /// <param name="model">The approval information value.</param>
        /// <returns></returns>
        ResultViewModel Save(ApprovalViewModel model);
        /// <summary>
        /// Update approval.
        /// </summary>
        /// <param name="model">The approval information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(ApprovalViewModel model);
        /// <summary>
        /// Remove approval.
        /// </summary>
        /// <param name="id">The identity approval.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

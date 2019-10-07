using EVF.CentralSetting.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.CentralSetting.Bll.Interfaces
{
    public interface IPurchasingOrgBll
    {
        /// <summary>
        /// Get PurchasingOrg list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PurchasingOrgViewModel> GetList();
        /// <summary>
        /// Get PurchasingOrg list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PurchasingOrgViewModel> GetAllPurchaseOrg();
        /// <summary>
        /// Get Detail of PurchasingOrg.
        /// </summary>
        /// <param name="purOrg">The identity PurchasingOrg.</param>
        /// <returns></returns>
        PurchasingOrgViewModel GetDetail(string purOrg);
        /// <summary>
        /// Insert new PurchasingOrg list.
        /// </summary>
        /// <param name="model">The PurchasingOrg information value.</param>
        /// <returns></returns>
        ResultViewModel Save(PurchasingOrgViewModel model);
        /// <summary>
        /// Update PurchasingOrg.
        /// </summary>
        /// <param name="model">The PurchasingOrg information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(PurchasingOrgViewModel model);
        /// <summary>
        /// Remove PurchasingOrg.
        /// </summary>
        /// <param name="purOrg">The identity PurchasingOrg.</param>
        /// <returns></returns>
        ResultViewModel Delete(string purOrg);
    }
}

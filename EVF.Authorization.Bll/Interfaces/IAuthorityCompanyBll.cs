using EVF.Authorization.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Authorization.Bll.Interfaces
{
    public interface IAuthorityCompanyBll
    {
        /// <summary>
        /// Get AuthorityCompany adUser group list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AuthorityCompanyViewModel> GetList();
        /// <summary>
        /// Get Detail of AuthorityCompany aduser.
        /// </summary>
        /// <param name="adUser">The target user.</param>
        /// <returns></returns>
        AuthorityCompanyViewModel GetDetail(string adUser);
        /// <summary>
        /// Insert new AuthorityCompany.
        /// </summary>
        /// <param name="model">The authority company information value.</param>
        /// <returns></returns>
        ResultViewModel Save(AuthorityCompanyViewModel model);
        /// <summary>
        /// Update AuthorityCompany.
        /// </summary>
        /// <param name="model">The authority company information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(AuthorityCompanyViewModel model);
        /// <summary>
        /// Remove authority company with aduser.
        /// </summary>
        /// <param name="adUser">The target adUsery.</param>
        /// <returns></returns>
        ResultViewModel Delete(string adUser);
    }
}

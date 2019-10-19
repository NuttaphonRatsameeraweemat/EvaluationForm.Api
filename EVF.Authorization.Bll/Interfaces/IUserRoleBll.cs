using EVF.Authorization.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Authorization.Bll.Interfaces
{
    public interface IUserRoleBll
    {
        /// <summary>
        /// Get all user list and roles.
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserRoleViewModel> GetList();
        /// <summary>
        /// Get user detail and roles.
        /// </summary>
        /// <param name="adUser">The identity user.</param>
        /// <returns></returns>
        UserRoleViewModel GetDetail(string adUser);
        /// <summary>
        /// Grant new roles to user.
        /// </summary>
        /// <param name="model">The information user and role.</param>
        /// <returns></returns>
        ResultViewModel Save(UserRoleViewModel model);
        /// <summary>
        /// Update roles and user.
        /// </summary>
        /// <param name="model">The information user and role.</param>
        /// <returns></returns>
        ResultViewModel Edit(UserRoleViewModel model);
        /// <summary>
        /// Remove all user roles.
        /// </summary>
        /// <param name="adUser">THe identity user.</param>
        /// <returns></returns>
        ResultViewModel Delete(UserRoleRequestDeleteModel adUser);
    }
}

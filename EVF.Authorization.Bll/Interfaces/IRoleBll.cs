using EVF.Authorization.Bll.Models;
using EVF.Data.Pocos;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Authorization.Bll.Interfaces
{
    public interface IRoleBll
    {
        /// <summary>
        /// Validate User Role is null or not.
        /// </summary>
        /// <param name="adUser">The identity user.</param>
        /// <returns></returns>
        ResultViewModel ValidateRole(string adUser);
        /// <summary>
        /// Get CompositeRole Item with ad user.
        /// </summary>
        /// <param name="adUser"></param>
        /// <returns></returns>
        IEnumerable<AppCompositeRoleItem> GetCompositeRoleItemByAdUser(string adUser);
        /// <summary>
        /// Get All Menu in system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleItemViewModel> GetAllMenu();
        /// <summary>
        /// Get RoleComposite Detail item.
        /// </summary>
        /// <param name="id">The identity composite role.</param>
        /// <returns></returns>
        RoleViewModel GetDetailCompositeRole(int id);
        /// <summary>
        /// Get All Role in system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleViewModel> GetRoleList();
        /// <summary>
        /// Get Active Role in system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleViewModel> GetActiveRoleList();
        /// <summary>
        /// Create new role and composite role item.
        /// </summary>
        /// <param name="model">The role information for create.</param>
        /// <returns></returns>
        ResultViewModel Save(RoleViewModel model);
        /// <summary>
        /// Edit Composite Role and Role Item.
        /// </summary>
        /// <param name="model">The role information.</param>
        /// <returns></returns>
        ResultViewModel Edit(RoleViewModel model);
        /// <summary>
        /// Delete Composite Role and Role Items.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <returns></returns>
        ResultViewModel Delete(int id);
    }
}

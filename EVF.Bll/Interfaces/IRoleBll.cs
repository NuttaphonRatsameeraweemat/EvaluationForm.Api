using EVF.Data.Pocos;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
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
        IEnumerable<AppCompositeRoleItem> GetCompositeRoleItem(string adUser);
    }
}

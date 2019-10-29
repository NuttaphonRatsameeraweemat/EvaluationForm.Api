using EVF.Authorization.Bll.Models;
using System.Collections.Generic;

namespace EVF.Authorization.Bll.Interfaces
{
    public interface IMenuBll
    {
        /// <summary>
        /// Generate Menu.
        /// </summary>
        /// <param name="adUser">The ad user.</param>
        /// <returns></returns>
        IEnumerable<MenuViewModel> GenerateMenu(string adUser);
        /// <summary>
        /// Redirect menu default first page case.
        /// </summary>
        /// <param name="menus">The menu current user.</param>
        /// <returns></returns>
        string RedirectFirstPage(IEnumerable<MenuViewModel> menus);
    }
}

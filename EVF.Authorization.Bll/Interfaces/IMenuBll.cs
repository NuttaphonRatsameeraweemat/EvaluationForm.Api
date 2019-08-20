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
    }
}

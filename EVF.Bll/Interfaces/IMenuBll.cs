using EVF.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
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

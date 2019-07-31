using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using EVF.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Bll
{
    public class MenuBll : IMenuBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The Role manager provides role functionality.
        /// </summary>
        private readonly IRoleBll _roleBll;
        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public MenuBll(IUnitOfWork unitOfWork, IRoleBll roleBll)
        {
            _unitOfWork = unitOfWork;
            _roleBll = roleBll;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Generate Menu.
        /// </summary>
        /// <param name="adUser">The ad user.</param>
        /// <returns></returns>
        public IEnumerable<MenuViewModel> GenerateMenu(string adUser)
        {
            return this.GetMenu(_roleBll.GetCompositeRoleItemByAdUser(adUser));
        }

        /// <summary>
        /// Get Menu View Model by user role.
        /// </summary>
        /// <param name="rolelist">The user role list.</param>
        /// <returns></returns>
        private IEnumerable<MenuViewModel> GetMenu(IEnumerable<AppCompositeRoleItem> rolelist)
        {
            List<MenuViewModel> result = new List<MenuViewModel>();
            var userMenuList = this.GetSideMenu(rolelist);
            var menuList = userMenuList.Where(a => a.ParentMenuCode.Equals(ConstantValue.RootMenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();
            foreach (var item in menuList)
            {
                result.AddRange(this.GetMenuItem(userMenuList, item, rolelist));
            }
            return result;
        }

        /// <summary>
        /// Get Side Menu Sorting.
        /// </summary>
        /// <param name="rolelist">The role user list can be visible menu.</param>
        /// <returns></returns>
        private IEnumerable<AppMenu> GetSideMenu(IEnumerable<AppCompositeRoleItem> rolelist)
        {
            List<AppMenu> userMenuList = new List<AppMenu>();

            var appMenus = _unitOfWork.GetRepository<AppMenu>().GetCache().ToList();

            //Check menu which user have role to display
            var roleMenuList = appMenus.Where(b => rolelist.Any(a => a.RoleMenu.Equals(b.RoleForDisplay, StringComparison.OrdinalIgnoreCase)) ||
                                                   rolelist.Any(a => a.RoleMenu.Equals(b.RoleForManage, StringComparison.OrdinalIgnoreCase))).ToList();

            //Find parent menu
            foreach (var item in roleMenuList)
            {
                userMenuList.Add(item);
                userMenuList.AddRange(this.GetSideParentMenu(appMenus, item.ParentMenuCode));
            }
            //Remove duplicate menu
            userMenuList = userMenuList.Distinct().ToList();
            return userMenuList;
        }

        /// <summary>
        /// Get Parent Menu Group.
        /// </summary>
        /// <param name="appMenus">The AppMenu Data.</param>
        /// <param name="parentMenuCode">The Parent Menu.</param>
        /// <returns></returns>
        private IEnumerable<AppMenu> GetSideParentMenu(IEnumerable<AppMenu> appMenus, string parentMenuCode)
        {
            List<AppMenu> result = new List<AppMenu>();
            bool findParent = true;
            int countParent = 0;
            int maxParent = 10; //Prevent infinity loop
            while (findParent && !string.IsNullOrEmpty(parentMenuCode) && countParent < maxParent)
            {
                AppMenu parentMenu = appMenus.FirstOrDefault(b => b.MenuCode.Equals(parentMenuCode, StringComparison.OrdinalIgnoreCase));
                if (!parentMenu.MenuCode.Equals(ConstantValue.RootMenuCode, StringComparison.OrdinalIgnoreCase))
                {
                    findParent = true;
                    parentMenuCode = parentMenu.ParentMenuCode;
                    result.Add(parentMenu);
                }
                else
                {
                    findParent = false;
                }
                countParent++;
            }
            return result;
        }

        /// <summary>
        /// Binding Menu to ViewModel.
        /// </summary>
        /// <param name="userMenuList">The Menu current user can be visible.</param>
        /// <param name="menu">The current menu.</param>
        /// <param name="userRoleList">The user role list.</param>
        /// <returns></returns>
        private List<MenuViewModel> GetMenuItem(IEnumerable<AppMenu> userMenuList, AppMenu menu, IEnumerable<AppCompositeRoleItem> userRoleList, string url = null)
        {
            List<MenuViewModel> result = new List<MenuViewModel>();
            url = string.Format("{0}/{1}", url, menu.MenuCode);
            if (menu.MenuType.Equals("GROUP", StringComparison.OrdinalIgnoreCase))
            {
                List<AppMenu> childMenuList = userMenuList.Where(a => a.ParentMenuCode.Equals(menu.MenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();

                MenuViewModel mItem = new MenuViewModel
                {
                    Name = menu.MenuName,
                    DisplayOnly = false,
                    Icon = menu.Icon,
                    Url = url,
                    Parent = new List<MenuViewModel>()
                };

                foreach (var item in childMenuList)
                {
                    mItem.Parent.AddRange(this.GetMenuItem(userMenuList, item, userRoleList, url));
                }

                if (mItem.Parent.Count <= 0)
                {
                    mItem.Parent = null;

                }

                result.Add(mItem);
            }
            else if (menu.MenuType.Equals("ITEM", StringComparison.OrdinalIgnoreCase))
            {
                MenuViewModel sItem = new MenuViewModel
                {
                    Name = menu.MenuName,
                    DisplayOnly = !userRoleList.Any(x => x.RoleMenu == menu.RoleForManage),
                    Icon = menu.Icon,
                    Url = url,
                    Parent = null,
                };
                result.Add(sItem);
            }
            return result;
        }

        #endregion

    }
}

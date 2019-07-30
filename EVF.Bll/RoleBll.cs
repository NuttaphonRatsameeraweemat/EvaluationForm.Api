using AutoMapper;
using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using EVF.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EVF.Bll
{
    public class RoleBll : IRoleBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public RoleBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Validate User Role is null or not.
        /// </summary>
        /// <param name="adUser">The identity user.</param>
        /// <returns></returns>
        public ResultViewModel ValidateRole(string adUser)
        {
            var result = new ResultViewModel();
            var data = _unitOfWork.GetRepository<UserRoles>().GetCache(x => x.AdUser == adUser).FirstOrDefault();
            if (data == null)
            {
                result = UtilityService.InitialResultError(MessageValue.UserRoleIsEmpty);
            }
            return result;
        }

        /// <summary>
        /// Get CompositeRole Item with ad user.
        /// </summary>
        /// <param name="adUser"></param>
        /// <returns></returns>
        public IEnumerable<AppCompositeRoleItem> GetCompositeRoleItemByAdUser(string adUser)
        {
            var result = new List<AppCompositeRoleItem>();
            var user = _unitOfWork.GetRepository<UserRoles>().GetCache(x => x.AdUser == adUser);
            foreach (var item in user)
            {
                var temp = _unitOfWork.GetRepository<AppCompositeRoleItem>().Get(x => x.CompositeRoleId == item.CompositeRoleId);
                result.AddRange(temp);
            }
            result = result.Distinct().ToList();
            return result;
        }

        /// <summary>
        /// Get All Menu in system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleItemViewModel> GetAllMenu()
        {
            var result = new List<RoleItemViewModel>();
            var data = _unitOfWork.GetRepository<AppMenu>().GetCache(x => x.MenuCode != ConstantValue.RootMenuCode);
            var menuList = data.Where(a => a.ParentMenuCode.Equals(ConstantValue.RootMenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();
            foreach (var item in menuList)
            {
                result.AddRange(this.SortingMenu(data, item, new List<AppCompositeRoleItem>()));
            }
            return result;
        }

        /// <summary>
        /// Sorting Menu SubMenu system.
        /// </summary>
        /// <param name="userMenuList"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public IEnumerable<RoleItemViewModel> SortingMenu(IEnumerable<AppMenu> menuList, AppMenu menu, IEnumerable<AppCompositeRoleItem> compositeRoleItems)
        {
            var result = new List<RoleItemViewModel>();
            if (menu.MenuType.Equals(ConstantValue.GroupMenuCode, StringComparison.OrdinalIgnoreCase))
            {
                List<AppMenu> childMenuList = menuList.Where(a => a.ParentMenuCode.Equals(menu.MenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();

                RoleItemViewModel mItem = new RoleItemViewModel
                {
                    MenuCode = menu.MenuCode,
                    MenuName = menu.MenuName
                };
                if (compositeRoleItems.Any(x => x.RoleMenu == menu.RoleForDisplay))
                {
                    mItem.IsDisplay = true;
                }
                if (compositeRoleItems.Any(x => x.RoleMenu == menu.RoleForManage))
                {
                    mItem.IsManage = true;
                }
                foreach (var item in childMenuList)
                {
                    mItem.ParentMenu.AddRange(this.SortingMenu(menuList, item, compositeRoleItems));
                }
                if (mItem.ParentMenu.Count <= 0)
                {
                    mItem.ParentMenu = null;
                }
                result.Add(mItem);
            }
            else if (menu.MenuType.Equals(ConstantValue.ItemMenuCode, StringComparison.OrdinalIgnoreCase))
            {
                RoleItemViewModel sItem = new RoleItemViewModel
                {
                    MenuCode = menu.MenuCode,
                    MenuName = menu.MenuName,
                    ParentMenu = null,
                };
                if (compositeRoleItems.Any(x => x.RoleMenu == menu.RoleForDisplay))
                {
                    sItem.IsDisplay = true;
                }
                if (compositeRoleItems.Any(x => x.RoleMenu == menu.RoleForManage))
                {
                    sItem.IsManage = true;
                }
                result.Add(sItem);
            }
            return result;
        }

        /// <summary>
        /// Get RoleComposite Detail item.
        /// </summary>
        /// <param name="id">The identity composite role.</param>
        /// <returns></returns>
        public RoleViewModel GetDetailCompositeRole(int id)
        {
            var result = _mapper.Map<AppCompositeRole, RoleViewModel>(_unitOfWork.GetRepository<AppCompositeRole>().GetCache(x => x.Id == id).FirstOrDefault());
            var compositeRoleItems = _unitOfWork.GetRepository<AppCompositeRoleItem>().GetCache(x => x.CompositeRoleId == id);

            var data = _unitOfWork.GetRepository<AppMenu>().GetCache(x => x.MenuCode != ConstantValue.RootMenuCode);
            var menuList = data.Where(a => a.ParentMenuCode.Equals(ConstantValue.RootMenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();
            foreach (var item in menuList)
            {
                result.RoleItem.AddRange(this.SortingMenu(data, item, compositeRoleItems));
            }
            return result;
        }

        /// <summary>
        /// Get All Role in system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleViewModel> GetRoleList()
        {
            return _mapper.Map<IEnumerable<AppCompositeRole>, IEnumerable<RoleViewModel>>(
                _unitOfWork.GetRepository<AppCompositeRole>().GetCache());
        }

        /// <summary>
        /// Get Active Role in system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleViewModel> GetActiveRoleList()
        {
            return _mapper.Map<IEnumerable<AppCompositeRole>, IEnumerable<RoleViewModel>>(
                _unitOfWork.GetRepository<AppCompositeRole>().GetCache(x => x.Active.Value));
        }

        /// <summary>
        /// Create new role and composite role item.
        /// </summary>
        /// <param name="model">The role information for create.</param>
        /// <returns></returns>
        public ResultViewModel Save(RoleViewModel model)
        {
            var result = new ResultViewModel();

            using (TransactionScope scope = new TransactionScope())
            {
                var compositeRole = _mapper.Map<RoleViewModel, AppCompositeRole>(model);
                compositeRole.CreateBy = _token.AdUser;
                compositeRole.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<AppCompositeRole>().Add(compositeRole);
                _unitOfWork.Complete();
                _unitOfWork.GetRepository<AppCompositeRoleItem>().AddRange(this.InitialRoleItem(compositeRole.Id, model.RoleItem));
                _unitOfWork.Complete(scope);
            }

            return result;
        }

        /// <summary>
        /// Edit Composite Role and Role Item.
        /// </summary>
        /// <param name="model">The role information.</param>
        /// <returns></returns>
        public ResultViewModel Edit(RoleViewModel model)
        {
            var result = new ResultViewModel();

            using (TransactionScope scope = new TransactionScope())
            {
                var compositeRole = _mapper.Map<RoleViewModel, AppCompositeRole>(model);
                compositeRole.ModifyBy = _token.AdUser;
                compositeRole.ModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<AppCompositeRole>().Update(compositeRole);
                this.UpdateRoleItem(compositeRole.Id, this.InitialRoleItem(compositeRole.Id, model.RoleItem));
                _unitOfWork.Complete(scope);
            }

            return result;
        }

        /// <summary>
        /// Delete Composite Role and Role Items.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var compositeRole = _unitOfWork.GetRepository<AppCompositeRole>().GetCache(x=>x.Id == id).FirstOrDefault();
                var compositeRoleItems = _unitOfWork.GetRepository<AppCompositeRoleItem>().GetCache(x => x.CompositeRoleId == id);
                _unitOfWork.GetRepository<AppCompositeRole>().Remove(compositeRole);
                _unitOfWork.GetRepository<AppCompositeRoleItem>().RemoveRange(compositeRoleItems);
                _unitOfWork.Complete(scope);
            }
            return result;
        }
        
        /// <summary>
        /// Mapping RoleItemViewModel Logic to AppcompositeRoleItem Model.
        /// </summary>
        /// <param name="compoId">The composite role id.</param>
        /// <param name="roleItems">The information role item data.</param>
        /// <returns></returns>
        private IEnumerable<AppCompositeRoleItem> InitialRoleItem(int compoId, IEnumerable<RoleItemViewModel> roleItems)
        {
            var result = new List<AppCompositeRoleItem>();
            foreach (var item in roleItems)
            {
                if (item.IsManage)
                {
                    result.Add(new AppCompositeRoleItem { CompositeRoleId = compoId, RoleMenu = item.RoleManageName });
                }
                if (item.IsDisplay)
                {
                    result.Add(new AppCompositeRoleItem { CompositeRoleId = compoId, RoleMenu = item.RoleDisplayName });
                }
                if (item.ParentMenu.Count > 0)
                {
                    result.AddRange(this.InitialRoleItem(compoId, item.ParentMenu));
                }
            }
            return result;
        }

        /// <summary>
        /// Update Composite Role Item.
        /// </summary>
        /// <param name="compoId">The Role id.</param>
        /// <param name="appRoleItems">The List of composite role update.</param>
        private void UpdateRoleItem(int compoId, IEnumerable<AppCompositeRoleItem> appRoleItems)
        {
            var data = _unitOfWork.GetRepository<AppCompositeRoleItem>().GetCache(x => x.CompositeRoleId == compoId);

            var roleAdd = appRoleItems.Where(x => !data.Any(y => x.RoleMenu == y.RoleMenu));
            var roleDelete = data.Where(x => !appRoleItems.Any(y => x.RoleMenu == y.RoleMenu));

            appRoleItems = appRoleItems.Where(x => data.Any(y => x.RoleMenu == y.RoleMenu));

            _unitOfWork.GetRepository<AppCompositeRoleItem>().UpdateRange(appRoleItems);
            _unitOfWork.GetRepository<AppCompositeRoleItem>().AddRange(roleAdd);
            _unitOfWork.GetRepository<AppCompositeRoleItem>().RemoveRange(roleDelete);
        }

        #endregion

    }
}

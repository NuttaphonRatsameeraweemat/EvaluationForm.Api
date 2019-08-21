using AutoMapper;
using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EVF.Authorization.Bll
{
    public class UserRoleBll : IUserRoleBll
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
        /// Initializes a new instance of the <see cref="UserRoleBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public UserRoleBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Grant new roles to user.
        /// </summary>
        /// <param name="model">The information user and role.</param>
        /// <returns></returns>
        public ResultViewModel Save(UserRoleViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var item in model.RoleList)
                {
                    var data = new UserRoles
                    {
                        AdUser = model.AdUser,
                        CompositeRoleId = item,
                        CreateBy = _token.EmpNo,
                        CreateDate = DateTime.Now
                    };
                    _unitOfWork.GetRepository<UserRoles>().Add(data);
                }
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheUserRoles();
            return result;
        }

        /// <summary>
        /// Update roles and user.
        /// </summary>
        /// <param name="model">The information user and role.</param>
        /// <returns></returns>
        public ResultViewModel Edit(UserRoleViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var userRoles = _unitOfWork.GetRepository<UserRoles>().GetCache(x => x.AdUser == model.AdUser);

                var addRoles = model.RoleList.Where(x => !userRoles.Any(y => y.CompositeRoleId == x));
                var removeRoles = userRoles.Where(x => !model.RoleList.Any(y => y == x.CompositeRoleId));

                foreach (var item in addRoles)
                {
                    var data = new UserRoles
                    {
                        AdUser = model.AdUser,
                        CompositeRoleId = item,
                        CreateBy = _token.EmpNo,
                        CreateDate = DateTime.Now
                    };
                    _unitOfWork.GetRepository<UserRoles>().Add(data);
                }
                _unitOfWork.GetRepository<UserRoles>().RemoveRange(removeRoles);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheUserRoles();
            return result;
        }

        /// <summary>
        /// Remove all user roles.
        /// </summary>
        /// <param name="adUser">THe identity user.</param>
        /// <returns></returns>
        public ResultViewModel Delete(string adUser)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var userRoles = _unitOfWork.GetRepository<UserRoles>().GetCache(x => x.AdUser == adUser);
                _unitOfWork.GetRepository<UserRoles>().RemoveRange(userRoles);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheUserRoles();
            return result;
        }

        /// <summary>
        /// Get all user list and roles.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserRoleViewModel> GetList()
        {
            var result = new List<UserRoleViewModel>();
            result = _mapper.Map<IEnumerable<Hremployee>, IEnumerable<UserRoleViewModel>>(
                _unitOfWork.GetRepository<Hremployee>().GetCache()).ToList();
            var roles = _unitOfWork.GetRepository<UserRoles>().GetCache();
            var orgList = _unitOfWork.GetRepository<Hrorg>().GetCache();
            foreach (var item in result)
            {
                var orgInfo = orgList.FirstOrDefault(x => x.OrgId == item.OrgId);
                item.OrgName = orgInfo?.OrgName;
            }
            foreach (var item in roles)
            {
                var data = result.FirstOrDefault(x => x.AdUser == item.AdUser);
                var roleInfo = _unitOfWork.GetRepository<AppCompositeRole>().GetCache(x => x.Id == item.CompositeRoleId).FirstOrDefault();
                
                data.RoleDisplay += $"{roleInfo?.Name} ";
            }
            return result;
        }

        /// <summary>
        /// Get user detail and roles.
        /// </summary>
        /// <param name="adUser">The identity user.</param>
        /// <returns></returns>
        public UserRoleViewModel GetDetail(string adUser)
        {
            var result = new UserRoleViewModel();
            result = _mapper.Map<Hremployee, UserRoleViewModel>(
                _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == adUser).FirstOrDefault());

            if (result == null)
            {
                throw new ArgumentException($"The {adUser} is null.");
            }

            var roles = _unitOfWork.GetRepository<UserRoles>().GetCache(x => x.AdUser == adUser);

            foreach (var item in roles)
            {
                result.RoleList.Add(item.CompositeRoleId.Value);
            }

            return result;
        }

        /// <summary>
        /// Reload Cache when UserRoles is change.
        /// </summary>
        private void ReloadCacheUserRoles()
        {
            _unitOfWork.GetRepository<UserRoles>().ReCache();
        }

        #endregion

    }
}

using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Bll
{
    public class RoleBll : IRoleBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public RoleBll(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IEnumerable<AppCompositeRoleItem> GetCompositeRoleItem(string adUser)
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

        #endregion

    }
}

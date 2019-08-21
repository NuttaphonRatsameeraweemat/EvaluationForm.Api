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
using System.Text;
using System.Transactions;

namespace EVF.Authorization.Bll
{
    public class AuthorityCompanyBll : IAuthorityCompanyBll
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
        /// Initializes a new instance of the <see cref="AuthorityCompanyBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public AuthorityCompanyBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get AuthorityCompany adUser group list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AuthorityCompanyViewModel> GetList()
        {
            var adUserGroup = _unitOfWork.GetRepository<AuthorityCompany>().GetCache().Select(x => x.AdUser).Distinct().ToList();
            var result = new List<AuthorityCompanyViewModel>();
            foreach (var item in adUserGroup)
            {
                result.Add(new AuthorityCompanyViewModel { AdUser = item });
            }
            return result;
        }

        /// <summary>
        /// Get Detail of AuthorityCompany aduser.
        /// </summary>
        /// <param name="adUser">The target user.</param>
        /// <returns></returns>
        public AuthorityCompanyViewModel GetDetail(string adUser)
        {
            var result = new AuthorityCompanyViewModel { AdUser = adUser };
            var data = _unitOfWork.GetRepository<AuthorityCompany>().GetCache(x => x.AdUser == adUser);
            foreach (var item in data)
            {
                result.ComCode.Add(item.ComCode);
            }
            return result;
        }

        /// <summary>
        /// Insert new AuthorityCompany.
        /// </summary>
        /// <param name="model">The authority company information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(AuthorityCompanyViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = this.InitialAuthorityCompany(model.AdUser, model.ComCode);
                _unitOfWork.GetRepository<AuthorityCompany>().AddRange(data);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheAuthorityCompany();
            return result;
        }

        /// <summary>
        /// Update AuthorityCompany.
        /// </summary>
        /// <param name="model">The authority company information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(AuthorityCompanyViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<AuthorityCompany>().GetCache(x => x.AdUser == model.AdUser);
                var addAuthorityCompany = this.InitialAuthorityCompany(model.AdUser, model.ComCode.Where(x => !data.Any(y => x == y.ComCode)));
                var deleteAuthorityCompany = data.Where(x => !model.ComCode.Any(y => x.ComCode == y)).ToList();
                _unitOfWork.GetRepository<AuthorityCompany>().AddRange(addAuthorityCompany);
                _unitOfWork.GetRepository<AuthorityCompany>().RemoveRange(deleteAuthorityCompany);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheAuthorityCompany();
            return result;
        }

        /// <summary>
        /// Initial AuthorityCompanyViewModel to AuthorityCompany Entity Model.
        /// </summary>
        /// <param name="adUser">The aduser.</param>
        /// <param name="comCode">The company code list.</param>
        /// <returns></returns>
        private IEnumerable<AuthorityCompany> InitialAuthorityCompany(string adUser, IEnumerable<string> comCode)
        {
            var result = new List<AuthorityCompany>();
            foreach (var item in comCode)
            {
                result.Add(new AuthorityCompany
                {
                    AdUser = adUser,
                    ComCode = item,
                    CreateBy = _token.EmpNo,
                    CreateDate = DateTime.Now
                });
            }
            return result;
        }

        /// <summary>
        /// Remove authority company with aduser.
        /// </summary>
        /// <param name="adUser">The target adUsery.</param>
        /// <returns></returns>
        public ResultViewModel Delete(string adUser)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<AuthorityCompany>().GetCache(x => x.AdUser == adUser);
                _unitOfWork.GetRepository<AuthorityCompany>().RemoveRange(data);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheAuthorityCompany();
            return result;
        }

        /// <summary>
        /// Reload Cache when AuthorityCompany is change.
        /// </summary>
        private void ReloadCacheAuthorityCompany()
        {
            _unitOfWork.GetRepository<AuthorityCompany>().ReCache();
        }


        #endregion


    }
}

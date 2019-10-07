using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Hr.Bll.Interfaces;
using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Hr.Bll
{
    public class HrCompanyBll : IHrCompanyBll
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
        /// Initializes a new instance of the <see cref="HrCompanyBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public HrCompanyBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Company list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrCompanyViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Hrcompany>, IEnumerable<HrCompanyViewModel>>(
                   _unitOfWork.GetRepository<Hrcompany>().GetCache(x=>x.ComCode == _token.ComCode));
        }

        /// <summary>
        /// Get All Company list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrCompanyViewModel> GetAllCompany()
        {
            return _mapper.Map<IEnumerable<Hrcompany>, IEnumerable<HrCompanyViewModel>>(
                   _unitOfWork.GetRepository<Hrcompany>().GetCache());
        }
        
        #endregion
        
    }
}

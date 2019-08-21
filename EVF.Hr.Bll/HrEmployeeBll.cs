using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Hr.Bll.Interfaces;
using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Hr.Bll
{
    public class HrEmployeeBll : IHrEmployeeBll
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HrEmployeeBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public HrEmployeeBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Employee list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrEmployeeViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Hremployee>, IEnumerable<HrEmployeeViewModel>>(
                   _unitOfWork.GetRepository<Hremployee>().GetCache());
        }

        /// <summary>
        /// Get Employee list filter by company code.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrEmployeeViewModel> GetListByComCode(string comCode)
        {
            return _mapper.Map<IEnumerable<Hremployee>, IEnumerable<HrEmployeeViewModel>>(
                   _unitOfWork.GetRepository<Hremployee>().GetCache(x=>x.ComCode == comCode));
        }

        /// <summary>
        /// Get Employee list filter by organization id.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrEmployeeViewModel> GetListByOrg(string orgId)
        {
            return _mapper.Map<IEnumerable<Hremployee>, IEnumerable<HrEmployeeViewModel>>(
                   _unitOfWork.GetRepository<Hremployee>().GetCache(x=>x.OrgId == orgId));
        }

        #endregion

    }
}

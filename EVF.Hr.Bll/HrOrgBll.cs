using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Hr.Bll.Interfaces;
using EVF.Hr.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVF.Hr.Bll
{
    public class HrOrgBll : IHrOrgBll
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
        /// Initializes a new instance of the <see cref="HrCompanyBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        public HrOrgBll(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Organization list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrOrgViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Hrorg>, IEnumerable<HrOrgViewModel>>(
                   _unitOfWork.GetRepository<Hrorg>().GetCache());
        }

        /// <summary>
        /// Get Organization list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HrOrgViewModel> GetListByComCode(string comCode)
        {
            var data = _unitOfWork.GetRepository<HrorgRelation>().GetCache();
            var temp = data.Where(x => x.ParentOrgId == comCode);

            var result = new List<HrOrgViewModel>();

            foreach (var item in temp)
            {
                result.Add(new HrOrgViewModel { OrgId = item.ChildOrgId, OrgName = item.ChildOrgName });
                result.AddRange(this.GetOrgBelow(data, item.ChildOrgId));
            }

            return result;
        }

        /// <summary>
        /// Get Org below level.
        /// </summary>
        /// <param name="list">The org relation data.</param>
        /// <param name="orgID">The parent org id.</param>
        /// <returns></returns>
        private IEnumerable<HrOrgViewModel> GetOrgBelow(IEnumerable<HrorgRelation> list, string orgID)
        {
            var result = new List<HrOrgViewModel>();
            var temp = list.Where(x => x.ParentOrgId == orgID).ToList();
            foreach (var item in temp)
            {
                result.Add(new HrOrgViewModel { OrgId = item.ChildOrgId, OrgName = item.ChildOrgName });
                result.AddRange(this.GetOrgBelow(list, item.ChildOrgId));
            }
            return result;
        }

        #endregion

    }
}

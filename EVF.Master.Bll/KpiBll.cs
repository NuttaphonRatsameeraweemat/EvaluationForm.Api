using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EVF.Master.Bll
{
    public class KpiBll : IKpiBll
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
        /// Initializes a new instance of the <see cref="KpiBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public KpiBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Kpi item list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KpiViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Kpi>, IEnumerable<KpiViewModel>>(
                   _unitOfWork.GetRepository<Kpi>().GetCache());
        }

        /// <summary>
        /// Get Detail of Kpi item.
        /// </summary>
        /// <param name="id">The identity of Kpi.</param>
        /// <returns></returns>
        public KpiViewModel GetDetail(int id)
        {
            return _mapper.Map<Kpi, KpiViewModel>(
                   _unitOfWork.GetRepository<Kpi>().GetById(id));
        }

        /// <summary>
        /// Insert new Kpi item.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(KpiViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var Kpi = _mapper.Map<KpiViewModel, Kpi>(model);
                Kpi.CreateBy = _token.EmpNo;
                Kpi.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Kpi>().Add(Kpi);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpi();
            return result;
        }

        /// <summary>
        /// Update Kpi item.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(KpiViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var Kpi = _mapper.Map<KpiViewModel, Kpi>(model);
                Kpi.LastModifyBy = _token.AdUser;
                Kpi.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Kpi>().Update(Kpi);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpi();
            return result;
        }

        /// <summary>
        /// Remove Kpi item.
        /// </summary>
        /// <param name="id">The identity of Kpi.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var Kpi = _unitOfWork.GetRepository<Kpi>().GetById(id);
                _unitOfWork.GetRepository<Kpi>().Remove(Kpi);
                var KpiGroupItem = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiId == id);
                _unitOfWork.GetRepository<KpiGroupItem>().RemoveRange(KpiGroupItem);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpi();
            return result;
        }

        /// <summary>
        /// Reload Cache when Kpi is change.
        /// </summary>
        private void ReloadCacheKpi()
        {
            _unitOfWork.GetRepository<Kpi>().ReCache();
            _unitOfWork.GetRepository<KpiGroupItem>().ReCache();
        }

        #endregion

    }
}

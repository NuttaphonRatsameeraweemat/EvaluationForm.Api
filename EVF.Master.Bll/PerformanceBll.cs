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
    public class PerformanceBll : IPerformanceBll
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
        /// Initializes a new instance of the <see cref="PerformanceBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public PerformanceBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Performance item list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PerformanceViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Performance>, IEnumerable<PerformanceViewModel>>(
                   _unitOfWork.GetRepository<Performance>().GetCache());
        }

        /// <summary>
        /// Get Detail of performance item.
        /// </summary>
        /// <param name="id">The identity of performance.</param>
        /// <returns></returns>
        public PerformanceViewModel GetDetail(int id)
        {
            return _mapper.Map<Performance, PerformanceViewModel>(
                   _unitOfWork.GetRepository<Performance>().GetById(id));
        }

        /// <summary>
        /// Insert new performance item.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(PerformanceViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var performance = _mapper.Map<PerformanceViewModel, Performance>(model);
                performance.CreateBy = _token.AdUser;
                performance.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Performance>().Add(performance);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePerformance();
            return result;
        }

        /// <summary>
        /// Update performance item.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(PerformanceViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var performance = _mapper.Map<PerformanceViewModel, Performance>(model);
                performance.LastModifyBy = _token.AdUser;
                performance.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Performance>().Update(performance);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePerformance();
            return result;
        }

        /// <summary>
        /// Remove performance item.
        /// </summary>
        /// <param name="id">The identity of performance.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var performance = _unitOfWork.GetRepository<Performance>().GetById(id);
                _unitOfWork.GetRepository<Performance>().Remove(performance);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePerformance();
            return result;
        }

        /// <summary>
        /// Reload Cache when Performance is change.
        /// </summary>
        private void ReloadCachePerformance()
        {
            _unitOfWork.GetRepository<Performance>().ReCache();
        }

        #endregion

    }
}

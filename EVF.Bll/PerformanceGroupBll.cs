using AutoMapper;
using EVF.Bll.Interfaces;
using EVF.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Bll
{
    public class PerformanceGroupBll : IPerformanceGroupBll
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
        /// Initializes a new instance of the <see cref="PerformanceGroupBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public PerformanceGroupBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Performance Group list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PerformanceGroupViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<PerformanceGroup>, IEnumerable<PerformanceGroupViewModel>>(
                   _unitOfWork.GetRepository<PerformanceGroup>().GetCache());
        }

        /// <summary>
        /// Get Detail of performance group.
        /// </summary>
        /// <param name="id">The identity of performance group.</param>
        /// <returns></returns>
        public PerformanceGroupViewModel GetDetail(int id)
        {
            var data = _mapper.Map<PerformanceGroup, PerformanceGroupViewModel>(
                   _unitOfWork.GetRepository<PerformanceGroup>().GetById(id));
            data.PerformanceGroupItems = this.GetPerformanceGroupItem(id).ToList();
            return data;
        }

        /// <summary>
        /// Get performance group item.
        /// </summary>
        /// <param name="performanceGroupId">The identity performance group.</param>
        /// <returns></returns>
        private IEnumerable<int> GetPerformanceGroupItem(int performanceGroupId)
        {
            var data = _unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(x => x.PerformanceGroupId == performanceGroupId);
            return data.Select(x => x.PerformanceItemId);
        }

        /// <summary>
        /// Insert new performance group.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(PerformanceGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var performanceGroup = _mapper.Map<PerformanceGroupViewModel, PerformanceGroup>(model);
                performanceGroup.CreateBy = _token.AdUser;
                performanceGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<PerformanceGroup>().Add(performanceGroup);
                _unitOfWork.Complete();
                this.SaveItem(performanceGroup.Id, model.PerformanceGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePerformanceGroup();
            return result;
        }

        /// <summary>
        /// Insert performance group item list.
        /// </summary>
        /// <param name="performanceGroupId">The identity of performance group.</param>
        /// <param name="performanceItems">The identity of performance items.</param>
        private void SaveItem(int performanceGroupId, IEnumerable<int> performanceItems)
        {
            var data = new List<PerformanceGroupItem>();
            foreach (var item in performanceItems)
            {
                data.Add(new PerformanceGroupItem { PerformanceGroupId = performanceGroupId, PerformanceItemId = item });
            }
            _unitOfWork.GetRepository<PerformanceGroupItem>().AddRange(data);
        }

        /// <summary>
        /// Update performance group.
        /// </summary>
        /// <param name="model">The performance information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(PerformanceGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var performanceGroup = _mapper.Map<PerformanceGroupViewModel, PerformanceGroup>(model);
                performanceGroup.LastModifyBy = _token.AdUser;
                performanceGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<PerformanceGroup>().Update(performanceGroup);
                this.EditItem(performanceGroup.Id, model.PerformanceGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePerformanceGroup();
            return result;
        }

        /// <summary>
        /// Update performance group items.
        /// </summary>
        /// <param name="performanceGroupId">The identity of performance group.</param>
        /// <param name="performanceGroupItems">The identity of performance items.</param>
        private void EditItem(int performanceGroupId, IEnumerable<int> performanceGroupItems)
        {
            var data = _unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(x => x.PerformanceItemId == performanceGroupId);

            var performanceItemAdd = performanceGroupItems.Where(x => !data.Any(y => x == y.PerformanceItemId));
            var performanceItemDelete = data.Where(x => !performanceGroupItems.Any(y => x.PerformanceItemId == y));

            this.SaveItem(performanceGroupId, performanceItemAdd);
            this.DeleteItem(performanceItemDelete);
        }

        /// <summary>
        /// Remove performance group.
        /// </summary>
        /// <param name="id">The identity of performance group.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<PerformanceGroup>().Remove(
                    _unitOfWork.GetRepository<PerformanceGroup>().GetById(id));
                this.DeleteItem(_unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(x => x.PerformanceGroupId == id));
            }
            this.ReloadCachePerformanceGroup();
            return result;
        }

        /// <summary>
        /// Remove performance group items.
        /// </summary>
        /// <param name="model">The performance group items.</param>
        private void DeleteItem(IEnumerable<PerformanceGroupItem> model)
        {
            _unitOfWork.GetRepository<PerformanceGroupItem>().RemoveRange(model);
        }

        /// <summary>
        /// Reload Cache when Performance Group is change.
        /// </summary>
        private void ReloadCachePerformanceGroup()
        {
            _unitOfWork.GetRepository<PerformanceGroup>().ReCache();
        }

        #endregion

    }
}

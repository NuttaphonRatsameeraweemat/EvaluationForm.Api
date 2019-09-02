using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace EVF.Master.Bll
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
        private IEnumerable<PerformanceGroupItemViewModel> GetPerformanceGroupItem(int performanceGroupId)
        {
            return _mapper.Map<IEnumerable<PerformanceGroupItem>, IEnumerable<PerformanceGroupItemViewModel>>(
                _unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(x => x.PerformanceGroupId == performanceGroupId,
                                                                           y => y.OrderBy(x => x.Sequence)));
        }

        /// <summary>
        /// Get performance group item for display on criteria.
        /// </summary>
        /// <param name="performanceGroupId">The identity performance group</param>
        /// <returns></returns>
        public IEnumerable<CriteriaItemViewModel> GetPerformanceItemDisplayCriteria(int performanceGroupId)
        {
            var result = new List<CriteriaItemViewModel>();
            var performanceGroupItems = _unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(
                                                                           x => x.PerformanceGroupId == performanceGroupId,
                                                                           y => y.OrderBy(x => x.Sequence));
            var arrayIds = performanceGroupItems.Select(x => x.PerformanceItemId).ToArray();
            var performanceList = _unitOfWork.GetRepository<Performance>().GetCache(x => arrayIds.Contains(x.Id));

            foreach (var item in performanceGroupItems)
            {
                var temp = performanceList.FirstOrDefault(x => x.Id == item.PerformanceItemId);
                if (temp != null)
                {
                    result.Add(new CriteriaItemViewModel
                    {
                        PerformanceId = temp.Id,
                        PerformanceNameTh = temp.PerformanceNameTh,
                        PerformanceNameEn = temp.PerformanceNameEn,
                        Sequence = item.Sequence.Value
                    });
                }
            }

            return result;
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
                performanceGroup.CreateBy = _token.EmpNo;
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
        private void SaveItem(int performanceGroupId, IEnumerable<PerformanceGroupItemViewModel> performanceItems)
        {
            var data = _mapper.Map<IEnumerable<PerformanceGroupItemViewModel>, IEnumerable<PerformanceGroupItem>>(performanceItems);
            data.Select(c => { c.PerformanceGroupId = performanceGroupId; return c; }).ToList();
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
                performanceGroup.LastModifyBy = _token.EmpNo;
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
        private void EditItem(int performanceGroupId, IEnumerable<PerformanceGroupItemViewModel> performanceGroupItems)
        {
            var data = _unitOfWork.GetRepository<PerformanceGroupItem>().GetCache(x => x.PerformanceGroupId == performanceGroupId);

            var performanceItemAdd = performanceGroupItems.Where(x => x.Id == 0);
            var performanceItemDelete = data.Where(x => !performanceGroupItems.Any(y => x.Id == y.Id));

            var performanceItemUpdate = _mapper.Map<IEnumerable<PerformanceGroupItemViewModel>, IEnumerable<PerformanceGroupItem>>(performanceGroupItems);
            performanceItemUpdate = performanceItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(performanceGroupId, performanceItemAdd);
            this.DeleteItem(performanceItemDelete);
            _unitOfWork.GetRepository<PerformanceGroupItem>().UpdateRange(performanceItemUpdate);
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
                _unitOfWork.Complete(scope);
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
            _unitOfWork.GetRepository<PerformanceGroupItem>().ReCache();
        }

        #endregion

    }
}

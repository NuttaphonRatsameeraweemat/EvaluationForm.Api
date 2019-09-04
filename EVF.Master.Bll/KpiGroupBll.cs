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
    public class KpiGroupBll : IKpiGroupBll
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
        /// Initializes a new instance of the <see cref="KpiGroupBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public KpiGroupBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Kpi Group list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KpiGroupViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<KpiGroup>, IEnumerable<KpiGroupViewModel>>(
                   _unitOfWork.GetRepository<KpiGroup>().GetCache());
        }

        /// <summary>
        /// Get Detail of Kpi group.
        /// </summary>
        /// <param name="id">The identity of Kpi group.</param>
        /// <returns></returns>
        public KpiGroupViewModel GetDetail(int id)
        {
            var data = _mapper.Map<KpiGroup, KpiGroupViewModel>(
                   _unitOfWork.GetRepository<KpiGroup>().GetById(id));
            data.KpiGroupItems = this.GetKpiGroupItem(id).ToList();
            return data;
        }

        /// <summary>
        /// Get Kpi group item.
        /// </summary>
        /// <param name="KpiGroupId">The identity Kpi group.</param>
        /// <returns></returns>
        private IEnumerable<KpiGroupItemViewModel> GetKpiGroupItem(int kpiGroupId)
        {
            return _mapper.Map<IEnumerable<KpiGroupItem>, IEnumerable<KpiGroupItemViewModel>>(
                _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == kpiGroupId,
                                                                           y => y.OrderBy(x => x.Sequence)));
        }

        /// <summary>
        /// Get Kpi group item for display on criteria.
        /// </summary>
        /// <param name="KpiGroupId">The identity Kpi group</param>
        /// <returns></returns>
        public IEnumerable<CriteriaItemViewModel> GetKpiItemDisplayCriteria(int KpiGroupId)
        {
            var result = new List<CriteriaItemViewModel>();
            var KpiGroupItems = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(
                                                                           x => x.KpiGroupId == KpiGroupId,
                                                                           y => y.OrderBy(x => x.Sequence));
            var arrayIds = KpiGroupItems.Select(x => x.KpiId).ToArray();
            var KpiList = _unitOfWork.GetRepository<Kpi>().GetCache(x => arrayIds.Contains(x.Id));

            foreach (var item in KpiGroupItems)
            {
                var temp = KpiList.FirstOrDefault(x => x.Id == item.KpiId);
                if (temp != null)
                {
                    result.Add(new CriteriaItemViewModel
                    {
                        KpiId = temp.Id,
                        KpiNameTh = temp.KpiNameTh,
                        KpiNameEn = temp.KpiNameEn,
                        Sequence = item.Sequence.Value
                    });
                }
            }

            return result;
        }

        /// <summary>   
        /// Insert new Kpi group.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(KpiGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var KpiGroup = _mapper.Map<KpiGroupViewModel, KpiGroup>(model);
                KpiGroup.CreateBy = _token.EmpNo;
                KpiGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<KpiGroup>().Add(KpiGroup);
                _unitOfWork.Complete();
                this.SaveItem(KpiGroup.Id, model.KpiGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpiGroup();
            return result;
        }

        /// <summary>
        /// Insert Kpi group item list.
        /// </summary>
        /// <param name="KpiGroupId">The identity of Kpi group.</param>
        /// <param name="KpiItems">The identity of Kpi items.</param>
        private void SaveItem(int KpiGroupId, IEnumerable<KpiGroupItemViewModel> KpiItems)
        {
            var data = _mapper.Map<IEnumerable<KpiGroupItemViewModel>, IEnumerable<KpiGroupItem>>(KpiItems);
            data.Select(c => { c.KpiGroupId = KpiGroupId; return c; }).ToList();
            _unitOfWork.GetRepository<KpiGroupItem>().AddRange(data);
        }

        /// <summary>
        /// Update Kpi group.
        /// </summary>
        /// <param name="model">The Kpi information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(KpiGroupViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var KpiGroup = _mapper.Map<KpiGroupViewModel, KpiGroup>(model);
                KpiGroup.LastModifyBy = _token.EmpNo;
                KpiGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<KpiGroup>().Update(KpiGroup);
                this.EditItem(KpiGroup.Id, model.KpiGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpiGroup();
            return result;
        }

        /// <summary>
        /// Update Kpi group items.
        /// </summary>
        /// <param name="KpiGroupId">The identity of Kpi group.</param>
        /// <param name="KpiGroupItems">The identity of Kpi items.</param>
        private void EditItem(int KpiGroupId, IEnumerable<KpiGroupItemViewModel> KpiGroupItems)
        {
            var data = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == KpiGroupId);

            var KpiItemAdd = KpiGroupItems.Where(x => x.Id == 0);
            var KpiItemDelete = data.Where(x => !KpiGroupItems.Any(y => x.Id == y.Id));

            var KpiItemUpdate = _mapper.Map<IEnumerable<KpiGroupItemViewModel>, IEnumerable<KpiGroupItem>>(KpiGroupItems);
            KpiItemUpdate = KpiItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(KpiGroupId, KpiItemAdd);
            this.DeleteItem(KpiItemDelete);
            _unitOfWork.GetRepository<KpiGroupItem>().UpdateRange(KpiItemUpdate);
        }

        /// <summary>
        /// Remove Kpi group.
        /// </summary>
        /// <param name="id">The identity of Kpi group.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<KpiGroup>().Remove(
                    _unitOfWork.GetRepository<KpiGroup>().GetById(id));
                this.DeleteItem(_unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpiGroup();
            return result;
        }

        /// <summary>
        /// Remove Kpi group items.
        /// </summary>
        /// <param name="model">The Kpi group items.</param>
        private void DeleteItem(IEnumerable<KpiGroupItem> model)
        {
            _unitOfWork.GetRepository<KpiGroupItem>().RemoveRange(model);
        }

        /// <summary>
        /// Reload Cache when Kpi Group is change.
        /// </summary>
        private void ReloadCacheKpiGroup()
        {
            _unitOfWork.GetRepository<KpiGroup>().ReCache();
            _unitOfWork.GetRepository<KpiGroupItem>().ReCache();
        }

        #endregion

    }
}

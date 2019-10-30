using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
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
        /// <summary>
        /// The kpi manager provides kpi functionality.
        /// </summary>
        private readonly IKpiBll _kpi;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="KpiGroupBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public KpiGroupBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IKpiBll kpi)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _kpi = kpi;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Kpi Group list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KpiGroupViewModel> GetList()
        {
            var result = new List<KpiGroupViewModel>();
            var data = _unitOfWork.GetRepository<KpiGroup>().GetCache(x => _token.PurchasingOrg.Contains(x.CreateByPurchaseOrg),
                                                                      x => x.OrderBy(y => y.KpiGroupNameTh).ThenBy(y => y.KpiGroupNameEn));
            var sapFields = _unitOfWork.GetRepository<SapFields>().GetCache();
            foreach (var item in data)
            {
                var kpiGroup = _mapper.Map<KpiGroup, KpiGroupViewModel>(item);
                kpiGroup.SapScoreField = sapFields.FirstOrDefault(x => x.Id == item.SapFieldsId)?.SapFields1;
                result.Add(kpiGroup);
            }
            return result;
        }

        /// <summary>
        /// Get Detail of Kpi group.
        /// </summary>
        /// <param name="id">The identity of Kpi group.</param>
        /// <returns></returns>
        public KpiGroupViewModel GetDetail(int id)
        {
            var result = new KpiGroupViewModel();
            var data = _unitOfWork.GetRepository<KpiGroup>().GetCache(x => x.Id == id).FirstOrDefault();
            result = _mapper.Map<KpiGroup, KpiGroupViewModel>(data);
            result.SapScoreField = _unitOfWork.GetRepository<SapFields>().GetCache(x => x.Id == data.SapFieldsId).FirstOrDefault()?.SapFields1;
            result.KpiGroupItems = this.GetKpiGroupItem(id).ToList();
            return result;
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
        /// <param name="kpiGroupId">The identity Kpi group</param>
        /// <returns></returns>
        public IEnumerable<CriteriaItemViewModel> GetKpiItemDisplayCriteria(int kpiGroupId)
        {
            var result = new List<CriteriaItemViewModel>();
            var kpiGroupItems = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(
                                                                           x => x.KpiGroupId == kpiGroupId,
                                                                           y => y.OrderBy(x => x.Sequence));
            var arrayIds = kpiGroupItems.Select(x => x.KpiId).ToArray();
            var KpiList = _unitOfWork.GetRepository<Kpi>().GetCache(x => arrayIds.Contains(x.Id));

            foreach (var item in kpiGroupItems)
            {
                var temp = KpiList.FirstOrDefault(x => x.Id == item.KpiId);
                if (temp != null)
                {
                    result.Add(new CriteriaItemViewModel
                    {
                        KpiId = temp.Id,
                        KpiNameTh = temp.KpiNameTh,
                        KpiNameEn = temp.KpiNameEn,
                        KpiShortTextTh = temp.KpiShortTextTh,
                        KpiShortTextEn = temp.KpiShortTextEn,
                        Sequence = item.Sequence.Value
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Validate Data before insert and update kpi group.
        /// </summary>
        /// <param name="model">The kpi group information.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(KpiGroupViewModel model)
        {
            var result = new ResultViewModel();
            var sapFields = _unitOfWork.GetRepository<SapFields>().GetCache(x => !x.IsUse).FirstOrDefault();
            if (sapFields == null)
            {
                result = UtilityService.InitialResultError(MessageValue.KpiGroupOverFiftySapFields, (int)System.Net.HttpStatusCode.BadRequest);
            }
            result = this.ValidateDuplicatesItems(model);
            return result;
        }

        /// <summary>
        /// Validate kpi group any duplicate item.
        /// </summary>
        /// <param name="model">The kpi group information.</param>
        /// <returns></returns>
        public ResultViewModel ValidateDuplicatesItems(KpiGroupViewModel model)
        {
            var result = new ResultViewModel();
            var duplicates = model.KpiGroupItems.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
            if (duplicates.Count() > 0)
            {
                result = UtilityService.InitialResultError(MessageValue.KpiGroupItemsDuplicates, (int)System.Net.HttpStatusCode.BadRequest);
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
                var kpiGroup = _mapper.Map<KpiGroupViewModel, KpiGroup>(model);
                kpiGroup.SapFieldsId = this.GetSapFields();
                kpiGroup.CreateBy = _token.EmpNo;
                kpiGroup.CreateDate = DateTime.Now;
                kpiGroup.CreateByPurchaseOrg = _token.PurchasingOrg[0];
                _unitOfWork.GetRepository<KpiGroup>().Add(kpiGroup);
                _unitOfWork.Complete();
                this.SaveItem(kpiGroup.Id, model.KpiGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpiGroup();
            return result;
        }

        /// <summary>
        /// Insert Kpi group item list.
        /// </summary>
        /// <param name="kpiGroupId">The identity of Kpi group.</param>
        /// <param name="kpiItems">The identity of Kpi items.</param>
        private void SaveItem(int kpiGroupId, IEnumerable<KpiGroupItemViewModel> kpiItems)
        {
            var data = _mapper.Map<IEnumerable<KpiGroupItemViewModel>, IEnumerable<KpiGroupItem>>(kpiItems);
            data.Select(c => { c.KpiGroupId = kpiGroupId; return c; }).ToList();
            _unitOfWork.GetRepository<KpiGroupItem>().AddRange(data);
            this.UpdateKpiUsingFlag(kpiGroupId, kpiItems.Select(x => x.KpiId).ToArray(), true);
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
                var kpiGroup = _unitOfWork.GetRepository<KpiGroup>().GetCache(x => x.Id == model.Id).FirstOrDefault();
                kpiGroup.KpiGroupNameTh = model.KpiGroupNameTh;
                kpiGroup.KpiGroupNameEn = model.KpiGroupNameEn;
                kpiGroup.KpiGroupShortTextTh = model.KpiGroupShortTextTh;
                kpiGroup.KpiGroupShortTextEn = model.KpiGroupShortTextEn;
                kpiGroup.LastModifyBy = _token.EmpNo;
                kpiGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<KpiGroup>().Update(kpiGroup);
                this.EditItem(kpiGroup.Id, model.KpiGroupItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpiGroup();
            return result;
        }

        /// <summary>
        /// Update Kpi group items.
        /// </summary>
        /// <param name="kpiGroupId">The identity of Kpi group.</param>
        /// <param name="kpiGroupItems">The identity of Kpi items.</param>
        private void EditItem(int kpiGroupId, IEnumerable<KpiGroupItemViewModel> kpiGroupItems)
        {
            kpiGroupItems.Select(c => { c.KpiGroupId = kpiGroupId; return c; }).ToList();
            var data = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == kpiGroupId);

            var kpiItemAdd = kpiGroupItems.Where(x => x.Id == 0);
            var kpiItemDelete = data.Where(x => !kpiGroupItems.Any(y => x.Id == y.Id));

            var kpiItemUpdate = _mapper.Map<IEnumerable<KpiGroupItemViewModel>, IEnumerable<KpiGroupItem>>(kpiGroupItems);
            kpiItemUpdate = kpiItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(kpiGroupId, kpiItemAdd);
            this.DeleteItem(kpiItemDelete);

            var kpiIds = data.Where(x => kpiItemUpdate.Any(y => x.Id == y.Id)).Select(x => x.KpiId.Value).ToArray();

            _kpi.SetIsUse(kpiItemUpdate.Select(x => x.KpiId.Value).ToArray(), true);
            this.UpdateKpiUsingFlag(kpiGroupId, kpiIds, false);
            _unitOfWork.GetRepository<KpiGroupItem>().UpdateRange(kpiItemUpdate);
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
                var data = _unitOfWork.GetRepository<KpiGroup>().GetById(id);
                this.SetSapFields(data.SapFieldsId.Value, false);
                _unitOfWork.GetRepository<KpiGroup>().Remove(data);
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
            this.UpdateKpiUsingFlag(model.FirstOrDefault() != null ? model.FirstOrDefault().KpiGroupId.Value : 0,
                                 model.Select(x => x.KpiId.Value).ToArray(), false);
        }

        /// <summary>
        /// Update kpi flag is use.
        /// </summary>
        /// <param name="kpiGroupId">The kpi group identity.</param>
        /// <param name="ids">The kpi identity list.</param>
        /// <param name="isUse">Flag is using</param>
        private void UpdateKpiUsingFlag(int kpiGroupId, int[] ids, bool isUse)
        {
            var kpiIds = new List<int>();
            if (!isUse)
            {
                foreach (var item in ids)
                {
                    var temp = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId != kpiGroupId && x.KpiId == item);
                    if (temp.Count() <= 0)
                    {
                        kpiIds.Add(item);
                    }
                }
            }
            else kpiIds.AddRange(ids);
            _kpi.SetIsUse(kpiIds.ToArray(), isUse);
        }

        /// <summary>
        /// Validate kpi group is using in criteria or not.
        /// </summary>
        /// <param name="id">The kpi group identity.</param>
        /// <returns></returns>
        public bool IsUse(int id)
        {
            var kpiGroup = _unitOfWork.GetRepository<KpiGroup>().GetCache(x => x.Id == id).FirstOrDefault();
            return kpiGroup.IsUse.Value;
        }

        /// <summary>
        /// Set flag is use in kpi group.
        /// </summary>
        /// <param name="ids">The kpi group identity list.</param>
        /// <param name="isUse">The flag is using.</param>
        public void SetIsUse(int[] ids, bool isUse)
        {
            var data = _unitOfWork.GetRepository<KpiGroup>().GetCache(x => ids.Contains(x.Id));
            data.Select(c => { c.IsUse = isUse; return c; }).ToList();
            _unitOfWork.GetRepository<KpiGroup>().UpdateRange(data);
        }

        /// <summary>
        /// Get Sap fields id.
        /// </summary>
        /// <returns></returns>
        private int GetSapFields()
        {
            int result = 0;
            var sapFields = _unitOfWork.GetRepository<SapFields>().GetCache(x => !x.IsUse, x => x.OrderBy(y => y.Id)).FirstOrDefault();
            if (sapFields != null)
            {
                result = sapFields.Id;
                this.SetSapFields(sapFields.Id, true);
            }
            return result;
        }

        /// <summary>
        /// Set sap fields using is true or false.
        /// </summary>
        /// <param name="sapFieldsId">The identity sap fields.</param>
        /// <param name="isUse">Flag is using sap fields.</param>
        private void SetSapFields(int sapFieldsId, bool isUse)
        {
            var sapFieldData = _unitOfWork.GetRepository<SapFields>().GetCache(x => x.Id == sapFieldsId).FirstOrDefault();
            sapFieldData.IsUse = isUse;
            _unitOfWork.GetRepository<SapFields>().Update(sapFieldData);
        }

        /// <summary>
        /// Reload Cache when Kpi Group is change.
        /// </summary>
        private void ReloadCacheKpiGroup()
        {
            _unitOfWork.GetRepository<KpiGroup>().ReCache();
            _unitOfWork.GetRepository<KpiGroupItem>().ReCache();
            _unitOfWork.GetRepository<Kpi>().ReCache();
            _unitOfWork.GetRepository<SapFields>().ReCache();
        }

        #endregion

    }
}

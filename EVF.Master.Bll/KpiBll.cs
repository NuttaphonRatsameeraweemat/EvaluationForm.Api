﻿using AutoMapper;
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
                   _unitOfWork.GetRepository<Kpi>().GetCache(x => _token.PurchasingOrg.Contains(x.CreateByPurchaseOrg),
                                                             x => x.OrderBy(y => y.KpiNameTh).ThenBy(y => y.KpiNameEn)));
        }

        /// <summary>
        /// Get Detail of Kpi item.
        /// </summary>
        /// <param name="id">The identity of Kpi.</param>
        /// <returns></returns>
        public KpiViewModel GetDetail(int id)
        {
            return _mapper.Map<Kpi, KpiViewModel>(
                   _unitOfWork.GetRepository<Kpi>().GetCache(x => x.Id == id).FirstOrDefault());
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
                var kpi = _mapper.Map<KpiViewModel, Kpi>(model);
                kpi.CreateBy = _token.EmpNo;
                kpi.CreateDate = DateTime.Now;
                kpi.CreateByPurchaseOrg = _token.PurchasingOrg[0];
                _unitOfWork.GetRepository<Kpi>().Add(kpi);
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
                var kpi = _unitOfWork.GetRepository<Kpi>().GetCache(x => x.Id == model.Id).FirstOrDefault();
                kpi.KpiNameTh = model.KpiNameTh;
                kpi.KpiNameEn = model.KpiNameEn;
                kpi.KpiShortTextTh = model.KpiShortTextTh;
                kpi.KpiShortTextEn = model.KpiShortTextEn;
                kpi.LastModifyBy = _token.EmpNo;
                kpi.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Kpi>().Update(kpi);
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
                var kpi = _unitOfWork.GetRepository<Kpi>().GetCache(x=>x.Id == id).FirstOrDefault();
                _unitOfWork.GetRepository<Kpi>().Remove(kpi);
                var kpiGroupItem = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiId == id);
                _unitOfWork.GetRepository<KpiGroupItem>().RemoveRange(kpiGroupItem);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheKpi();
            return result;
        }

        /// <summary>
        /// Validate kpi is using in kpi group or not.
        /// </summary>
        /// <param name="id">The kpi identity.</param>
        /// <returns></returns>
        public bool IsUse(int id)
        {
            var kpi = _unitOfWork.GetRepository<Kpi>().GetCache(x => x.Id == id).FirstOrDefault();
            return kpi.IsUse.Value;
        }

        /// <summary>
        /// Set flag is use in kpi.
        /// </summary>
        /// <param name="ids">The kpi identity list.</param>
        /// <param name="isUse">The flag is using.</param>
        public void SetIsUse(int[] ids, bool isUse)
        {
            var data = _unitOfWork.GetRepository<Kpi>().GetCache(x => ids.Contains(x.Id));
            data.Select(c => { c.IsUse = isUse; return c; }).ToList();
            _unitOfWork.GetRepository<Kpi>().UpdateRange(data);
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

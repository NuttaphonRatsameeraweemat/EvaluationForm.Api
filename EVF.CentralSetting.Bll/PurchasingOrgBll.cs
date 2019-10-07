using AutoMapper;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.CentralSetting.Bll
{
    public class PurchasingOrgBll : IPurchasingOrgBll
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
        /// Initializes a new instance of the <see cref="PurchasingOrgBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public PurchasingOrgBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get PurchasingOrg list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchasingOrgViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<PurchaseOrg>, IEnumerable<PurchasingOrgViewModel>>(
                                        _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x=> _token.PurchasingOrg.Contains(x.PurchaseOrg1)));
        }

        /// <summary>
        /// Get PurchasingOrg list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PurchasingOrgViewModel> GetAllPurchaseOrg()
        {
            return _mapper.Map<IEnumerable<PurchaseOrg>, IEnumerable<PurchasingOrgViewModel>>(_unitOfWork.GetRepository<PurchaseOrg>().GetCache());
        }

        /// <summary>
        /// Get Detail of PurchasingOrg.
        /// </summary>
        /// <param name="purOrg">The identity PurchasingOrg.</param>
        /// <returns></returns>
        public PurchasingOrgViewModel GetDetail(string purOrg)
        {
            var result = _mapper.Map<PurchaseOrg, PurchasingOrgViewModel>(_unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == purOrg).FirstOrDefault());
            result.PurchasingItems.AddRange(_mapper.Map<IEnumerable<PurchaseOrgItem>, IEnumerable<PurchasingOrgItemViewModel>>(
                _unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.PuchaseOrg == purOrg, y => y.OrderBy(x => x.Type))));
            return result;
        }

        /// <summary>
        /// Insert new PurchasingOrg list.
        /// </summary>
        /// <param name="model">The PurchasingOrg information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(PurchasingOrgViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var purchasingOrg = _mapper.Map<PurchasingOrgViewModel, PurchaseOrg>(model);
                purchasingOrg.CreateBy = _token.EmpNo;
                purchasingOrg.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<PurchaseOrg>().Add(purchasingOrg);
                this.SaveItem(purchasingOrg.PurchaseOrg1, _mapper.Map<IEnumerable<PurchasingOrgItemViewModel>, IEnumerable<PurchaseOrgItem>>(model.PurchasingItems));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePurchasingOrg();
            return result;
        }

        /// <summary>
        /// Insert new PurchasingOrg item.
        /// </summary>
        /// <param name="purchasingOrg">The PurchasingOrg id.</param>
        /// <param name="purchasingOrgItems">The PurchasingOrg items.</param>
        private void SaveItem(string purchasingOrg, IEnumerable<PurchaseOrgItem> purchasingOrgItems)
        {
            purchasingOrgItems.Select(c => { c.PuchaseOrg = purchasingOrg; return c; }).ToList();
            _unitOfWork.GetRepository<PurchaseOrgItem>().AddRange(purchasingOrgItems);
        }

        /// <summary>
        /// Update PurchasingOrg.
        /// </summary>
        /// <param name="model">The PurchasingOrg information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(PurchasingOrgViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == model.PurchaseOrg1).FirstOrDefault();
                data.PurchaseOrg1 = model.PurchaseOrg1;
                data.PurchaseName = model.PurchaseName;
                data.LastModifyBy = _token.EmpNo;
                data.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<PurchaseOrg>().Update(data);
                this.EditItem(data.PurchaseOrg1, model.PurchasingItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePurchasingOrg();
            return result;
        }

        /// <summary>
        /// Edit PurchasingOrg items.
        /// </summary>
        /// <param name="purchasingOrg">The identity PurchasingOrg.</param>
        /// <param name="purchasingOrgList">The PurchasingOrg items.</param>
        private void EditItem(string purchasingOrg, IEnumerable<PurchasingOrgItemViewModel> purchasingOrgList)
        {
            this.DeleteItem(_unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.PuchaseOrg == purchasingOrg));
            this.SaveItem(purchasingOrg, _mapper.Map<IEnumerable<PurchasingOrgItemViewModel>, IEnumerable<PurchaseOrgItem>>(purchasingOrgList));
        }

        /// <summary>
        /// Remove PurchasingOrg.
        /// </summary>
        /// <param name="purOrg">The identity PurchasingOrg.</param>
        /// <returns></returns>
        public ResultViewModel Delete(string purOrg)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var PurchasingOrg = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == purOrg);
                this.DeleteItem(_unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.PuchaseOrg == purOrg));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePurchasingOrg();
            return result;
        }

        /// <summary>
        /// Remove PurchasingOrg item list.
        /// </summary>
        /// <param name="purchasingOrgItems">The PurchasingOrg items.</param>
        private void DeleteItem(IEnumerable<PurchaseOrgItem> purchasingOrgItems)
        {
            _unitOfWork.GetRepository<PurchaseOrgItem>().RemoveRange(purchasingOrgItems);
        }

        /// <summary>
        /// Reload Cache when PurchasingOrg is change.
        /// </summary>
        private void ReloadCachePurchasingOrg()
        {
            _unitOfWork.GetRepository<PurchaseOrg>().ReCache();
            _unitOfWork.GetRepository<PurchaseOrgItem>().ReCache();
        }

        #endregion

    }
}

using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Interfaces;
using EVF.Vendor.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Vendor.Bll
{
    public class VendorTransectionBll : IVendorTransectionBll
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
        /// Initializes a new instance of the <see cref="VendorTransectionBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorTransectionBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionViewModel> GetList()
        {
            return this.InitialVendorName(_mapper.Map<IEnumerable<VendorTransection>, IEnumerable<VendorTransectionViewModel>>(
                _unitOfWork.GetRepository<VendorTransection>().Get()));
        }

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionViewModel> GetListSearch(VendorTransectionSearchViewModel model)
        {
            return this.InitialVendorName(this.SearchTransection(model));
        }

        /// <summary>
        /// Search by criteria model search transection.
        /// </summary>
        /// <param name="model">The criteria search vendor transection model.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransectionViewModel> SearchTransection(VendorTransectionSearchViewModel model)
        {
            return _mapper.Map<IEnumerable<VendorTransection>, IEnumerable<VendorTransectionViewModel>>(
                    this.GetTransections(model.PeriodItemId,new string[] { model.PurGroup }));
        }

        /// <summary>
        /// Initial vendor transection view model.
        /// </summary>
        /// <param name="result">The vendor transection infomation.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransectionViewModel> InitialVendorName(IEnumerable<VendorTransectionViewModel> result)
        {
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            foreach (var item in result)
            {
                item.VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.Vendor)?.VendorName;
            }
            return result;
        }

        /// <summary>
        /// Get Vendor transection.
        /// </summary>
        /// <param name="model">The criteria search vendor transection model.</param>
        /// <returns></returns>
        public IEnumerable<VendorTransection> GetTransections(int periodItemid, string[] purGroup)
        {
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemid).FirstOrDefault();
            var startReceipt = periodItem.StartEvaDate.Value.AddMonths(-6);
            return _unitOfWork.GetRepository<VendorTransection>().Get(x => purGroup.Contains(x.PurgropCode) &&
                                                                           _token.PurchasingOrg.Contains(x.PurorgCode) &&
                                                                                   x.ReceiptDate.Value.Date <= periodItem.StartEvaDate.Value.Date &&
                                                                                   x.ReceiptDate.Value.Date >= startReceipt.Date);
        }

        /// <summary>
        /// Get Transection list by condition.
        /// </summary>
        /// <param name="periodItemid">The period item identity.</param>
        /// <param name="purGroup">The purGroup code.</param>
        /// <param name="comCode">The company code.</param>
        /// <param name="purOrg">The purchase org.</param>
        /// <returns></returns>
        public IEnumerable<VendorTransection> GetTransections(int periodItemid, string[] purGroup, string comCode, string purOrg)
        {
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemid).FirstOrDefault();
            var startReceipt = periodItem.StartEvaDate.Value.AddMonths(-6);
            return _unitOfWork.GetRepository<VendorTransection>().Get(x => purGroup.Contains(x.PurgropCode) &&
                                                                                   x.CompanyCode == comCode &&
                                                                                   x.PurorgCode == purOrg &&
                                                                                   x.ReceiptDate.Value.Date <= periodItem.StartEvaDate.Value.Date &&
                                                                                   x.ReceiptDate.Value.Date >= startReceipt.Date);
        }

        /// <summary>
        /// Get Detail of VendorTransection.
        /// </summary>
        /// <param name="id">The identity VendorTranection.</param>
        /// <returns></returns>
        public VendorTransectionViewModel GetDetail(int id)
        {
            var result = _mapper.Map<VendorTransection, VendorTransectionViewModel>(
                _unitOfWork.GetRepository<VendorTransection>().Get(x => x.Id == id).FirstOrDefault());
            result.VendorName = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == result.Vendor).FirstOrDefault().VendorName;
            return result;
        }

        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel MarkWeightingKey(VendorTransectionRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<VendorTransection>().GetById(model.Id);
                data.MarkWeightingKey = model.WeightingKey;
                _unitOfWork.GetRepository<VendorTransection>().Update(data);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        #endregion

    }
}

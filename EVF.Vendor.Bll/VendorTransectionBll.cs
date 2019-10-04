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
            return _mapper.Map<IEnumerable<VendorTransection>, IEnumerable<VendorTransectionViewModel>>(
                _unitOfWork.GetRepository<VendorTransection>().GetCache());
        }

        /// <summary>
        /// Get VendorTransection list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorTransectionViewModel> GetListSearch(VendorTransectionSearchViewModel model)
        {
            return this.SearchTransection(model);
        }

        /// <summary>
        /// Search by criteria model search transection.
        /// </summary>
        /// <param name="model">The criteria search vendor transection model.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransectionViewModel> SearchTransection(VendorTransectionSearchViewModel model)
        {
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == model.PeriodItemId).FirstOrDefault();
            var startReceipt = periodItem.StartEvaDate.Value.AddMonths(-6);
            var data = _unitOfWork.GetRepository<VendorTransection>().GetCache(x => x.PurgropCode == model.PurGroup &&
                                                                                    x.ReceiptDate.Value.Date <= periodItem.StartEvaDate.Value.Date &&
                                                                                    x.ReceiptDate.Value.Date >= startReceipt.Date);
            return _mapper.Map<IEnumerable<VendorTransection>, IEnumerable<VendorTransectionViewModel>>(data);
        }

        /// <summary>
        /// Get Detail of VendorTransection.
        /// </summary>
        /// <param name="id">The identity VendorTranection.</param>
        /// <returns></returns>
        public VendorTransectionViewModel GetDetail(int id)
        {
            var result = _mapper.Map<VendorTransection, VendorTransectionViewModel>(
                _unitOfWork.GetRepository<VendorTransection>().GetCache(x => x.Id == id).FirstOrDefault());
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

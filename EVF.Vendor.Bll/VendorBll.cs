using AutoMapper;
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
    public class VendorBll : IVendorBll
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
        /// Initializes a new instance of the <see cref="VendorBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public VendorBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Vendor list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VendorViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Data.Pocos.Vendor>, IEnumerable<VendorViewModel>>(
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache());
        }

        /// <summary>
        /// Get Detail of Vendor.
        /// </summary>
        /// <param name="vendorNo">The identity Vendor.</param>
        /// <returns></returns>
        public VendorViewModel GetDetail(string vendorNo)
        {
            var result = _mapper.Map<Data.Pocos.Vendor, VendorViewModel>(
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == vendorNo).FirstOrDefault());
            return result;
        }

        /// <summary>
        /// Update Vendor.
        /// </summary>
        /// <param name="model">The Vendor information value.</param>
        /// <returns></returns>
        public ResultViewModel UpdateVendorContact(VendorRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == model.VendorNo).FirstOrDefault();
                data.Email = model.Email;
                data.TelephoneNo = model.TelephoneNo;
                _unitOfWork.GetRepository<Data.Pocos.Vendor>().Update(data);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheVendor();
            return result;
        }

        /// <summary>
        /// Reload Cache when Vendor is change.
        /// </summary>
        private void ReloadCacheVendor()
        {
            _unitOfWork.GetRepository<Data.Pocos.Vendor>().ReCache();
        }

        #endregion

    }
}

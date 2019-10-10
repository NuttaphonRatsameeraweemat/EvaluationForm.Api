using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Tranfer.Service.Bll.Interfaces;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static EVF.Tranfer.Service.Data.UnitOfWorkControl;

namespace EVF.Tranfer.Service.Bll
{
    public class VendorTranferBll : IVendorTranferBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in spe database.
        /// </summary>
        private readonly IUnitOfWork _evfUnitOfWork;
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in data mart.
        /// </summary>
        private readonly IUnitOfWork _dmUnitOfWork;
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILoggerManager _logger;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorTransectionTranferBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work control.</param>
        /// <param name="mapper">The auto mapper.</param>
        public VendorTranferBll(ServiceResolver unitOfWork, IMapper mapper, IConfigSetting config, ILoggerManager logger)
        {
            _dmUnitOfWork = unitOfWork("DM");
            _evfUnitOfWork = unitOfWork("EVF");
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Tranfer data vendor master in zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel TranferVendorData()
        {
            var result = new ResultViewModel();
            var vendorMaster = _dmUnitOfWork.GetRepository<ZNCR_03>().Get();
            var allVendorNo = _evfUnitOfWork.GetRepository<Vendor>().Get().Select(x => x.VendorNo).ToArray();
            vendorMaster = vendorMaster.Where(x => !allVendorNo.Contains(x.VendorNo));
            var data = _mapper.Map<IEnumerable<ZNCR_03>, IEnumerable<Vendor>>(vendorMaster);
            _evfUnitOfWork.GetRepository<Vendor>().AddRange(data);
            _evfUnitOfWork.Complete();
            System.Threading.Tasks.Task.Run(() =>
            {
                this.LogTranferData(data);
            });
            return result;
        }

        /// <summary>
        /// Update data vendor master from zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel UpdateVendorDataFromZncr03(string vendorNo)
        {
            var result = new ResultViewModel();
            var vendor = _evfUnitOfWork.GetRepository<Vendor>().Get(x => x.VendorNo == vendorNo).FirstOrDefault();
            var vendorMaster = _dmUnitOfWork.GetRepository<ZNCR_03>().Get(x => x.VendorNo == vendorNo).FirstOrDefault();
            //Update Fields value.
            vendor.Address = vendorMaster.Address;
            vendor.CountyDesc = vendorMaster.CountyDesc;
            vendor.CountyKey = vendorMaster.CountyKey;
            vendor.CreateBy = vendorMaster.CreateBy;
            vendor.CreateDate = vendorMaster.CreateDate;
            vendor.CreateTime = vendorMaster.CreateTime;
            vendor.CustNo = vendorMaster.CustNo;
            vendor.DelFlag = vendorMaster.DelFlag;
            vendor.FaxExt = vendorMaster.FaxExt;
            vendor.FaxNo = vendorMaster.FaxNo;
            vendor.LanguageKey = vendorMaster.LanguageKey;
            vendor.NoDel = vendorMaster.NoDel;
            vendor.OneTimeInd = vendorMaster.OneTimeInd;
            vendor.PostBlock = vendorMaster.PostBlock;
            vendor.PurBlock = vendorMaster.PurBlock;
            vendor.SearchTerm1 = vendorMaster.SearchTerm1;
            vendor.TaxNo3 = vendorMaster.TaxNo3;
            vendor.TimeZone = vendorMaster.TimeZone;
            vendor.TrZone = vendorMaster.TrZone;
            vendor.TrZoneDesc = vendorMaster.TrZoneDesc;
            vendor.VatRegNo = vendorMaster.VatRegNo;
            vendor.VendAccGrpName = vendorMaster.VendAccGrpName;
            vendor.VendorAccGrp = vendorMaster.VendorAccGrp;
            vendor.VendorName = vendorMaster.VendorName;
            _evfUnitOfWork.GetRepository<Vendor>().Update(vendor);
            _evfUnitOfWork.Complete();
            System.Threading.Tasks.Task.Run(() =>
            {
                this.LogTranferData(vendor);
            });
            return result;
        }

        /// <summary>
        /// Insert new data vendor master from zncr 03 to spe database.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel AddNewVendorDataFromZncr03(string vendorNo)
        {
            var result = new ResultViewModel();
            var vendorMaster = _dmUnitOfWork.GetRepository<ZNCR_03>().Get(x => x.VendorNo == vendorNo).FirstOrDefault();
            var data = _mapper.Map<ZNCR_03, Vendor>(vendorMaster);
            _evfUnitOfWork.GetRepository<Vendor>().Add(data);
            _evfUnitOfWork.Complete();
            System.Threading.Tasks.Task.Run(() =>
            {
                this.LogTranferData(data);
            });
            return result;
        }

        /// <summary>
        /// Logging debug when tranfer is finish. 
        /// </summary>
        /// <param name="evfData"></param>
        private void LogTranferData(IEnumerable<Vendor> vendorTransections)
        {
            foreach (var item in vendorTransections)
            {
                _logger.LogDebug($"VendorNo : {item.VendorNo}, VendorName : {item.VendorName}");
            }
        }

        /// <summary>
        /// Logging debug when tranfer is finish. 
        /// </summary>
        /// <param name="evfData"></param>
        private void LogTranferData(Vendor vendorTransections)
        {
            _logger.LogDebug($"VendorNo : {vendorTransections.VendorNo}, VendorName : {vendorTransections.VendorName}");
        }

        /// <summary>
        /// Try to connect zncr 03 table.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ZNCR_03> TryToConnect()
        {
            return _dmUnitOfWork.GetRepository<ZNCR_03>().Get();
        }

        #endregion

    }
}

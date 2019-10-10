using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Tranfer.Service.Bll.Interfaces;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using static EVF.Tranfer.Service.Data.UnitOfWorkControl;

namespace EVF.Tranfer.Service.Bll
{
    public class VendorTransectionTranferBll : IVendorTransectionTranferBll
    {

        #region [Fields]
        
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in spe database.
        /// </summary>
        private readonly IUnitOfWork _evfUnitOfWork;
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in brb util database.
        /// </summary>
        private readonly IUnitOfWork _brbUnitOfWork;
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
        public VendorTransectionTranferBll(ServiceResolver unitOfWork, IMapper mapper, IConfigSetting config, ILoggerManager logger)
        {
            _brbUnitOfWork = unitOfWork("BRB");
            _evfUnitOfWork = unitOfWork("EVF");
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Tranfer data vendor transection in brb util to spe database.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel TranferVendorTransection()
        {
            var result = new ResultViewModel();
            var vendorTransections = _brbUnitOfWork.GetRepository<SPE_TRANSAC_PO_QA>().Get();
            var data = _mapper.Map<IEnumerable<SPE_TRANSAC_PO_QA>, IEnumerable<VendorTransection>>(vendorTransections);
            _evfUnitOfWork.GetRepository<VendorTransection>().AddRange(data);
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
        private void LogTranferData(IEnumerable<VendorTransection> vendorTransections)
        {
            foreach (var item in vendorTransections)
            {
                _logger.LogDebug($"ReceiptDate : {item.ReceiptDate}, Vendor : {item.Vendor}, MaterialCode : {item.MaterialCode}, PurorgCode : {item.PurorgCode}, PurgropCode : {item.PurgropCode}");
            }
        }

        /// <summary>
        /// Try to connect brb util db.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SPE_TRANSAC_PO_QA> TryToConnect()
        {
            return _brbUnitOfWork.GetRepository<SPE_TRANSAC_PO_QA>().Get();
        }

        #endregion

    }
}

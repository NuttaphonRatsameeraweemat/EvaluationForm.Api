using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Tranfer.Service.Bll.Interfaces;
using EVF.Tranfer.Service.Data.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
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
            var vendorTransaction = _evfUnitOfWork.GetRepository<VendorTransaction>().Get().Select(x => x.DataUpdateDate).Distinct();
            var vendorTransactionsMaster = _brbUnitOfWork.GetRepository<SPE_TRANSAC_PO_QA>().Get(x => !vendorTransaction.Contains(x.DataUpdateDate)).ToList();
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 30, 0)))
            {
                var transactionUpdate = this.UpdateTransaction(vendorTransactionsMaster);
                var data = _mapper.Map<IEnumerable<SPE_TRANSAC_PO_QA>, IEnumerable<VendorTransaction>>(vendorTransactionsMaster);
                data.Select(c => { c.ElasticStatus = ConstantValue.ElasticStatusWaiting; return c; }).ToList();
                _evfUnitOfWork.GetRepository<VendorTransaction>().AddRange(data);
                _evfUnitOfWork.GetRepository<VendorTransaction>().UpdateRange(transactionUpdate);
                _evfUnitOfWork.Complete(scope);
                System.Threading.Tasks.Task.Run(() =>
                {
                    this.LogTranferData(data);
                    this.LogTranferData(transactionUpdate);
                });
            }
            return result;
        }

        /// <summary>
        /// Validate exits transection update or not.
        /// </summary>
        /// <param name="vendorTransactionsMaster">The vendor transection.</param>
        /// <returns></returns>
        private IEnumerable<VendorTransaction> UpdateTransaction(List<SPE_TRANSAC_PO_QA> vendorTransactionsMaster)
        {
            var transactionUpdate = new List<VendorTransaction>();
            var gjharArray = vendorTransactionsMaster.Select(x => x.Gjahr).ToArray().Distinct();
            var belnrArray = vendorTransactionsMaster.Select(x => x.Belnr).ToArray().Distinct();
            var buzeiArray = vendorTransactionsMaster.Select(x => x.Buzei).ToArray().Distinct();
            var paraArray = vendorTransactionsMaster.Select(x => x.Para).ToArray().Distinct();

            var transactionList = _evfUnitOfWork.GetRepository<VendorTransaction>().Get(x => gjharArray.Contains(x.Gjahr) &&
                                                                                            belnrArray.Contains(x.Belnr) &&
                                                                                            buzeiArray.Contains(x.Buzei.Value) &&
                                                                                            paraArray.Contains(x.Para.Value));

            foreach (var item in vendorTransactionsMaster.ToList())
            {
                var transaction = transactionList.FirstOrDefault(x => x.Gjahr == item.Gjahr &&
                                                                                      x.Belnr == item.Belnr &&
                                                                                      x.Buzei == item.Buzei &&
                                                                                      x.Para == item.Para);
                if (transaction != null)
                {
                    transaction = this.MappingVendorTransaction(item, transaction);
                    transactionUpdate.Add(transaction);
                    vendorTransactionsMaster.Remove(item);
                }
            }
            return transactionUpdate;
        }

        /// <summary>
        /// Mapping model vendor transaction.
        /// </summary>
        /// <param name="master">The vendor transaction from brb util.</param>
        /// <param name="transaction">The vendor transaction from spe database.</param>
        /// <returns></returns>
        private VendorTransaction MappingVendorTransaction(SPE_TRANSAC_PO_QA master, VendorTransaction transaction)
        {
            int id = transaction.Id;
            string weightingKey = transaction.MarkWeightingKey;
            transaction = _mapper.Map<SPE_TRANSAC_PO_QA, VendorTransaction>(master, transaction);
            transaction.Id = id;
            transaction.MarkWeightingKey = weightingKey;
            transaction.ElasticStatus = ConstantValue.ElasticStatusUpdate;
            return transaction;
        }

        /// <summary>
        /// Logging debug when tranfer is finish. 
        /// </summary>
        /// <param name="evfData"></param>
        private void LogTranferData(IEnumerable<VendorTransaction> vendorTransections)
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

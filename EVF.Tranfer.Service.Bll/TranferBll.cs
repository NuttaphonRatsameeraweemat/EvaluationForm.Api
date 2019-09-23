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
using System.Transactions;
using static EVF.Tranfer.Service.Data.UnitOfWorkControl;

namespace EVF.Tranfer.Service.Bll
{
    public class TranferBll : ITranferBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in data mart.
        /// </summary>
        private readonly IUnitOfWork _dmUnitOfWork;
        /// <summary>
        /// The utilities unit of work for manipulating utilities data in spe database.
        /// </summary>
        private readonly IUnitOfWork _evfUnitOfWork;
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
        /// Initializes a new instance of the <see cref="TranferBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work control.</param>
        /// <param name="mapper">The auto mapper.</param>
        public TranferBll(ServiceResolver unitOfWork, IMapper mapper, IConfigSetting config, ILoggerManager logger)
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
        /// Tranfer data in evaluation sap result to zncr 02.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel TranferToDataMart()
        {
            var result = new ResultViewModel();
            var evfData = _evfUnitOfWork.GetRepository<EvaluationSapResult>().Get(x => !x.SendToSap);
            this.SaveToZncr(_mapper.Map<IEnumerable<EvaluationSapResult>, IEnumerable<ZSPE_02>>(evfData));
            this.UpdateEvaluationSapResult(evfData);
            System.Threading.Tasks.Task.Run(() =>
            {
                this.LogTranferData(evfData);
            });
            return result;
        }

        /// <summary>
        /// Insert new sap result to zncr 02 table.
        /// </summary>
        /// <param name="dmData">The sap score result.</param>
        private void SaveToZncr(IEnumerable<ZSPE_02> dmData)
        {
            var now = DateTime.Now;
            dmData.Select(c => { c.UpdateBy = _config.AppName; return c; }).ToList();
            dmData.Select(c => { c.UpdateOn = now; return c; }).ToList();
            _dmUnitOfWork.GetRepository<ZSPE_02>().AddRange(dmData);
            _dmUnitOfWork.Complete();
        }

        /// <summary>
        /// Update Evaluation sap result table set send to sap flag to true.
        /// </summary>
        /// <param name="evfData">The sap score result.</param>
        private void UpdateEvaluationSapResult(IEnumerable<EvaluationSapResult> evfData)
        {
            evfData.Select(c => { c.SendToSap = true; return c; }).ToList();
            _evfUnitOfWork.GetRepository<EvaluationSapResult>().UpdateRange(evfData);
            _evfUnitOfWork.Complete();
        }

        /// <summary>
        /// Logging debug when tranfer is finish. 
        /// </summary>
        /// <param name="evfData"></param>
        private void LogTranferData(IEnumerable<EvaluationSapResult> evfData)
        {
            foreach (var item in evfData)
            {
                _logger.LogDebug($"ComCode : {item.ComCode}, PurOrg : {item.PurOrg}, Vendor : {item.Vendor}, WeightKey : {item.WeightKey}, YearMonth : {item.YearMonth}");
            }
        }

        /// <summary>
        /// Try to connect zncr db.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ZSPE_02> TryToConnect()
        {
            return _dmUnitOfWork.GetRepository<ZSPE_02>().Get();
        }

        /// <summary>
        /// Try to insert sap result score to evf db.
        /// </summary>
        /// <returns></returns>
        public ResultViewModel TryToInsertSapResult(IEnumerable<EvaluationSapResult> model)
        {
            var result = new ResultViewModel();
            model.Select(c => { c.SendToSap = false; return c; }).ToList();
            _evfUnitOfWork.GetRepository<EvaluationSapResult>().AddRange(model);
            return result;
        }

        /// <summary>
        /// Get Evaluation Sap Result Score.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationSapResult> GetEvaluationSapResult()
        {
            return _evfUnitOfWork.GetRepository<EvaluationSapResult>().Get();
        }

        #endregion

    }
}

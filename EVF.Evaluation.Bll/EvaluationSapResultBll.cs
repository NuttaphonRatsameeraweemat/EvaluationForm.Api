using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Evaluation.Bll
{
    public class EvaluationSapResultBll : IEvaluationSapResultBll
    {

        #region [Fields]

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The summary evaluation manager provides summary evaluation functionality.
        /// </summary>
        private readonly ISummaryEvaluationBll _summaryEvaluation;
        /// <summary>
        /// The Logger.
        /// </summary>
        private readonly ILoggerManager _logger;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationSapResultBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationSapResultBll(IUnitOfWork unitOfWork, IManageToken token, ISummaryEvaluationBll summaryEvaluation, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _token = token;
            _summaryEvaluation = summaryEvaluation;
            _logger = logger;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Save evaluation result score to sap result table and upadte status flag.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        public void Save(int evaluationId)
        {
            var evaInfo = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(evaluationId);
            using (var scope = new TransactionScope())
            {
                var result = this.SendToEvaluationSapResult(evaInfo);
                if (result.IsError)
                {
                    evaInfo.SendToEvaluationSapResultStatus = ConstantValue.SendToEvaluationSapResultFailed;
                }
                else evaInfo.SendToEvaluationSapResultStatus = ConstantValue.SendToEvaluationSapResultComplete;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(evaInfo);
                _unitOfWork.Complete(scope);
            }
        }

        /// <summary>
        /// Save evaluation result score to sap result table and upadte status flag.
        /// </summary>
        /// <param name="evaInfo">The evaluation information.</param>
        private void Save(Data.Pocos.Evaluation evaInfo)
        {
            using (var scope = new TransactionScope())
            {
                var result = this.SendToEvaluationSapResult(evaInfo);
                if (result.IsError)
                {
                    evaInfo.SendToEvaluationSapResultStatus = ConstantValue.SendToEvaluationSapResultFailed;
                }
                else evaInfo.SendToEvaluationSapResultStatus = ConstantValue.SendToEvaluationSapResultComplete;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(evaInfo);
                _unitOfWork.Complete(scope);
            }
        }

        /// <summary>
        /// Execute evaluation send to sap result status failed again.
        /// </summary>
        public ResultViewModel ExecuteFailedProcess()
        {
            var result = new ResultViewModel();
            var evaluationList = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => 
                                             x.SendToEvaluationSapResultStatus == ConstantValue.SendToEvaluationSapResultFailed);
            foreach (var item in evaluationList)
            {
                this.Save(item);
            }
            return result;
        }

        /// <summary>
        /// Save evaluation information to sap result.
        /// </summary>
        /// <param name="evaInfo">The evaluation information.</param>
        /// <returns></returns>
        private ResultViewModel SendToEvaluationSapResult(Data.Pocos.Evaluation evaInfo)
        {
            var result = new ResultViewModel();
            try
            {
                var evaluationResult = _summaryEvaluation.GetDetail(evaInfo.Id);
                var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == evaInfo.PeriodItemId).FirstOrDefault();
                var sapResult = new EvaluationSapResult
                {
                    ComCode = evaInfo.ComCode,
                    PurOrg = evaInfo.PurchasingOrg,
                    SendToSap = false,
                    Vendor = evaInfo.VendorNo,
                    WeightKey = evaInfo.WeightingKey,
                    YearMonth = UtilityService.DateTimeToString(periodItem.EndEvaDate.Value, "yy.MM")
                };
                this.DeclareScore(sapResult, evaluationResult.Summarys);
                _unitOfWork.GetRepository<EvaluationSapResult>().Add(sapResult);
            }
            catch (Exception ex)
            {
                result = UtilityService.InitialResultError(ex.Message);
                _logger.LogError(ex, "The Errors Message : ");
            }
            return result;
        }

        /// <summary>
        /// Declare kpi group score to sap fields.
        /// </summary>
        /// <param name="sapResult">The sap result entity model.</param>
        /// <param name="summaries">The summary score.</param>
        private void DeclareScore(EvaluationSapResult sapResult, IEnumerable<Models.SummaryEvaluationDetailViewModel> summaries)
        {
            var kpiGroups = _unitOfWork.GetRepository<KpiGroup>().GetCache();
            var sapFields = _unitOfWork.GetRepository<SapFields>().GetCache();
            foreach (var item in summaries)
            {
                if (item.KpiId == 0)
                {
                    var kpiGroup = kpiGroups.FirstOrDefault(x => x.Id == item.KpiGroupId);
                    var sapField = sapFields.FirstOrDefault(x => x.Id == kpiGroup.SapFieldsId);
                    sapResult.GetType().GetProperty(sapField.SapFields1).SetValue(sapResult, Convert.ToDecimal(item.Score));
                }
            }
        }

        #endregion

    }
}

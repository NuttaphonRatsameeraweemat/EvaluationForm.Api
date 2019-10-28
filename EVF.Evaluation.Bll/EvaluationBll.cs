using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Vendor.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Evaluation.Bll
{
    public class EvaluationBll : IEvaluationBll
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
        /// <summary>
        /// The evaluation assign manager provides evaluation assign functionality.
        /// </summary>
        private readonly IEvaluationAssignBll _evaluationAssign;
        /// <summary>
        /// The vendor filter manager provides vendor filter functionality.
        /// </summary>
        private readonly IVendorFilterBll _vendorFilter;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        /// <param name="evaluationAssign">The evaluation assign manager provides evaluation assign functionality.</param>
        /// <param name="vendorFilter">The vendor filter manager provides vendor filter functionality.</param>
        public EvaluationBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IEvaluationAssignBll evaluationAssign, IVendorFilterBll vendorFilter)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _evaluationAssign = evaluationAssign;
            _vendorFilter = vendorFilter;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation waiting List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetList()
        {
            var evaluationAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.AdUser == _token.AdUser && (x.IsReject == null || !x.IsReject.Value));
            var evaluationWaitingIds = evaluationAssign.Where(x => !x.IsAction.Value).Select(x => x.EvaluationId).Distinct();
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaluationWaitingIds.Contains(x.Id) && x.Status == ConstantValue.EvaWaiting), false);
        }

        /// <summary>
        /// Get Evaluation action List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetListHistory()
        {
            var evaluationAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.AdUser == _token.AdUser && (x.IsReject == null || !x.IsReject.Value));
            var evaluationActionIds = evaluationAssign.Where(x => x.IsAction.Value).Select(x => x.EvaluationId).Distinct();
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaluationActionIds.Contains(x.Id)), true);
        }


        /// <summary>
        /// Initial Evaluation Viewmodel.
        /// </summary>
        /// <param name="data">The evaluation entity model.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationViewModel> MappingModel(IEnumerable<Data.Pocos.Evaluation> evaluation, bool isAction)
        {
            var result = new List<EvaluationViewModel>();
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var purList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var evaluationTemplateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var periodList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            result.AddRange(this.InitialEvaluationViewModel(comList, purList, periodList, vendorList, evaluationTemplateList, evaluation, isAction));
            return result.OrderByDescending(x => x.Id);
        }

        /// <summary>
        /// Initial Evaluation Form list information viewmodel.
        /// </summary>
        /// <param name="comList">The company information values.</param>
        /// <param name="purList">The purchasing org information values.</param>
        /// <param name="periodList">The period information values.</param>
        /// <param name="vendorList">The vendor list information values.</param>
        /// <param name="evaluationTemplateList">The evaluation template information values.</param>
        /// <param name="data">The evaluation task owner data.</param>
        /// <param name="isAction">The evaluator is action or not.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationViewModel> InitialEvaluationViewModel(
            IEnumerable<Hrcompany> comList, IEnumerable<PurchaseOrg> purList, IEnumerable<PeriodItem> periodList,
            IEnumerable<Data.Pocos.Vendor> vendorList, IEnumerable<EvaluationTemplate> evaluationTemplateList,
            IEnumerable<Data.Pocos.Evaluation> data, bool isAction)
        {
            var result = new List<EvaluationViewModel>();
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            foreach (var item in data)
            {
                var periodTemp = periodList.FirstOrDefault(x => x.Id == item.PeriodItemId);
                var valueHelp = valueHelpList.FirstOrDefault(x => x.ValueKey == item.WeightingKey);
                var status = this.GetStatus(isAction, periodTemp);
                result.Add(new EvaluationViewModel
                {
                    Id = item.Id,
                    ComCode = item.ComCode,
                    DocNo = item.DocNo,
                    EvaluationTemplateId = item.EvaluationTemplateId.Value,
                    PurchasingOrg = item.PurchasingOrg,
                    VendorNo = item.VendorNo,
                    WeightingKey = item.WeightingKey,
                    PeriodItemId = item.PeriodItemId.Value,
                    Status = status[0],
                    //Display Value.
                    CompanyName = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode)?.LongText,
                    StartEvaDateString = UtilityService.DateTimeToString(periodTemp.StartEvaDate.Value, ConstantValue.DateTimeFormat),
                    EndEvaDateString = UtilityService.DateTimeToString(periodTemp.EndEvaDate.Value, ConstantValue.DateTimeFormat),
                    PurchasingOrgName = purList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    EvaluationTemplateName = evaluationTemplateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId)?.EvaluationTemplateName,
                    StatusName = status[1],
                    Categorys = item.Category.Split(','),
                    Remark = item.Remark,
                    WeightingKeyName = valueHelp.ValueText
                });
            }
            return result;
        }

        /// <summary>
        /// Get status and display status evaluation.
        /// </summary>
        /// <param name="isAction">The evaluator is action or not.</param>
        /// <param name="periodItem">The evaluation period.</param>
        /// <returns></returns>
        private string[] GetStatus(bool isAction, PeriodItem periodItem)
        {
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeEvaStatus);
            string[] result = new string[2] { ConstantValue.EvaWaiting, valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaWaiting)?.ValueText };
            if (isAction)
            {
                result[0] = ConstantValue.EvaComplete;
                result[1] = valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaComplete)?.ValueText;
            }
            else
            {
                if (periodItem.EndEvaDate.Value.Date < DateTime.Now.Date)
                {
                    result[0] = ConstantValue.EvaExpire;
                    result[1] = valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaExpire)?.ValueText;
                }
            }
            return result;
        }

        /// <summary>
        /// Validate model data logic.
        /// </summary>
        /// <param name="model">The evaluation information value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(EvaluationRequestViewModel model)
        {
            var result = new ResultViewModel();
            if (model.EvaluatorGroup == 0 && model.EvaluatorList == null)
            {
                result = UtilityService.InitialResultError(MessageValue.EvaluatorEmpty, (int)System.Net.HttpStatusCode.BadRequest);
            }
            return result;
        }

        /// <summary>
        /// Insert new evaluation.
        /// </summary>
        /// <param name="model">The evaluation information value.</param>
        public ResultViewModel Save(EvaluationRequestViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluation = _mapper.Map<EvaluationRequestViewModel, Data.Pocos.Evaluation>(model);
                evaluation.DocNo = this.GetDocNo();
                evaluation.EvaPercentageId = this.GetEvaluationPercentage();
                evaluation.Status = ConstantValue.EvaWaiting;
                evaluation.Category = string.Join(",", model.Categorys);
                evaluation.CreateBy = _token.EmpNo;
                evaluation.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Add(evaluation);
                _unitOfWork.Complete();
                _evaluationAssign.SaveList(evaluation.Id, model.EvaluatorPurchasing, this.GetEvaluatorGroup(model.EvaluatorList, model.EvaluatorGroup));
                _vendorFilter.UpdateStatus(model.PeriodItemId.Value, model.ComCode, model.PurchasingOrg, model.WeightingKey, model.VendorNo);
                this.SetEvaluationTemplateFlagUsing(evaluation.EvaluationTemplateId.Value);
                if (model.ImageList != null && model.ImageList.Count > 0)
                {
                    UtilityService.SaveImages(model.ImageList, model.Id, ConstantValue.EvaluationProcessCode);
                }
                _unitOfWork.Complete(scope);
            }
            _unitOfWork.GetRepository<EvaluationTemplate>().ReCache();
            return result;
        }

        /// <summary>
        /// Set is using flag in evaluation template to true.
        /// </summary>
        /// <param name="evaluationTemplateId">The evaluation template id.</param>
        private void SetEvaluationTemplateFlagUsing(int evaluationTemplateId)
        {
            var evaluation = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == evaluationTemplateId).FirstOrDefault();
            evaluation.IsUse = true;
            _unitOfWork.GetRepository<EvaluationTemplate>().Update(evaluation);
        }

        /// <summary>
        /// Get documentation number.
        /// </summary>
        /// <returns></returns>
        private string GetDocNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Get evaluation percentage config.
        /// </summary>
        /// <returns></returns>
        private int GetEvaluationPercentage()
        {
            var percenConfig = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.StartDate.Value.Date <= DateTime.Now.Date &&
                                                                                                     !x.EndDate.HasValue).FirstOrDefault();
            return percenConfig.Id;
        }

        /// <summary>
        /// Get evaluator by group id.
        /// </summary>
        /// <param name="evaluators">The other evaluators.</param>
        /// <param name="evaluatorGroupId">The evaluator group identity.</param>
        /// <returns></returns>
        private string[] GetEvaluatorGroup(string[] evaluators, int evaluatorGroupId)
        {
            var result = new List<string>();
            if (evaluatorGroupId != 0)
            {
                var evaGroup = _unitOfWork.GetRepository<EvaluatorGroupItem>().GetCache(x => x.EvaluatorGroupId == evaluatorGroupId);
                foreach (var item in evaGroup)
                {
                    result.Add(item.AdUser);
                }
            }
            if (evaluators != null)
            {
                foreach (var item in evaluators)
                {
                    result.Add(item);
                }
            }
            return result.ToArray();

        }

        /// <summary>
        /// Reject evaluation task.
        /// </summary>
        /// <param name="model">The evaluation reject information.</param>
        /// <returns></returns>
        public ResultViewModel Reject(EvaluationRejectViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == model.Id && x.AdUser == _token.AdUser).FirstOrDefault();
                if (data.IsAction.Value)
                {
                    result = UtilityService.InitialResultError(MessageValue.EvaluationRejectTaskIsAction, (int)System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    data.IsReject = true;
                    data.ReasonReject = model.Reason;
                    _unitOfWork.GetRepository<EvaluationAssign>().Update(data);
                    _unitOfWork.Complete(scope);
                }
            }
            return result;
        }

        #endregion

    }
}

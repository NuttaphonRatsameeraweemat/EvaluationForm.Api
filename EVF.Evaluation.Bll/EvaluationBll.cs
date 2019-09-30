using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
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
        private readonly IEvaluationAssignBll _evaluationAssign;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IEvaluationAssignBll evaluationAssign)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _evaluationAssign = evaluationAssign;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetEvaluator()
        {
            var evaluationAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.AdUser == _token.AdUser);
            var evaluationActionIds = evaluationAssign.Where(x => x.IsAction.Value).Select(x => x.EvaluationId).Distinct();
            var evaluationWaitingIds = evaluationAssign.Where(x => !x.IsAction.Value).Select(x => x.EvaluationId).Distinct();
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaluationActionIds.Contains(x.Id)),
                                     _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => evaluationWaitingIds.Contains(x.Id)));
        }

        /// <summary>
        /// Initial Evaluation Viewmodel.
        /// </summary>
        /// <param name="data">The evaluation entity model.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationViewModel> MappingModel(IEnumerable<Data.Pocos.Evaluation> evaAction, IEnumerable<Data.Pocos.Evaluation> evaWaiting)
        {
            var result = new List<EvaluationViewModel>();
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var purList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var vendorList = _unitOfWork.GetRepository<Vendor>().GetCache();
            var evaluationTemplateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var periodList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            result.AddRange(this.InitialEvaluationViewModel(comList, purList, periodList, vendorList, evaluationTemplateList, evaAction, true));
            result.AddRange(this.InitialEvaluationViewModel(comList, purList, periodList, vendorList, evaluationTemplateList, evaWaiting, false));
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
            IEnumerable<Vendor> vendorList, IEnumerable<EvaluationTemplate> evaluationTemplateList,
            IEnumerable<Data.Pocos.Evaluation> data, bool isAction)
        {
            var result = new List<EvaluationViewModel>();
            foreach (var item in data)
            {
                var periodTemp = periodList.FirstOrDefault(x => x.Id == item.PeriodId);
                var status = this.GetStatus(isAction, periodTemp);
                result.Add(new EvaluationViewModel
                {
                    Id = item.Id,
                    ComCode = item.ComCode,
                    EvaluationTemplateId = item.EvaluationTemplateId.Value,
                    PurchasingOrg = item.PurchasingOrg,
                    VendorNo = item.VendorNo,
                    WeightingKey = item.WeightingKey,
                    PeriodItemId = item.PeriodId.Value,
                    Status = status[0],
                    //Display Value.
                    CompanyName = comList.FirstOrDefault(x => x.SapcomCode == item.ComCode)?.LongText,
                    StartEvaDateString = periodTemp.StartEvaDate.Value.ToString(ConstantValue.DateTimeFormat),
                    EndEvaDateString = periodTemp.EndEvaDate.Value.ToString(ConstantValue.DateTimeFormat),
                    PurchasingOrgName = purList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    EvaluationTemplateName = evaluationTemplateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId)?.EvaluationTemplateName,
                    StatusName = status[1]
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
            string[] result = new string[2] { ConstantValue.EvaExpire, valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaExpire)?.ValueText };
            if (isAction)
            {
                result[0] = ConstantValue.EvaComplete;
                result[1] = valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaComplete)?.ValueText;
            }
            else
            {
                if (periodItem.StartEvaDate.Value.Date <= DateTime.Now.Date &&
                    periodItem.EndEvaDate.Value.Date >= DateTime.Now.Date)
                {
                    result[0] = ConstantValue.EvaWaiting;
                    result[1] = valueHelp.FirstOrDefault(x => x.ValueKey == ConstantValue.EvaWaiting)?.ValueText;
                }
            }
            return result;
        }

        /// <summary>
        /// Insert new evaluation.
        /// </summary>
        /// <param name="model">The evaluation information value.</param>
        public ResultViewModel Save(EvaluationViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluation = _mapper.Map<EvaluationViewModel, Data.Pocos.Evaluation>(model);
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Add(evaluation);
                _unitOfWork.Complete();
                _evaluationAssign.Save(evaluation.Id, model.EvaluatorPurchasing, model.EvaluatorList);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        #endregion

    }
}

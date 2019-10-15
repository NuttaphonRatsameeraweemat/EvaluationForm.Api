using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.Evaluation.Bll
{
    public class SummaryEvaluationBll : ISummaryEvaluationBll
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
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflowBll _workflow;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEvaluationBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEvaluationBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IConfigSetting config, IWorkflowBll workflow)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _config = config;
            _workflow = workflow;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Summary Evaluation List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetList()
        {
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => _token.PurchasingOrg.Contains(x.PurchasingOrg)));
        }

        /// <summary>
        /// Get Summary Evaluation List by period item id.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetListSearch(int periodItemId)
        {
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => _token.PurchasingOrg.Contains(x.PurchasingOrg) &&
                                                                                                 x.PeriodItemId == periodItemId));
        }

        /// <summary>
        /// Initial Evaluation Viewmodel.
        /// </summary>
        /// <param name="data">The evaluation entity model.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationViewModel> MappingModel(IEnumerable<Data.Pocos.Evaluation> evaluation)
        {
            var result = new List<EvaluationViewModel>();
            var comList = _unitOfWork.GetRepository<Hrcompany>().GetCache();
            var purList = _unitOfWork.GetRepository<PurchaseOrg>().GetCache();
            var vendorList = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache();
            var evaluationTemplateList = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache();
            var periodList = _unitOfWork.GetRepository<PeriodItem>().GetCache();
            result.AddRange(this.InitialEvaluationViewModel(comList, purList, periodList, vendorList, evaluationTemplateList, evaluation));
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
        /// <returns></returns>
        private IEnumerable<EvaluationViewModel> InitialEvaluationViewModel(
            IEnumerable<Hrcompany> comList, IEnumerable<PurchaseOrg> purList, IEnumerable<PeriodItem> periodList,
            IEnumerable<Data.Pocos.Vendor> vendorList, IEnumerable<EvaluationTemplate> evaluationTemplateList,
            IEnumerable<Data.Pocos.Evaluation> data)
        {
            var result = new List<EvaluationViewModel>();
            foreach (var item in data)
            {
                var periodTemp = periodList.FirstOrDefault(x => x.Id == item.PeriodItemId);
                var status = this.GetStatus(item.Status);
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
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private string[] GetStatus(string status)
        {
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeEvaStatus &&
                                                                                 x.ValueKey == status).FirstOrDefault();
            return new string[2] { valueHelp.ValueKey, valueHelp.ValueText };
        }

        /// <summary>
        /// Get Detail summary evaluation.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public SummaryEvaluationViewModel GetDetail(int id)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            var templateInfo = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == data.EvaluationTemplateId).FirstOrDefault();
            var result = this.GetHeaderInformation(data);
            result.UserLists.AddRange(this.GetEvaluators(data.Id, templateInfo.CriteriaId.Value));
            result.Summarys.AddRange(this.GetSummaryPoint(result.UserLists));
            result.Total = this.GetTotalScore(result.Summarys);
            result.GradeName = this.GetGrade(templateInfo.GradeId.Value, result.Total);
            return result;
        }

        /// <summary>
        /// Get evaluation information data.
        /// </summary>
        /// <param name="data">The evaluation information data.</param>
        /// <returns></returns>
        private SummaryEvaluationViewModel GetHeaderInformation(Data.Pocos.Evaluation data)
        {
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
            var purOrg = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == data.PurchasingOrg).FirstOrDefault();
            return new SummaryEvaluationViewModel
            {
                VendorName = vendor?.VendorName,
                PurchasingOrgName = purOrg?.PurchaseName,
                WeightingKey = data.WeightingKey,
            };
        }

        /// <summary>
        /// Get evaluator list in evaluation.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        /// <returns></returns>
        private IEnumerable<UserEvaluationViewModel> GetEvaluators(int evaluationId, int criteriaId)
        {
            var result = new List<UserEvaluationViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId);
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            var orgList = _unitOfWork.GetRepository<Hrorg>().GetCache();
            foreach (var item in data)
            {
                var evaluator = this.InitialEvaluationAssignViewModel(item, empList.FirstOrDefault(x => x.EmpNo == item.EmpNo),
                                                                            orgList);
                if (evaluator.IsAction)
                {
                    evaluator.EvaluationLogs.AddRange(this.GetEvaluationLogs(evaluator.AdUser, evaluationId, criteriaId));
                }
                result.Add(evaluator);
            }
            return result;
        }

        /// <summary>
        /// Get evaluation all log result.
        /// </summary>
        /// <param name="adUser">The owner result evaluation.</param>
        /// <param name="evaluationId">The evaluation identity.</param>
        /// <returns></returns>
        private IEnumerable<UserEvaluationDetailViewModel> GetEvaluationLogs(string adUser, int evaluationId, int criteriaId)
        {
            var result = new List<UserEvaluationDetailViewModel>();
            var evaluationLogs = _unitOfWork.GetRepository<EvaluationLog>().Get(x => x.AdUser == adUser && x.EvaluationId == evaluationId);
            foreach (var item in evaluationLogs)
            {
                var evaLog = new UserEvaluationDetailViewModel();
                var log = _unitOfWork.GetRepository<EvaluationLogItem>().Get(x => x.EvaluationLogId == item.Id);
                evaLog.Id = item.Id;
                evaLog.ActionDate = item.ActionDate;
                foreach (var logItem in log)
                {
                    evaLog.EvaluationLogs.Add(new UserEvaluationLogItemViewModel
                    {
                        Id = logItem.Id,
                        KpiGroupId = logItem.KpiGroupId,
                        KpiId = logItem.KpiId,
                        LevelPoint = logItem.LevelPoint,
                        Reason = logItem.Reason,
                        Score = logItem.Score,
                        Sequence = this.GetSequence(logItem.KpiGroupId.Value, logItem.KpiId, criteriaId)
                    });
                }
                result.Add(evaLog);
            }
            return result;
        }

        /// <summary>
        /// Get summary point between purchasing and user.
        /// </summary>
        /// <param name="userLists">The evaluators list.</param>
        /// <returns></returns>
        private IEnumerable<SummaryEvaluationDetailViewModel> GetSummaryPoint(IEnumerable<UserEvaluationViewModel> userLists)
        {
            var purResult = new List<SummaryEvaluationDetailViewModel>();
            var userResult = new List<SummaryEvaluationDetailViewModel>();
            var purUser = userLists.FirstOrDefault(x => x.UserType == ConstantValue.UserTypePurchasing);
            var users = userLists.Where(x => x.UserType == ConstantValue.UserTypeEvaluator);
            purResult = this.GetLastEvaluation(purUser.EvaluationLogs, purResult);
            foreach (var item in users)
            {
                userResult = this.GetLastEvaluation(item.EvaluationLogs, userResult);
            }
            return this.SummaryScore(purResult, userResult);
        }

        /// <summary>
        /// Get last evaluation log for calculate.
        /// </summary>
        /// <param name="evaluationLogs">The evaluation logs.</param>
        /// <param name="result">The summary result for return.</param>
        /// <returns></returns>
        private List<SummaryEvaluationDetailViewModel> GetLastEvaluation(List<UserEvaluationDetailViewModel> evaluationLogs, List<SummaryEvaluationDetailViewModel> result)
        {
            var lastEva = evaluationLogs.OrderByDescending(x => x.ActionDate).FirstOrDefault();
            if (lastEva != null)
            {
                foreach (var item in lastEva.EvaluationLogs)
                {
                    var temp = result.FirstOrDefault(x => x.KpiGroupId == item.KpiGroupId && x.KpiId == item.KpiId);
                    if (temp != null)
                    {
                        temp.Score = temp.Score + Convert.ToDouble(item.Score);
                    }
                    else
                    {
                        result.Add(new SummaryEvaluationDetailViewModel
                        {
                            KpiGroupId = item.KpiGroupId.Value,
                            KpiId = item.KpiId,
                            Score = item.Score.Value,
                            Sequence = item.Sequence
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get Sequence kpi or kpigroup.
        /// </summary>
        /// <param name="kpiGroupId">The identity kpi group.</param>
        /// <param name="kpiId">The identity kpi.</param>
        /// <param name="criteriaId">The identity criteria.</param>
        /// <returns></returns>
        private int GetSequence(int kpiGroupId, int? kpiId, int criteriaId)
        {
            int result = 0;
            if (kpiId.HasValue)
            {
                result = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == kpiGroupId && x.KpiId == kpiId).FirstOrDefault().Sequence.Value;
            }
            else result = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaId && x.KpiGroupId == kpiGroupId).FirstOrDefault().Sequence.Value;
            return result;
        }

        /// <summary>
        /// Sum score between purchasing and users.
        /// </summary>
        /// <param name="purResult">The purchasing score.</param>
        /// <param name="userResult">The users score.</param>
        /// <returns></returns>
        private IEnumerable<SummaryEvaluationDetailViewModel> SummaryScore(IEnumerable<SummaryEvaluationDetailViewModel> purResult,
                                                                             IEnumerable<SummaryEvaluationDetailViewModel> userResult)
        {
            var result = new List<SummaryEvaluationDetailViewModel>();
            int userCount = userResult.Count();
            if (userCount > 0 && purResult.Count() <= 0)
            {
                foreach (var item in userResult)
                {
                    result.Add(this.InitialModel(item, UtilityService.CalculateScore(0, UtilityService.AverageScore(item.Score, userCount),
                                                                                     _config.UserPercentage, _config.PurchasingPercentage)));
                }
            }
            else
            {
                foreach (var item in purResult)
                {
                    double uPoint = 0;
                    var userPoint = userResult.FirstOrDefault(x => x.KpiGroupId == item.KpiGroupId && x.KpiId == item.KpiId);
                    if (userPoint != null)
                    {
                        uPoint = UtilityService.AverageScore(userPoint.Score, userCount);
                    }
                    result.Add(this.InitialModel(item, uPoint));
                }
            }
            return result;
        }

        /// <summary>
        /// Get total score summary.
        /// </summary>
        /// <param name="summaryEvaluations">The summary information.</param>
        /// <returns></returns>
        private double GetTotalScore(IEnumerable<SummaryEvaluationDetailViewModel> summaryEvaluations)
        {
            var groupSummary = summaryEvaluations.Where(x => !x.KpiId.HasValue);
            double rawTotalScore = groupSummary.Sum(x => x.Score);
            int countKpiGroup = groupSummary.Count();
            return rawTotalScore / Convert.ToDouble(countKpiGroup);
        }

        /// <summary>
        /// Get Grade name.
        /// </summary>
        /// <param name="gradeId">The grade identity using in template.</param>
        /// <param name="totalScore">The total score.</param>
        /// <returns></returns>
        private string GetGrade(int gradeId, double totalScore)
        {
            var gradeInfo = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId);
            var gradePoint = gradeInfo.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore);
            return gradePoint.GradeNameTh;
        }

        /// <summary>
        /// Initial Summary Evaluation Detail ViewModel.
        /// </summary>
        /// <param name="item">The summary evaluation detail viewmodel.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        private SummaryEvaluationDetailViewModel InitialModel(SummaryEvaluationDetailViewModel item, double score)
        {
            return new SummaryEvaluationDetailViewModel
            {
                KpiGroupId = item.KpiGroupId,
                KpiId = item.KpiId,
                Score = UtilityService.CalculateScore(item.Score, score, _config.UserPercentage, _config.PurchasingPercentage),
                Sequence = item.Sequence
            };
        }

        /// <summary>
        /// Initial user evaluation viewmodel.
        /// </summary>
        /// <param name="item">The evaluators information.</param>
        /// <param name="emp">The employee information.</param>
        /// <returns></returns>
        private UserEvaluationViewModel InitialEvaluationAssignViewModel(EvaluationAssign item, Hremployee emp, IEnumerable<Hrorg> orgList)
        {
            var org = orgList.FirstOrDefault(x => x.OrgId == emp?.OrgId);
            return new UserEvaluationViewModel
            {
                Id = item.Id,
                EmpNo = item.EmpNo,
                AdUser = item.AdUser,
                IsReject = item.IsReject.Value,
                IsAction = item.IsAction.Value,
                UserType = item.UserType,
                FullName = string.Format(ConstantValue.EmpTemplate, emp?.FirstnameTh, emp?.LastnameTh),
                OrgName = org?.OrgName
            };
        }

        /// <summary>
        /// Send evaluation approve.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public ResultViewModel SendApprove(int id)
        {
            var result = new ResultViewModel();
            var model = this.GetDetail(id);
            using (TransactionScope scope = new TransactionScope())
            {
                var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
                data.TotalScore = Convert.ToInt32(model.Total);
                data.Status = ConstantValue.WorkflowStatusInWorkflowProcess;

                //Sending workflow k2
                var approval = _unitOfWork.GetRepository<Approval>().GetCache(x => x.PurchasingOrg == data.PurchasingOrg).FirstOrDefault();
                var approvalList = this.GetApproval(_unitOfWork.GetRepository<ApprovalItem>().GetCache(x => x.ApprovalId == approval.Id));
                var vendorInfo = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
                _workflow.Start(data.Id, ConstantValue.EvaluationProcessCode,
                                string.Format(MessageValue.WorkflowFiloEvaluationProcess, vendorInfo.VendorName), approvalList);
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(data);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Submit action task approve or reject.
        /// </summary>
        /// <param name="model">The evaluation task information.</param>
        /// <returns></returns>
        public ResultViewModel SubmitAction(WorkflowViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var response = _workflow.Action(model);
                if (string.IsNullOrEmpty(response))
                {
                    var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(model.DataId);
                    if (string.Equals(model.Action, ConstantValue.WorkflowActionApprove, StringComparison.OrdinalIgnoreCase))
                    {
                        data.Status = ConstantValue.WorkflowStatusApproved;
                    }
                    else data.Status = ConstantValue.WorkflowActionReject;
                    _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(data);
                }
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Validate Status before send approve.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        public ResultViewModel ValidateStatus(int id)
        {
            var result = new ResultViewModel();
            string[] status = new string[] { ConstantValue.EvaComplete, ConstantValue.EvaExpire };
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache();
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            if (!status.Contains(data.Status))
            {
                var temp = valueHelp.FirstOrDefault(x => x.ValueType == ConstantValue.ValueTypeEvaStatus && x.ValueKey == data.Status);
                result = UtilityService.InitialResultError(string.Format(MessageValue.StatusInvalidAction, temp.ValueText),
                                                          (int)System.Net.HttpStatusCode.BadRequest);
            }
            return result;
        }

        /// <summary>
        /// Mapping approve to dictionary.
        /// </summary>
        /// <param name="approvalList">The approval list.</param>
        /// <returns></returns>
        private Dictionary<int, string> GetApproval(IEnumerable<ApprovalItem> approvalList)
        {
            var result = new Dictionary<int, string>();
            foreach (var item in approvalList)
            {
                result.Add(item.Step.Value, item.AdUser);
            }
            return result;
        }

        public void SendEmail()
        {

        }

        public void PrintReport()
        {

        }

        #endregion

    }
}

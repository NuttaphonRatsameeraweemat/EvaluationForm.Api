using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Email.Bll.Interfaces;
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
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflowBll _workflow;
        /// <summary>
        /// The email task provides email task functionality.
        /// </summary>
        private readonly IEmailTaskBll _emailTask;
        /// <summary>
        /// The email service provides email service functionality.
        /// </summary>
        private readonly IEmailService _emailService;
        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEvaluationBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEvaluationBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IWorkflowBll workflow,
                                    IEmailTaskBll emailTask, IEmailService emailService, IConfigSetting config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _workflow = workflow;
            _emailTask = emailTask;
            _emailService = emailService;
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Summary Evaluation List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetList()
        {
            //if admin get all else filter create by
            var purchaseOrg = _unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.AdUser == _token.AdUser &&
                                                                                         x.Type == ConstantValue.PurchasingTypeAdmin)
                                                                                         .Select(x => x.PuchaseOrg).ToArray().Distinct();
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => purchaseOrg.Contains(x.PurchasingOrg) ||
                                                                                                 x.CreateBy == _token.EmpNo));
        }

        /// <summary>
        /// Get Summary Evaluation List by period item id.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetListSearch(int periodItemId)
        {
            //if admin get all else filter create by
            var purchaseOrg = _unitOfWork.GetRepository<PurchaseOrgItem>().GetCache(x => x.AdUser == _token.AdUser &&
                                                                                         x.Type == ConstantValue.PurchasingTypeAdmin)
                                                                                         .Select(x => x.PuchaseOrg).ToArray().Distinct();
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => (purchaseOrg.Contains(x.PurchasingOrg) ||
                                                                                                 x.CreateBy == _token.EmpNo) &&
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
            var valueHelpList = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey);
            foreach (var item in data)
            {
                var periodTemp = periodList.FirstOrDefault(x => x.Id == item.PeriodItemId);
                var valueHelp = valueHelpList.FirstOrDefault(x => x.ValueKey == item.WeightingKey);
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
                    StartEvaDateString = UtilityService.DateTimeToString(periodTemp.StartEvaDate.Value, ConstantValue.DateTimeFormat),
                    EndEvaDateString = UtilityService.DateTimeToString(periodTemp.EndEvaDate.Value, ConstantValue.DateTimeFormat),
                    PurchasingOrgName = purList.FirstOrDefault(x => x.PurchaseOrg1 == item.PurchasingOrg)?.PurchaseName,
                    VendorName = vendorList.FirstOrDefault(x => x.VendorNo == item.VendorNo)?.VendorName,
                    EvaluationTemplateName = evaluationTemplateList.FirstOrDefault(x => x.Id == item.EvaluationTemplateId)?.EvaluationTemplateName,
                    StatusName = status[1],
                    WeightingKeyName = valueHelp.ValueText,
                    CreateDate = UtilityService.DateTimeToString(item.CreateDate.Value, ConstantValue.DateTimeFormat)
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
            result.UserLists.AddRange(this.GetEvaluators(data.Id, templateInfo.CriteriaId.Value, data.WeightingKey));
            result.Summarys.AddRange(this.GetSummaryPoint(result.UserLists, data.EvaPercentageId.Value, data.WeightingKey));
            result.Total = this.GetTotalScore(result.UserLists, data.WeightingKey, data.EvaPercentageId.Value);
            if (double.IsNaN(result.Total))
            {
                result.Total = 0;
            }
            else result.Total = Math.Round(result.Total);
            string[] grade = this.GetGrade(templateInfo.GradeId.Value, result.Total);
            result.GradeName = grade[0];
            result.GradeNameEn = grade[1];
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
            var company = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == data.ComCode).FirstOrDefault();
            return new SummaryEvaluationViewModel
            {
                Id = data.Id,
                CompanyName = company?.LongText,
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
        private IEnumerable<UserEvaluationViewModel> GetEvaluators(int evaluationId, int criteriaId, string weigtingKey)
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
                    evaluator.TotalScore = this.GetTotalScoreByUser(evaluator.EvaluationLogs.OrderByDescending(x => x.ActionDate).FirstOrDefault().EvaluationLogs
                                                                    , weigtingKey);
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
                    bool isHaveKpi = false;
                    if (logItem.KpiId == 0 || logItem.KpiId == null)
                    {
                        isHaveKpi = log.Any(x => x.KpiGroupId == logItem.KpiGroupId && (x.KpiId != 0 && x.KpiId != null));
                    }

                    evaLog.EvaluationLogs.Add(new UserEvaluationLogItemViewModel
                    {
                        Id = logItem.Id,
                        KpiGroupId = logItem.KpiGroupId,
                        KpiId = logItem.KpiId,
                        LevelPoint = logItem.LevelPoint,
                        Reason = logItem.Reason,
                        Score = logItem.Score,
                        Sequence = this.GetSequence(logItem.KpiGroupId.Value, logItem.KpiId, criteriaId),
                        RawScore = isHaveKpi ? 0 : Convert.ToDouble(logItem.RawScore.Value),
                        MaxScore = this.GetMaxScore(logItem.KpiGroupId.Value, logItem.KpiId, criteriaId)
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
        private IEnumerable<SummaryEvaluationDetailViewModel> GetSummaryPoint(IEnumerable<UserEvaluationViewModel> userLists, int percenConfigId, string weightingKey)
        {
            var purResult = new List<SummaryEvaluationDetailViewModel>();
            var userResult = new List<SummaryEvaluationDetailViewModel>();
            var purUser = userLists.FirstOrDefault(x => x.UserType == ConstantValue.UserTypePurchasing);
            var users = userLists.Where(x => x.UserType == ConstantValue.UserTypeEvaluator);
            int userCount = 0;
            purResult = this.GetLastEvaluation(purUser.EvaluationLogs, purResult, ref userCount);
            foreach (var item in users)
            {
                userResult = this.GetLastEvaluation(item.EvaluationLogs, userResult, ref userCount);
            }
            return this.SummaryScore(purResult, userResult, percenConfigId, userCount, weightingKey);
        }

        /// <summary>
        /// Get last evaluation log for calculate.
        /// </summary>
        /// <param name="evaluationLogs">The evaluation logs.</param>
        /// <param name="result">The summary result for return.</param>
        /// <returns></returns>
        private List<SummaryEvaluationDetailViewModel> GetLastEvaluation(List<UserEvaluationDetailViewModel> evaluationLogs, List<SummaryEvaluationDetailViewModel> result,
                                                                         ref int userCount)
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
                userCount++;
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
            if (kpiId.HasValue && kpiId.Value != 0)
            {
                result = _unitOfWork.GetRepository<KpiGroupItem>().GetCache(x => x.KpiGroupId == kpiGroupId && x.KpiId == kpiId).FirstOrDefault().Sequence.Value;
            }
            else result = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaId && x.KpiGroupId == kpiGroupId).FirstOrDefault().Sequence.Value;
            return result;
        }

        /// <summary>
        /// Get Max score.
        /// </summary>
        /// <param name="kpiGroupId">The identity kpi group.</param>
        /// <param name="kpiId">The identity kpi.</param>
        /// <param name="criteriaId">The identity criteria.</param>
        /// <returns></returns>
        private int GetMaxScore(int kpiGroupId, int? kpiId, int criteriaId)
        {
            int result = 0;
            var criteria = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaId && x.KpiGroupId == kpiGroupId).FirstOrDefault();
            if (kpiId.HasValue && kpiId.Value != 0)
            {
                result = _unitOfWork.GetRepository<CriteriaItem>().GetCache(x => x.CriteriaGroupId == criteria.Id && x.KpiId == kpiId).FirstOrDefault().MaxScore.Value;
            }
            else result = criteria.MaxScore.Value;
            return result;
        }

        /// <summary>
        /// Sum score between purchasing and users.
        /// </summary>
        /// <param name="purResult">The purchasing score.</param>
        /// <param name="userResult">The users score.</param>
        /// <returns></returns>
        private IEnumerable<SummaryEvaluationDetailViewModel> SummaryScore(IEnumerable<SummaryEvaluationDetailViewModel> purResult,
                                                                             IEnumerable<SummaryEvaluationDetailViewModel> userResult,
                                                                             int percenConfigId, int userCount, string weightingKey)
        {
            var result = new List<SummaryEvaluationDetailViewModel>();
            var percentageConfig = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.Id == percenConfigId).FirstOrDefault();
            if (userCount > 0 && purResult.Count() <= 0)
            {
                foreach (var item in userResult)
                {
                    result.Add(this.InitialModel(new SummaryEvaluationDetailViewModel { KpiGroupId = item.KpiGroupId, KpiId = item.KpiId, Sequence = item.Sequence },
                                                 UtilityService.CalculateScore(0, UtilityService.AverageScore(item.Score, userCount),
                                                                                     percentageConfig.UserPercentage, percentageConfig.PurchasePercentage, weightingKey),
                                                 percentageConfig, weightingKey, userCount, userResult, "fromUser"));
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
                        if (string.Equals(weightingKey, "A2", StringComparison.OrdinalIgnoreCase))
                        {
                            uPoint = UtilityService.AverageScore(userPoint.Score, userCount - 1);
                        }
                        else uPoint = userPoint.Score;
                    }
                    result.Add(this.InitialModel(item, uPoint, percentageConfig, weightingKey, userCount, userResult));
                }
            }
            return result;
        }

        /// <summary>
        /// Get total score summary.
        /// </summary>
        /// <param name="summaryEvaluations">The summary information.</param>
        /// <returns></returns>
        private double GetTotalScore(IEnumerable<UserEvaluationViewModel> evaluations, string weigtingKey, int percentageId)
        {
            double result = 0;
            if (!string.Equals(weigtingKey, "A2", StringComparison.OrdinalIgnoreCase))
            {
                var evaCount = evaluations.Where(x => x.EvaluationLogs.Count > 0);
                result = evaluations.Sum(x => x.TotalScore) / evaCount.Count();
            }
            else
            {
                var percentageConfig = _unitOfWork.GetRepository<EvaluationPercentageConfig>().GetCache(x => x.Id == percentageId).FirstOrDefault();
                var purEva = evaluations.FirstOrDefault(x => x.UserType == ConstantValue.UserTypePurchasing);
                var userEva = evaluations.Where(x => x.UserType == ConstantValue.UserTypeEvaluator);
                double userAverageScore = 0;
                int[] percentage = new int[] { percentageConfig.PurchasePercentage, percentageConfig.UserPercentage };
                if (userEva.Count() == 0)
                {
                    percentage[0] = 100; //purchase percentage calculate 100 percen when no user evaluation
                    percentage[1] = 0; //user percentage
                }
                else userAverageScore = UtilityService.AverageScore(userEva.Sum(x => x.TotalScore), userEva.Count());
                result = UtilityService.CalculateScore(purEva.TotalScore, userAverageScore,
                                                       percentage[1], percentage[0], weigtingKey);
            }
            return result;
        }

        /// <summary>
        /// Get total score summary.
        /// </summary>
        /// <param name="summaryEvaluations">The summary information.</param>
        /// <returns></returns>
        private double GetTotalScoreByUser(IEnumerable<UserEvaluationLogItemViewModel> userEvaluationDetailViewModel, string weigtingKey)
        {
            double result = 0;
            var groupSummary = userEvaluationDetailViewModel.Where(x => !x.KpiId.HasValue || x.KpiId == 0);
            double rawTotalScore = groupSummary.Sum(x => x.Score.Value);
            if (!string.Equals(weigtingKey, "A2", StringComparison.OrdinalIgnoreCase))
            {
                result = rawTotalScore;
            }
            else
            {
                double totalCal = 0;
                foreach (var item in groupSummary)
                {
                    totalCal = totalCal + (item.Score.Value * item.MaxScore);
                }
                result = totalCal / 100;
            }
            return result;
        }

        /// <summary>
        /// Get Grade name.
        /// </summary>
        /// <param name="gradeId">The grade identity using in template.</param>
        /// <param name="totalScore">The total score.</param>
        /// <returns></returns>
        private string[] GetGrade(int gradeId, double totalScore)
        {
            var gradeInfo = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId);
            var gradePoint = gradeInfo.FirstOrDefault(x => x.StartPoint <= totalScore && x.EndPoint >= totalScore);
            return new string[] { gradePoint.GradeNameTh, gradePoint.GradeNameEn };
        }

        /// <summary>
        /// Initial Summary Evaluation Detail ViewModel.
        /// </summary>
        /// <param name="item">The summary evaluation detail viewmodel.</param>
        /// <param name="score">The score.</param>
        /// <returns></returns>
        private SummaryEvaluationDetailViewModel InitialModel(SummaryEvaluationDetailViewModel item, double score,
                                                              EvaluationPercentageConfig percentageConfig, string weightingKey, int userCount,
                                                              IEnumerable<SummaryEvaluationDetailViewModel> userResult = null,
                                                              string fromUser = "")
        {
            double totalScore = 0;
            if (!string.Equals(weightingKey, "A2", StringComparison.OrdinalIgnoreCase))
            {
                totalScore = Math.Round(UtilityService.AverageScore((item.Score + score), userCount));
            }
            else
            {
                int[] percentage = new int[] { percentageConfig.PurchasePercentage, percentageConfig.UserPercentage };
                if (userResult == null || userResult.Count() == 0)
                {
                    percentage[0] = 100; //purchase percentage calculate 100 percen when no user evaluation
                    percentage[1] = 0; //user percentage
                }
                totalScore = UtilityService.CalculateScore(item.Score, score, percentage[1], percentage[0], weightingKey);
            }
            return new SummaryEvaluationDetailViewModel
            {
                KpiGroupId = item.KpiGroupId,
                KpiId = item.KpiId.HasValue ? item.KpiId : 0,
                Score = totalScore,
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
                ReasonReject = item.ReasonReject,
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
            this.SendEmailToApproval(id);
            return result;
        }

        /// <summary>
        /// Send email vendor evaluation report.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        private void SendEmailToApproval(int id)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
            var company = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == data.ComCode).FirstOrDefault();
            var purchaseOrg = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == data.PurchasingOrg).FirstOrDefault();
            var valueHelp = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey &&
                                                                               x.ValueKey == data.WeightingKey).FirstOrDefault();

            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeEvaluationApproveNotice).FirstOrDefault();
            var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == data.Id).FirstOrDefault();

            string[] periodInfo = this.GetPeriodName(data.PeriodItemId.Value);
            var approvalInfo = this.GetApprovalInfo(id);
            string[] grade = this.GetGrade(evaluationTemplate.GradeId.Value, data.TotalScore.Value);

            string subject = emailTemplate.Subject;
            string content = emailTemplate.Content;

            subject = subject.Replace("%PERIOD%", periodInfo[0]);
            subject = subject.Replace("%PERIODITEM%", periodInfo[1]);

            content = content.Replace("%TO%", string.Format($"คุณ{ConstantValue.EmpTemplate}",
                                                              approvalInfo?.FirstnameTh, approvalInfo?.LastnameTh));
            content = content.Replace("%DOCNO%", data.DocNo);
            content = content.Replace("%VENDOR%", vendor?.VendorName);
            content = content.Replace("%WEIGTHNIGKEY%", valueHelp?.ValueText);
            content = content.Replace("%COMNAME%", company?.LongText);
            content = content.Replace("%PURCHASEORG%", purchaseOrg?.PurchaseName);
            content = content.Replace("%TOTALSCORE%", data.TotalScore.Value.ToString());
            content = content.Replace("%GRADE%", grade[0]);
            content = content.Replace("%URL%", _config.TaskUrl + "Inbox");

            _emailService.SendEmail(new EmailModel
            {
                Body = content,
                Receiver = approvalInfo.Email,
                Subject = subject,
            });

            _emailTask.Save(new Email.Bll.Models.EmailTaskViewModel
            {
                DocNo = data.DocNo,
                Content = $"{content}",
                Subject = subject,
                TaskCode = ConstantValue.EmailEvaluationApproveNotice,
                Receivers = new List<Email.Bll.Models.EmailTaskReceiveViewModel>
                {
                    new Email.Bll.Models.EmailTaskReceiveViewModel{ Email = vendor.Email, FullName = vendor.VendorName, ReceiverType = ConstantValue.ReceiverTypeTo }
                },
                TaskBy = _token.AdUser,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusSending
            });
        }

        /// <summary>
        /// Get approval information.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        /// <returns></returns>
        private Hremployee GetApprovalInfo(int id)
        {
            var processInstances = _unitOfWork.GetRepository<WorkflowProcessInstance>().Get(x => x.DataId == id &&
                                                                                                 x.ProcessCode == ConstantValue.EvaluationProcessCode,
                                                                                                 orderBy: x => x.OrderByDescending(y => y.ProcessInstanceId)).FirstOrDefault();
            var workflowStep = _unitOfWork.GetRepository<WorkflowActivityStep>().Get(x => x.ProcessInstanceId == processInstances.ProcessInstanceId &&
                                                                                        x.Step == processInstances.CurrentStep).FirstOrDefault();
            return _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.Aduser == workflowStep.ActionUser).FirstOrDefault();
        }

        /// <summary>
        /// Get Period information for generate content.
        /// </summary>
        /// <param name="periodItemId">The period item identity.</param>
        /// <returns></returns>
        private string[] GetPeriodName(int periodItemId)
        {
            var periodItem = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.Id == periodItemId).FirstOrDefault();
            var period = _unitOfWork.GetRepository<Period>().GetCache(x => x.Id == periodItem.PeriodId).FirstOrDefault();
            return new string[] { period.Year.ToString(), periodItem.PeriodName };
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
            this.SendEmailToCreater(model.DataId);
            return result;
        }

        /// <summary>
        /// Send email vendor evaluation report.
        /// </summary>
        /// <param name="id">The evaluation identity.</param>
        private void SendEmailToCreater(int id)
        {
            var data = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id);
            var vendor = _unitOfWork.GetRepository<Data.Pocos.Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
            var company = _unitOfWork.GetRepository<Hrcompany>().GetCache(x => x.SapcomCode == data.ComCode).FirstOrDefault();
            var purchaseOrg = _unitOfWork.GetRepository<PurchaseOrg>().GetCache(x => x.PurchaseOrg1 == data.PurchasingOrg).FirstOrDefault();
            var weightingKey = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeWeightingKey &&
                                                                               x.ValueKey == data.WeightingKey).FirstOrDefault();
            var status = _unitOfWork.GetRepository<ValueHelp>().GetCache(x => x.ValueType == ConstantValue.ValueTypeEvaStatus &&
                                                                              x.ValueKey == data.Status).FirstOrDefault();
            var emp = _unitOfWork.GetRepository<Hremployee>().GetCache(x => x.EmpNo == data.CreateBy).FirstOrDefault();

            var emailTemplate = _unitOfWork.GetRepository<EmailTemplate>().GetCache(x => x.EmailType == ConstantValue.EmailTypeEvaluationNotice).FirstOrDefault();
            var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == data.Id).FirstOrDefault();

            string[] periodInfo = this.GetPeriodName(data.PeriodItemId.Value);
            string[] grade = this.GetGrade(evaluationTemplate.GradeId.Value, data.TotalScore.Value);

            string subject = emailTemplate.Subject;
            string content = emailTemplate.Content;

            subject = subject.Replace("%PERIOD%", periodInfo[0]);
            subject = subject.Replace("%PERIODITEM%", periodInfo[1]);

            content = content.Replace("%TO%", string.Format($"คุณ{ConstantValue.EmpTemplate}",
                                                              emp?.FirstnameTh, emp?.LastnameTh));
            content = content.Replace("%DOCNO%", data.DocNo);
            content = content.Replace("%VENDOR%", vendor?.VendorName);
            content = content.Replace("%WEIGTHNIGKEY%", weightingKey?.ValueText);
            content = content.Replace("%COMNAME%", company?.LongText);
            content = content.Replace("%PURCHASEORG%", purchaseOrg?.PurchaseName);
            content = content.Replace("%TOTALSCORE%", data.TotalScore.Value.ToString());
            content = content.Replace("%GRADE%", grade[0]);
            content = content.Replace("%STATUS%", status?.ValueText);

            _emailService.SendEmail(new EmailModel
            {
                Body = content,
                Receiver = emp.Email,
                Subject = subject,
            });

            _emailTask.Save(new Email.Bll.Models.EmailTaskViewModel
            {
                DocNo = data.DocNo,
                Content = $"{content}",
                Subject = subject,
                TaskCode = ConstantValue.EmailEvaluationNotice,
                Receivers = new List<Email.Bll.Models.EmailTaskReceiveViewModel>
                {
                    new Email.Bll.Models.EmailTaskReceiveViewModel{ Email = vendor.Email, FullName = vendor.VendorName, ReceiverType = ConstantValue.ReceiverTypeTo }
                },
                TaskBy = _token.AdUser,
                TaskDate = DateTime.Now,
                Status = ConstantValue.EmailTaskStatusSending
            });
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

        #endregion

    }
}

﻿using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEvaluationBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public SummaryEvaluationBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IConfigSetting config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation waiting List.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationViewModel> GetList()
        {
            return this.MappingModel(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => _token.PurchasingOrg.Contains(x.PurchasingOrg)));
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
            var vendorList = _unitOfWork.GetRepository<Vendor>().GetCache();
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
            IEnumerable<Vendor> vendorList, IEnumerable<EvaluationTemplate> evaluationTemplateList,
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
            var result = this.GetHeaderInformation(_unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(id));
            result.UserLists.AddRange(this.GetEvaluators(id));
            result.Summarys.AddRange(this.GetSummaryPoint(result.UserLists));
            return result;
        }

        /// <summary>
        /// Get evaluation information data.
        /// </summary>
        /// <param name="data">The evaluation information data.</param>
        /// <returns></returns>
        private SummaryEvaluationViewModel GetHeaderInformation(Data.Pocos.Evaluation data)
        {
            var vendor = _unitOfWork.GetRepository<Vendor>().GetCache(x => x.VendorNo == data.VendorNo).FirstOrDefault();
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
        private IEnumerable<UserEvaluationViewModel> GetEvaluators(int evaluationId)
        {
            var result = new List<UserEvaluationViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId);
            var empList = _unitOfWork.GetRepository<Hremployee>().GetCache();
            foreach (var item in data)
            {
                var evaluator = this.InitialEvaluationAssignViewModel(item, empList.FirstOrDefault(x => x.EmpNo == item.EmpNo));
                if (evaluator.IsAction)
                {
                    evaluator.EvaluationLogs.AddRange(this.GetEvaluationLogs(evaluator.AdUser, evaluationId));
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
        private IEnumerable<UserEvaluationDetailViewModel> GetEvaluationLogs(string adUser, int evaluationId)
        {
            var result = new List<UserEvaluationDetailViewModel>();
            var evaluationLogs = _unitOfWork.GetRepository<EvaluationLog>().Get(x => x.AdUser == adUser && x.EvaluationId == evaluationId);
            foreach (var item in evaluationLogs)
            {
                var evaLog = new UserEvaluationDetailViewModel();
                var log = _unitOfWork.GetRepository<EvaluationLogItem>().Get(x => x.EvaluationLogId == item.Id);
                evaLog.ActionDate = item.ActionDate;
                evaLog.EvaluationLogs.AddRange(_mapper.Map<IEnumerable<EvaluationLogItem>, IEnumerable<EvaluationLogItemViewModel>>(log));
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
                            Score = item.Score.Value
                        });
                    }
                }
            }
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
                    result.Add(new SummaryEvaluationDetailViewModel
                    {
                        KpiGroupId = item.KpiGroupId,
                        KpiId = item.KpiId,
                        Score = this.CalculateScore(0, this.AverageScore(item.Score, userCount))
                    });
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
                        uPoint = this.AverageScore(userPoint.Score, userCount);
                    }
                    result.Add(new SummaryEvaluationDetailViewModel
                    {
                        KpiGroupId = item.KpiGroupId,
                        KpiId = item.KpiId,
                        Score = this.CalculateScore(item.Score, uPoint)
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Calculate average score.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="userTotal">The average.</param>
        /// <returns></returns>
        private double AverageScore(double score, int userTotal)
        {
            return score / userTotal;
        }

        /// <summary>
        /// Calculate kpi group score.
        /// </summary>
        /// <param name="purScore">The purchasing score.</param>
        /// <param name="userScore">The average users score.</param>
        /// <returns></returns>
        private double CalculateScore(double purScore, double userScore)
        {
            purScore = (purScore * _config.PurchasingPercentage) / 100;
            userScore = (userScore * _config.UserPercentage) / 100;
            return Math.Round(purScore + userScore);
        }

        /// <summary>
        /// Initial user evaluation viewmodel.
        /// </summary>
        /// <param name="item">The evaluators information.</param>
        /// <param name="emp">The employee information.</param>
        /// <returns></returns>
        private UserEvaluationViewModel InitialEvaluationAssignViewModel(EvaluationAssign item, Hremployee emp)
        {
            return new UserEvaluationViewModel
            {
                EmpNo = item.EmpNo,
                AdUser = item.AdUser,
                IsReject = item.IsReject.Value,
                IsAction = item.IsAction.Value,
                UserType = item.UserType,
                FullName = string.Format(ConstantValue.EmpTemplate, emp?.FirstnameTh, emp?.LastnameTh)
            };
        }

        #endregion

    }
}
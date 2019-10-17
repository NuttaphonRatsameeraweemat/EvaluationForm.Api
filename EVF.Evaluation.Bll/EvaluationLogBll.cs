using AutoMapper;
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
    public class EvaluationLogBll : IEvaluationLogBll
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

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationLogBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public EvaluationLogBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation Log.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationLogViewModel> GetEvaluationLog(int evaluationId)
        {
            return this.InitialEvaluationLogViewModel(_unitOfWork.GetRepository<EvaluationLog>().Get(x => x.EvaluationId == evaluationId &&
                                                                                                          x.AdUser == _token.AdUser));
        }

        /// <summary>
        /// Get Evaluation Log with id.
        /// </summary>
        /// <param name="id">The evaluation log identity.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationLogViewModel> GetEvaluationLogById(int id)
        {
            return this.InitialEvaluationLogViewModel(_unitOfWork.GetRepository<EvaluationLog>().Get(x => x.Id == id));
        }

        /// <summary>
        /// Initial Evaluation Viewmodel.
        /// </summary>
        /// <param name="data">The evaluation log entity model.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationLogViewModel> InitialEvaluationLogViewModel(IEnumerable<EvaluationLog> data)
        {
            var result = new List<EvaluationLogViewModel>();
            foreach (var item in data)
            {
                var logItems = _mapper.Map<IEnumerable<EvaluationLogItem>, IEnumerable<EvaluationLogItemViewModel>>(
                    _unitOfWork.GetRepository<EvaluationLogItem>().Get(x => x.EvaluationLogId == item.Id)).ToList();

                result.Add(new EvaluationLogViewModel
                {
                    EmpNo = item.EmpNo,
                    AdUser = item.AdUser,
                    ActionDate = item.ActionDate,
                    EvaluationLogs = logItems
                });
            }
            return result;
        }

        /// <summary>
        /// Validate Evaluation value before save.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(IEnumerable<EvaluationLogItemViewModel> model)
        {
            var result = new ResultViewModel();
            var kpiGroup = model.Where(x => x.KpiId == null || x.KpiId == 0);
            foreach (var item in kpiGroup)
            {
                var temp = model.Where(x => x.KpiGroupId == item.KpiGroupId && (x.KpiId != null && x.KpiId != 0));
                if (temp.Count() > 0)
                {
                    if (temp.Any(x => x.RawScore == 0))
                    {
                        result = UtilityService.InitialResultError(MessageValue.EvaluationLogSaveValidate, (int)System.Net.HttpStatusCode.BadRequest);
                        break;
                    }
                }
                else
                {
                    if (item.RawScore == 0)
                    {
                        result = UtilityService.InitialResultError(MessageValue.EvaluationLogSaveValidate, (int)System.Net.HttpStatusCode.BadRequest);
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Insert new evaluation log.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        public ResultViewModel Save(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                model = this.SumKpiGroupScore(evaluationId, model);
                var evaluationLog = new EvaluationLog
                {
                    EvaluationId = evaluationId,
                    ActionDate = DateTime.Now,
                    EmpNo = _token.EmpNo,
                    AdUser = _token.AdUser
                };
                this.SetIsAction(evaluationId);
                _unitOfWork.GetRepository<EvaluationLog>().Add(evaluationLog);
                _unitOfWork.Complete();
                this.SaveItem(evaluationLog.Id, model);
                this.IsEvaluationFinish(evaluationId);
                _unitOfWork.Complete(scope);
            }
            return result;
        }

        /// <summary>
        /// Sum Kpi Group Score.
        /// </summary>
        /// <param name="model">The evaluation log item information value.</param>
        /// <returns></returns>
        private IEnumerable<EvaluationLogItemViewModel> SumKpiGroupScore(int evaluationId, IEnumerable<EvaluationLogItemViewModel> model)
        {
            var evaInfo = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().GetById(evaluationId);
            var template = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == evaInfo.EvaluationTemplateId).FirstOrDefault();
            var levelpointMax = _unitOfWork.GetRepository<LevelPointItem>().GetCache(x => x.LevelPointId == template.LevelPointId).Count();
            var kpiGroups = model.Where(x => x.KpiId == null || x.KpiId == 0);
            var kpis = model.Where(x => x.KpiId != null && x.KpiId != 0);
            switch (evaInfo.WeightingKey)
            {
                case "A2":
                    foreach (var item in kpiGroups)
                    {
                        var temp = model.Where(x => x.KpiGroupId == item.KpiGroupId && x.KpiId != null);
                        if (temp.Count() > 0)
                        {
                            var score = temp.Sum(x => x.RawScore);
                            var sumMaxScore = temp.Sum(x => x.MaxScore);
                            item.Score = this.CalculateScore(sumMaxScore, levelpointMax, score);
                            item.RawScore = score;
                        }
                        else
                        {
                            item.Score = this.CalculateScore(item.MaxScore, levelpointMax, item.RawScore);
                        }
                    }
                    foreach (var item in kpis)
                    {
                        item.Score = this.CalculateScore(item.MaxScore, levelpointMax, item.RawScore);
                    }
                    break;
                default:
                    foreach (var item in kpiGroups)
                    {
                        var temp = model.Where(x => x.KpiGroupId == item.KpiGroupId && x.KpiId != null);
                        if (temp.Count() > 0)
                        {
                            var score = temp.Sum(x => x.RawScore);
                            item.Score = score;
                        }
                    }
                    foreach (var item in kpis)
                    {
                        item.Score = item.RawScore;
                    }
                    break;
            }
            
            return model;
        }

        /// <summary>
        /// Calculate score weighting key a2.
        /// </summary>
        /// <param name="maxScore">The max score.</param>
        /// <param name="levelPointMax">The level point max.</param>
        /// <param name="rawScore">The raw score.</param>
        /// <returns></returns>
        private int CalculateScore(int maxScore, int levelPointMax, int rawScore)
        {
            int maxRawScore = (maxScore * levelPointMax);
            double result = (Convert.ToDouble(rawScore) / Convert.ToDouble(maxRawScore));
            return Convert.ToInt32(result * 100);
        }

        /// <summary>
        /// Save evaluation log item.
        /// </summary>
        /// <param name="evaluationLogId">The evaluation log identity.</param>
        /// <param name="model">The evaluation log item information value.</param>
        private void SaveItem(int evaluationLogId, IEnumerable<EvaluationLogItemViewModel> model)
        {
            var evaluationLogItems = _mapper.Map<IEnumerable<EvaluationLogItemViewModel>, IEnumerable<EvaluationLogItem>>(model);
            evaluationLogItems.Select(c => { c.EvaluationLogId = evaluationLogId; return c; }).ToList();
            _unitOfWork.GetRepository<EvaluationLogItem>().AddRange(evaluationLogItems);
        }

        /// <summary>
        /// Validate evaluation all user is action or not.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        private void IsEvaluationFinish(int evaluationId)
        {
            var evaluationAssigns = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId);
            if (!evaluationAssigns.Any(x => !x.IsAction.Value))
            {
                var evaluation = _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Get(x => x.Id == evaluationId).FirstOrDefault();
                evaluation.Status = ConstantValue.EvaComplete;
                _unitOfWork.GetRepository<Data.Pocos.Evaluation>().Update(evaluation);
            }
        }

        /// <summary>
        /// Set assign user action to true.
        /// </summary>
        /// <param name="evaluationId">The evaluation identity.</param>
        private void SetIsAction(int evaluationId)
        {
            var evaluationAssign = _unitOfWork.GetRepository<EvaluationAssign>().Get(x => x.EvaluationId == evaluationId && x.AdUser == _token.AdUser).FirstOrDefault();
            evaluationAssign.IsAction = true;
            _unitOfWork.GetRepository<EvaluationAssign>().Update(evaluationAssign);
        }

        /// <summary>
        /// Inital model for post method save.
        /// </summary>
        /// <param name="evaluationTemplateId">The evaluation template identity.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationLogItemViewModel> GetModelEvaluation(int evaluationTemplateId)
        {
            var result = new List<EvaluationLogItemViewModel>();
            var template = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == evaluationTemplateId).FirstOrDefault();
            if (template != null)
            {
                var criteria = _unitOfWork.GetRepository<Criteria>().GetCache(x => x.Id == template.CriteriaId).FirstOrDefault();
                var criteriaGroups = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteria.Id);
                foreach (var item in criteriaGroups)
                {
                    result.Add(new EvaluationLogItemViewModel
                    {
                        Id = 0,
                        KpiGroupId = item.KpiGroupId,
                        KpiId = 0,
                        LevelPoint = 0,
                        Score = 0,
                        RawScore = 0,
                        Reason = string.Empty,
                        MaxScore = item.MaxScore.Value
                    });
                    var criteriaItems = _unitOfWork.GetRepository<CriteriaItem>().GetCache(x => x.CriteriaGroupId == item.Id);
                    foreach (var subItem in criteriaItems)
                    {
                        result.Add(new EvaluationLogItemViewModel
                        {
                            Id = 0,
                            KpiGroupId = item.KpiGroupId,
                            KpiId = subItem.KpiId,
                            LevelPoint = 0,
                            Score = 0,
                            RawScore = 0,
                            Reason = string.Empty,
                            MaxScore = subItem.MaxScore.Value
                        });
                    }
                }
            }
            return result;
        }

        #endregion

    }
}

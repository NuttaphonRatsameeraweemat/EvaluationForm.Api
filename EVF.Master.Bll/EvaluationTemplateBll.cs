using AutoMapper;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;

namespace EVF.Master.Bll
{
    public class EvaluationTemplateBll : IEvaluationTemplateBll
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
        /// The criteria manager provides criteria functionality.
        /// </summary>
        private readonly ICriteriaBll _criteria;
        /// <summary>
        /// The level point manager provides level point functionality.
        /// </summary>
        private readonly ILevelPointBll _levelPoint;
        /// <summary>
        /// The grade manager provides grade functionality.
        /// </summary>
        private readonly IGradeBll _grade;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationTemplateBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        /// <param name="kpiGroup">The criteria manager provides criteria functionality.</param>
        public EvaluationTemplateBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token,
                                    ICriteriaBll criteria, ILevelPointBll levelPoint, IGradeBll grade)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _criteria = criteria;
            _levelPoint = levelPoint;
            _grade = grade;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Evaluation template list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvaluationTemplateViewModel> GetList()
        {
            var result = new List<EvaluationTemplateViewModel>();
            var data = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => (_token.PurchasingOrg.Contains(x.CreateByPurchaseOrg) ||
                                                                                      _token.EmpNo == x.CreateBy),
                                                                            x => x.OrderBy(y => y.EvaluationTemplateName));
            foreach (var item in data)
            {
                var temp = _mapper.Map<EvaluationTemplate, EvaluationTemplateViewModel>(item);
                var purSpilt = item.ForPurchaseOrg != null ? item.ForPurchaseOrg.Split(',') : new string[] { };
                temp.PurchaseOrgs = purSpilt;
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// Get Evaluation template list filter by weighting key.
        /// </summary>
        /// <param name="weightingKey">The weighting key.</param>
        /// <returns></returns>
        public IEnumerable<EvaluationTemplateViewModel> GetListByWeightingKey(string weightingKey, string purchaseOrg)
        {
            var levelPointIds = this.GetLevelPointIds(weightingKey);
            return _mapper.Map<IEnumerable<EvaluationTemplate>, IEnumerable<EvaluationTemplateViewModel>>(
                   _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.ForPurchaseOrg.Contains(purchaseOrg) &&
                                                                                 levelPointIds.Contains(x.LevelPointId.Value),
                                                                            x => x.OrderBy(y => y.EvaluationTemplateName)));
        }

        /// <summary>
        /// Get Detail of Evaluation Template.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        public EvaluationTemplateViewModel GetDetail(int id)
        {
            return _mapper.Map<EvaluationTemplate, EvaluationTemplateViewModel>(
                   _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == id).FirstOrDefault());
        }

        /// <summary>
        /// Validate information value in evaluation template logic.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(EvaluationTemplateViewModel model)
        {
            var result = new ResultViewModel();
            return result;
        }

        /// <summary>
        /// Insert new evaluation template.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(EvaluationTemplateViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluationTemplate = _mapper.Map<EvaluationTemplateViewModel, EvaluationTemplate>(model);
                evaluationTemplate.CreateBy = _token.EmpNo;
                evaluationTemplate.CreateDate = DateTime.Now;
                evaluationTemplate.CreateByPurchaseOrg = _token.PurchasingOrg[0];
                if (model.PurchaseOrgs != null)
                {
                    evaluationTemplate.ForPurchaseOrg = string.Join(",", model.PurchaseOrgs);
                }
                _unitOfWork.GetRepository<EvaluationTemplate>().Add(evaluationTemplate);
                _unitOfWork.Complete();
                this.UpdateFlagUsing(evaluationTemplate.CriteriaId.Value, evaluationTemplate.GradeId.Value,
                                     evaluationTemplate.LevelPointId.Value, evaluationTemplate.Id, true);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationTemplate();
            return result;
        }

        /// <summary>
        /// Update evaluation template group.
        /// </summary>
        /// <param name="model">The evaluation template information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(EvaluationTemplateViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetById(model.Id);
                evaluationTemplate.EvaluationTemplateName = model.EvaluationTemplateName;
                evaluationTemplate.CriteriaId = model.CriteriaId;
                evaluationTemplate.GradeId = model.GradeId;
                evaluationTemplate.LevelPointId = model.LevelPointId;
                evaluationTemplate.LastModifyBy = _token.EmpNo;
                evaluationTemplate.LastModifyDate = DateTime.Now;
                if (model.PurchaseOrgs != null)
                {
                    evaluationTemplate.ForPurchaseOrg = string.Join(",", model.PurchaseOrgs);
                }
                _unitOfWork.GetRepository<EvaluationTemplate>().Update(evaluationTemplate);
                this.UpdateFlagUsing(evaluationTemplate.CriteriaId.Value, evaluationTemplate.GradeId.Value,
                                     evaluationTemplate.LevelPointId.Value, evaluationTemplate.Id, true);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationTemplate();
            return result;
        }

        /// <summary>
        /// Remove evaluation template.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var evaluationTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == id).FirstOrDefault();
                _unitOfWork.GetRepository<EvaluationTemplate>().Remove(evaluationTemplate);
                this.UpdateFlagUsing(evaluationTemplate.CriteriaId.Value, evaluationTemplate.GradeId.Value,
                                     evaluationTemplate.LevelPointId.Value, id, false);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationTemplate();
            return result;
        }

        /// <summary>
        /// Set flag criteria, grade and levelpoint using in evaluation template.
        /// </summary>
        /// <param name="criteriaId">The criteria identity.</param>
        /// <param name="gradeId">The grade identity.</param>
        /// <param name="levelPointId">The level point identity.</param>
        /// <param name="isUse">The flag is using.</param>
        private void UpdateFlagUsing(int criteriaId, int gradeId, int levelPointId, int evaluationTemplateId, bool isUse)
        {
            var criterias = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id != evaluationTemplateId &&
                                                                                          x.CriteriaId == criteriaId);
            var grades = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id != evaluationTemplateId &&
                                                                                         x.GradeId == gradeId);
            var levelPoints = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id != evaluationTemplateId &&
                                                                                         x.LevelPointId == levelPointId);
            if (criterias.Count() <= 0)
            {
                _criteria.SetIsUse(criteriaId, isUse);
            }
            if (grades.Count() <= 0)
            {
                _grade.SetIsUse(gradeId, isUse);
            }
            if (levelPoints.Count() <= 0)
            {
                _levelPoint.SetIsUse(levelPointId, isUse);
            }
        }

        /// <summary>
        /// Validate grade is using in evaluation template or not.
        /// </summary>
        /// <param name="id">The grade identity.</param>
        /// <returns></returns>
        public bool IsUse(int id)
        {
            var evaTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == id).FirstOrDefault();
            return evaTemplate.IsUse.Value;
        }

        /// <summary>
        /// Set flag is use in grade.
        /// </summary>
        /// <param name="ids">The grade identity.</param>
        /// <param name="isUse">The flag is using.</param>
        public void SetIsUse(int id)
        {
            var data = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == id).FirstOrDefault();
            data.IsUse = true;
            _unitOfWork.GetRepository<EvaluationTemplate>().Update(data);
        }

        /// <summary>
        /// Reload Cache when evaluation template is change.
        /// </summary>
        private void ReloadCacheEvaluationTemplate()
        {
            _unitOfWork.GetRepository<EvaluationTemplate>().ReCache();
            _unitOfWork.GetRepository<Criteria>().ReCache();
            _unitOfWork.GetRepository<LevelPoint>().ReCache();
            _unitOfWork.GetRepository<Grade>().ReCache();
        }

        /// <summary>
        /// Get evaluation template for display.
        /// </summary>
        /// <param name="id">The identity of evaluation template.</param>
        /// <returns></returns>
        public EvaluationTemplateDisplayViewModel LoadTemplate(int id)
        {
            var result = new EvaluationTemplateDisplayViewModel();
            var evaTemplate = _unitOfWork.GetRepository<EvaluationTemplate>().GetCache(x => x.Id == id).FirstOrDefault();
            if (evaTemplate != null)
            {
                result.Name = evaTemplate.EvaluationTemplateName;
                result.Criteria = _criteria.GetDetail(evaTemplate.CriteriaId.Value);
                result.LevelPoint = _levelPoint.GetDetail(evaTemplate.LevelPointId.Value);
                result.Grade = _grade.GetDetail(evaTemplate.GradeId.Value);
                result.MaxTotalScore = this.GetMaxTotalScore(result.LevelPoint.WeightingKey, result.LevelPoint.LevelPointItems.Count, result.Criteria);
            }
            return result;
        }

        /// <summary>
        /// Get evaluation template preview for display.
        /// </summary>
        /// <param name="model">The master data setup template.</param>
        /// <returns></returns>
        public EvaluationTemplateDisplayViewModel PreviewTemplate(EvaluationTemplatePreviewRequestModel model)
        {
            var result = new EvaluationTemplateDisplayViewModel
            {
                Name = string.Empty,
                Criteria = _criteria.GetDetail(model.CriteriaId.Value),
                LevelPoint = _levelPoint.GetDetail(model.LevelPointId.Value),
                Grade = _grade.GetDetail(model.GradeId.Value)
            };
            result.MaxTotalScore = this.GetMaxTotalScore(result.LevelPoint.WeightingKey, result.LevelPoint.LevelPointItems.Count, result.Criteria);
            return result;
        }

        /// <summary>
        /// Get Max total score calculate for a2 type.
        /// </summary>
        /// <param name="weightingKey">The weighting key.</param>
        /// <param name="levelPoint">The maximun level point.</param>
        /// <param name="criteria">The criteria and maxscore.</param>
        /// <returns></returns>
        private int GetMaxTotalScore(string weightingKey, int levelPoint, CriteriaViewModel criteria)
        {
            int result = 0;
            if (string.Equals(weightingKey, "A2", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var item in criteria.CriteriaGroups)
                {
                    result = result + (item.MaxScore * levelPoint);
                }
            }
            return result;
        }

        /// <summary>
        /// Get Level point identitys.
        /// </summary>
        /// <param name="weightingKey">The weighting key.</param>
        /// <returns></returns>
        private int[] GetLevelPointIds(string weightingKey)
        {
            switch (weightingKey)
            {
                case "A2":
                    return _unitOfWork.GetRepository<LevelPoint>().GetCache(x => x.WeightingKey == weightingKey).Select(x => x.Id).ToArray();
                default:
                    return _unitOfWork.GetRepository<LevelPoint>().GetCache(x => x.WeightingKey != "A2").Select(x => x.Id).ToArray();
            }
        }

        #endregion

    }
}

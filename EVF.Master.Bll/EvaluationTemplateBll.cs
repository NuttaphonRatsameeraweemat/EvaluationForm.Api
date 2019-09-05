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
            return _mapper.Map<IEnumerable<EvaluationTemplate>, IEnumerable<EvaluationTemplateViewModel>>(
                   _unitOfWork.GetRepository<EvaluationTemplate>().GetCache());
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
                _unitOfWork.GetRepository<EvaluationTemplate>().Add(evaluationTemplate);
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
                _unitOfWork.GetRepository<EvaluationTemplate>().Update(evaluationTemplate);
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
                _unitOfWork.GetRepository<EvaluationTemplate>().Remove(
                    _unitOfWork.GetRepository<EvaluationTemplate>().GetById(id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheEvaluationTemplate();
            return result;
        }

        /// <summary>
        /// Reload Cache when evaluation template is change.
        /// </summary>
        private void ReloadCacheEvaluationTemplate()
        {
            _unitOfWork.GetRepository<EvaluationTemplate>().ReCache();
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
            }
            return result;
        }

        #endregion

    }
}

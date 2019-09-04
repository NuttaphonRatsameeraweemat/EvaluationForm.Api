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
    public class CriteriaBll : ICriteriaBll
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
        /// The Performance Group manager provides performance group functionality.
        /// </summary>
        private readonly IKpiGroupBll _performanceGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="CriteriaBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        /// <param name="performanceGroup">The Performance Group manager provides performance group functionality.</param>
        public CriteriaBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token, IKpiGroupBll performanceGroup)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
            _performanceGroup = performanceGroup;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Criteria list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CriteriaViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Criteria>, IEnumerable<CriteriaViewModel>>(
                   _unitOfWork.GetRepository<Criteria>().GetCache());
        }

        /// <summary>
        /// Get Detail of criteria.
        /// </summary>
        /// <param name="id">The identity of criteria group.</param>
        /// <returns></returns>
        public CriteriaViewModel GetDetail(int id)
        {
            var data = _mapper.Map<Criteria, CriteriaViewModel>(
                   _unitOfWork.GetRepository<Criteria>().GetById(id));
            data.CriteriaGroups = this.GetCriteriaGroups(id).ToList();
            return data;
        }

        /// <summary>
        /// Get criteria group item.
        /// </summary>
        /// <param name="criteriaId">The identity criteria group.</param>
        /// <returns></returns>
        private IEnumerable<CriteriaGroupViewModel> GetCriteriaGroups(int criteriaId)
        {
            var criteriaGroup = _mapper.Map<IEnumerable<CriteriaGroup>, IEnumerable<CriteriaGroupViewModel>>(
                   _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaId));
            var groupIds = criteriaGroup.Select(x => x.Id).ToArray();
            var criteriaItems = _unitOfWork.GetRepository<CriteriaItem>().GetCache(x => groupIds.Contains(x.CriteriaGroupId.Value));
            foreach (var item in criteriaGroup)
            {
                item.CriteriaItems = _performanceGroup.GetKpiItemDisplayCriteria(item.KpiGroupId).ToList();
            }
            return criteriaGroup;
        }

        /// <summary>
        /// Validate information value in criteria logic.
        /// </summary>
        /// <param name="model">The criteria information value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(CriteriaViewModel model)
        {
            var result = new ResultViewModel();
            int totalScore = model.CriteriaGroups.Sum(x => x.Score);
            if (totalScore > 100)
            {
                result = UtilityService.InitialResultError(MessageValue.CriteriaOverScore, (int)HttpStatusCode.BadRequest);
                return result;
            }
            foreach (var item in model.CriteriaGroups)
            {
                int scoreGroup = item.CriteriaItems.Sum(x => x.Score);
                if (scoreGroup > item.Score || scoreGroup < item.Score)
                {
                    result = UtilityService.InitialResultError(MessageValue.CriteriaItemScoreGreatethanScoreGroup, (int)HttpStatusCode.BadRequest);
                }
            }
            return result;
        }

        /// <summary>
        /// Insert new criteria group.
        /// </summary>
        /// <param name="model">The criteria information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(CriteriaViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var criteria = _mapper.Map<CriteriaViewModel, Criteria>(model);
                criteria.CreateBy = _token.EmpNo;
                criteria.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Criteria>().Add(criteria);
                _unitOfWork.Complete();
                this.SaveCriteriaGroup(criteria.Id, model.CriteriaGroups);
                this.SetIsDefault(model);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Insert criteria group list.
        /// </summary>
        /// <param name="criteriaId">The identity of criteria.</param>
        /// <param name="criteriaGroups">The criteria groups information value.</param>
        private void SaveCriteriaGroup(int criteriaId, IEnumerable<CriteriaGroupViewModel> criteriaGroups)
        {
            foreach (var item in criteriaGroups)
            {
                var data = _mapper.Map<CriteriaGroupViewModel, CriteriaGroup>(item);
                data.CriteriaId = criteriaId;
                _unitOfWork.GetRepository<CriteriaGroup>().Add(data);
                _unitOfWork.Complete();
                if (item.CriteriaItems.Any())
                {
                    this.SaveCriteriaItem(data.Id, item.CriteriaItems);
                }
            }
            
        }

        /// <summary>
        /// Insert criteria item list.
        /// </summary>
        /// <param name="criteriaGroupId">The identity of criteria group.</param>
        /// <param name="criteriaItems">The criteria items information value.</param>
        private void SaveCriteriaItem(int criteriaGroupId, IEnumerable<CriteriaItemViewModel> criteriaItems)
        {
            var data = _mapper.Map<IEnumerable<CriteriaItemViewModel>, IEnumerable<CriteriaItem>>(criteriaItems);
            data.Select(c => { c.CriteriaGroupId = criteriaGroupId; return c; }).ToList();
            _unitOfWork.GetRepository<CriteriaItem>().AddRange(data);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Update grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(CriteriaViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var criteria = _mapper.Map<CriteriaViewModel, Criteria>(model);
                criteria.LastModifyBy = _token.EmpNo;
                criteria.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Criteria>().Update(criteria);
                this.EditCriteriaGroup(criteria.Id, model.CriteriaGroups);
                this.SetIsDefault(model);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Update criteria group list.
        /// </summary>
        /// <param name="criteriaId">The identity of criteria.</param>
        /// <param name="criteriaGroups">The criteria groups information value.</param>
        private void EditCriteriaGroup(int criteriaId, IEnumerable<CriteriaGroupViewModel> criteriaGroups)
        {
            var data = _unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == criteriaId);

            var criteriaGroupAdd = criteriaGroups.Where(x => x.Id == 0);
            var criteriaGroupDelete = data.Where(x => !criteriaGroups.Any(y => x.Id == y.Id));

            var criteriaGroupUpdate = _mapper.Map<IEnumerable<CriteriaGroupViewModel>, IEnumerable<CriteriaGroup>>(criteriaGroups);
            criteriaGroupUpdate = criteriaGroupUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveCriteriaGroup(criteriaId, criteriaGroupAdd);
            this.DeleteCriteriaGroups(criteriaGroupDelete);
            _unitOfWork.GetRepository<CriteriaGroup>().UpdateRange(criteriaGroupUpdate);

            criteriaGroups = criteriaGroups.Where(x => criteriaGroupUpdate.Any(y => y.Id == x.Id));
            foreach (var item in criteriaGroups)
            {
                this.EditCriteriaItem(item.CriteriaItems);
            }
            
        }

        /// <summary>
        /// Update criteria item list.
        /// </summary>
        /// <param name="criteriaItems">The criteria items information value.</param>
        private void EditCriteriaItem(IEnumerable<CriteriaItemViewModel> criteriaItems)
        {
            _unitOfWork.GetRepository<CriteriaItem>().UpdateRange(
                _mapper.Map<IEnumerable<CriteriaItemViewModel>, IEnumerable<CriteriaItem>>(criteriaItems));
        }

        /// <summary>
        /// Remove criteria.
        /// </summary>
        /// <param name="id">The identity of criteria.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<Criteria>().Remove(
                    _unitOfWork.GetRepository<Criteria>().GetById(id));
                this.DeleteCriteriaGroups(_unitOfWork.GetRepository<CriteriaGroup>().GetCache(x => x.CriteriaId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Set other default false when update this criteria is true.
        /// </summary>
        /// <param name="isDefault"></param>
        private void SetIsDefault(CriteriaViewModel model)
        {
            if (model.IsDefault)
            {
                var data = _unitOfWork.GetRepository<Criteria>().GetCache(x => x.IsDefault != null && x.IsDefault.Value).FirstOrDefault();
                if (data != null && data.Id != model.Id)
                {
                    data.IsDefault = false;
                    _unitOfWork.GetRepository<Criteria>().Update(data);
                }
            }
        }

        /// <summary>
        /// Remove criteria groups.
        /// </summary>
        /// <param name="model">The criteria groups.</param>
        private void DeleteCriteriaGroups(IEnumerable<CriteriaGroup> model)
        {
            foreach (var item in model)
            {
                var data = _unitOfWork.GetRepository<CriteriaItem>().GetCache(x => x.CriteriaGroupId == item.Id);
                this.DeleteCriteriaItems(data);
            }
            _unitOfWork.GetRepository<CriteriaGroup>().RemoveRange(model);
        }

        /// <summary>
        /// Remove criteria items.
        /// </summary>
        /// <param name="model">The criteria items.</param>
        private void DeleteCriteriaItems(IEnumerable<CriteriaItem> model)
        {
            _unitOfWork.GetRepository<CriteriaItem>().RemoveRange(model);
        }

        /// <summary>
        /// Reload Cache when grade and gradeItems is change.
        /// </summary>
        private void ReloadCacheGrade()
        {
            _unitOfWork.GetRepository<Grade>().ReCache();
            _unitOfWork.GetRepository<GradeItem>().ReCache();
        }

        #endregion

    }
}

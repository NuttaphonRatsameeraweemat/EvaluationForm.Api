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
    public class GradeBll : IGradeBll
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
        /// Initializes a new instance of the <see cref="GradeBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public GradeBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Grade list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GradeViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Grade>, IEnumerable<GradeViewModel>>(
                   _unitOfWork.GetRepository<Grade>().GetCache());
        }

        /// <summary>
        /// Get Detail of grade.
        /// </summary>
        /// <param name="id">The identity of grade group.</param>
        /// <returns></returns>
        public GradeViewModel GetDetail(int id)
        {
            var data = _mapper.Map<Grade, GradeViewModel>(
                   _unitOfWork.GetRepository<Grade>().GetById(id));
            data.GradeItems = this.GetGradeItem(id).ToList();
            return data;
        }

        /// <summary>
        /// Get grade group item.
        /// </summary>
        /// <param name="periodId">The identity grade group.</param>
        /// <returns></returns>
        private IEnumerable<GradeItemViewModel> GetGradeItem(int gradeId)
        {
            return _mapper.Map<IEnumerable<GradeItem>, IEnumerable<GradeItemViewModel>>(
                   _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId));
        }

        /// <summary>
        /// Validate information value in grade logic.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        public ResultViewModel ValidateData(GradeViewModel model)
        {
            var result = new ResultViewModel();
            int oldEnd = int.MinValue;
            foreach (var item in model.GradeItems)
            {
                if (item.StartPoint >= item.EndPoint)
                {
                    result = UtilityService.InitialResultError(MessageValue.GradePointIncorrect, (int)HttpStatusCode.BadRequest);
                    break;
                }
                if (oldEnd >= item.StartPoint)
                {
                    result = UtilityService.InitialResultError(MessageValue.GradePointIncorrect, (int)HttpStatusCode.BadRequest);
                    break;
                }
                oldEnd = item.EndPoint.Value;
            }
            return result;
        }

        /// <summary>
        /// Insert new grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(GradeViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var gradeGroup = _mapper.Map<GradeViewModel, Grade>(model);
                this.SetIsDefault(model);
                gradeGroup.CreateBy = _token.EmpNo;
                gradeGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Grade>().Add(gradeGroup);
                _unitOfWork.Complete();
                this.SaveItem(gradeGroup.Id, model.GradeItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Insert grade group item list.
        /// </summary>
        /// <param name="gradeId">The identity of grade group.</param>
        /// <param name="gradeItems">The identity of grade items.</param>
        private void SaveItem(int gradeId, IEnumerable<GradeItemViewModel> gradeItems)
        {
            var data = _mapper.Map<IEnumerable<GradeItemViewModel>, IEnumerable<GradeItem>>(gradeItems);
            data.Select(c => { c.GradeId = gradeId; return c; }).ToList();
            _unitOfWork.GetRepository<GradeItem>().AddRange(data);
        }

        /// <summary>
        /// Update grade group.
        /// </summary>
        /// <param name="model">The grade information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(GradeViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                this.SetIsDefault(model);
                var gradeGroup = _unitOfWork.GetRepository<Grade>().GetById(model.Id);
                gradeGroup.Name = model.Name;
                gradeGroup.IsDefault = model.IsDefault;
                gradeGroup.LastModifyBy = _token.EmpNo;
                gradeGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Grade>().Update(gradeGroup);
                this.EditItem(gradeGroup.Id, model.GradeItems);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Update grade group items.
        /// </summary>
        /// <param name="gradeId">The identity of grade group.</param>
        /// <param name="gradeItems">The identity of grade items.</param>
        private void EditItem(int gradeId, IEnumerable<GradeItemViewModel> gradeItems)
        {
            var data = _unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == gradeId);

            var gradeItemAdd = gradeItems.Where(x => x.Id == 0);
            var gradeItemDelete = data.Where(x => !gradeItems.Any(y => x.Id == y.Id));

            var gradeItemUpdate = _mapper.Map<IEnumerable<GradeItemViewModel>, IEnumerable<GradeItem>>(gradeItems);
            gradeItemUpdate = gradeItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(gradeId, gradeItemAdd);
            this.DeleteItem(gradeItemDelete);
            _unitOfWork.GetRepository<GradeItem>().UpdateRange(gradeItemUpdate);
        }

        /// <summary>
        /// Remove grade group.
        /// </summary>
        /// <param name="id">The identity of grade group.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<Grade>().Remove(
                    _unitOfWork.GetRepository<Grade>().GetById(id));
                this.DeleteItem(_unitOfWork.GetRepository<GradeItem>().GetCache(x => x.GradeId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheGrade();
            return result;
        }

        /// <summary>
        /// Set other default false when update this grade is true.
        /// </summary>
        /// <param name="isDefault"></param>
        private void SetIsDefault(GradeViewModel model)
        {
            if (model.IsDefault)
            {
                var data = _unitOfWork.GetRepository<Grade>().GetCache(x => x.IsDefault != null && x.IsDefault.Value).FirstOrDefault();
                if (data != null && data.Id != model.Id)
                {
                    data.IsDefault = false;
                    _unitOfWork.GetRepository<Grade>().Update(data);
                }
            }
        }

        /// <summary>
        /// Remove grade group items.
        /// </summary>
        /// <param name="model">The grade group items.</param>
        private void DeleteItem(IEnumerable<GradeItem> model)
        {
            _unitOfWork.GetRepository<GradeItem>().RemoveRange(model);
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

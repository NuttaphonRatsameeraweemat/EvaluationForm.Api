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
using System.Text;
using System.Transactions;

namespace EVF.Master.Bll
{
    public class PeriodBll : IPeriodBll
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
        /// Initializes a new instance of the <see cref="PeriodBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public PeriodBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get Period list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PeriodViewModel> GetList()
        {
            return _mapper.Map<IEnumerable<Period>, IEnumerable<PeriodViewModel>>(
                   _unitOfWork.GetRepository<Period>().GetCache(orderBy: x => x.OrderByDescending(y => y.Year)));
        }

        /// <summary>
        /// Get Detail of period.
        /// </summary>
        /// <param name="id">The identity of period group.</param>
        /// <returns></returns>
        public PeriodViewModel GetDetail(int id)
        {
            var data = _mapper.Map<Period, PeriodViewModel>(
                   _unitOfWork.GetRepository<Period>().GetCache(x=>x.Id == id).FirstOrDefault());
            data.PeriodItems = this.GetPeriodItem(id).ToList();
            return data;
        }

        /// <summary>
        /// Get period group item.
        /// </summary>
        /// <param name="periodId">The identity period group.</param>
        /// <returns></returns>
        private IEnumerable<PeriodItemViewModel> GetPeriodItem(int periodId)
        {
            var data = _mapper.Map<IEnumerable<PeriodItem>, IEnumerable<PeriodItemViewModel>>(
                   _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == periodId));
            foreach (var item in data)
            {
                item.StartEvaDateString = item.StartEvaDate.ToString(ConstantValue.DateTimeFormat);
                item.EndEvaDateString = item.EndEvaDate.ToString(ConstantValue.DateTimeFormat);
            }
            return data;
        }

        /// <summary>
        /// Insert new period group.
        /// </summary>
        /// <param name="model">The period information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(PeriodViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var periodGroup = _mapper.Map<PeriodViewModel, Period>(model);
                periodGroup.CreateBy = _token.EmpNo;
                periodGroup.CreateDate = DateTime.Now;
                _unitOfWork.GetRepository<Period>().Add(periodGroup);
                _unitOfWork.Complete();
                this.SaveItem(periodGroup.Id, this.InitialEvaluationDate(model.PeriodItems));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePeriod();
            return result;
        }

        /// <summary>
        /// Insert period group item list.
        /// </summary>
        /// <param name="periodGroupId">The identity of period group.</param>
        /// <param name="periodItems">The identity of period items.</param>
        private void SaveItem(int periodGroupId, IEnumerable<PeriodItemViewModel> periodItems)
        {
            var data = _mapper.Map<IEnumerable<PeriodItemViewModel>, IEnumerable<PeriodItem>>(periodItems);
            data.Select(c => { c.PeriodId = periodGroupId; return c; }).ToList();
            _unitOfWork.GetRepository<PeriodItem>().AddRange(data);
        }

        /// <summary>
        /// Update period group.
        /// </summary>
        /// <param name="model">The period information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(PeriodViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var periodGroup = _unitOfWork.GetRepository<Period>().GetById(model.Id);
                periodGroup.Year = Convert.ToInt32(model.Year);
                periodGroup.LastModifyBy = _token.EmpNo;
                periodGroup.LastModifyDate = DateTime.Now;
                _unitOfWork.GetRepository<Period>().Update(periodGroup);
                this.EditItem(periodGroup.Id, this.InitialEvaluationDate(model.PeriodItems));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePeriod();
            return result;
        }

        /// <summary>
        /// Update period group items.
        /// </summary>
        /// <param name="periodGroupId">The identity of period group.</param>
        /// <param name="periodItems">The identity of period items.</param>
        private void EditItem(int periodGroupId, IEnumerable<PeriodItemViewModel> periodItems)
        {
            periodItems.Select(c => { c.PeriodID = periodGroupId; return c; }).ToList();
            var data = _unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == periodGroupId);

            var periodItemAdd = periodItems.Where(x => x.Id == 0);
            var periodItemDelete = data.Where(x => !periodItems.Any(y => x.Id == y.Id));

            var periodItemUpdate = _mapper.Map<IEnumerable<PeriodItemViewModel>, IEnumerable<PeriodItem>>(periodItems);
            periodItemUpdate = periodItemUpdate.Where(x => data.Any(y => x.Id == y.Id));

            this.SaveItem(periodGroupId, periodItemAdd);
            this.DeleteItem(periodItemDelete);
            _unitOfWork.GetRepository<PeriodItem>().UpdateRange(periodItemUpdate);
        }

        /// <summary>
        /// Remove period group.
        /// </summary>
        /// <param name="id">The identity of period group.</param>
        /// <returns></returns>
        public ResultViewModel Delete(int id)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                _unitOfWork.GetRepository<Period>().Remove(
                    _unitOfWork.GetRepository<Period>().GetById(id));
                this.DeleteItem(_unitOfWork.GetRepository<PeriodItem>().GetCache(x => x.PeriodId == id));
                _unitOfWork.Complete(scope);
            }
            this.ReloadCachePeriod();
            return result;
        }

        /// <summary>
        /// Remove period group items.
        /// </summary>
        /// <param name="model">The period group items.</param>
        private void DeleteItem(IEnumerable<PeriodItem> model)
        {
            _unitOfWork.GetRepository<PeriodItem>().RemoveRange(model);
        }

        /// <summary>
        /// Reload Cache when period and periodItems is change.
        /// </summary>
        private void ReloadCachePeriod()
        {
            _unitOfWork.GetRepository<Period>().ReCache();
            _unitOfWork.GetRepository<PeriodItem>().ReCache();
        }

        /// <summary>
        /// Initial evaluation string to datetime.
        /// </summary>
        /// <param name="periodItems">The period group items.</param>
        private IEnumerable<PeriodItemViewModel> InitialEvaluationDate(IEnumerable<PeriodItemViewModel> periodItems)
        {
            foreach (var item in periodItems)
            {
                item.StartEvaDate = UtilityService.ConvertToDateTime(item.StartEvaDateString, ConstantValue.DateTimeFormat);
                item.EndEvaDate = UtilityService.ConvertToDateTime(item.EndEvaDateString, ConstantValue.DateTimeFormat);
            }
            return periodItems;
        }

        #endregion

    }
}

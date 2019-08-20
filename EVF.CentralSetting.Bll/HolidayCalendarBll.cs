using AutoMapper;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Data.Pocos;
using EVF.Data.Repository.Interfaces;
using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EVF.CentralSetting.Bll
{
    public class HolidayCalendarBll : IHolidayCalendarBll
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
        /// Initializes a new instance of the <see cref="HolidayCalendarBll" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        /// <param name="mapper">The auto mapper.</param>
        /// <param name="token">The ClaimsIdentity in token management.</param>
        public HolidayCalendarBll(IUnitOfWork unitOfWork, IMapper mapper, IManageToken token)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _token = token;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get HolidayCalendar year group list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HolidayCalendarViewModel> GetList()
        {
            var yearGroup = _unitOfWork.GetRepository<HolidayCalendar>().GetCache().Select(x => x.Year).Distinct().ToList();
            var result = new List<HolidayCalendarViewModel>();
            foreach (var item in yearGroup)
            {
                result.Add(new HolidayCalendarViewModel { Year = item });
            }
            return result;
        }

        /// <summary>
        /// Get Detail of HolidayCalendar year.
        /// </summary>
        /// <param name="year">The target holiday year.</param>
        /// <returns></returns>
        public HolidayCalendarViewModel GetDetail(string year)
        {
            var result = new HolidayCalendarViewModel { Year = year };
            var data = _unitOfWork.GetRepository<HolidayCalendar>().GetCache(x => x.Year == year);
            foreach (var item in data)
            {
                result.HolidayList.Add(new HolidayCalendarViewModel.HolidayCalendarDetail
                {
                    HolidayDateString = item.HolidayDate.Value.ToString(ConstantValue.DateTimeFormat),
                    Description = item.Description
                });
            }
            return result;
        }

        /// <summary>
        /// Insert new holiday canlendar.
        /// </summary>
        /// <param name="model">The holiday calendar information value.</param>
        /// <returns></returns>
        public ResultViewModel Save(HolidayCalendarViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {
                var data = new List<HolidayCalendar>();
                foreach (var item in model.HolidayList)
                {
                    data.Add(new HolidayCalendar
                    {
                        Year = model.Year,
                        HolidayDate = UtilityService.ConvertToDateTime(item.HolidayDateString, ConstantValue.DateTimeFormat),
                        Description = item.Description,
                        CreateBy = _token.EmpNo,
                        CreateDate = DateTime.Now
                    });
                }
                _unitOfWork.GetRepository<HolidayCalendar>().AddRange(data);
                _unitOfWork.Complete(scope);
            }
            this.ReloadCacheHolidayCalendar();
            return result;
        }

        /// <summary>
        /// Update holiday calendar year.
        /// </summary>
        /// <param name="model">The holiday calendar information value.</param>
        /// <returns></returns>
        public ResultViewModel Edit(HolidayCalendarViewModel model)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {

            }
            this.ReloadCacheHolidayCalendar();
            return result;
        }

        /// <summary>
        /// Remove holiday calendar year group.
        /// </summary>
        /// <param name="year">The target year holiday.</param>
        /// <returns></returns>
        public ResultViewModel Delete(string year)
        {
            var result = new ResultViewModel();
            using (TransactionScope scope = new TransactionScope())
            {

            }
            this.ReloadCacheHolidayCalendar();
            return result;
        }

        /// <summary>
        /// Reload Cache when HolidayCalendar is change.
        /// </summary>
        private void ReloadCacheHolidayCalendar()
        {
            _unitOfWork.GetRepository<HolidayCalendar>().ReCache();
        }

        #endregion

    }
}

using EVF.Bll.Models;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Bll.Interfaces
{
    public interface IHolidayCalendarBll
    {
        /// <summary>
        /// Get HolidayCalendar year group list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HolidayCalendarViewModel> GetList();
        /// <summary>
        /// Get Detail of HolidayCalendar year.
        /// </summary>
        /// <param name="year">The target holiday year.</param>
        /// <returns></returns>
        HolidayCalendarViewModel GetDetail(string year);
        /// <summary>
        /// Insert new holiday canlendar.
        /// </summary>
        /// <param name="model">The holiday calendar information value.</param>
        /// <returns></returns>
        ResultViewModel Save(HolidayCalendarViewModel model);
        /// <summary>
        /// Update holiday calendar year.
        /// </summary>
        /// <param name="model">The holiday calendar information value.</param>
        /// <returns></returns>
        ResultViewModel Edit(HolidayCalendarViewModel model);
        /// <summary>
        /// Remove holiday calendar year group.
        /// </summary>
        /// <param name="year">The target year holiday.</param>
        /// <returns></returns>
        ResultViewModel Delete(string year);
    }
}

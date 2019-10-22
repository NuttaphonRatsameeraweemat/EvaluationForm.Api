using System.Net;
using System.Text.RegularExpressions;
using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = HolidayCalendarViewModel.RoleForManageData)]
    public class HolidayCalendarController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The performance group manager provides performance group functionality.
        /// </summary>
        private readonly IHolidayCalendarBll _holidayCalendar;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="HolidayCalendarController" /> class.
        /// </summary>
        /// <param name="performance"></param>
        public HolidayCalendarController(IHolidayCalendarBll holidayCalendar)
        {
            _holidayCalendar = holidayCalendar;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_holidayCalendar.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(string year)
        {
            IActionResult response;
            if (string.IsNullOrEmpty(year) || !Regex.IsMatch(year, ConstantValue.RegexYearFormat))
            {
                response = BadRequest(UtilityService.InitialResultError(ConstantValue.YearIncorrectFormat, (int)HttpStatusCode.BadRequest));
            }
            else response = Ok(_holidayCalendar.GetDetail(year));
            return response;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]HolidayCalendarViewModel model)
        {
            return Ok(_holidayCalendar.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]HolidayCalendarViewModel model)
        {
            return Ok(_holidayCalendar.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody]HolidayDeleteRequestModel model)
        {
            return Ok(_holidayCalendar.Delete(model));
        }

        #endregion

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using EVF.Bll.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = HolidayCalendarViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
                response = BadRequest(ConstantValue.YearIncorrectFormat);
            }
            else response = Ok(_holidayCalendar.GetDetail(year));
            return response;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(HolidayCalendarViewModel model)
        {
            return Ok(_holidayCalendar.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(HolidayCalendarViewModel model)
        {
            return Ok(_holidayCalendar.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(string year)
        {
            IActionResult response;
            if (!Regex.IsMatch(year, ConstantValue.RegexYearFormat))
            {
                response = BadRequest(ConstantValue.YearIncorrectFormat);
            }
            else response = Ok(_holidayCalendar.Delete(year));
            return response;
        }

        #endregion

    }
}
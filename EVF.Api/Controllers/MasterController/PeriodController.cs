﻿using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.MasterController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PeriodController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The period manager provides period functionality.
        /// </summary>
        private readonly IPeriodBll _period;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PeriodController" /> class.
        /// </summary>
        /// <param name="period"></param>
        public PeriodController(IPeriodBll period)
        {
            _period = period;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_period.GetList());
        }

        [HttpGet]
        [Route("GetListAll")]
        public IActionResult GetListAll()
        {
            return Ok(_period.GetListAll());
        }

        [HttpGet]
        [Route("GetYear")]
        public IActionResult GetYear()
        {
            return Ok(_period.GetYear());
        }

        [HttpGet]
        [Route("GetAllPeriodByYear")]
        public IActionResult GetAllPeriodByYear(int[] years)
        {
            return Ok(_period.GetAllPeriodByYear(years));
        }

        [HttpGet]
        [Route("GetListInformation")]
        public IActionResult GetListInformation()
        {
            return Ok(_period.GetListInformation());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_period.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = PeriodViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]PeriodViewModel model)
        {
            return Ok(_period.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = PeriodViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]PeriodViewModel model)
        {
            return Ok(_period.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = PeriodViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            return Ok(_period.Delete(id));
        }

        #endregion

    }
}
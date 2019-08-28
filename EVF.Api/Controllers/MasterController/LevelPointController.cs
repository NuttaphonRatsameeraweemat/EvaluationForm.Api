using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.MasterController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = LevelPointViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class LevelPointController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The level point manager provides level point functionality.
        /// </summary>
        private readonly ILevelPointBll _levelPoint;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="LevelPointController" /> class.
        /// </summary>
        /// <param name="grade"></param>
        public LevelPointController(ILevelPointBll levelPoint)
        {
            _levelPoint = levelPoint;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_levelPoint.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_levelPoint.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]LevelPointViewModel model)
        {
            return Ok(_levelPoint.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]LevelPointViewModel model)
        {
            return Ok(_levelPoint.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_levelPoint.Delete(id));
        }

        #endregion

    }
}
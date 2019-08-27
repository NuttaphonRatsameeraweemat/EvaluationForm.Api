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
    [Authorize(Roles = GradeViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class GradeController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The period manager provides period functionality.
        /// </summary>
        private readonly IGradeBll _grade;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="GradeController" /> class.
        /// </summary>
        /// <param name="grade"></param>
        public GradeController(IGradeBll grade)
        {
            _grade = grade;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_grade.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_grade.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]GradeViewModel model)
        {
            var response = _grade.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_grade.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]GradeViewModel model)
        {
            var response = _grade.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_grade.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_grade.Delete(id));
        }

        #endregion

    }
}
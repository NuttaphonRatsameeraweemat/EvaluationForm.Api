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
    [Authorize(Roles = CriteriaViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CriteriaController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The criteria manager provides criteria functionality.
        /// </summary>
        private readonly ICriteriaBll _criteria;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CriteriaController" /> class.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaController(ICriteriaBll criteria)
        {
            _criteria = criteria;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_criteria.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_criteria.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]CriteriaViewModel model)
        {
            var response = _criteria.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_criteria.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]CriteriaViewModel model)
        {
            var response = _criteria.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_criteria.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_criteria.Delete(id));
        }

        #endregion

    }
}
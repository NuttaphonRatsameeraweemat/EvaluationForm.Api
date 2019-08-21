using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Hr.Bll.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.HrController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HrEmployeeController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The hr company manager provides hr company functionality.
        /// </summary>
        private readonly IHrEmployeeBll _hrEmployee;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="HrEmployeeController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public HrEmployeeController(IHrEmployeeBll hrEmployee)
        {
            _hrEmployee = hrEmployee;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_hrEmployee.GetList());
        }

        [HttpGet]
        [Route("GetListByComCode")]
        public IActionResult GetListByComCode(string comCode)
        {
            return Ok(_hrEmployee.GetListByComCode(comCode));
        }

        [HttpGet]
        [Route("GetListByOrg")]
        public IActionResult GetListByOrg(string orgId)
        {
            return Ok(_hrEmployee.GetListByOrg(orgId));
        }

        #endregion


    }
}
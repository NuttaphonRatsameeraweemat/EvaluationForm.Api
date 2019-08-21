﻿using EVF.Hr.Bll.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.HrController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HrCompanyController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The hr company manager provides hr company functionality.
        /// </summary>
        private readonly IHrCompanyBll _hrCompany;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="HrCompanyController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public HrCompanyController(IHrCompanyBll hrCompany)
        {
            _hrCompany = hrCompany;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_hrCompany.GetList());
        }

        #endregion

    }
}
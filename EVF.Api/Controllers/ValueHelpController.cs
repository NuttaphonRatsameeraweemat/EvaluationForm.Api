using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Bll.Components;
using EVF.Bll.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ValueHelpController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The value help manager provides value help functionality.
        /// </summary>
        private readonly IValueHelpBll _valueHelp;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValueHelpController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public ValueHelpController(IValueHelpBll valueHelp)
        {
            _valueHelp = valueHelp;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetTypeProject")]
        public IActionResult GetTypeProject()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeActiveStatus));
        }

        #endregion

    }
}
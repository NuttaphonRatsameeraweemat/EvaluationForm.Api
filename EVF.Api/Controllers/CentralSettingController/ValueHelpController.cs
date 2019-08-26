using EVF.CentralSetting.Bll.Interfaces;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        [Route("GetActiveStatus")]
        public IActionResult GetActiveStatus()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeActiveStatus));
        }

        [HttpGet]
        [Route("GetSapScoreFields")]
        public IActionResult GetSapScoreFields()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeSAPScoreFields));
        }

        [HttpGet]
        [Route("GetPeriodRound")]
        public IActionResult GetPeriodRound()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypePeriodRound));
        }

        #endregion

    }
}
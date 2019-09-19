using EVF.Hr.Bll.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.HrController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class HrOrgController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The hr organization manager provides hr organization functionality.
        /// </summary>
        private readonly IHrOrgBll _hrOrg;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="HrOrgController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public HrOrgController(IHrOrgBll hrOrg)
        {
            _hrOrg = hrOrg;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_hrOrg.GetList());
        }

        [HttpGet]
        [Route("GetListByComCode")]
        public IActionResult GetListByComCode(string comCode)
        {
            return Ok(_hrOrg.GetListByComCode(comCode));
        }

        #endregion

    }
}
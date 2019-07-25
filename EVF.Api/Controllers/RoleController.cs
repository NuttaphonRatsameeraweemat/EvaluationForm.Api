using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Role manager provides Role functionality.
        /// </summary>
        private readonly IRoleBll _role;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="RoleController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public RoleController(IRoleBll role)
        {
            _role = role;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetMenuByCode")]
        public IActionResult GetMenuByCode(string menuCode)
        {
            return Ok();
        }

        #endregion

    }
}
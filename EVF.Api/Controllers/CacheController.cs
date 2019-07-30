using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Bll.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class CacheController : ControllerBase
    {
        [HttpPost]
        public IActionResult InitialCache()
        {
            return Ok();
        }
    }
}
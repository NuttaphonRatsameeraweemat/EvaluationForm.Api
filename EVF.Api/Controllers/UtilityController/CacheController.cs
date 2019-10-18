using EVF.Utility.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class CacheController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The cache manager provides cache functionality.
        /// </summary>
        private readonly ICacheBll _cache;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheController" /> class.
        /// </summary>
        /// <param name="vendor"></param>
        public CacheController(ICacheBll cache)
        {
            _cache = cache;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        public IActionResult InitialCache()
        {
            return Ok(_cache.ReInitialCache());
        }

        #endregion


    }
}
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = PerformanceGroupViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PerformanceGroupController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The performance group manager provides performance group functionality.
        /// </summary>
        private readonly IPerformanceGroupBll _performanceGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PerformanceGroupController" /> class.
        /// </summary>
        /// <param name="performance"></param>
        public PerformanceGroupController(IPerformanceGroupBll performance)
        {
            _performanceGroup = performance;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_performanceGroup.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_performanceGroup.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(PerformanceGroupViewModel model)
        {
            return Ok(_performanceGroup.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(PerformanceGroupViewModel model)
        {
            return Ok(_performanceGroup.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_performanceGroup.Delete(id));
        }

        #endregion

    }
}
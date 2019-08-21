using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = PerformanceViewModel.RoleForManageData, AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PerformanceController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The performance manager provides performance functionality.
        /// </summary>
        private readonly IPerformanceBll _performance;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PerformanceController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public PerformanceController(IPerformanceBll performance)
        {
            _performance = performance;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_performance.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_performance.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(PerformanceViewModel model)
        {
            return Ok(_performance.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(PerformanceViewModel model)
        {
            return Ok(_performance.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_performance.Delete(id));
        }

        #endregion


    }
}
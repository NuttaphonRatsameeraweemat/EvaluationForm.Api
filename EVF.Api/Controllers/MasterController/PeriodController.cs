using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.MasterController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = PeriodViewModel.RoleForManageData, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeriodController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The period manager provides period functionality.
        /// </summary>
        private readonly IPeriodBll _period;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="PeriodController" /> class.
        /// </summary>
        /// <param name="period"></param>
        public PeriodController(IPeriodBll period)
        {
            _period = period;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_period.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_period.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]PeriodViewModel model)
        {
            return Ok(_period.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]PeriodViewModel model)
        {
            return Ok(_period.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_period.Delete(id));
        }

        #endregion

    }
}
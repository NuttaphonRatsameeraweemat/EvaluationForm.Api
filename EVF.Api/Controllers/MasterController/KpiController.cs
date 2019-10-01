using EVF.Helper;
using EVF.Helper.Components;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = KpiViewModel.RoleForManageData)]
    public class KpiController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The kpi manager provides kpi functionality.
        /// </summary>
        private readonly IKpiBll _kpi;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="KpiController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public KpiController(IKpiBll kpi)
        {
            _kpi = kpi;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_kpi.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_kpi.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]KpiViewModel model)
        {
            return Ok(_kpi.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]KpiViewModel model)
        {
            IActionResult response;
            if (_kpi.IsUse(model.Id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.KpiMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_kpi.Edit(model));
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_kpi.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.KpiMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_kpi.Delete(id));
            return response;
        }

        #endregion


    }
}
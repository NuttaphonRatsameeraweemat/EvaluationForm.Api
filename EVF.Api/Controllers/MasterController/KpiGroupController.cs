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
    [Authorize(Roles = KpiGroupViewModel.RoleForManageData)]
    public class KpiGroupController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The kpi group manager provides kpi group functionality.
        /// </summary>
        private readonly IKpiGroupBll _kpiGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="KpiGroupController" /> class.
        /// </summary>
        /// <param name="kpi"></param>
        public KpiGroupController(IKpiGroupBll kpi)
        {
            _kpiGroup = kpi;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_kpiGroup.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_kpiGroup.GetDetail(id));
        }

        [HttpGet]
        [Route("GetKpiIteDisplayCriteria")]
        public IActionResult GetKpiIteDisplayCriteria(int id)
        {
            return Ok(_kpiGroup.GetKpiItemDisplayCriteria(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]KpiGroupViewModel model)
        {
            IActionResult response;
            var result = _kpiGroup.ValidateData();
            if (result.IsError)
            {
                response = BadRequest(result);
            }
            else response = Ok(_kpiGroup.Save(model));
            return response;
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]KpiGroupViewModel model)
        {
            IActionResult response;
            if (_kpiGroup.IsUse(model.Id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.KpiMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_kpiGroup.Edit(model));
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_kpiGroup.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.KpiGroupMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_kpiGroup.Delete(id));
            return response;
        }

        #endregion

    }
}
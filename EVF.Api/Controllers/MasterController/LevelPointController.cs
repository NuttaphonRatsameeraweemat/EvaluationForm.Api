using EVF.Helper;
using EVF.Helper.Components;
using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.MasterController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = LevelPointViewModel.RoleForManageData)]
    public class LevelPointController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The level point manager provides level point functionality.
        /// </summary>
        private readonly ILevelPointBll _levelPoint;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="LevelPointController" /> class.
        /// </summary>
        /// <param name="grade"></param>
        public LevelPointController(ILevelPointBll levelPoint)
        {
            _levelPoint = levelPoint;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_levelPoint.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_levelPoint.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]LevelPointViewModel model)
        {
            return Ok(_levelPoint.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]LevelPointViewModel model)
        {
            IActionResult response;
            if (_levelPoint.IsUse(model.Id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.LevelPointMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_levelPoint.Edit(model));
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_levelPoint.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.LevelPointMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_levelPoint.Delete(id));
            return response;
        }

        #endregion

    }
}
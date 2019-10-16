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
    [Authorize]
    public class CriteriaController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The criteria manager provides criteria functionality.
        /// </summary>
        private readonly ICriteriaBll _criteria;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CriteriaController" /> class.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaController(ICriteriaBll criteria)
        {
            _criteria = criteria;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_criteria.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_criteria.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = CriteriaViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]CriteriaViewModel model)
        {
            var response = _criteria.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_criteria.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = CriteriaViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]CriteriaViewModel model)
        {
            IActionResult response;
            if (_criteria.IsUse(model.Id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.CriteriaMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else
            {
                var validate = _criteria.ValidateData(model);
                if (validate.IsError)
                {
                    response = BadRequest(validate);
                }
                else response = Ok(_criteria.Edit(model));
            }
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = CriteriaViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_criteria.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.CriteriaMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_criteria.Delete(id));
            return response;
        }

        #endregion

    }
}
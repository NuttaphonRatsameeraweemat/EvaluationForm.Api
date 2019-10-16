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
    public class GradeController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The grade manager provides grade functionality.
        /// </summary>
        private readonly IGradeBll _grade;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="GradeController" /> class.
        /// </summary>
        /// <param name="grade"></param>
        public GradeController(IGradeBll grade)
        {
            _grade = grade;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_grade.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_grade.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = GradeViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]GradeViewModel model)
        {
            var response = _grade.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_grade.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = GradeViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]GradeViewModel model)
        {
            IActionResult response;
            if (_grade.IsUse(model.Id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.GradeMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else
            {
                var validate = _grade.ValidateData(model);
                if (validate.IsError)
                {
                    response = BadRequest(validate);
                }
                else response = Ok(_grade.Edit(model));
            }
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = GradeViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_grade.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.CriteriaMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_grade.Delete(id));
            return response;
        }

        #endregion

    }
}
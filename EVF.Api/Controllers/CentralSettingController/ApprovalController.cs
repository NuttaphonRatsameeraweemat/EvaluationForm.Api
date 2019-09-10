using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.CentralSettingController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = ApprovalViewModel.RoleForManageData, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApprovalController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The approval manager provides approval functionality.
        /// </summary>
        private readonly IApprovalBll _approval;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="ApprovalController" /> class.
        /// </summary>
        /// <param name="approval"></param>
        public ApprovalController(IApprovalBll approval)
        {
            _approval = approval;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_approval.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        { 
            return Ok(_approval.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]ApprovalViewModel model)
        {
            return Ok(_approval.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]ApprovalViewModel model)
        {
            return Ok(_approval.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_approval.Delete(id));
        }

        #endregion

    }
}
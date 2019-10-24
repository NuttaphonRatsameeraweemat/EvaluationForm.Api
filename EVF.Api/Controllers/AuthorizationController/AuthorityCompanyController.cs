using EVF.Authorization.Bll.Interfaces;
using EVF.Authorization.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = AuthorityCompanyViewModel.RoleForManageData)]
    public class AuthorityCompanyController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Authority Company manager provides Authority Company functionality.
        /// </summary>
        private readonly IAuthorityCompanyBll _authorityCompany;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="AuthorityCompanyController" /> class.
        /// </summary>
        /// <param name="role"></param>
        public AuthorityCompanyController(IAuthorityCompanyBll authorityCompany)
        {
            _authorityCompany = authorityCompany;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_authorityCompany.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(string adUser)
        {
            IActionResult response;
            if (string.IsNullOrEmpty(adUser))
            {
                response = BadRequest(UtilityService.InitialResultError(ConstantValue.ArgullmentNullOrEmptyMessage, (int)HttpStatusCode.BadRequest));
            }
            else response = Ok(_authorityCompany.GetDetail(adUser));
            return response;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]AuthorityCompanyViewModel model)
        {
            return Ok(_authorityCompany.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]AuthorityCompanyViewModel model)
        {
            return Ok(_authorityCompany.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete([FromBody]AuthorityCompanyRequestViewModel model)
        {
            return Ok(_authorityCompany.Delete(model.AdUser));
        }

        #endregion

    }
}
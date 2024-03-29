﻿using EVF.Helper;
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
    public class EvaluationTemplateController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluationTemplate manager provides evaluationTemplate functionality.
        /// </summary>
        private readonly IEvaluationTemplateBll _evaluationTemplate;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationTemplateController" /> class.
        /// </summary>
        /// <param name="evaluationTemplate"></param>
        public EvaluationTemplateController(IEvaluationTemplateBll evaluationTemplate)
        {
            _evaluationTemplate = evaluationTemplate;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_evaluationTemplate.GetList());
        }

        [HttpGet]
        [Route("GetListByWeightingKey")]
        public IActionResult GetListByWeightingKey(string weightingKey, string purchaseOrg)
        {
            return Ok(_evaluationTemplate.GetListByWeightingKey(weightingKey, purchaseOrg));
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_evaluationTemplate.GetDetail(id));
        }

        [HttpGet]
        [Route("GetTemplate")]
        public IActionResult GetTemplate(int id)
        {
            return Ok(_evaluationTemplate.LoadTemplate(id));
        }

        [HttpGet]
        [Route("PreviewTemplate")]
        public IActionResult PreviewTemplate([FromQuery]EvaluationTemplatePreviewRequestModel model)
        {
            return Ok(_evaluationTemplate.PreviewTemplate(model));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = EvaluationTemplateViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]EvaluationTemplateViewModel model)
        {
            var response = _evaluationTemplate.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_evaluationTemplate.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = EvaluationTemplateViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]EvaluationTemplateViewModel model)
        {
            var response = _evaluationTemplate.ValidateData(model);
            if (response.IsError)
            {
                return BadRequest(response);
            }
            else return Ok(_evaluationTemplate.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = EvaluationTemplateViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            IActionResult response;
            if (_evaluationTemplate.IsUse(id))
            {
                response = BadRequest(UtilityService.InitialResultError(string.Format(MessageValue.IsUseMessageFormat, MessageValue.EvaluationTemplateMessage),
                                      (int)System.Net.HttpStatusCode.BadRequest));
            }
            else response = Ok(_evaluationTemplate.Delete(id));
            return response;
        }

        #endregion

    }
}
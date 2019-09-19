using EVF.Master.Bll.Interfaces;
using EVF.Master.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.MasterController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluationTemplateViewModel.RoleForManageData)]
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

        [HttpPost]
        [Route("Save")]
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
        public IActionResult Delete(int id)
        {
            return Ok(_evaluationTemplate.Delete(id));
        }

        #endregion

    }
}
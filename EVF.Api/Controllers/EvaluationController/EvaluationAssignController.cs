using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EvaluationController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EvaluationAssignController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation assign manager provides evaluation assign functionality.
        /// </summary>
        private readonly IEvaluationAssignBll _evaluationAssign;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationAssignController" /> class.
        /// </summary>
        /// <param name="evaluationAssign"></param>
        public EvaluationAssignController(IEvaluationAssignBll evaluationAssign)
        {
            _evaluationAssign = evaluationAssign;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetEvaluators")]
        public IActionResult GetEvaluators(int evaluationId)
        {
            return Ok(_evaluationAssign.GetEvaluators(evaluationId));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = EvaluationAssignViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]EvaluationAssignRequestViewModel model)
        {
            IActionResult response;
            var result = _evaluationAssign.ValidateData(model);
            if (result.IsError)
            {
                response = BadRequest(result);
            }
            else response = Ok(_evaluationAssign.Save(model));
            return response;
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = EvaluationAssignViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]EvaluationAssignRequestViewModel model)
        {
            IActionResult response;
            var result = _evaluationAssign.ValidateData(model);
            if (result.IsError)
            {
                response = BadRequest(result);
            }
            else response = Ok(_evaluationAssign.Edit(model));
            return response;
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = EvaluationAssignViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            return Ok(_evaluationAssign.Delete(id));
        }

        #endregion

    }
}
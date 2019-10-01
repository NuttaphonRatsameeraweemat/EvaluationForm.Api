using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EvaluationController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluationAssignViewModel.RoleForManageData)]
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
        public IActionResult Save([FromBody]EvaluationAssignRequestViewModel model)
        {
            return Ok(_evaluationAssign.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]EvaluationAssignRequestViewModel model)
        {
            return Ok(_evaluationAssign.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_evaluationAssign.Delete(id));
        }

        #endregion

    }
}
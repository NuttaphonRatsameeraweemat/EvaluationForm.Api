using EVF.Workflow.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.WorkflowController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowActivityController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflowActivityBll _workflowActivity;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="WorkflowActivityController" /> class.
        /// </summary>
        /// <param name="workflow"></param>
        public WorkflowActivityController(IWorkflowActivityBll workflowActivity)
        {
            _workflowActivity = workflowActivity;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetWorkflowActivity/{evaluationId}")]
        public IActionResult GetWorkflowActivity(int evaluationId)
        {
            return Ok(_workflowActivity.GetWorkflowActivity(evaluationId));
        }

        #endregion

    }
}
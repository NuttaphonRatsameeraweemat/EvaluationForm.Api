using EVF.Workflow.Bll.Interfaces;
using EVF.Workflow.Bll.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.WorkflowController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowDelegateController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflowDelegateBll _workflowDelegate;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="WorkflowDelegateController" /> class.
        /// </summary>
        /// <param name="workflow"></param>
        public WorkflowDelegateController(IWorkflowDelegateBll workflowDelegate)
        {
            _workflowDelegate = workflowDelegate;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_workflowDelegate.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_workflowDelegate.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]WorkflowDelegateViewModel model)
        {
            return Ok(_workflowDelegate.SaveDelegate(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]WorkflowDelegateViewModel model)
        {
            return Ok(_workflowDelegate.UpdateDelegate(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_workflowDelegate.RemoveDelegate(id));
        }

        #endregion


    }
}
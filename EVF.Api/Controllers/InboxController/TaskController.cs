using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Inbox.Bll.Interfaces;
using EVF.Inbox.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EVF.Api.Controllers.InboxController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Task manager provides Task functionality.
        /// </summary>
        private readonly ITaskBll _task;
        /// <summary>
        /// The Task Action manager provides Task Action functionality.
        /// </summary>
        private readonly ITaskActionBll _taskAction;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="TaskController" /> class.
        /// </summary>
        /// <param name="task">The Task manager provides Task functionality.</param>
        /// <param name="taskAction">The Task Action manager provides Task Action functionality.</param>
        public TaskController(ITaskBll task, ITaskActionBll taskAction, IManageToken token)
        {
            _task = task;
            _taskAction = taskAction;
            _token = token;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetTaskList")]
        public IActionResult GetTaskList()
        {
            return Ok(_task.GetTaskList(_token.AdUser));
        }

        [HttpGet]
        [Route("GetTaskListDelegate/{fromUser}")]
        public IActionResult GetTaskListDelegate(string fromUser)
        {
            return Ok(_task.GetTaskList(fromUser));
        }

        [HttpPost]
        [Route("ApproveTask")]
        public IActionResult ApproveTask(TaskActionViewModel model)
        {
            return Ok(_taskAction.ActionTask(model,ConstantValue.WorkflowActionApprove));
        }

        [HttpPost]
        [Route("RejectTask")]
        public IActionResult RejectTask(TaskActionViewModel model)
        {
            return Ok(_taskAction.ActionTask(model, ConstantValue.WorkflowActionReject));
        }

        [HttpPost]
        [Route("ApproveMultiTask")]
        public IActionResult ApproveMultiTask(IEnumerable<TaskActionViewModel> models)
        {
            return Ok(_taskAction.ActionMultiTask(models, ConstantValue.WorkflowActionApprove));
        }

        [HttpPost]
        [Route("RejectMultiTask")]
        public IActionResult RejectMultiTask(IEnumerable<TaskActionViewModel> models)
        {
            return Ok(_taskAction.ActionMultiTask(models, ConstantValue.WorkflowActionReject));
        }

        #endregion

    }
}
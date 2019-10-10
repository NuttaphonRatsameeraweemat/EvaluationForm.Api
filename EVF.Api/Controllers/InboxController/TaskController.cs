using EVF.Helper.Components;
using EVF.Inbox.Bll.Interfaces;
using EVF.Inbox.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="TaskController" /> class.
        /// </summary>
        /// <param name="task">The Task manager provides Task functionality.</param>
        /// <param name="taskAction">The Task Action manager provides Task Action functionality.</param>
        public TaskController(ITaskBll task, ITaskActionBll taskAction)
        {
            _task = task;
            _taskAction = taskAction;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetTaskList")]
        public IActionResult GetTaskList()
        {
            return Ok(_task.GetTaskList());
        }

        [HttpGet]
        [Route("GetTaskListDelegate/{fromUser}")]
        public IActionResult GetTaskListDelegate(string fromUser)
        {
            return Ok(_task.GetTaskListDelegate(fromUser));
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

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Email.Bll.Interfaces;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EmailController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SummaryEmailTaskController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The summary email task manager provides summary email task functionality.
        /// </summary>
        private readonly ISummaryEmailTaskBll _summaryEmailTask;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="SummaryEmailTaskController" /> class.
        /// </summary>
        /// <param name="evaluationSummaryReport"></param>
        public SummaryEmailTaskController(ISummaryEmailTaskBll summaryEmailTask)
        {
            _summaryEmailTask = summaryEmailTask;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("ExecuteEmailTaskWaiting")]
        public IActionResult ExecuteEmailTaskWaiting()
        {
            _summaryEmailTask.ExecuteEmailTaskWaiting(ConstantValue.EmailTaskStatusWaiting);
            return Ok();
        }

        [HttpPost]
        [Route("ExecuteEmailTaskError")]
        public IActionResult ExecuteEmailTaskError()
        {
            _summaryEmailTask.ExecuteEmailTaskWaiting(ConstantValue.EmailTaskStatusError);
            return Ok();
        }

        [HttpPost]
        [Route("ProcessSummaryTask")]
        public IActionResult ProcessSummaryTask()
        {
            _summaryEmailTask.ProcessSummaryTask();
            return Ok();
        }

        [HttpPost]
        [Route("ProcessSummaryTaskEvaWaiting")]
        public IActionResult ProcessSummaryTaskEvaWaiting()
        {
            _summaryEmailTask.ProcessSummaryTaskEvaWaiting();
            return Ok();
        }

        [HttpPost]
        [Route("ProcessSummaryTaskReject")]
        public IActionResult ProcessSummaryTaskReject()
        {
            _summaryEmailTask.ProcessSummaryTaskReject();
            return Ok();
        }

        #endregion

    }
}
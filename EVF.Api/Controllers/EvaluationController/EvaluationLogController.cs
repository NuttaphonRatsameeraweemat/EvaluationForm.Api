﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EvaluationController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluationViewModel.RoleForManageData)]
    public class EvaluationLogController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation manager provides evaluation functionality.
        /// </summary>
        private readonly IEvaluationLogBll _evaluationLog;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationLogController" /> class.
        /// </summary>
        /// <param name="evaluationLog"></param>
        public EvaluationLogController(IEvaluationLogBll evaluationLog)
        {
            _evaluationLog = evaluationLog;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetEvaluationLog")]
        public IActionResult GetEvaluationLog(int evaluationId)
        {
            return Ok(_evaluationLog.GetEvaluationLog(evaluationId));
        }

        [HttpGet]
        [Route("GetListEvaluationLogs")]
        public IActionResult GetListEvaluationLogs(int evaluationId)
        {
            return Ok(_evaluationLog.GetListEvaluationLogs(evaluationId));
        }

        [HttpPost]
        [Route("Save/{evaluationId}")]
        public IActionResult Save(int evaluationId, [FromBody]IEnumerable<EvaluationLogItemViewModel> model)
        {
            return Ok(_evaluationLog.Save(evaluationId, model));
        }

        #endregion

    }
}
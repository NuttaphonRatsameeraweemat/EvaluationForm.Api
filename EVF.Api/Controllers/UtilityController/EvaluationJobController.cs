using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVF.Helper.Components;
using EVF.Utility.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.UtilityController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class EvaluationJobController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation job manager provides evaluation job functionality.
        /// </summary>
        private readonly IEvaluationJobBll _evaluationJob;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheController" /> class.
        /// </summary>
        /// <param name="evaluationJob"></param>
        public EvaluationJobController(IEvaluationJobBll evaluationJob)
        {
            _evaluationJob = evaluationJob;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        public IActionResult ExecuteEvaluationProcess()
        {
            return Ok(_evaluationJob.ExecuteEvaluationProcess());
        }

        #endregion

    }
}
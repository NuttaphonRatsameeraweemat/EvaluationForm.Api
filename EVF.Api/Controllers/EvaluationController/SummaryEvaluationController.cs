using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EvaluationController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluationViewModel.RoleForManageData)]
    public class SummaryEvaluationController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation manager provides evaluation functionality.
        /// </summary>
        private readonly ISummaryEvaluationBll _summaryEva;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="SummaryEvaluationController" /> class.
        /// </summary>
        /// <param name="summaryEva"></param>
        public SummaryEvaluationController(ISummaryEvaluationBll summaryEva)
        {
            _summaryEva = summaryEva;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_summaryEva.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_summaryEva.GetDetail(id));
        }

        #endregion

    }
}
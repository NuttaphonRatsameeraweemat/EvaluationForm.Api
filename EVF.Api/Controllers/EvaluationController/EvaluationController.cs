using EVF.Evaluation.Bll.Interfaces;
using EVF.Evaluation.Bll.Models;
using EVF.Helper;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.EvaluationController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluationViewModel.RoleForManageData)]
    public class EvaluationController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The evaluation manager provides evaluation functionality.
        /// </summary>
        private readonly IEvaluationBll _evaluation;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationController" /> class.
        /// </summary>
        /// <param name="evaluation"></param>
        public EvaluationController(IEvaluationBll evaluation)
        {
            _evaluation = evaluation;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_evaluation.GetList());
        }

        [HttpGet]
        [Route("GetListHistory")]
        public IActionResult GetListHistory()
        {
            return Ok(_evaluation.GetListHistory());
        }

        [HttpGet]
        [Route("GetImages")]
        public IActionResult GetImages(int id)
        {
            return Ok(UtilityService.GetImages(id, ConstantValue.EvaluationProcessCode));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]EvaluationRequestViewModel model)
        {
            IActionResult response;
            var result = _evaluation.ValidateData(model);
            if (result.IsError)
            {
                response = BadRequest(result);
            }
            else response = Ok(_evaluation.Save(model));
            return response;
        }

        [HttpPost]
        [Route("RejectTask")]
        public IActionResult RejectTask([FromBody]EvaluationRejectViewModel model)
        {
            IActionResult response;
            var result = _evaluation.Reject(model);
            if (result.IsError)
            {
                response = BadRequest(result);
            }
            else response = Ok(result);
            return response;
        }

        #endregion

    }
}
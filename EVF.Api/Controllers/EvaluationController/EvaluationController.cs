using System;
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
        
        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]EvaluationRequestViewModel model)
        {
            return Ok(_evaluation.Save(model));
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
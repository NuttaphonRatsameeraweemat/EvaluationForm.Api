using EVF.Data.Pocos;
using EVF.Helper.Components;
using EVF.Tranfer.Service.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EVF.Tranfer.Service.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "EVF", AuthenticationSchemes = ConstantValue.BasicAuthentication)]
    public class TranferController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The tranfer to datamart manager provides tranfer to datamart functionality.
        /// </summary>
        private readonly ITranferBll _tranfer;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="TranferController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public TranferController(ITranferBll tranfer)
        {
            _tranfer = tranfer;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("TranferToDataMart")]
        public IActionResult TranferToDataMart()
        {
            return Ok(_tranfer.TranferToDataMart());
        }

        [HttpPost]
        [Route("TryToInsertSapResult")]
        public IActionResult TryToInsertSapResult(IEnumerable<EvaluationSapResult> model)
        {
            return Ok(_tranfer.TryToInsertSapResult(model));
        }

        [HttpGet]
        [Route("TryToConnect")]
        public IActionResult TryToConnect()
        {
            return Ok(_tranfer.TryToConnect());
        }

        [HttpGet]
        [Route("GetEvaluationSapResult")]
        public IActionResult GetEvaluationSapResult()
        {
            return Ok(_tranfer.GetEvaluationSapResult());
        }

        #endregion
    }
}
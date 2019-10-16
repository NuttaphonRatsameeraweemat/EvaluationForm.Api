using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.CentralSettingController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EvaluationPercentageConfigController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The EvaluatorGroup manager provides EvaluatorGroup functionality.
        /// </summary>
        private readonly IEvaluationPercentageConfigBll _evaConfig;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluationPercentageConfigController" /> class.
        /// </summary>
        /// <param name="evaConfig"></param>
        public EvaluationPercentageConfigController(IEvaluationPercentageConfigBll evaConfig)
        {
            _evaConfig = evaConfig;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_evaConfig.GetList());
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_evaConfig.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = EvaluationPercentageConfigViewModel.RoleForManageData)]
        public IActionResult Save([FromBody]EvaluationPercentageConfigRequestModel model)
        {
            return Ok(_evaConfig.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = EvaluationPercentageConfigViewModel.RoleForManageData)]
        public IActionResult Edit([FromBody]EvaluationPercentageConfigRequestModel model)
        {
            return Ok(_evaConfig.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = EvaluationPercentageConfigViewModel.RoleForManageData)]
        public IActionResult Delete(int id)
        {
            return Ok(_evaConfig.Delete(id));
        }

        #endregion

    }
}
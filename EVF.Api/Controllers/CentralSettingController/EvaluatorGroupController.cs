using EVF.CentralSetting.Bll.Interfaces;
using EVF.CentralSetting.Bll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers.CentralSettingController
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = EvaluatorGroupViewModel.RoleForManageData)]
    public class EvaluatorGroupController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The EvaluatorGroup manager provides EvaluatorGroup functionality.
        /// </summary>
        private readonly IEvaluatorGroupBll _evaluatorGroup;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="EvaluatorGroupController" /> class.
        /// </summary>
        /// <param name="evaluatorGroup"></param>
        public EvaluatorGroupController(IEvaluatorGroupBll evaluatorGroup)
        {
            _evaluatorGroup = evaluatorGroup;
        }

        #endregion

        #region [Methods]

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_evaluatorGroup.GetList());
        }

        [HttpGet]
        [Route("GetEvaluatorGroups")]
        public IActionResult GetEvaluatorGroups(int periodItemid)
        {
            return Ok(_evaluatorGroup.GetEvaluatorGroups(periodItemid));
        }

        [HttpGet]
        [Route("GetDetail")]
        public IActionResult GetDetail(int id)
        {
            return Ok(_evaluatorGroup.GetDetail(id));
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save([FromBody]EvaluatorGroupViewModel model)
        {
            return Ok(_evaluatorGroup.Save(model));
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit([FromBody]EvaluatorGroupViewModel model)
        {
            return Ok(_evaluatorGroup.Edit(model));
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            return Ok(_evaluatorGroup.Delete(id));
        }

        #endregion

    }
}
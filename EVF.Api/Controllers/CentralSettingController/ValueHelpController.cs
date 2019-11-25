using EVF.CentralSetting.Bll.Interfaces;
using EVF.Helper.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EVF.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ValueHelpController : ControllerBase
    {

        #region Fields

        /// <summary>
        /// The value help manager provides value help functionality.
        /// </summary>
        private readonly IValueHelpBll _valueHelp;

        #endregion

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValueHelpController" /> class.
        /// </summary>
        /// <param name="valueHelp"></param>
        public ValueHelpController(IValueHelpBll valueHelp)
        {
            _valueHelp = valueHelp;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GetActiveStatus")]
        public IActionResult GetActiveStatus()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeActiveStatus));
        }

        [HttpGet]
        [Route("GetSapScoreFields")]
        public IActionResult GetSapScoreFields()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeSAPScoreFields));
        }

        [HttpGet]
        [Route("GetPurchaseUserType")]
        public IActionResult GetPurchaseUserType()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypePurchaseUserType));
        }

        [HttpGet]
        [Route("GetWeightingKey")]
        public IActionResult GetWeightingKey()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeWeightingKey));
        }

        [HttpGet]
        [Route("GetVendorFilterCondition")]
        public IActionResult GetVendorFilterCondition()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeVendorFilterCondition));
        }

        [HttpGet]
        [Route("GetLevelPointCalculate")]
        public IActionResult GetLevelPointCalculate()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeLevelPointCalculate));
        }

        [HttpGet]
        [Route("GetCategory")]
        public IActionResult GetCategory()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeCategory));
        }

        [HttpGet]
        [Route("GetPurGroup")]
        public IActionResult GetPurGroup()
        {
            return Ok(_valueHelp.GetPurGroup());
        }

        [HttpGet]
        [Route("GetEvaluationStatus")]
        public IActionResult GetEvaluationStatus()
        {
            return Ok(_valueHelp.Get(ConstantValue.ValueTypeEvaStatus));
        }

        #endregion

    }
}
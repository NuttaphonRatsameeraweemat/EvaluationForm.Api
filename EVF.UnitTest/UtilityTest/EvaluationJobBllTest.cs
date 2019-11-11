using EVF.Utility.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.UtilityTest
{
    public class EvaluationJobBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The evaluation job service manager provides evaluation job service functionality.
        /// </summary>
        private IEvaluationJobBll _evaluationJobBll;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationJobBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public EvaluationJobBllTest(IoCConfig io)
        {
            _evaluationJobBll = io.ServiceProvider.GetRequiredService<IEvaluationJobBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void ExecuteEvaluationProcess()
        {
            try
            {
                var result = _evaluationJobBll.ExecuteEvaluationProcess();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

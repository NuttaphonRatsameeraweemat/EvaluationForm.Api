using EVF.Evaluation.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.EvaluationTest
{
    public class EvaluationSapResultBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The Kpi service manager provides Kpi service functionality.
        /// </summary>
        private IEvaluationSapResultBll _evaluationSapResult;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationSapResultBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public EvaluationSapResultBllTest(IoCConfig io)
        {
            _evaluationSapResult = io.ServiceProvider.GetRequiredService<IEvaluationSapResultBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void Save()
        {
            try
            {
                _evaluationSapResult.Save(12);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

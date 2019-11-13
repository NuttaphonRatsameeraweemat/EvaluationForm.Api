using EVF.Email.Bll.Interfaces;
using EVF.Helper.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.EmailTest
{
    public class SummaryEmailTaskBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The summary email task service manager provides summary email task service functionality.
        /// </summary>
        private ISummaryEmailTaskBll _summaryEmailTask;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryEmailTaskBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public SummaryEmailTaskBllTest(IoCConfig io)
        {
            _summaryEmailTask = io.ServiceProvider.GetRequiredService<ISummaryEmailTaskBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void ExecuteEmailTaskWaiting()
        {
            try
            {
                _summaryEmailTask.ExecuteEmailTaskWaiting(ConstantValue.EmailTaskStatusWaiting);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void ProcessSummaryTask()
        {
            try
            {
                _summaryEmailTask.ProcessSummaryTask();
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

using EVF.CentralSetting.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.CentralSettingTest
{
    public class ValueHelpBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The ValueHelp service manager provides ValueHelp service functionality.
        /// </summary>
        private IValueHelpBll _valueHelp;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayCalendarBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public ValueHelpBllTest(IoCConfig io)
        {
            _valueHelp = io.ServiceProvider.GetRequiredService<IValueHelpBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("ACTIVE_STATUS")]
        public void GetList(string valueType)
        {
            try
            {
                var response = _valueHelp.Get(valueType);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

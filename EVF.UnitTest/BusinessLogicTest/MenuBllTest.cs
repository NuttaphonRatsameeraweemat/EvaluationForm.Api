using EVF.Bll.Interfaces;
using EVF.Helper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.BusinessLogicTest
{
    public class MenuBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The menu service manager provides menu service functionality.
        /// </summary>
        private IMenuBll _menu;
        /// <summary>
        /// The config setting provides config setting functionality.
        /// </summary>
        private IConfigSetting _config; 

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="K2ServiceTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public MenuBllTest(IoCConfig io)
        {
            _menu = io.ServiceProvider.GetRequiredService<IMenuBll>();
            _config = io.ServiceProvider.GetRequiredService<IConfigSetting>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("")]
        [InlineData("ds01")]
        public void GetMenu(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _menu.GenerateMenu(adUser);
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

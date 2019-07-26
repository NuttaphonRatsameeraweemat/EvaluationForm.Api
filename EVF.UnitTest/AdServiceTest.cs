using EVF.Bll.Components.InterfaceComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest
{
    /// <summary>
    /// The AdServiceTest class testing adservice class.
    /// </summary>
    public class AdServiceTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The ad service manager provides ad service functionality.
        /// </summary>
        private IAdService _adService;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="AdServiceTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public AdServiceTest(IoCConfig io)
        {
            _adService = io.ServiceProvider.GetRequiredService<IAdService>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("","")]
        [InlineData("ds01", "hw_2931")]
        public void TestConnectAd(string username, string password)
        {
            try
            {
                _adService.Authen(username, password);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

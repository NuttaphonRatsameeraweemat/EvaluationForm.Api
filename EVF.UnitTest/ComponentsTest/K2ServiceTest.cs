using EVF.Bll.Components.InterfaceComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.ComponentsTest
{
    public class K2ServiceTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The k2 service manager provides k2 service functionality.
        /// </summary>
        private IK2Service _k2Service;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="K2ServiceTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public K2ServiceTest(IoCConfig io)
        {
            _k2Service = io.ServiceProvider.GetRequiredService<IK2Service>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void K2ServiceStartWorkflow()
        {
            try
            {
                var httpContextAccessor = new HttpContextAccessor();
                var httpContext = new DefaultHttpContext();
                httpContext.Request.Headers["TEST"] = "12345";

                httpContextAccessor.HttpContext = httpContext;

                _k2Service.StartWorkflow("", "", null);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        #endregion

    }
}

using EVF.Authorization.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.AuthorizationTest
{
    public class LoginBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The login service manager provides login service functionality.
        /// </summary>
        private ILoginBll _login;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public LoginBllTest(IoCConfig io)
        {
            _login = io.ServiceProvider.GetRequiredService<ILoginBll>();
        }

        #endregion

        #region [Methods]

        [Theory]
        [InlineData("","")]
        [InlineData("ds01","123456789")]
        public void Authenticate(string username, string password)
        {
            try
            {
                var response = _login.Authenticate(new Authorization.Bll.Models.LoginViewModel { Username = username, Password = password });
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

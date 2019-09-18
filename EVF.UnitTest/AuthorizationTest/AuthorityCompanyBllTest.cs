using EVF.Authorization.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.AuthorizationTest
{
    public class AuthorityCompanyBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The AuthorityCompany service manager provides AuthorityCompany service functionality.
        /// </summary>
        private IAuthorityCompanyBll _authorityCompany;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorityCompanyBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public AuthorityCompanyBllTest(IoCConfig io)
        {
            _authorityCompany = io.ServiceProvider.GetRequiredService<IAuthorityCompanyBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _authorityCompany.GetList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("BOONRAWD_LOCAL\\ds01")]
        [InlineData("BOONRAWD_LOCAL\\ds02")]
        public void GetDetail(string adUser)
        {
            try
            {
                var response = _authorityCompany.GetDetail(adUser);
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

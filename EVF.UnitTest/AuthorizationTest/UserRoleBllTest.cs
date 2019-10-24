using EVF.Authorization.Bll.Interfaces;
using EVF.Helper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.AuthorizationTest
{
    public class UserRoleBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The userrole service manager provides userrole service functionality.
        /// </summary>
        private IUserRoleBll _userRole;
        /// <summary>
        /// The config setting provides config setting functionality.
        /// </summary>
        private IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public UserRoleBllTest(IoCConfig io)
        {
            _userRole = io.ServiceProvider.GetRequiredService<IUserRoleBll>();
            _config = io.ServiceProvider.GetRequiredService<IConfigSetting>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _userRole.GetList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("ds01")]
        public void GetAllMenu(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _userRole.GetDetail(adUser);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("ds01")]
        public void Save(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _userRole.Save(new Authorization.Bll.Models.UserRoleViewModel
                {
                    AdUser = adUser,
                    RoleList = new List<int> { 2, 6 }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("ds01")]
        public void Edit(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _userRole.Edit(new Authorization.Bll.Models.UserRoleViewModel
                {
                    AdUser = adUser,
                    RoleList = new List<int> { 2, 6, 8 }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("ds01")]
        public void Delete(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _userRole.Delete(new Authorization.Bll.Models.UserRoleRequestDeleteModel { AdUser = adUser });
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

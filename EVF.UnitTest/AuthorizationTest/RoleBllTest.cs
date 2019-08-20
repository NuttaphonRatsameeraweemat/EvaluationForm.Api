using EVF.Authorization.Bll.Interfaces;
using EVF.Helper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.AuthorizationTest
{
    public class RoleBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The role service manager provides role service functionality.
        /// </summary>
        private IRoleBll _role;
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
        public RoleBllTest(IoCConfig io)
        {
            _role = io.ServiceProvider.GetRequiredService<IRoleBll>();
            _config = io.ServiceProvider.GetRequiredService<IConfigSetting>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetActiveRoleList()
        {
            try
            {
                var response = _role.GetActiveRoleList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void GetAllMenu()
        {
            try
            {
                var response = _role.GetAllMenu();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("ds01")]
        public void Authenticate(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _role.GetCompositeRoleItemByAdUser(adUser);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetDetailCompositeRole(int id)
        {
            try
            {
                var response = _role.GetDetailCompositeRole(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public void GetRoleList()
        {
            try
            {
                var response = _role.GetRoleList();
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("ds01")]
        public void ValidateRole(string adUser)
        {
            try
            {
                adUser = _config.DomainUser + adUser;
                var response = _role.ValidateRole(adUser);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Save(int id)
        {
            try
            {
                var compositeRole = _role.GetDetailCompositeRole(id);
                compositeRole.Id = 0;
                var response = _role.Save(compositeRole);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Edit(int id)
        {
            try
            {
                var compositeRole = _role.GetDetailCompositeRole(id);
                compositeRole.Description = compositeRole.Description + "#Edit";
                var response = _role.Edit(compositeRole);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(1)]
        public void Delete(int id)
        {
            try
            {
                var response = _role.Delete(id);
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

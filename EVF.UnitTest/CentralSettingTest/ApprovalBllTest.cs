using EVF.CentralSetting.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EVF.UnitTest.CentralSettingTest
{
    public class ApprovalBllTest : IClassFixture<IoCConfig>
    {

        #region [Fields]

        /// <summary>
        /// The Approval service manager provides Approval service functionality.
        /// </summary>
        private IApprovalBll _approval;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovalBllTest" /> class.
        /// </summary>
        /// <param name="io">The IoCConfig class provide installing all components needed to use.</param>
        public ApprovalBllTest(IoCConfig io)
        {
            _approval = io.ServiceProvider.GetRequiredService<IApprovalBll>();
        }

        #endregion

        #region [Methods]

        [Fact]
        public void GetList()
        {
            try
            {
                var response = _approval.GetList();
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
        public void GetDetail(int id)
        {
            try
            {
                var response = _approval.GetDetail(id);
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("10001416", "10000001", "BOONRAWD_LOCAL\\ds01")]
        [InlineData("10001416", "10000001", "BOONRAWD_LOCAL\\ds02")]
        public void Save(string orgId, string comCode, string adUser)
        {
            try
            {
                var response = _approval.Save(new CentralSetting.Bll.Models.ApprovalViewModel
                {
                    PurchasingOrg = orgId,
                    PurchasingOrgName = comCode,
                    ApprovalList = new List<CentralSetting.Bll.Models.ApprovalItemViewModel>
                    {
                        new CentralSetting.Bll.Models.ApprovalItemViewModel { AdUser = adUser, Step = 1 }
                    }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData("10001416", "10000001", "BOONRAWD_LOCAL\\ds01")]
        [InlineData("10001416", "10000001", "BOONRAWD_LOCAL\\ds02")]
        public void Edit(string orgId, string comCode, string adUser)
        {
            try
            {
                var response = _approval.Edit(new CentralSetting.Bll.Models.ApprovalViewModel
                {
                    Id = 1,
                    PurchasingOrg = orgId,
                    PurchasingOrgName = comCode,
                    ApprovalList = new List<CentralSetting.Bll.Models.ApprovalItemViewModel>
                    {
                        new CentralSetting.Bll.Models.ApprovalItemViewModel { AdUser = adUser, Step = 1 }
                    }
                });
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Theory]
        [InlineData(2)]
        public void Delete(int id)
        {
            try
            {
                var response = _approval.Delete(id);
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

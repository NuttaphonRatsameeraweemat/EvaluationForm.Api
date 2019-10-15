using EVF.Helper;
using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using EVF.Report.Bll.Interfaces;
using EVF.Report.Bll.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EVF.Report.Bll
{
    public class ReportService : IReportService
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The HttpClient request.
        /// </summary>
        private readonly HttpClient _client;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public ReportService(IConfigSetting config)
        {
            _config = config;
            _client = new HttpClient();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get base url plus controller route api. 
        /// </summary>
        /// <param name="ControllersName">The controller route api.</param>
        /// <returns></returns>
        private Uri CallCommonApi(string ControllersName)
        {
            Uri url = new Uri(_config.ReportUrl + ControllersName);
            return new Uri(url + "");
        }
        
        public ResponseFileModel Try()
        {
            using (HttpResponseMessage response = _client.GetAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", "VendorEvaluationReport", "Try"))).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<ResponseFileModel>(response.Content);
            }
        }
        
        #endregion

    }
}

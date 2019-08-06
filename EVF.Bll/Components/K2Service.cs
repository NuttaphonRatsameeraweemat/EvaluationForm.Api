using EVF.Bll.Components.InterfaceComponents;
using EVF.Bll.Components.ModelComponents;
using EVF.Bll.Interfaces;
using EVF.Helper;
using EVF.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EVF.Bll.Components
{
    public class K2Service : IK2Service
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;
        /// <summary>
        /// The ClaimsIdentity in token management.
        /// </summary>
        private readonly IManageToken _token;
        /// <summary>
        /// The K2 Profile Information for authentication k2 service.
        /// </summary>
        private readonly K2ProfileModel _k2ProfileModel;
        /// <summary>
        /// The HttpClient request.
        /// </summary>
        private readonly HttpClient _client;
        /// <summary>
        /// K2 Service workflow route api.
        /// </summary>
        private const string K2RouteWorkflow = "Workflow";
        /// <summary>
        /// K2 Service start workflow route api.
        /// </summary>
        private const string K2RouteStartWorkflow = "StartWorkflow";

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="K2Service" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public K2Service(IConfigSetting config, IManageToken token)
        {
            _config = config;
            _token = token;
            _client = new HttpClient();
            _k2ProfileModel = this.InitialK2Profile();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Set authentication profile k2 service.
        /// </summary>
        /// <returns></returns>
        private K2ProfileModel InitialK2Profile()
        {
            return new K2ProfileModel
            {
                UserName = _token.AdUser,
                Password = _token.Encrypt
            };
        }

        /// <summary>
        /// Get base url plus controller route api. 
        /// </summary>
        /// <param name="ControllersName">The controller route api.</param>
        /// <returns></returns>
        private Uri CallCommonApi(string ControllersName)
        {
            Uri url = new Uri(_config.K2ServiceUrl + ControllersName);
            return new Uri(url + "");
        }

        /// <summary>
        /// Call k2 service start workflow route.
        /// </summary>
        /// <param name="processName">The workflow name to start.</param>
        /// <param name="folio">The task title display in k2.</param>
        /// <param name="dataFields">The datafields value in workflow.</param>
        /// <returns></returns>
        public int StartWorkflow(string processName, string folio, Dictionary<string,object> dataFields)
        {
            var model = new K2Model.StartWorkflowModel
            {
                K2Connect = _k2ProfileModel,
                ProcessName = processName,
                Folio =  folio,
                DataFields = dataFields
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteWorkflow, K2RouteStartWorkflow)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<int>(response.Content);
            }
        }

        public void ActionWorkflow()
        {

        }

        public void GetWorkList()
        {

        }

        public void SetOutofOffice()
        {

        }

        #endregion

    }
}

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
        /// <summary>
        /// K2 Service action workflow route api.
        /// </summary>
        private const string K2RouteActionWorkflow = "ActionWorkflow";
        /// <summary>
        /// K2 Service get worklist task route api.
        /// </summary>
        private const string K2RouteGetWorkList = "GetWorkList";
        /// <summary>
        /// K2 Service set out of office route api.
        /// </summary>
        private const string K2RouteSetOutOfOffice = "SetOutOfOffice";
        /// <summary>
        /// K2 Service smartobject route api.
        /// </summary>
        private const string K2RouteSmartObject = "SmartObject";
        /// <summary>
        /// K2 Service execute smartobject route api.
        /// </summary>
        private const string K2RouteGetSmartObject = "GetSmartObject";

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
        public int StartWorkflow(string processName, string folio, Dictionary<string, object> dataFields)
        {
            var model = new K2Model.StartWorkflowModel
            {
                K2Connect = _k2ProfileModel,
                ProcessName = processName,
                Folio = folio,
                DataFields = dataFields
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteWorkflow, K2RouteActionWorkflow)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<int>(response.Content);
            }
        }

        /// <summary>
        /// Call K2 service action workflow route.
        /// </summary>
        /// <param name="serialNumber">The identity workflow task number.</param>
        /// <param name="action">The action outcome workflow.</param>
        /// <param name="dataFields">The datafields value in workflow</param>
        /// <param name="allocatedUser">The allocated user action.</param>
        /// <returns></returns>
        public string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> dataFields, string allocatedUser = "")
        {
            var model = new K2Model.ActionWorkflowModel
            {
                K2Connect = _k2ProfileModel,
                SerialNumber = serialNumber,
                Action = action,
                AllocatedUser = allocatedUser,
                Datafields = dataFields
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteWorkflow, K2RouteStartWorkflow)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<string>(response.Content);
            }
        }

        /// <summary>
        /// Call K2 service get workflist from k2.
        /// </summary>
        /// <param name="fromUser"></param>
        /// <returns></returns>
        public IEnumerable<K2Model.TaskListModel> GetWorkList(string fromUser)
        {
            var model = new K2Model.WorklistModel
            {
                K2Connect = _k2ProfileModel,
                FromUser = fromUser,
                ProcessFolder = _config.K2ProcessFolder
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteWorkflow, K2RouteGetWorkList)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<List<K2Model.TaskListModel>>(response.Content);
            }
        }

        /// <summary>
        /// Call K2 service set out of office user.
        /// </summary>
        /// <param name="fromUser">The delegate task from user.</param>
        /// <param name="toUser">The delegate task to user.</param>
        /// <param name="action">The action out of office (create, edit or delete)</param>
        /// <param name="startDate">The startdate of delegate task.</param>
        /// <param name="endDate">The enddate of delegate task.</param>
        /// <returns></returns>
        public string SetOutofOffice(string fromUser, string toUser, string action, DateTime startDate, DateTime endDate)
        {
            var model = new K2Model.SetOutOfOfficeModel
            {
                K2Connect = new K2ProfileModel { Management = true },
                WorkflowDelegate = new K2Model.WorkflowDelegateModel
                {
                    FromUser = fromUser,
                    ToUser = toUser,
                    Action = action,
                    StartDate = startDate,
                    EndDate = endDate
                }
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteWorkflow, K2RouteSetOutOfOffice)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<string>(response.Content);
            }
        }

        /// <summary>
        /// Execute Smartobject from k2.
        /// </summary>
        /// <param name="smartObjectName">The smartobject name.</param>
        /// <param name="methodName">The method name to execute.</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ExecuteSmartObject(string smartObjectName, string methodName)
        {
            var model = new K2Model.SmartObjectModel
            {
                SmartObjectName = smartObjectName,
                ExecuteMethodName = methodName
            };
            using (HttpResponseMessage response = _client.PostAsync(
                                                    this.CallCommonApi(string.Format("{0}/{1}", K2RouteSmartObject, K2RouteGetSmartObject)),
                                                    UtilityService.SerializeContent(model)).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(ConstantValue.HttpRequestFailedMessage, (int)response.StatusCode, response.StatusCode.ToString()));
                }
                return UtilityService.DeserializeContent<List<Dictionary<string, object>>>(response.Content);
            }
        }

        #endregion

    }
}

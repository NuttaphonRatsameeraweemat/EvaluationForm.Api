using EVF.Helper.Components;
using EVF.Helper.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace EVF.Helper
{
    public class AdService : IAdService
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="AdService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public AdService(IConfigSetting config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Connect and Validate username and passowrd from ad service.
        /// </summary>
        /// <param name="username">The string username.</param>
        /// <param name="password">The string password.</param>
        /// <returns></returns>
        public bool Authen(string username, string password)
        {
            bool result = false;
#if DEBUG
            return true;
#endif
            using (WebClient webClient = new WebClient())
            {
                username = username != null ? username.ToLower() : "";
                NameValueCollection formData = new NameValueCollection
                {
                    ["username"] = username,
                    ["password"] = EncodePassword(password),
                    ["system_id"] = _config.SystemId
                };

                byte[] responseBytes = webClient.UploadValues(_config.AdUrl, ConstantValue.HttpMethodPost, formData);
                string responsefromserver = Encoding.UTF8.GetString(responseBytes);

                var json = JObject.Parse(responsefromserver);
                string checkResult = json["result"].Value<string>();

                if (string.Equals(checkResult, "success", StringComparison.OrdinalIgnoreCase))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// EnCoding string password to string base 64.
        /// </summary>
        /// <param name="password">The string password</param>
        /// <returns></returns>
        private string EncodePassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        #endregion

    }
}

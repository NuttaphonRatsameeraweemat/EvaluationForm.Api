using EVF.Helper.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper
{
    public class ConfigSetting : IConfigSetting
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfiguration _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="AdService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public ConfigSetting(IConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get SystemId for AD Authentication.
        /// </summary>
        public string SystemId => _config["SystemID"];
        /// <summary>
        /// Get Ad Employee Url.
        /// </summary>
        public string AdUrl => _config["ADEmployeeURL"];
        /// <summary>
        /// Get SMTP Host Url.
        /// </summary>
        public string SmtpHost => _config["SMTP:Host"];
        /// <summary>
        /// Get SMTP Port.
        /// </summary>
        public string SmtpPort => _config["SMTP:Port"];
        /// <summary>
        /// Get RequireCredential SMTP.
        /// </summary>
        public string SmtpRequireCredential => _config["SMTP:RequireCredential"];
        /// <summary>
        /// Get SMTP EnableSSL.
        /// </summary>
        public string SmtpEnableSSL => _config["SMTP:EnableSSL"];
        /// <summary>
        /// Get User Authenticate SMTP Server.
        /// </summary>
        public string SmtpUser => _config["SMTP:User"];
        /// <summary>
        /// Get Password Authenticate SMTP Server.
        /// </summary>
        public string SmtpPassword => _config["SMTP:Password"];
        /// <summary>
        /// Get Json Web Token Config Issuer value.
        /// </summary>
        public string JwtIssuer => _config["Jwt:Issuer"];
        /// <summary>
        /// Get Json Web Token Config Key value
        /// </summary>
        public string JwtKey => _config["Jwt:Key"];
        /// <summary>
        /// Get Domain User value
        /// </summary>
        public string DomainUser => _config["DomainUser"];
        #endregion

    }
}

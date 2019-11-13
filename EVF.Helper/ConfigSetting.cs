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
        /// Read value in appsetting config with key name;
        /// </summary>
        /// <param name="name">The key name parameter in appsetting.</param>
        /// <returns></returns>
        private string GetAppSetting(string name) => _config[name];

        /// <summary>
        /// Get SystemId for AD Authentication.
        /// </summary>
        public string SystemId => this.GetAppSetting("SystemID");
        /// <summary>
        /// Get Ad Employee Url.
        /// </summary>
        public string AdUrl => this.GetAppSetting("ADEmployeeURL");
        /// <summary>
        /// Get SMTP Host Url.
        /// </summary>
        public string SmtpHost => this.GetAppSetting("SMTP:Host");
        /// <summary>
        /// Get SMTP Port.
        /// </summary>
        public string SmtpPort => this.GetAppSetting("SMTP:Port");
        /// <summary>
        /// Get RequireCredential SMTP.
        /// </summary>
        public string SmtpRequireCredential => this.GetAppSetting("SMTP:RequireCredential");
        /// <summary>
        /// Get SMTP EnableSSL.
        /// </summary>
        public string SmtpEnableSSL => this.GetAppSetting("SMTP:EnableSSL");
        /// <summary>
        /// Get User Authenticate SMTP Server.
        /// </summary>
        public string SmtpUser => this.GetAppSetting("SMTP:User");
        /// <summary>
        /// Get Password Authenticate SMTP Server.
        /// </summary>
        public string SmtpPassword => this.GetAppSetting("SMTP:Password");
        /// <summary>
        /// Get SMTP Sender Email.
        /// </summary>
        public string SmtpSender => this.GetAppSetting("SMTP:EmailSender");
        /// <summary>
        /// Get SMTP Sender Name.
        /// </summary>
        public string SmtpSenderName => this.GetAppSetting("SMTP:EmailSenderName");
        /// <summary>
        /// Get Json Web Token Config Issuer value.
        /// </summary>
        public string JwtIssuer => this.GetAppSetting("Jwt:Issuer");
        /// <summary>
        /// Get Json Web Token Config Key value
        /// </summary>
        public string JwtKey => this.GetAppSetting("Jwt:Key");
        /// <summary>
        /// Get Domain User value
        /// </summary>
        public string DomainUser => this.GetAppSetting("DomainUser");
        /// <summary>
        /// Get Users Basic Authen.
        /// </summary>
        public string BasicAuthUsers => this.GetAppSetting("BasicAuth:Users");
        /// <summary>
        /// Get Passwords Basic Authen.
        /// </summary>
        public string BasicAuthPasswords => this.GetAppSetting("BasicAuth:Passwords");
        /// <summary>
        /// Get Roles Basic Authen.
        /// </summary>
        public string BasicAuthRoles => this.GetAppSetting("BasicAuth:Roles");
        /// <summary>
        /// Get EncrptionKey parameter.
        /// </summary>
        public string EncryptionKey => this.GetAppSetting("EncryptionKey");
        /// <summary>
        /// Get K2 service url.
        /// </summary>
        public string K2ServiceUrl => this.GetAppSetting("K2:Url");
        /// <summary>
        /// Get K2 Process Folder.
        /// </summary>
        public string K2ProcessFolder => this.GetAppSetting("K2:ProcessFolder");
        /// <summary>
        /// Get K2 Spe Evaluation Process workflow.
        /// </summary>
        public string SpeEvaluationProcess => this.GetAppSetting("K2:SpeEvaluationProcess");
        /// <summary>
        /// Get Application Name.
        /// </summary>
        public string AppName => this.GetAppSetting("AppName");
        /// <summary>
        /// Get Elastic Search Url.
        /// </summary>
        public string ElasticSearchUrl => this.GetAppSetting("ElasticSearch:Url");
        /// <summary>
        /// Get Report Url.
        /// </summary>
        public string ReportUrl => this.GetAppSetting("Report:Url");

        #endregion

    }
}

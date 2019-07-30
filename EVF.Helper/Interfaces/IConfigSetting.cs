using System;
using System.Collections.Generic;
using System.Text;

namespace EVF.Helper.Interfaces
{
    public interface IConfigSetting
    {
        /// <summary>
        /// Get SystemId for AD Authentication.
        /// </summary>
        string SystemId { get; }
        /// <summary>
        /// Get Ad Employee Url.
        /// </summary>
        string AdUrl { get; }
        /// <summary>
        /// Get SMTP Host Url.
        /// </summary>
        string SmtpHost { get; }
        /// <summary>
        /// Get SMTP Port.
        /// </summary>
        string SmtpPort { get; }
        /// <summary>
        /// Get RequireCredential SMTP.
        /// </summary>
        string SmtpRequireCredential { get; }
        /// <summary>
        /// Get SMTP EnableSSL.
        /// </summary>
        string SmtpEnableSSL { get; }
        /// <summary>
        /// Get User Authenticate SMTP Server.
        /// </summary>
        string SmtpUser { get; }
        /// <summary>
        /// Get Password Authenticate SMTP Server.
        /// </summary>
        string SmtpPassword { get; }
        /// <summary>
        /// Get Json Web Token Config Issuer value.
        /// </summary>
        string JwtIssuer { get; }
        /// <summary>
        /// Get Json Web Token Config Key value
        /// </summary>
        string JwtKey { get; }
        /// <summary>
        /// Get Domain User value
        /// </summary>
        string DomainUser { get; }
        /// <summary>
        /// Get Users Basic Authen.
        /// </summary>
        string BasicAuthUsers { get; }
        /// <summary>
        /// Get Passwords Basic Authen.
        /// </summary>
        string BasicAuthPasswords { get; }
        /// <summary>
        /// Get Roles Basic Authen.
        /// </summary>
        string BasicAuthRoles { get; }
        /// <summary>
        /// Get EncryptionKey parameter.
        /// </summary>
        string EncryptionKey { get; }

    }
}

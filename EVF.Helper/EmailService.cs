using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace EVF.Helper
{
    public class EmailService : IEmailService
    {

        #region [Fields]

        /// <summary>
        /// The config value in appsetting.json
        /// </summary>
        private readonly IConfigSetting _config;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService" /> class.
        /// </summary>
        /// <param name="config">The config value.</param>
        public EmailService(IConfigSetting config)
        {
            _config = config;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Send email without specified template.  
        /// </summary>
        /// <param name="email">Email detail.</param>
        public void SendEmail(EmailModel email)
        {
            //Call method to send the email.
            this.SendTheEmail(email);
        }
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        public void SendEmailWithTemplate(EmailModel email)
        {
            //Call method to send the email.
            this.SendTheEmail(email);
        }
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="email">Email detail</param>
        private void SendTheEmail(EmailModel email)
        {
            //Get email configuration
            var smtpHost = _config.SmtpHost;
            var smtpPort = _config.SmtpPort;
            var requireCredential = _config.SmtpRequireCredential;
            var enableSSL = _config.SmtpEnableSSL;
            var user = _config.SmtpUser;
            var password = _config.SmtpPassword;

            SmtpClient client = new SmtpClient(smtpHost, int.Parse(smtpPort))
            {
                EnableSsl = Convert.ToBoolean(enableSSL),
                UseDefaultCredentials = false
            };
            if (requireCredential == "true")
            {
                client.Credentials = new NetworkCredential(user, password);
            }
            //Create an email.
            MailMessage mailItem = new MailMessage
            {
                From = new MailAddress(email.Sender) // must use organization email
            };
            mailItem.To.Add(email.Receiver);
            mailItem.Subject = email.Subject;
            mailItem.IsBodyHtml = true;
            mailItem.Body = email.Body;
            try
            {
                //Send an email 
                client.Send(mailItem);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message + ", Send :" + email.Sender + "Receiver :" + email.Receiver);
            }
        }

        #endregion

    }
}

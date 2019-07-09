using EVF.Helper.Models;

namespace EVF.Helper.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email without specified template.  
        /// </summary>
        /// <param name="email">Email detail.</param>
        void SendEmail(EmailModel email);
        /// <summary>
        /// Send email with specified template.
        /// </summary>
        /// <param name="email">Email detail.</param>
        void SendEmailWithTemplate(EmailModel email);
    }
}

using Microsoft.AspNetCore.Http;
using System;

namespace EVF.Helper.Interfaces
{
    /// <summary>
    /// Logger class is intended for log all activities by using NLog library.
    /// </summary>
    public interface ILoggerManager
    {

        /// <summary>
        /// Creates a new session for logging.
        /// </summary>
        /// <param name="context">Http request infomation</param>
        void CreateNewSession(HttpContext context);
        /// <summary>
        /// Logs the information such as start and end.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogInfo(string message);
        /// <summary>
        /// Logs the Warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogWarn(string message);
        /// <summary>
        /// Logs the debug, the message will be logged when debug mode is on.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogDebug(string message);
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The additional message.</param>
        void LogError(Exception ex, string message = null);

    }
}

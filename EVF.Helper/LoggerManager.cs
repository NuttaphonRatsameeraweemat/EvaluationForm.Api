using EVF.Helper.Interfaces;
using Microsoft.AspNetCore.Http;
using NLog;
using System;

namespace EVF.Helper
{
    /// <summary>
    /// Logger class is intended for log all activities by using NLog library.
    /// </summary>
    public class LoggerManager : ILoggerManager
    {
        #region [Fields]

        /// <summary>
        /// The session key for Mapped Diagnostic Logical Context (MDLC) of NLog.
        /// </summary>
        private const string NLOG_MDC_SESSION_ID_KEY = "session-id";

        /// <summary>
        /// The identity key for Mapped Diagnostic Logical Context (MDLC) of NLog.
        /// </summary>
        private const string NLOG_MDC_IDENTITY_KEY = "identity";

        /// <summary>
        /// The NLog logger object.
        /// </summary>
        private static readonly ILogger loggerInfo = LogManager.GetLogger("InfoProfile");

        /// <summary>
        /// The NLog logger error object.
        /// </summary>
        private static readonly ILogger loggerError = LogManager.GetLogger("ErrorProfile");

        /// <summary>
        /// The NLog logger debug object.
        /// </summary>
        private static readonly ILogger loggerDebug = LogManager.GetLogger("DebugProfile");

        /// <summary>
        /// The NLog logger connection object.
        /// </summary>
        private static readonly ILogger loggerConnection = LogManager.GetLogger("ConnectionProfile");

        #endregion

        #region [Methods]

        /// <summary>
        /// Creates a new session for logging.
        /// </summary>
        /// <param name="context">Http request infomation</param>
        public void CreateNewSession(HttpContext context)
        {
            NLog.MappedDiagnosticsContext.Set(NLOG_MDC_SESSION_ID_KEY, Guid.NewGuid().ToString());
            NLog.MappedDiagnosticsContext.Set(NLOG_MDC_IDENTITY_KEY, context.User.Identity.Name ?? "");
        }

        /// <summary>
        /// Logs the debug, the message will be logged when debug mode is on.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogDebug(string message)
        {
            loggerDebug.Debug(message);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The additional message.</param>
        public void LogError(Exception ex, string message = null)
        {
            loggerError.Error(ex, message ?? string.Empty);
        }

        /// <summary>
        /// Logs the information such as start and end.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInfo(string message)
        {
            loggerInfo.Info(message);
        }

        /// <summary>
        /// Logs the Warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarn(string message)
        {
            loggerInfo.Warn(message);
        }

        /// <summary>
        /// Log the connection information.
        /// </summary>
        /// <param name="context">Http request infomation</param>
        public void LogConnection(HttpContext context)
        {
            loggerConnection.Info(string.Format("IP:{0}| User-Agent {1}", context.Connection.RemoteIpAddress, 
                                                                          context.Request.Headers["User-Agent"].ToString()));
        }

        #endregion

    }
}

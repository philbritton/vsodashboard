using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VSOOAuthClientSample.VSO
{
    /// <summary>
    /// The Logger class provides wrapper methods to the LogManager.
    /// </summary>
    public class FileLogger
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(FileLogger));

        /// <summary>
        /// Initializes the <see cref="FileLogger"/> class.
        /// </summary>
        static FileLogger()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Logs the specified message as a debug statement
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Debugging(string message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// Logs the specified message as an info statement
        /// </summary>
        /// <param name="message">The message to be logged</param>
        public static void Information(string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// Logs the specified formatted message string with arguments
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="vars"></param>
        public void Information(string fmt, params object[] vars)
        {
            Information(string.Format(fmt, vars));
        }

        /// <summary>
        /// Logs the specified message as a warning statement
        /// </summary>
        /// <param name="message">The message to be logged</param>
        /// <param name="ex">The exception to be included in the log</param>
        public static void Warning(string message, Exception ex = null)
        {
            log.Warn(message, ex);
        }

        /// <summary>
        /// Logs the specified message as a warning statement
        /// </summary>
        /// <param name="fmt">The message to be logged</param>
        /// <param name="vars">collection of values to be injected in string format</param>
        public static void Warning(string fmt, params object[] vars)
        {
            Warning(string.Format(fmt, vars));
        }

        /// <summary>
        /// Logs the specified message as an error
        /// </summary>
        /// <param name="ex">The exception to be logged</param>
        /// <param name="message">(OPTIONAL) The message to be logged</param>
        public static void Error(Exception ex, string message = null)
        {
            log.Error(message, ex);
        }

        /// <summary>
        /// Logs the exception with the specified message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="fmt"></param>
        /// <param name="vars"></param>
        public static void Error(Exception ex, string fmt, params object[] vars)
        {
            log.Error(string.Format(fmt, vars), ex);
        }

        /// <summary>
        /// Simple exception formatting: for a more comprehensive version see 
        ///     http://code.msdn.microsoft.com/windowsazure/Fix-It-app-for-Building-cdd80df4
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="fmt"></param>
        /// <param name="vars"></param>
        /// <returns></returns>
        private static string FormatExceptionMessage(Exception exception, string fmt, object[] vars)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(fmt, vars);
            sb.Append(" Exception: ");
            sb.Append(exception.ToString());
            return sb.ToString();
        }
    }
}
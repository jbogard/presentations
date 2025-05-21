using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace LegacyApp
{
    public class Logger
    {
        private static readonly ILog Log;

        #region Step 2

        // private static ILogger<Logger> InnerLogger;

        #endregion

        static Logger()
        {
            #region Step 1
            // var loggerFactory = new LoggerFactory();
            // loggerFactory.AddLog4Net("log4net.config", true);
            // InnerLogger = loggerFactory.CreateLogger<Logger>();
            #endregion

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Log = LogManager.GetLogger("LegacyApp");
        }

        public static void Debug(string message, params object[] args)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat(message, args);
            }
        }

        public static void Info(string message, params object[] args)
        {
            if (Log.IsInfoEnabled)
            {
                Log.InfoFormat(message, args);
            }

            #region Step 3
            //InnerLogger.LogInformation(message, args);
            #endregion
        }

        public static void Warning(string message, params object[] args)
        {
            if (Log.IsWarnEnabled)
            {
                Log.WarnFormat(message, args);
            }
        }

        public static void Error(string message, params object[] args)
        {
            if (Log.IsErrorEnabled)
            {
                Log.ErrorFormat(message, args);
            }
        }
    }
}
using log4net;

namespace LegacyApp
{
    public static class Logger
    {
        private static ILog Log = LogManager.GetLogger("LegacyApp");

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
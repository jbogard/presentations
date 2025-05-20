using log4net;

namespace LegacyApp
{
    internal class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            var service = new MyAwesomeService();

            service.DoAwesomeThings();
        }
    }
}
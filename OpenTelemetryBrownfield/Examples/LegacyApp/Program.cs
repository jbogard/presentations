using log4net;

namespace LegacyApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var service = new MyAwesomeService();

            service.DoAwesomeThings();
        }
    }
}
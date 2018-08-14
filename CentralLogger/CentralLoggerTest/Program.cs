using GHC.Operations;

namespace CentralLoggerTest
{
    class Program
    {
        static void Main(string[] args)
        {            
            CentralLogger log = new CentralLogger("", "CentralLoggerTest");

            log.WriteLine("This is message at the default level WRN");
            log.WriteLine("This is a message at log level ERR", eLogLevel.Error);
            log.WriteLine("This is a message at log level WRN", eLogLevel.Warning);
            log.WriteLine("This message should not appear", eLogLevel.Informational);
            log.WriteLine("Changing default log level from WRN to DBG");
            log.LogLevel = eLogLevel.Debug;
            log.WriteLine("This is a message at log level INF", eLogLevel.Informational);
            log.WriteLine("This is a message at log level DBG", eLogLevel.Debug);

            //log.Close();
        }
    }
}

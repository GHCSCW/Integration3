using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHC.Operations;

namespace FixedWidthSerializerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate central engine
            CentralEngine engine = new CentralEngine();
            CentralLogger log = new CentralLogger(engine.OutputPath, engine.AppName, eLogLevel.Informational);
            CentralProcessor processor = new CentralProcessor(engine, log);

            try
            {
                log.WriteLine("************************************************************", eLogLevel.Informational);
                log.WriteLine("Initializing Sample Application.", eLogLevel.Informational);
                log.WriteLine("************************************************************", eLogLevel.Informational);

                processor.RunProcess();


                log.WriteLine("*****************************************************************", eLogLevel.Informational);
                log.WriteLine("The Sample application has finished Successfully!!!", eLogLevel.Informational);
                log.WriteLine("*****************************************************************", eLogLevel.Informational);

            }
            catch (Exception ex)
            {
                log.WriteLine("*****************************************************************", eLogLevel.Informational);
                log.WriteLine("The Sample Application has finished processing with Errors - ", eLogLevel.Error);
                log.WriteLine("An error has occurred: " + ex.Message + ". " + ex.InnerException, eLogLevel.Error);
                log.WriteLine("*****************************************************************", eLogLevel.Informational);
            }
            finally
            {
                log.WriteLine("Job Complete!");
            }
        }
    }
}

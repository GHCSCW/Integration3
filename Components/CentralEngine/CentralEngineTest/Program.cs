using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmdLine;
using GHC.Operations;
using System.Reflection;
using System.IO;

namespace CentralEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //eLogLevel logLevel = eLogLevel.Informational;
            //CentralArguments arguments = null;

            //try
            //{
            //    if(Environment.GetCommandLineArgs().Count() > 1)
            //    {
            //        arguments = CommandLine.Parse<CentralArguments>();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    TextWriter errorWriter = Console.Error;
            //    errorWriter.WriteLine("*** ERR *** :: " + ex.Message);
            //    Environment.Exit(4);
            //}

            //CentralEngine ce = new CentralEngine();

            //if (arguments != null)
            //{
            //    if (arguments.LogLevel != 0)
            //    {
            //        logLevel = (eLogLevel)arguments.LogLevel;
            //    }
            //}            

            //Console.WriteLine("Log level set to " + logLevel.ToString());

            //foreach(PropertyInfo property in ce.GetType().GetProperties())
            //{
            //    Console.WriteLine(property.Name + ": " + property.GetValue(ce, null));
            //}            

            //Console.ReadKey();

            CentralEngine ce = new CentralEngine("GHC-HMO\\cminnickel", "CLM136RPS", "CENTRAL");
            string test = ce.OutputPath;

        }
    }
}

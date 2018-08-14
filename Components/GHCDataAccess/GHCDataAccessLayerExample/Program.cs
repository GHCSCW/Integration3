using System;
using System.Data;
using System.Data.SqlClient;
using Ghc.Utility.DataAccess;

namespace GHCDataAccessLayerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now + " - START TEST");

            ConcurrentConnections connTest = new ConcurrentConnections();

            connTest.RunTest();

            Console.WriteLine(DateTime.Now + " - STOP TEST");

            Console.ReadKey();
        }
    }
}

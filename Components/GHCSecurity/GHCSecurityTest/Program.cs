using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ghc.Utility.Security;

namespace GHCSecurityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start testing...");

            FindContext();

            //IsMemberOfTest();

            //GetUserDetailsTest();            

            Console.ReadKey();
        }

        static void FindContext()
        {
            string principalName = Environment.MachineName;
            //string principalName = "ASDEVP";
            string groupName = "Dev_App_Environment";

            if (GHCActiveDirectory.IsMemberOf(principalName, GHCActiveDirectory.PrincipalType.Machine, groupName))
            {
                Console.WriteLine("We are in a development environment");
            }
            else
            {
                Console.WriteLine("We are in a production environment");
            }
        }

        static void IsMemberOfTest()
        {
            string principalName = "jschmidt";
            string groupName = "DB_ClarityProd_Read";

            if (GHCActiveDirectory.IsMemberOf("jschmidt", GHCActiveDirectory.PrincipalType.User, "DB_ClarityProd_Read"))
            {
                Console.WriteLine("User '" + principalName + "' is a member of '" + groupName + "'");
            }
            else
            {
                Console.WriteLine("User '" + principalName + "' is NOT a member of '" + groupName + "'");
            }
        }

        static void GetUserDetailsTest()
        {
            Console.WriteLine(GHCActiveDirectory.GetUserDescription("CLARITYPROD"));
        }
    }
}

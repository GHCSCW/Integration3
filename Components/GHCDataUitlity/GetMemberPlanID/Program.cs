using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghc.Utility.DataAccess;
using Ghc.Utility;

namespace GetMemberPlanID
{
    class Program
    {
       public static void Main(string[] args)
        {
           string MemberID = "284563";
           
           GetRxGroupByMemberID(MemberID);



           Console.ReadKey();
        }

       public static void GetRxGroupByMemberID(string memberID)
       {
           string rxGroup = DataTool.GetRxGroup(memberID);

           Console.WriteLine("RxGroup for {0}: {1}", memberID, rxGroup);

       }

       
    }
}

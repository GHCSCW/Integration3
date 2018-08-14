using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ghc.Utility
{
   public static class DataTool
    {
        public static List<MemberPlan> GetMemberPlanID(string memberID)
        {
            return UtilityDataLayer.GetMemberPlanID(memberID);          
        }
       
        public static List<MemberSSN> GetMemberSSN(string memberID)
        {
            return UtilityDataLayer.GetMemberSSN(memberID);
        }

        public static string GetRxGroup(string memberID)
        {
            return UtilityDataLayer.GetRxGroup(memberID);
        }
    }
}

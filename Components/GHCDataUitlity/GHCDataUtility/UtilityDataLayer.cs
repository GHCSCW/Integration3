using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using Ghc.Utility.DataAccess;
using System.Configuration;


namespace Ghc.Utility
{
    class UtilityDataLayer
        {
            const string DATABASE = "clarity";            
        
        //    GHCDataAccessLayer dataLayer = null;
        ////CentralLogger log = null;
        //// DATABASE = "CLARITY";


            //public UtilityDataLayer()
            //{
            //    string database = ConfigurationManager.AppSettings["database"];
            //    dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);
            //    // dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DBEnvironment.PROD, DataProviderType.Sql, DATABASE);
            //}


            public static List<MemberPlan> GetMemberPlanID(string PAT_MRN_ID)
            {

                GHCDataAccessLayer dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, DATABASE);

                string sql = "fw.dbo.proc_GetMemberPlanID";

                SqlParameter[] sqlParams =
                {
                    new SqlParameter("@PAT_MRN_ID", SqlDbType.VarChar) { Value = PAT_MRN_ID }
                };

                DataTable dt = dataLayer.ExecuteDataSet(sql, CommandType.StoredProcedure, 0, sqlParams).Tables[0];

                var members = dt.AsEnumerable().Select(dr => new MemberPlan
                {
                    MemberNumber = dr.Field<string>("MEMBER_NUMBER"),
                    PlanID = dr.Field<decimal>("PLAN_ID"),
                    PlanName = dr.Field<string>("PLAN_NAME")
                }).ToList();
                               
                return members;
            }

            public static List<MemberSSN> GetMemberSSN(string PAT_MRN_ID)
            {

                GHCDataAccessLayer dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, DATABASE);

                string sql = "fw.dbo.proc_GetMemberSSN";

                SqlParameter[] sqlParams =
                {
                    new SqlParameter("@PAT_MRN_ID", SqlDbType.VarChar) { Value = PAT_MRN_ID }
                };

                DataTable dt = dataLayer.ExecuteDataSet(sql, CommandType.StoredProcedure, 0, sqlParams).Tables[0];

                var SSN = dt.AsEnumerable().Select(dr => new MemberSSN
                {
                    MemberNumber = dr.Field<string>("MEMBER_NUMBER"),
                    SSN = dr.Field<string>("SSN"),
                    
                }).ToList();

                return SSN;
            }

            public static string GetRxGroup(string PAT_MRN_ID)
            {
                GHCDataAccessLayer dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, DATABASE);
                
                string sql = "fw.dbo.proc_GetRxGroup";

                SqlParameter[] sqlParams =
                {
                    new SqlParameter("@PAT_MRN_ID", SqlDbType.VarChar) { Value = PAT_MRN_ID }
                };

                return dataLayer.ExecuteDataSet(sql, CommandType.StoredProcedure, 0, sqlParams).Tables[0].AsEnumerable().Select(dr => dr.Field<string>("RX_GROUP")).First();
            }
     }
}


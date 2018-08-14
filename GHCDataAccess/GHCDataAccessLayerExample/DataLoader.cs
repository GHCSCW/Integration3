using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using Ghc.Utility.DataAccess;

namespace GHCDataAccessLayerExample
{
    public class DataLoader
    {
        GHCDataAccessLayer dataLayer = null;

        public DataLoader()
        {
            dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "CLARITY");
        }

        public int TestQueryResultCount()
        {
            string sql = "quotingengine.dbo.TEST_PROC";

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@NAME", SqlDbType.VarChar) { Value = "Josh" },
                new SqlParameter("@IMPORT_DATE", SqlDbType.DateTime) { Value = DateTime.Now }
            };

            return dataLayer.ExecuteQuery(sql, CommandType.StoredProcedure, 0, sqlParams);
        }

        /// <summary>
        /// There is a bug in GHCDataAccess where parameters passed into a execute method remain
        /// even if the next execute does not use parameters.  This method will simulate this.
        /// </summary>
        /// <returns></returns>
        public bool TestConsecutiveStatements()
        {
            string sql = "";

            Console.WriteLine("Executing query without parameters...");

            sql = "fw.dbo.GetAccumulatorImportBatchID";
            Console.WriteLine(dataLayer.ExecuteScalar(sql, CommandType.StoredProcedure, 0, null).ToString());

            Console.WriteLine("Executing query with parameters...");

            sql = "SELECT TOP 10 MemberNumber, ClaimNumber " +
                "FROM fw.dbo.AccumulatorImportRemediation " +
                "WHERE Disposition = @Disposition AND PlanType = @PlanType";

            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@Disposition", SqlDbType.VarChar) { Value = "VALID" },
                new SqlParameter("@PlanType", SqlDbType.VarChar) { Value = "HSA" }
            };

            DataTable firstTable = dataLayer.ExecuteDataSet(sql,CommandType.Text, 0, sqlParams).Tables[0];

            Console.WriteLine("Retrieved " + firstTable.Rows.Count + " records!");
            Console.WriteLine("Executing query without parameters...");

            //sql = "SELECT TOP 10 MemberNumber, ClaimNumber " +
            //    "FROM fw.dbo.AccumulatorImportRemediation " +
            //    "WHERE Disposition = 'INELIGIBLE' AND ClaimStatus = 'PAID'";

            sql = "fw.dbo.GetAccumulatorImportBatchID";

            //Console.WriteLine(dataLayer.ExecuteScalar(sql).ToString());   // WORKED!

            // Throws error: {"Procedure GetAccumulatorImportBatchID has no parameters and arguments were supplied."}
            Console.WriteLine(dataLayer.ExecuteScalar(sql, CommandType.StoredProcedure, 0, null).ToString());

            //DataTable secondTable = dataLayer.ExecuteDataSet(sql).Tables[0];
            //Console.WriteLine("Retrieved " + secondTable.Rows.Count + " records!");

            return true;
        }

        #region "ExecuteScalar"
        //simple sql example from MAA database
        //try
        //{
        //    Console.WriteLine("Inline SELECT on MAA database...");

        //    GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "MAA");                  

        //    string sql = "SELECT COUNT(*) FROM maa.dbo.rx_claim_ids";

        //    string recordCount = dataAccess.ExecuteScalar(sql).ToString();

        //    Console.WriteLine(sql);
        //    Console.WriteLine("Returns: " + dataAccess.ExecuteScalar(sql));

        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //finally
        //{
        //    Console.WriteLine("");
        //}
        #endregion "ExecuteScalar"

        #region "ExecuteQuery"
        //try
        //{
        //    Console.WriteLine("Execute Query test");

        //    GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "CLARITY");

        //    string sql = "dbo.MyStoredProc";
        //    int rowsAffected = dataAccess.ExecuteQuery(sql, CommandType.StoredProcedure);
        //}
        #endregion "ExecuteQuery"

        #region "ExecuteDataSet"
        // table-valued function with parameters from fw database
        //try
        //{
        //    Console.WriteLine("Table-Valued Function with parameters");

        //    GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "CLARITY");


        //    string sqlCommandText = "SELECT PAT_MRN_ID, PAT_LAST_NAME, PAT_FIRST_NAME FROM fw.dbo.RadiologyProductivityInDateRange(@DateFrom,@DateTo)";

        //    SqlParameter[] sqlParams = 
        //    {
        //        new SqlParameter("@DateFrom", SqlDbType.DateTime) { Value = "1/1/2014" },
        //        new SqlParameter("@DateTo", SqlDbType.DateTime) { Value = "1/31/2014" }
        //    };

        //    DataSet ds = dataAccess.ExecuteDataSet(sqlCommandText, CommandType.Text, 0, sqlParams);

        //    int i = 0;
        //    foreach(DataRow dr in ds.Tables[0].Rows)
        //    {
        //        Console.WriteLine(dr["PAT_MRN_ID"] + "," + dr["PAT_LAST_NAME"] + "," + dr["PAT_FIRST_NAME"]);

        //        i++;

        //        if(i > 5)
        //        {
        //            break;
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //finally
        //{
        //    Console.WriteLine("");
        //}
        #endregion "ExecuteDataSet"

        #region "Error Test: Invalid Layer Request"
        // invalid parameter for data access layer
        //try
        //{
        //    Console.WriteLine("Invalid parameter for data access layer retrieval");

        //    string databaseName = "NOTAREALDATABASE";
        //    GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, databaseName);

        //    string strSQL = "SELECT COUNT(*) FROM clarity.dbo.EX_RDS_CLAIMS_INPUT";

        //    Console.WriteLine("Database name: " + databaseName);
        //    Console.WriteLine(dataAccess.ExecuteScalar(strSQL));
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //finally
        //{
        //    Console.WriteLine("");
        //}
        #endregion "Error Test: Invalid Layer Request"
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Ghc.Utility.DataAccess;

namespace GHCDataAccessLayerExample
{
    public class ConcurrentConnections
    {
        private GHCDataAccessLayer dataLayer;

        public ConcurrentConnections()
        {
            // establish data layer
            dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "CLARITY");
        }

        public void RunTest()
        {
            Dictionary<int, string> claims = new Dictionary<int, string>();
            int claimMax = 500;

            string claimNumber = "8109106405197G";
            string claimStatus = "PAID";
            
            DataRow claim = GetClaimByClaimNumber(claimNumber, claimStatus);

            if(claim != null)
            {
                Console.WriteLine("Claim " + claimNumber + " was found!");
            }
            else
            {
                Console.WriteLine("Claim " + claimNumber + " was not found!");
            }

            for(int i = 1; i <= claimMax; i++)
            {
                // add claims from db
                DataRow dr = GetClaimByID(i);

                if(dr != null)
                {
                    claims.Add(Convert.ToInt32(dr["ClaimID"].ToString()), dr["ClaimNumber"].ToString());
                }                
            }

            Console.WriteLine(claims.Count().ToString() + " claims loaded!");
        }

        private DataRow GetClaimByClaimNumber(string claimNumber, string claimStatus)
        {
            DataRow record = null;
            string sql = "SELECT ClaimID, ClaimNumber FROM fw.dbo.AccumulatorImport WHERE ClaimNumber = @CLAIM_NUMBER AND ClaimStatus = @CLAIM_STATUS";

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@CLAIM_NUMBER", SqlDbType.VarChar) { Value = claimNumber },
                new SqlParameter("@CLAIM_STATUS", SqlDbType.VarChar) { Value = claimStatus }
            };

            DataTable dt = dataLayer.ExecuteDataSet(sql, CommandType.Text, 0, sqlParams).Tables[0];

            if(dt.Rows.Count > 0)
            {
                record = dt.Rows[0];
            }

            return record;
        }

        private DataRow GetClaimByID(int claimID)
        {
            DataRow record = null;
            //GHCDataAccessLayer dataLayer = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, "CLARITY");
            string sql = "SELECT ClaimID, ClaimNumber FROM fw.dbo.AccumulatorImport WHERE ClaimID = @CLAIM_ID";

            SqlParameter[] sqlParams =
            {
                new SqlParameter("@CLAIM_ID", SqlDbType.Int) { Value = claimID }
            };
            
            DataTable dt = dataLayer.ExecuteDataSet(sql, CommandType.Text, 0, sqlParams).Tables[0];

            if(dt.Rows.Count > 0)
            {
                record = dt.Rows[0];
            }

            return record;
        }
    }
}

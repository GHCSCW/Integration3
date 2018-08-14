using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghc.Utility.DataAccess;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace GHCDataAccessTest
{
    [TestClass]
    public class GHCDataAccessLayerTests
    {
        #region "ExecuteDataReader Tests"        
        
        // ExecuteDataReader(string commandText)
        [TestMethod]
        public void ExecuteDataReader_ByText()
        {
            // arrange            
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);            

            // act            
            DbDataReader dr = (DbDataReader)dataAccess.ExecuteDataReader(sql);
            
            // assert            
            Assert.IsTrue(dr.HasRows, "0 rows returned, 1 expected");
        }

        // ExecuteDataReader(string commandText, CommandType commandType)
        [TestMethod]
        public void ExecuteDataReader_ByTextAndType()
        {
            // arrange
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            DbDataReader dr = (DbDataReader)dataAccess.ExecuteDataReader(sql, CommandType.Text);

            // assert
            Assert.IsTrue(dr.HasRows, "0 rows returned, 1 expected");
        }

        // ExecuteDataReader(string commandText, IDataParameter[] commandParameters)    
        [TestMethod]
        public void ExecuteDataReader_ByTextAndParams()
        {
            // arrange
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT WHERE City = @City";
            string database = "CLARITY";
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            DbDataReader dr = (DbDataReader)dataAccess.ExecuteDataReader(sql, sqlParams);

            // assert
            Assert.IsTrue(dr.HasRows, "0 rows returned, 1 expected");
        }
        
        // ExecuteDataReader(string commandText, CommandType commandType, int commandTimeout, IDataPrameter[] commandParameters)
        [TestMethod]
        public void ExecuteDataReader_All()
        {
            // arrange
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT WHERE City = @City";
            string database = "CLARITY";
            int timeout = 0;
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            DbDataReader dr = (DbDataReader)dataAccess.ExecuteDataReader(sql, CommandType.Text, timeout, sqlParams);

            // assert
            Assert.IsTrue(dr.HasRows, "0 rows returned, 1 expected");
        }

        #endregion "ExecuteDataReader Tests"

        #region "ExecuteDataSet Tests"
        // ExecuteDataSet(string commandText)
        [TestMethod]
        public void ExecuteDataSet_ByText()
        {
            // arrange            
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act             
            DataSet ds = dataAccess.ExecuteDataSet(sql);            

            // assert            
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0, "0 rows returned, 1 expected");
        }

        // ExecuteDataSet(string commandText, CommandType commandType)
        [TestMethod]
        public void ExecuteDataSet_ByTextAndType()
        {
            // arrange            
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act            
            DataSet ds = dataAccess.ExecuteDataSet(sql, CommandType.Text);

            // assert            
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0, "0 rows returned, 1 expected");
        }

        // ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        [TestMethod]
        public void ExecuteDataSet_ByTextAndParams()
        {
            // arrange
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT WHERE City = @City";
            string database = "CLARITY";
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            DataSet ds = dataAccess.ExecuteDataSet(sql, sqlParams);

            // assert
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0, "0 rows returned, 1 expected");
        }

        // ExecuteDataSet(string commandText, CommandType commandType, int commandTimeout, IDataPrameter[] commandParameters)
        [TestMethod]
        public void ExecuteDataSet_All()
        {
            // arrange
            string sql = "SELECT TOP 1 * FROM clarity.dbo.PATIENT WHERE City = @City";
            string database = "CLARITY";
            int timeout = 0;
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            DataSet ds = dataAccess.ExecuteDataSet(sql, CommandType.Text, timeout, sqlParams);

            // assert
            Assert.IsTrue(ds.Tables[0].Rows.Count > 0, "0 rows returned, 1 expected");
        }

        #endregion "ExecuteDataSet Tests"

        #region "ExecuteQuery Tests"
        // ExecuteQuery(string commandText)
        [TestMethod]
        public void ExecuteQuery_ByText()
        {
            // arrange
            string sql = "UPDATE fw.dbo.DIMENSION SET ATTR_DESCRIPTION = ATTR_DESCRIPTION WHERE ATTR_NAME = 'UTGDA'";
            string database = "CLARITY";
            int expected = 1;
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            int actual = dataAccess.ExecuteQuery(sql);            

            // assert
            Assert.AreEqual(actual, expected, "Unexpected count of affected records");
        }

        // ExecuteQuery(string commandText, CommandType commandType)
        [TestMethod]
        public void ExecuteQuery_ByTextAndType()
        {
            // arrange
            string sql = "UPDATE fw.dbo.DIMENSION SET ATTR_DESCRIPTION = ATTR_DESCRIPTION WHERE ATTR_NAME = 'UTGDA'";
            string database = "CLARITY";
            int expected = 1;
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            int actual = dataAccess.ExecuteQuery(sql, CommandType.Text);

            // assert
            Assert.AreEqual(actual, expected, "Unexpected count of affected records");
        }

        // ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        [TestMethod]
        public void ExecuteQuery_ByTextAndParams()
        {
            // arrange
            string sql = "UPDATE fw.dbo.DIMENSION SET ATTR_DESCRIPTION = ATTR_DESCRIPTION WHERE ATTR_NAME = @Name";
            string database = "CLARITY";
            int expected = 1;
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = "UTGDA" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            int actual = dataAccess.ExecuteQuery(sql, sqlParams);

            // assert
            Assert.AreEqual(actual, expected, "Unexpected count of affected records");
        }

        // ExecuteQuery(string commandText, CommandType commandType, int commandTimeout, IDataPrameter[] commandParameters)
        [TestMethod]
        public void ExecuteQuery_All()
        {
            // arrange
            string sql = "UPDATE fw.dbo.DIMENSION SET ATTR_DESCRIPTION = ATTR_DESCRIPTION WHERE ATTR_NAME = @Name";
            string database = "CLARITY";
            int timeout = 0;
            int expected = 1;
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@Name", SqlDbType.VarChar) { Value = "UTGDA" }
            };
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            int actual = dataAccess.ExecuteQuery(sql, CommandType.Text, timeout, sqlParams);

            // assert
            Assert.AreEqual(actual, expected, "Unexpected count of affected records");
        }

        #endregion "ExecuteQuery Tests"

        #region "ExecuteScalar Tests"
        // ExecuteScalar(string commandText)
        [TestMethod]
        public void ExecuteScalar_ByText()
        {
            // arrange
            string sql = "SELECT TOP 1 city FROM clarity.dbo.PATIENT WHERE city IS NOT NULL and LEN(city) > 0";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            string value = dataAccess.ExecuteScalar(sql).ToString();

            // assert
            Assert.IsTrue(value.Length > 0, "Returned value is below expected length");
        }

        // ExecuteScalar(string commandText, CommandType commandType)
        [TestMethod]
        public void ExecuteScalar_ByTextAndType()
        {
            // arrange
            string sql = "SELECT TOP 1 city FROM clarity.dbo.PATIENT WHERE city IS NOT NULL and LEN(city) > 0";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // act
            string value = dataAccess.ExecuteScalar(sql, CommandType.Text).ToString();

            // assert
            Assert.IsTrue(value.Length > 0, "Returned value is below expected length");
        }

        // ExecuteScalar(string commandText, IDataParameter[] commandParameters)      
        [TestMethod]
        public void ExecuteScalar_ByTextAndParams()
        {
            // arrange
            string sql = "SELECT TOP 1 city FROM clarity.dbo.PATIENT WHERE city IS NOT NULL AND LEN(city) > 0 AND city = @City";
            string database = "CLARITY";
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };

            // act
            string value = dataAccess.ExecuteScalar(sql, sqlParams).ToString();

            // assert
            Assert.IsTrue(value.Length > 0, "Returned value is below expected length");
        }

        // ExecuteScalar(string commandText, CommandType commandType, int commandTimeout, IDataPrameter[] commandParameters)
        [TestMethod]
        public void ExecuteScalar_All()
        {
            // arrange
            string sql = "SELECT TOP 1 city FROM clarity.dbo.PATIENT WHERE city IS NOT NULL AND LEN(city) > 0 AND city = @City";
            string database = "CLARITY";
            int timeout = 0;
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@City", SqlDbType.VarChar) { Value = "MADISON" }
            };

            // act
            string value = dataAccess.ExecuteScalar(sql, CommandType.Text, timeout, sqlParams).ToString();

            // assert
            Assert.IsTrue(value.Length > 0, "Returned value is below expected length");
        }

        #endregion "ExecuteScalar Tests"        

        // CommitTransaction()

        // RollbackTransaction()       
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ghc.Utility.DataAccess;

namespace GHCDataAccessTest
{
    [TestClass]
    public class GHCDataAccessLayerFactoryTests
    {
        // GetDataAccessLayer(DataProviderType dataProviderType, string database)

        /// <summary>
        /// this method should return a development connection string when run from a developer machine (where unit 
        /// tests should be run from.  if this is eventually run from a build server, that server will either need 
        /// to be added to the dev machine OU in AD, or this test will need to be modified.
        /// </summary>
        [TestMethod]
        public void GetDataAccessLayer_ByTypeAndDatabase()
        {
            // arrange
            string database = "CLARITY";
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000";            

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "ConnectionString does not match expected");
        }

        /// <summary>
        /// test creating a GHCDataAccessLayer, overriding the default environment.
        /// </summary>
        [TestMethod]
        public void GetDataAccessLayer_ByEnvironmentTypeAndDatabase()
        {
            // arrange
            string database = "CLARITY";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "ConnectionString does not match expected");
        }

        /// <summary>
        /// test creating a GHCDataAccessLayer from a custom connection string.
        /// </summary>
        [TestMethod]
        public void GetDataAccessLayer_ByConnectionStringAndType()
        {
            // arrange
            string connectionString = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000";
            
            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(connectionString, DataProviderType.Sql);
            
            // assert
            Assert.IsInstanceOfType(dataAccess, typeof(GHCDataAccessLayer));
        }

        #region "Test all released connection strings"
        // These tests verify that the requested and released connection strings in the
        // ActiveDirectory DatabaseConnectionStrings OU match the exected values.
        // This will be tested by using GetDataAccessLayer, setting the environment
        // and database to get each variation of each connection string.

        // BADGERCAREDEV
        [TestMethod]
        public void BadgerCareDev_ConnectionString()
        {
            // arrange
            string database = "BADGERCARE";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=BADGERCARE;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "BADGERCAREDEV connection string does not match");
        }

        // BADGERCAREPROD
        [TestMethod]
        public void BadgerCareProd_ConnectionString()
        {
            // arrange
            string database = "BADGERCARE";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=BADGERCARE;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "BADGERCAREPROD connection string does not match");
        }

        // CLARITYDEV
        [TestMethod]
        public void ClarityDev_ConnectionString()
        {
            // arrange
            string database = "CLARITY";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "CLARITYDEV connection string does not match");
        }

        // CLARITYPROD
        [TestMethod]
        public void ClarityProd_ConnectionString()
        {
            // arrange
            string database = "CLARITY";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "CLARITYPROD connection string does not match");
        }
                
        // MAADEV        
        [TestMethod]
        public void MaaDev_ConnectionString()
        {
            // arrange
            string database = "MAA";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=MAA;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "MAADEV connection string does not match");
        }

        // MAAPROD
        [TestMethod]
        public void MaaProd_ConnectionString()
        {
            // arrange
            string database = "MAA";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=MAA;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "MAAPROD connection string does not match");
        }


        // NURTURDEV        
        [TestMethod]
        public void NurturDev_ConnectionString()
        {
            // arrange
            string database = "NURTUR";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=NURTUR;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "NURTURDEV connection string does not match");
        }

        // NURTURPROD
        [TestMethod]
        public void NurturProd_ConnectionString()
        {
            // arrange
            string database = "NURTUR";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=NURTUR;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "NURTURPROD connection string does not match");
        }

        // PLANMAKERDEV        
        [TestMethod]
        public void PlanMakerDev_ConnectionString()
        {
            // arrange
            string database = "PLANMAKER";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=PLANMAKER;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "PLANMAKERDEV connection string does not match");
        }

        // PLANMAKERPROD
        [TestMethod]
        public void PlanMakerProd_ConnectionString()
        {
            // arrange
            string database = "PLANMAKER";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=PLANMAKER;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "PLANMAKERPROD connection string does not match");
        }

        // RXCLAIMDEV        
        [TestMethod]
        public void RxClaimDev_ConnectionString()
        {
            // arrange
            string database = "RXCLAIM";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=SQLDEV1;INITIAL CATALOG=RXCLAIM;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "RXCLAIMDEV connection string does not match");
        }

        // RXCLAIMPROD
        [TestMethod]
        public void RxClaimProd_ConnectionString()
        {
            // arrange
            string database = "RXCLAIM";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=SQLPROD1;INITIAL CATALOG=RXCLAIM;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "RXCLAIMPROD connection string does not match");
        }

        // WHIODEV        
        [TestMethod]
        public void WHIODev_ConnectionString()
        {
            // arrange
            string database = "WHIO";
            DBEnvironment environment = DBEnvironment.DEV;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=WHIO;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "WHIODEV connection string does not match");
        }

        // WHIOPROD
        [TestMethod]
        public void WHIOProd_ConnectionString()
        {
            // arrange
            string database = "WHIO";
            DBEnvironment environment = DBEnvironment.PROD;
            string expected = "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITY;INITIAL CATALOG=WHIO;CONNECT TIMEOUT=6000";

            // act
            GHCDataAccessLayer dataAccess = GHCDataAccessLayerFactory.GetDataAccessLayer(environment, DataProviderType.Sql, database);

            // assert
            string actual = dataAccess.ConnectionString;
            Assert.AreEqual(expected, actual, "WHIOPROD connection string does not match");
        }
        #endregion "Test all released connection strings"
    }
}

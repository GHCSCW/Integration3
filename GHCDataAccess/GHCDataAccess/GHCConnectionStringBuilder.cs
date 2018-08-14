using System.Collections.Generic;
using Ghc.Utility.Security;

namespace Ghc.Utility.DataAccess
{
    /// <summary>    
    /// Determine user context using Active Directory membership and return the correct connection string.
    /// </summary>
    internal static class GHCConnectionStringBuilder
    {
        const string DEV_GROUP_NAME = "Dev_App_Environment";

        public static GHCDataAccessLayerFactory GHCDataAccessLayerFactory
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        #region "connection string samples"

        // SQL Server Connection String
        // "PERSIST SECURITY INFO=FALSE;INTEGRATED SECURITY=SSPI;DATA SOURCE=ASCLARITYDEV;INITIAL CATALOG=CLARITY;CONNECT TIMEOUT=6000"

        // OLE DB Connection String
        // "Provider=SQLOLEDB;Server=ASCLARITYDEV;Database=CLARITY;Integrated Security=SSPI"

        // ODBC Connection String
        // "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;"
        // "Driver={SQL Server Native Client 11.0};Server=ASCLARITYDEV;Database=CLARITY;Trusted_Connection=True;"

        #endregion        
        
        /// <summary>
        /// Gets the connection string for the database and provider.  The environment is inferred based on the machin name.
        /// If the machine is identified in Active Directory as belonging to the DEV_GROUP_NAME, then the connection string
        /// will be for DEV.  All other machines will retrieve the production connection string unless overridden.
        /// </summary>
        /// <param name="database">string</param>
        /// <param name="dataProviderType">DataProviderType</param>
        /// <returns>string</returns>
        public static string GetConnectionString(string database, DataProviderType dataProviderType)
        {
            string machineName = System.Environment.MachineName;
            string connectionString = "";
                        
            if(GHCActiveDirectory.IsMemberOf(machineName, GHCActiveDirectory.PrincipalType.Machine, DEV_GROUP_NAME))
            {
                connectionString = GetConnectionString(database, dataProviderType, DBEnvironment.DEV);
            }
            else
            {
                connectionString = GetConnectionString(database, dataProviderType, DBEnvironment.PROD);
            }
            
            return connectionString;
        }

        /// <summary>
        /// Gets the connection string for the database, provider, and environment.
        /// </summary>
        /// <param name="database">string</param>
        /// <param name="dataProviderType">DataProviderType</param>
        /// <param name="environment">Environment</param>
        /// <returns>string</returns>
        public static string GetConnectionString(string database, DataProviderType dataProviderType, DBEnvironment environment)
        {
            return GetStringFromRepository(database, dataProviderType, environment);
        }

        /// <summary>
        /// Retrieves the actual string from the repository.  This could be Active Directory,
        /// app.config, registry, or something else entirely.  The current implementation is 
        /// using active directory through the GHCSecurity object, retrieving the connection
        /// string from the description of an AD account created specifically to store the
        /// connection strings
        /// </summary>
        /// <param name="database">string</param>
        /// <param name="environment">Environment</param>
        /// <returns>string</returns>
        private static string GetStringFromRepository(string database, DataProviderType dataProviderType, DBEnvironment environment)
        {            
            string connectionString = "";

            // connection strings are stored in active directory as the description for a user.
            // the username is 'database + environment', so CLARITYDEV would return the dev
            // connection string.
            connectionString = GHCActiveDirectory.GetUserDescription(database + environment);

            #region "ResourceManager Repository"
            //System.Resources.ResourceManager rm = GHCDataAccess.Properties.Resources.ResourceManager;
            //connectionString = rm.GetString(database + environment);

            //if(connectionString == null)
            //{                
            //    throw new System.ArgumentOutOfRangeException("ConnectionName", "Invalid connection name.  The database and environment requested does not exist as a resource. (" + database + " :: " + environment + ")");
            //}
            #endregion "ResourceManager Repository"

            return connectionString;            
        }
    }    
}

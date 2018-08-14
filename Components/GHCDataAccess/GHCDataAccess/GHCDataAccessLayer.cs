using System;
using System.Data;

// This component is based largely on the CodeProject article 'Data Access
// Component and the Factory Design Pattern' by Waleed Al Tamimi.
//
// http://www.codeproject.com/Articles/13695/Data-Access-Component-and-the-Factory-Design-Patte
//
// The main difference is how the connection string is built, including pointing to
// the correct GHC data source for the prod vs dev, and using integrated security.

namespace Ghc.Utility.DataAccess
{
    /// <summary>
    /// Options for DataProviderType to determine correct implementation to return.
    /// </summary>
    public enum DataProviderType
    {        
        /// <summary>The DataProviderType for SQL Server connections.</summary>
        Sql
        //Odbc,
        //OleDb
    }


    /// <summary>
    /// Options for database environment, PROD or DEV.  Used when retrieving the connection string.
    /// </summary>
    public enum DBEnvironment
    {
        /// <summary>Used for connecting to the production environment database(s).</summary>
        PROD,
        /// <summary>Used for connecting to the development environment database(s).</summary>
        DEV
    }

    /// <summary>
    /// The GHCDataAccessLayer is an abstract class for defining database connections, implemented
    /// by the GHCDataAccessFactory.  The GHCDataAccessLayer contains the method definitions for
    /// most commonly used database transactions.
    /// </summary>
    public abstract class GHCDataAccessLayer
    {

        #region private data members, methods & constructors

        // private members
        private string connectionString;
        private IDbConnection connection;
        private IDbCommand command;
        private IDbTransaction transaction;

        const int commandTimeoutDefault = 6000;

        /// <summary>
        /// Database connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                // make sure the connection string is not empty
                if (connectionString == string.Empty || connectionString.Length == 0)
                {
                    throw new ArgumentException("Invalid database connection string.");
                } 

                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        // methods

        // protected constructor
        protected GHCDataAccessLayer() { }

        private void PrepareCommand(CommandType commandType, string commandText, int commandTimeout, IDataParameter[] commandParameters)
        {
            if (connection == null)
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.connectionString;
            }

            if (connection.State != ConnectionState.Open)
                connection.Open();

            if (command == null)
                command = GetDataProviderCommand();

            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.CommandTimeout = commandTimeout;

            if (transaction != null)
                command.Transaction = transaction;

            if (commandParameters != null)
            {
                command.Parameters.Clear();
                foreach (IDataParameter param in commandParameters)
                    command.Parameters.Add(param);
            }
            else
            {
                command.Parameters.Clear();
            }
        }

        #endregion

        #region abstract methods

        internal abstract IDbConnection GetDataProviderConnection();
        internal abstract IDbCommand GetDataProviderCommand();
        internal abstract IDbDataAdapter GetDataProviderDataAdapter();

        #endregion

        #region database transaction

        /// <summary>
        /// Open a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (transaction != null)
                return;

            try
            {
                connection = GetDataProviderConnection();
                connection.ConnectionString = this.connectionString;
                connection.Open();
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                connection.Close();

                throw;
            }
        }

        /// <summary>
        /// Commit a database transaction.  If there is no open transaction this will raise an exception.
        /// </summary>        
        public void CommitTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }
                
        /// <summary>
        /// Rollback a database transaction.  If there is no open transaction this will raise an exception.
        /// </summary>
        public void RollbackTransaction()
        {
            if (transaction == null)
                return;

            try
            {
                transaction.Rollback();
            }
            catch 
            { 
                throw; 
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
        }

        #endregion

        #region ExecuteDataReader
                
        /// <summary>
        /// Execute a database command, text or stored procedure name, and return a DataReader.
        /// The command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteDataReader(string commandText)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandTimeoutDefault, null);
        }
            
        /// <summary>
        /// Execute a database command, text or stored procedure name, and return a DataReader.  
        /// Specifying the type increases performance for stored procedures.  The command timeout is
        /// defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return this.ExecuteDataReader(commandText, commandType, commandTimeoutDefault, null);            
        }
                
        /// <summary>
        /// Execute a database command, text or stored procedure name, with parameters, and 
        /// return a DataReader.  Specifying the type increases performance for stored procedures.  The
        /// command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandTimeoutDefault, commandParameters);
        }        
                
        /// <summary>
        /// Execute a database command, text or stored procedure name, with parameters, and 
        /// return a DataReader.  Specifying the type increases performance for stored procedures.  
        /// The command timeout must be set using milliseconds.  A timeout of 0 is infinite.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandTimeout">int</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType, int commandTimeout, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandTimeout, commandParameters);
                IDataReader dr;
                if (transaction == null)
                    dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                else
                    dr = command.ExecuteReader();

                return dr;
            }
            catch
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
                else
                    RollbackTransaction();

                throw;
            }
        }

        #endregion

        #region ExecuteDataSet
                
        /// <summary>
        /// Execute a database command, text or stored procedure name, and return a DataReader.
        /// The command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute a database command, text or stored procedure name, and return a DataReader.  
        /// Specifying the type increases performance for stored procedures.  The command timeout is
        /// defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return this.ExecuteDataSet(commandText, commandType, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute a database command, text or stored procedure name, with parameters, and 
        /// return a DataReader.  Specifying the type increases performance for stored procedures.  The
        /// command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandTimeoutDefault, commandParameters);
        }

        /// Execute a database command, text or stored procedure name, with parameters, and 
        /// return a DataReader.  Specifying the type increases performance for stored procedures.  
        /// The command timeout must be set using milliseconds.  A timeout of 0 is infinite.
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, int commandTimeout, IDataParameter[] commandParameters)
        {
            try
            {                
                PrepareCommand(commandType, commandText, commandTimeout, commandParameters);
                IDbDataAdapter da = GetDataProviderDataAdapter();
                da.SelectCommand = command;
                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch
            {
                if (transaction == null)
                    connection.Close();
                else
                    RollbackTransaction();

                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        #endregion        

        #region ExecuteQuery

        /// <summary>
        /// Execute an arbitrary SQL statement, text or stored procedure, returning the number of 
        /// rows affected.  Often used for INSERT, UPDATE, or DELETE statements.  The command timeout 
        /// is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <returns>int</returns>
        public int ExecuteQuery(string commandText)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute an arbitrary SQL statement, text or stored procedure, returning the number
        /// of rows affected.  Often used for INSERT, UPDATE, or DELETE statements.  Specifying the
        /// type increases performance for stored procedures.  The command timeout is defaulted to
        /// 6000ms
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <returns>int</returns>
        public int ExecuteQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteQuery(commandText, commandType, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute an arbitrary SQL statement, text or stored procedure, with parameters, 
        /// returning the number of rows affected.  Often used for INSERT, UPDATE, or DELETE statements.
        /// The command timeout is defaulted to 6000ms
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>int</returns>
        public int ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandTimeoutDefault, commandParameters);
        }

        /// <summary>
        /// Execute an arbitrary SQL statement, text or stored procedure, with parameters,
        /// returning the number of rows affected.  Specifying the type increases performance for
        /// stored procedures.  The command timeout must be set using milliseconds. A timeout of
        /// 0 is infinite.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandTimeout">int</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>int</returns>
        public int ExecuteQuery(string commandText, CommandType commandType, int commandTimeout, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandTimeout, commandParameters);

                int intAffectedRows = command.ExecuteNonQuery();                

                return intAffectedRows;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Execute a database command, text or stored procedure, returning a single value.  The command
        /// timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute a database command, text or stored procedure, returning a single value.  Specifying the
        /// type improved performance for stored procedures.  The command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return this.ExecuteScalar(commandText, commandType, commandTimeoutDefault, null);
        }

        /// <summary>
        /// Execute a database command, text or stored procedure, with parameters, returning a single value.
        /// The command timeout is defaulted to 6000ms.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, commandTimeoutDefault, commandParameters);
        }

        /// <summary>
        /// Execute a database command, text or stored procedure, with parameters, returning a single value.
        /// Specifying the type improves performance for stored procedures.  The command time out must be
        /// set using milliseconds.  A timeout of 0 is infinite.
        /// </summary>
        /// <param name="commandText">string</param>
        /// <param name="commandType">CommandType</param>
        /// <param name="commandTimeout">int</param>
        /// <param name="commandParameters">IDataParameter[]</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string commandText, CommandType commandType, int commandTimeout, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandTimeout, commandParameters);

                object objValue = command.ExecuteScalar();
                if (objValue != DBNull.Value)
                    return objValue;
                else
                    return null;
            }
            catch
            {
                if (transaction != null)
                    RollbackTransaction();

                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// The factory returns a concrete class of the specified data access layer.
    /// </summary>
    public sealed class GHCDataAccessLayerFactory
    {
        // private constructor - all methods are static
        private GHCDataAccessLayerFactory() { }

        public GHCDataAccessLayer GHCDataAccessLayer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        
        /// <summary>
        /// Get the data access layer for the specified database. The environment (Prod vs Dev) 
        /// will be inferred from the machine context.
        /// </summary>
        /// <param name="dataProviderType">DataProviderType</param>
        /// <param name="database">string</param>
        /// <returns>GHCDataAccessLayer</returns>
        public static GHCDataAccessLayer GetDataAccessLayer(DataProviderType dataProviderType, string database)
        {            
            return GetDataAccessLayer(GHCConnectionStringBuilder.GetConnectionString(database, dataProviderType), dataProviderType);
        }

        /// <summary>
        /// Get the data access layer for the specified database.  The environment (Prod vs Dev) is specified overrides
        /// the environment inferred from the machine context.
        /// </summary>
        /// <param name="environment">Environment</param>
        /// <param name="dataProviderType">DataProviderType</param>
        /// <param name="database">string</param>
        /// <returns>GHCDataAccessLayer</returns>
        public static GHCDataAccessLayer GetDataAccessLayer(DBEnvironment environment, DataProviderType dataProviderType, string database)
        {
            return GetDataAccessLayer(GHCConnectionStringBuilder.GetConnectionString(database, dataProviderType, environment), dataProviderType);
        }       

        /// <summary>
        /// Directly gets the data access layer using the provided connection string.  The environment and default database
        /// is overridden.
        /// </summary>
        /// <param name="connectionString">string</param>
        /// <param name="dataProviderType">DataProviderType</param>
        /// <returns></returns>
        public static GHCDataAccessLayer GetDataAccessLayer(string connectionString, DataProviderType dataProviderType)
        {
            switch (dataProviderType)
            {
                //case DataProviderType.OleDb:
                //    return new OleDbDataAccessLayer(connectionString);

                //case DataProviderType.Odbc:
                //    return new OdbcDataAccessLayer(connectionString);

                case DataProviderType.Sql:
                    return new SqlDataAccessLayer(connectionString);

                default:
                    throw new ArgumentException("Invalid data access layer provider type.");
            }
        }
        
    }
}

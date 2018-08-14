using System;
using System.Data;
using System.Data.SqlClient;

namespace Ghc.Utility.DataAccess
{
    public class SqlDataAccessLayer : GHCDataAccessLayer
    {
        public SqlDataAccessLayer() { }
        public SqlDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        internal override IDbConnection GetDataProviderConnection()
        {
            return new SqlConnection();
        }

        internal override IDbCommand GetDataProviderCommand()
        {
            return new SqlCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new SqlDataAdapter();
        }
    }
}

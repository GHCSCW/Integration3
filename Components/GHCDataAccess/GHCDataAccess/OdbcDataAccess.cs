using System;
using System.Data;
using System.Data.Odbc;

namespace Ghc.Utility.DataAccess
{
    public class OdbcDataAccessLayer : GHCDataAccessLayer
    {
        public OdbcDataAccessLayer() { }
        public OdbcDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        internal override IDbConnection GetDataProviderConnection()
        {
            return new OdbcConnection();
        }

        internal override IDbCommand GetDataProviderCommand()
        {
            return new OdbcCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new OdbcDataAdapter();
        }
    }
}

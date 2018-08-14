using System;
using System.Data;
using System.Data.OleDb;

namespace Ghc.Utility.DataAccess
{
    public class OleDbDataAccessLayer : GHCDataAccessLayer
    {
        public OleDbDataAccessLayer() {}
        public OleDbDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        internal override IDbConnection GetDataProviderConnection()
        {
            return new OleDbConnection();
        }

        internal override IDbCommand GetDataProviderCommand()
        {
            return new OleDbCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new OleDbDataAdapter();
        }
    }
}

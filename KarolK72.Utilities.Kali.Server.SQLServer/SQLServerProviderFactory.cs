using KarolK72.Utilities.Kali.Server.Library.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.SQLServer
{
    public class SQLServerProviderFactory : ISqlProviderFactory<ISqlProvider>
    {
        private readonly SQLServerOptions _options;
        public SQLServerProviderFactory(SQLServerOptions sqlServerOptions)
        {
            _options = sqlServerOptions;
            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            {
                throw new ArgumentNullException(nameof(_options.ConnectionString));
            }
            try
            {
                SqlConnection connection = new SqlConnection(_options.ConnectionString);
                connection.Open();
            } catch(Exception ex)
            {
                throw new Exception("Could not connect to the SQL database", ex);
            }
        }
        public ISqlProvider CreateNew()
        {
            SqlConnection connection = new SqlConnection(_options.ConnectionString);
            connection.Open();
            return new SQLServerProvider(connection);
        }
    }
}

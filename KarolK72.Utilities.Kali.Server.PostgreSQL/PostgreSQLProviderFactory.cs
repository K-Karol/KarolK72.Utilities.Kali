using KarolK72.Utilities.Kali.Server.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarolK72.Utilities.Kali.Server.PostgreSQL
{
    public class PostgreSQLProviderFactory : ISqlProviderFactory<ISqlProvider>
    {
        private readonly PostgreSQLOptions _options;
        public PostgreSQLProviderFactory(PostgreSQLOptions postgreSQLOptions)
        {
            _options = postgreSQLOptions;
            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            {
                throw new ArgumentNullException(nameof(_options.ConnectionString));
            }
        }
        public ISqlProvider CreateNew()
        {
            var conn = new Npgsql.NpgsqlConnection(_options.ConnectionString);
            conn.Open();
            return new PostgreSQLProvider(conn);
        }
    }
}

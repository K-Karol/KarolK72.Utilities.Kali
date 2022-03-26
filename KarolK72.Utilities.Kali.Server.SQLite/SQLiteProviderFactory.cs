using KarolK72.Utilities.Kali.Server.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KarolK72.Utilities.Kali.Server.SQLite
{
    public class SQLiteProviderFactory : ISqlProviderFactory<ISqlProvider>
    {
        private readonly SQLiteOptions _options;
        public SQLiteProviderFactory(SQLiteOptions sqliteOptions)
        {
            _options = sqliteOptions;
            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            {
                throw new ArgumentNullException(nameof(_options.ConnectionString));
            }
        }
        public ISqlProvider CreateNew()
        {
            System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(_options.ConnectionString);
            connection.Open();
            return new SQLiteProvider(connection);
        }
    }
}

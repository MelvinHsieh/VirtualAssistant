using Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;

namespace dataservice.Tests
{
    internal class MedicineDbTestConnection : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<MedicineDbContext> _contextOptions;

        public MedicineDbTestConnection()
        {
            var builder = new DbContextOptionsBuilder<MedicineDbContext>();
            builder.UseSqlite(CreateInMemoryDatabase());
            builder.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));

            _contextOptions = builder.Options;
            _connection = RelationalOptionsExtension.Extract(_contextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Data Source =:memory:");

            connection.Open();

            return connection;
        }

        public DbContextOptions<MedicineDbContext> GetOptions()
        {
            return _contextOptions;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}

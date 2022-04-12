using Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;

namespace dataservice.Tests
{
    internal class PatientDbTestConnection : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<PatientDbContext> _contextOptions;

        public PatientDbTestConnection()
        {
            var builder = new DbContextOptionsBuilder<PatientDbContext>();
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

        public DbContextOptions<PatientDbContext> GetOptions()
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

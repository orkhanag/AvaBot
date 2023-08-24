using AvaBot.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace AvaBot.Infrastructure.Persistence
{
    public class DapperContext : IDapperDbContext
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresDB");
        }
        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}

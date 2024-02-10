using System.Data;
using CQRS.BankAPI.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace CQRS.BankAPI.Persistence.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
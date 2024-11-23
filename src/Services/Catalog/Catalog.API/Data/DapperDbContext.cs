using System.Data;
using Npgsql;

namespace Catalog.API.Data;
public class DapperDbContext : IDisposable
{
    private readonly string _connectionString;
    private IDbConnection? _connection;

    public DapperDbContext(string connectionString)
    {   
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        _connection = new NpgsqlConnection(_connectionString);
        return _connection;
    }
    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open )
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
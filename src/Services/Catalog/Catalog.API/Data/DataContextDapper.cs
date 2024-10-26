using System.Data;
using Dapper;
using Npgsql;

namespace Catalog.API.Data;

class DataContextDapper
{
    private readonly IConfiguration _config;

    public DataContextDapper(IConfiguration config)
    {
        Console.WriteLine("DataContextDapper created");
        _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sqlQuery, object? parameters = null)
    {
        IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sqlQuery, parameters);
    }

    public T LoadSingleData<T>(string sqlQuery, object? parameters = null)
    {
        IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sqlQuery, parameters);
    }

    public bool ExecuteSql(string sql, object? parameters = null)
    {
        IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql, parameters) > 0;
    }
}

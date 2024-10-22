using System.Data;
using Dapper;
using Npgsql;

namespace PlaygroundAPI.Data
{
    class DataContextDapper
    {  
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sqlQuery)
        {
            IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sqlQuery);
        }

        public T LoadDataSingle<T>(string sqlQuery)
        {
            IDbConnection dbConnection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sqlQuery);
        }   
    }
}
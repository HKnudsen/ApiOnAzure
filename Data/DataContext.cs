using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ApiOnAzure.Data
{
    class DataContext
    {
        private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public IEnumerable<T> LoadDataWithParameters<T>(string sql, DynamicParameters sqlParameters)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql, sqlParameters);
        }

        public bool ExecuteSqlWithParameters(string sql, DynamicParameters sqlParameters)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql, sqlParameters) > 0;
        }

         
    }
}
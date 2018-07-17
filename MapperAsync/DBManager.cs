using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Dapper;

namespace MapperAsync
{
    public static class DBManager<TEntity> where TEntity : class
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;

        public static IDbConnection GetOpenConnection()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        public static async Task<IEnumerable<dynamic>> ExecuteDynamic(string sql, object param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.QueryAsync(sql, param, commandType: type);
            }
        }

        public static async Task<IEnumerable<TEntity>> ExecuteReader(string sql, object param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.QueryAsync<TEntity>(sql, param, commandType: type);
            }
        }

        // use when get id after inserted
        public static int ExecuteSingle(string sql, object param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return db.Query<int>(sql, param, commandType: type).Single();
            }
        }

        public static async Task<TEntity> FindById(string sql, int id, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.QueryFirstOrDefaultAsync<TEntity>(sql, new { ID = id }, commandType: type);
            }
        }

        public static async Task<int> Execute(string sql, object param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.ExecuteAsync(sql, param, commandType: type);
            }
        }

        public static async Task<int> ExecuteMultiple(string sql, object[] param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.ExecuteAsync(sql, param, commandType: type);
            }
        }

        public static async Task<object> ExecuteScalar(string sql, object param = null, CommandType type = CommandType.Text)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return await db.ExecuteScalarAsync(sql, param, commandType: type);
            }
        }

    }
}

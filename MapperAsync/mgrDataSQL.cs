using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MapperAsync
{
    public class mgrDataSQL
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["cnnString"].ConnectionString;
        public static string mysql = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;

        public static async Task<DataTable> ExecuteReader(string sql, Dictionary<string, object> param = null, string ConnectionString = null)
        {
            var connectionStr = connStr;
            if (!string.IsNullOrEmpty(ConnectionString)) connectionStr = ConnectionString;
            using (var connect = new SqlConnection(connectionStr))
            {
                using (var command = new SqlCommand(sql, connect))
                {
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    DataTable dtb = new DataTable();
                    connect.Open();
                    var reader = await command.ExecuteReaderAsync();
                    dtb.Load(reader);
                    return dtb;
                }
            }

        }
        public static async Task<int> ExecuteNonQuery(string sql, Dictionary<string, object> param = null)
        {
            using (var connect = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand(sql, connect))
                {
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    connect.Open();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }


        public static async Task<object> ExecuteScalar(string sql, Dictionary<string, object> param = null)
        {
            using (var connect = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand(sql, connect))
                {
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    connect.Open();
                    return await command.ExecuteScalarAsync();
                }
            }
        }
        // store procedure
        public static async Task<DataTable> ExecuteStoreReader(string storename, Dictionary<string, object> param = null)
        {
            using (var connect = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand(storename, connect))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    connect.Open();
                    var reader = await command.ExecuteReaderAsync();
                    DataTable dtb = new DataTable();
                    dtb.Load(reader);
                    return dtb;
                }
            }
        }

        public static async Task<int> ExecuteStoreNonQuery(string storename, Dictionary<string, object> param = null)
        {
            using (var connect = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand(storename, connect))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    connect.Open();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        public static async Task<object> ExecuteStoreScalar(string storename, Dictionary<string, object> param = null)
        {
            using (var connect = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand(storename, connect))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            command.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                        }
                    }
                    connect.Open();
                    return await command.ExecuteScalarAsync();
                }
            }
        }
    }
}

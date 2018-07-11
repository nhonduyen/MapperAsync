using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MapperAsync
{
    public class DbHandler
    {
        public async Task<DataTable> GetAllDbName(string connectionString)
        {
            var sql = "SELECT name from sys.databases where name not in('master','tempdb','model','msdb','ReportServer','ReportServerTempDB')";
            return await mgrDataSQL.ExecuteReader(sql, null, connectionString);
        }
        public async Task<DataTable> GetTableNames(string db, string connectionString)
        {
            var sql = "USE " + db + ";  SELECT name FROM sys.Tables ";
            return await mgrDataSQL.ExecuteReader(sql, null, connectionString);
        }
        public async Task<DataTable> GetColumnsNames(string dbName, string table, string connectionString)
        {
            var sql = "SELECT COLUMN_NAME, DATA_TYPE FROM " + dbName + ".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + table + "'";
            return await mgrDataSQL.ExecuteReader(sql, null, connectionString);
        }

        public string GetType(string dataType)
        {
            if (dataType.Contains("char") || dataType.Contains("text"))
                return "string";
            if (dataType.Contains("int"))
                return "int";
            if (dataType.Contains("float"))
                return "float";
            if (dataType.Contains("double"))
                return "double";
            if (dataType.Contains("date"))
                return "DateTime";
            if (dataType.Contains("decimal"))
                return "decimal";
            return "string";
        }
    }
}

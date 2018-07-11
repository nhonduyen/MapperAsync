using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MapperAsync.Template
{
    public class ADO
    {
public string ID { get; set; }

        public ADO() { }

        void constructor() { }

        public virtual async Task<int> Update1Column(int id, string COLUMN, string VALUE)
        {
            string sql = string.Format(@"UPDATE ADO SET {0}=@VALUE WHERE ID=@ID", COLUMN);
            Dictionary<string, object> param = new Dictionary<string, object>(2);
            param.Add("@ID", id);
            param.Add("@VALUE", VALUE);

            return await mgrDataSQL.ExecuteNonQuery(sql, param);
        }
        public virtual async Task<int> Delete(int id)
        {
            string sql = "DELETE ADO WHERE ID=@ID";
            Dictionary<string, object> param = new Dictionary<string, object>(1);
            param.Add("@ID", id);
            return await mgrDataSQL.ExecuteNonQuery(sql, param);
        }

        public virtual async Task<DataTable> GetAll(string query = "", string listcolumn = "")
        {
            var sql = "SELECT * FROM ADO WHERE 1=1 " + query;
            if (!string.IsNullOrEmpty(listcolumn))
            {
                sql = sql.Replace("*", listcolumn);
            }
            return await mgrDataSQL.ExecuteReader(sql);
        }

        public virtual async Task DeleteAll()
        {
            await mgrDataSQL.ExecuteNonQuery("TRUNCATE TABLE ADO");
        }

        public virtual async Task<int> GetCount(string query = "")
        {
            var result = await mgrDataSQL.ExecuteScalar("SELECT COUNT(1) FROM ADO WHERE 1=1 " + query);
            return Convert.ToInt32(result);
        }
        public async Task<DataTable> GetAllPaging(int start = 1, int end = 10, string query = "", string listcolumn = "")
        {
            string sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id DESC) AS ROWNUM,* FROM ADO WHERE 1=1 " + query + ") AS V WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum ";
            if (!string.IsNullOrEmpty(listcolumn))
            {
                sql = sql.Replace("*", listcolumn);
            }
            Dictionary<string, object> param = new Dictionary<string, object>(2);
            param.Add("@start", start);
            param.Add("@end", end);
            return await mgrDataSQL.ExecuteReader(sql, param);

        }

        public async Task<int> SpecialCount()
        {
            var sql = string.Format(@"
SELECT SUM (row_count)
FROM sys.dm_db_partition_stats
WHERE object_id=OBJECT_ID('ADO')   
AND (index_id=0 or index_id=1);
");
            var result = await mgrDataSQL.ExecuteScalar(sql);
            return Convert.ToInt32(result);
        }
        void insertfunc() { }
        void updatefunc() { }
    }
}

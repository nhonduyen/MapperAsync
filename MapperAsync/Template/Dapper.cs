using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperAsync.Template
{
    public class Dapper
    {

public int ID { get; set; }

        void constructor() { }

        public virtual async Task<IEnumerable<Dapper>> Select(int ID = 0, string listcolumn = "*")
        {
            var sql = string.Format(@"SELECT {0} FROM Dapper ", listcolumn);
            if (ID == 0)
            {
                return await DBManager<Dapper>.ExecuteReader(sql);
            }
            sql += " WHERE ID=@ID";
            return await DBManager<Dapper>.ExecuteReader(sql, new { ID = ID });
        }

        public virtual async Task<Dapper> FindById(int ID = 0, string listcolumn = "*")
        {
            var sql = string.Format(@"SELECT TOP 1 {0} FROM Dapper WHERE ID=@ID", listcolumn);
            return await DBManager<Dapper>.FindById(sql, ID);
        }

        public virtual async Task<IEnumerable<Dapper>> SelectPaging(int start = 0, int end = 10, string query = "", string listcolumn = "*")
        {
            var sql = string.Format(@"
SELECT {0} FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, {1} FROM Dapper WHERE 1=1 {2}) as u  
WHERE   RowNum BETWEEN @start AND @end ORDER BY RowNum;", listcolumn, listcolumn, query);
            return await DBManager<Dapper>.ExecuteReader(sql, new { start = start, end = end });
        }

        public virtual async Task<int> GetCount(string query = "")
        {
            var sql = "SELECT COUNT(1) FROM Dapper WHERE 1=1 " + query;
            var result = await DBManager<Dapper>.ExecuteScalar(sql);
            return Convert.ToInt32(result);
        }

        public virtual async Task<int> Update1Column(int ID, string COLUMN, string VALUE)
        {
            var sql = string.Format(@"UPDATE Dapper SET {0}=@VALUE WHERE ID=@ID", COLUMN);

            return await DBManager<Dapper>.Execute(sql, new { ID = ID, VALUE = VALUE });
        }
        public virtual async Task<int> Delete(int ID = 0)
        {
            var sql = "DELETE FROM Dapper ";
            if (ID == 0)
            {
                return await DBManager<Dapper>.Execute(sql);
            }
            sql += " WHERE ID=@ID ";
            return await DBManager<Dapper>.Execute(sql, new { ID = ID });
        }

        public async Task DeleteAll()
        {
            await DBManager<Dapper>.Execute("TRUNCATE TABLE Dapper");
        }
        public async Task<int> SpecialCount()
        {
            var sql = string.Format(@"
SELECT SUM (row_count)
FROM sys.dm_db_partition_stats
WHERE object_id=OBJECT_ID('Dapper')   
AND (index_id=0 or index_id=1);
");
            var result = await DBManager<Dapper>.ExecuteScalar(sql);
            return Convert.ToInt32(result);
        }

        void insert() { }
        void update() { }
    }
}

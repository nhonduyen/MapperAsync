﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MapperAsync
{
    public class MapDapper
    {
        public string MakeInsert(string table, DataTable columns)
        {
            DbHandler db = new DbHandler();
            var insert = "INSERT INTO " + table + "(";
            var tempInsert = "";
            var parameter = "";
            var parameter1 = "new { ";
            foreach (DataRow col in columns.Rows)
            {
                var colName = col["COLUMN_NAME"].ToString();
                var type = db.GetType(col["DATA_TYPE"].ToString());

                if (string.Compare(colName, "ID") != 0)
                {
                    parameter += type + " " + colName + ",";
                    insert += colName + ",";
                    tempInsert += "@" + colName + ",";
                    parameter1 += colName + " = " + colName + ",";

                }
            }
            insert = insert.Remove(insert.Length - 1) + ") VALUES(" + tempInsert.Remove(tempInsert.Length - 1) + ")";
            parameter1 = parameter1.Remove(parameter1.Length - 1);
            string insertFunc = "public virtual async Task<int> Insert(" + parameter.Remove(parameter.Length - 1) + ")" + System.Environment.NewLine;
            insertFunc += "        {" + System.Environment.NewLine;
            insertFunc += "            var sql = \"" + insert.Remove(insert.Length - 1) + ")\";" + System.Environment.NewLine;
            insertFunc += "            return await DBManager<" + table + ">.Execute(sql, " + parameter1 + "});" + System.Environment.NewLine;
            insertFunc += "        }" + System.Environment.NewLine;
            var insert2 = insertFunc.Replace(parameter.Remove(parameter.Length - 1), table + " " + table);

            insert2 = insert2.Replace(parameter1, table);
            return insertFunc + insert2;
        }

        public string MakeUpdateFunc(string table, DataTable columns)
        {
            DbHandler db = new DbHandler();
            var update = "UPDATE " + table + " SET ";

            var parameter1 = " new { ";
            var parameter = "";
            foreach (DataRow col in columns.Rows)
            {
                var colName = col["COLUMN_NAME"].ToString();
                var type = db.GetType(col["DATA_TYPE"].ToString());
                parameter += type + " " + colName + ", ";
                parameter1 += colName + " = " + colName + ",";
                if (string.Compare(colName, "ID") != 0)
                {
                    update += colName + "=@" + colName + ",";
                }
            }
            update = update.Remove(update.Length - 1) + " WHERE ID=@ID\"";

            string UpdateFunc = "public virtual async Task<int> Update(" + parameter.Remove(parameter.Length - 2) + ")" + System.Environment.NewLine;
            UpdateFunc += "        {" + System.Environment.NewLine;
            UpdateFunc += "            var sql = \"" + update + ";" + System.Environment.NewLine;
            UpdateFunc += System.Environment.NewLine;
            UpdateFunc += "            return await DBManager<" + table + ">.Execute(sql, " + parameter1.Remove(parameter1.Length - 1) + "});" + System.Environment.NewLine;
            UpdateFunc += "        }" + System.Environment.NewLine;
            var update2 = UpdateFunc.Replace(parameter.Remove(parameter.Length - 2), table + " " + table);
            update2 = update2.Replace(parameter1.Remove(parameter1.Length - 1), table);
            return UpdateFunc + update2;
        }


        public string MakeConstructor(string table, DataTable columns)
        {
            DbHandler db = new DbHandler();

            var parameter = "";
            var content = "";
            foreach (DataRow col in columns.Rows)
            {
                var colName = col["COLUMN_NAME"].ToString();
                var type = db.GetType(col["DATA_TYPE"].ToString());
                parameter += type + " " + colName + ", ";
                content += "            this." + colName + " = " + colName + ";" + System.Environment.NewLine;
            }

            string UpdateFunc = "public " + table + "(" + parameter.Remove(parameter.Length - 2) + ")" + System.Environment.NewLine;
            UpdateFunc += "        {" + System.Environment.NewLine;
            UpdateFunc += content;
            UpdateFunc += "        }" + System.Environment.NewLine;
            UpdateFunc += "        public " + table + "() { }" + System.Environment.NewLine;
            return UpdateFunc;
        }

    }
}

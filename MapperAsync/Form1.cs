using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MapperAsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mgrDataSQL.connStr))
            {
                txtIp.Text = "172.25.215.17";
                txtDbName.Text = "MBO";
                txtUsername.Text = "sa";
                txtPassword.Text = "pvst123@";
            }
            r1.Checked = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
            {
                lblLink.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private async void btnMap_Click(object sender, EventArgs e)
        {
            var location = lblLink.Text;
            var template = "../../Template/ADO.cs";
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please choose file path");
                return;
            }
            MapADOFunction map = new MapADOFunction();
            FileHandler filehandler = new FileHandler();
            DbHandler db = new DbHandler();
            string connectionString = mgrDataSQL.connStr;
            if (string.Compare(txtDbName.Text.Trim(), "MBO") != 0)
                connectionString = "Data Source=" + txtIp.Text + ";Initial Catalog=" + txtDbName.Text + ";User ID=" + txtUsername.Text
                    + ";Password=" + txtPassword.Text + ";Integrated Security=False";
            var tables = await db.GetTableNames(txtDbName.Text, connectionString);
            foreach (DataRow row in tables.Rows)
            {
                var tbname = row["NAME"].ToString();

                var columns = await db.GetColumnsNames(txtDbName.Text, tbname, connectionString);

                var constructor = map.MakeConstructor(tbname, columns);
                var insert = map.MakeInsert(tbname, columns);
                var update = map.MakeUpdateFunc(tbname, columns);

                var property = "";
                foreach (DataRow col in columns.Rows)
                {
                    var colName = col["COLUMN_NAME"].ToString();
                    var type = db.GetType(col["DATA_TYPE"].ToString());
                    property += "        public " + type + " " + colName + " { get; set; }" + System.Environment.NewLine;

                }

                string strTemplate = filehandler.ReadFile(template);
                strTemplate = strTemplate.Replace("MapperAsync.Template", txtNamespace.Text);
                strTemplate = strTemplate.Replace("ADO", tbname);
                strTemplate = strTemplate.Replace("public string ID { get; set; }", property);
                strTemplate = strTemplate.Replace("void constructor() { }", constructor);
                strTemplate = strTemplate.Replace("void insertfunc() { }", insert);
                strTemplate = strTemplate.Replace("void updatefunc() { }", update);


                if (filehandler.FileExist(location + "\\" + tbname + ".cs"))
                {
                    filehandler.DeleteFile(location + "\\" + tbname + ".cs");
                }

                try
                {
                    filehandler.WriteFile(strTemplate, location + "\\" + tbname + ".cs");
                }
                catch
                {
                    MessageBox.Show("Cannot open file");
                    return;
                }
                txtResult.Text += "Mapping " + tbname + ".cs success" + System.Environment.NewLine;
            }
            MessageBox.Show("Mapping done!");
            Process.Start(location);
        }

        private async void btnDapper_Click(object sender, EventArgs e)
        {
            var location = lblLink.Text;
            var template = "../../Template/Dapper.cs";
            if (string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please choose file path");
                return;
            }
            MapDapper map = new MapDapper();
            FileHandler filehandler = new FileHandler();
            DbHandler db = new DbHandler();
            string connectionString = mgrDataSQL.connStr;
            if (string.Compare(txtDbName.Text.Trim(), "MBO") != 0)
                connectionString = "Data Source=" + txtIp.Text + ";Initial Catalog=" + txtDbName.Text + ";User ID=" + txtUsername.Text
                    + ";Password=" + txtPassword.Text + ";Integrated Security=False";
            var tables =await db.GetTableNames(txtDbName.Text, connectionString);
            foreach (DataRow row in tables.Rows)
            {
                var tbname = row["NAME"].ToString();
                var columns =await db.GetColumnsNames(txtDbName.Text, tbname, connectionString);

                var constructor = map.MakeConstructor(tbname, columns);
                var insert = map.MakeInsert(tbname, columns);
                var update = map.MakeUpdateFunc(tbname, columns);
                var param = "";
                foreach (DataRow col in columns.Rows)
                {
                    var colName = col["COLUMN_NAME"].ToString();
                    var type = db.GetType(col["DATA_TYPE"].ToString());
                    param += "        public " + type + " " + colName + " { get; set; }" + System.Environment.NewLine;
                }

                string strTemplate = filehandler.ReadFile(template);
                strTemplate = strTemplate.Replace("MapperAsync.Template", txtNamespace.Text);
                strTemplate = strTemplate.Replace("Dapper", tbname);
                strTemplate = strTemplate.Replace("void constructor() { }", constructor);
                strTemplate = strTemplate.Replace("void insert() { }", insert);
                strTemplate = strTemplate.Replace("void update() { }", update);
                strTemplate = strTemplate.Replace("public int ID { get; set; }", param);
                if (filehandler.FileExist(location + "\\" + tbname + ".cs"))
                {
                    filehandler.DeleteFile(location + "\\" + tbname + ".cs");
                }
                try
                {
                    filehandler.WriteFile(strTemplate, location + "\\" + tbname + ".cs");
                }
                catch
                {
                    MessageBox.Show("Cannot open file");
                    return;
                }


                txtResult.Text += "Mapping " + tbname + ".cs success" + System.Environment.NewLine;
            }
            MessageBox.Show("Mapping done!");
            Process.Start(location);
        }
    }
}

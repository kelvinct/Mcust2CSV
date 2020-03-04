using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DbfDataReader;
using System.Text.RegularExpressions;
namespace WindowsFormsApp1
{
    public partial class Mcustimport : Form
    {
        private DataTable table = null;
        private string folderPath = "";
        private int minFormWidth = 900;
        private int minFormHeight = 700;

        private DataTable _dataTable;
        private string _filter = string.Empty;
        private string _sql = string.Empty;
        private string _database = string.Empty;

        public Mcustimport()
        {
            InitializeComponent();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            load(textBox2.Text);
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();

            this.Hide();
            f1.ShowDialog();
            this.Close();
        }

        private void Mcustimport_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        void load(string path)
        {


            //           comboBox1.SelectedIndex = 0;
            string target = @"c:\temp";
            Console.WriteLine("The current directory is {0}", path);

            DbfTable dbfTable = null;

            var dbfPath = path + "\\mcust.dbf";
            dbfTable = new DbfTable(dbfPath, Encoding.GetEncoding(950));
            DataTable dt = new DataTable();
            dt.Clear();


            foreach (var dbfColumn in dbfTable.Columns)
            {
                var name = dbfColumn.Name;
                var columnType = dbfColumn.ColumnType;
                var length = dbfColumn.Length;
                var decimalCount = dbfColumn.DecimalCount;
                name = name.Replace("CUST_NO", "Customer Code");
                name = name.Replace("CUST_NAME", "Company name");
                name = name.Replace("TEL", "Phone Number");
                name = name.Replace("FAX", "Fax Number");
                name = name.Replace("CONTACT", "Contact person");
                name = name.Replace("INPDATE", "Create Date");
                name = name.Replace("INPUSER", "Create Date By");
                name = name.Replace("UPDDATE", "Last Update");
                name = name.Replace("NAMETREE", "Name Tree");
                name = name.Replace("UPDUSER", "Last Update By");
                name = name.Replace("JOBTITLE", "Job Tile");

                dt.Columns.Add(name);
            }

            DataColumn[] keyColumn = new DataColumn[2];
            keyColumn[0] = dt.Columns["CUST_ID"];

            dt.PrimaryKey = keyColumn;

            var dbfRecord = new DbfRecord(dbfTable);

            while (dbfTable.Read(dbfRecord))
            {
                DataRow _ravi = dt.NewRow();
                int I = 0;
                foreach (var dbfValue in dbfRecord.Values)
                {
                    _ravi[I] = dbfValue.ToString().Trim();
                    I++;
                }
                dt.Rows.Add(_ravi);
            }
            //    dt.Columns.Remove("CABLE");
            //  label1.Text = dt.Rows.Count.ToString();
            dataGridView1.DataSource = dt;

            dbfPath = path + "\\mcust_d.dbf";
            var dbfTable2 = new DbfTable(dbfPath, Encoding.GetEncoding(950));
            var mcust_d_table = new DataTable();
            mcust_d_table.Clear();


            foreach (var dbfColumn in dbfTable2.Columns)
            {
                var name = dbfColumn.Name;
                var columnType = dbfColumn.ColumnType;
                var length = dbfColumn.Length;
                var decimalCount = dbfColumn.DecimalCount;

                name = name.Replace("TELEX", "TELEX NUMBER");
                name = name.Replace("CABLE", "MOBILE");
                name = name.Replace("ADD1", "ADDRESS1");
                name = name.Replace("ADD2", "ADDRESS2");
                name = name.Replace("ADD3", "ADDRESS3");
                name = name.Replace("ADD4", "ADDRESS4");
                //  name = name.Replace("REMARKS", "Remarks");

                name = name.Replace("PASSWD", "PASSWORD");

                name = name.Replace("PHOTO", "Picture File Name");
                name = name.Replace("ISWHATSAPP", "WHATSAPP");
                name = name.Replace("TRADETERM", "TRADE TERM");
                name = name.Replace("PAYTERM", "PAYMENT TERM");
                name = name.Replace("CUR", "CURRENCY");
                name = name.Replace("BK_NO", "BROKER NO");
                name = name.Replace("CCASS_ID", "CCASS ID");
                name = name.Replace("ISWHATSAPP", "WHATSAPP");
                name = name.Replace("ISWECHAT", "WECHAT");
                name = name.Replace("ISLINE", "LINE");
                mcust_d_table.Columns.Add(name);
            }
            var dbfRecord2 = new DbfRecord(dbfTable2);
            DataColumn[] keyColumn2 = new DataColumn[2];
            keyColumn2[0] = mcust_d_table.Columns["CUST_ID"];
            mcust_d_table.PrimaryKey = keyColumn2;

            while (dbfTable2.Read(dbfRecord2))
            {
                DataRow _ravi2 = mcust_d_table.NewRow();
                int I = 0;
                foreach (var dbfValue2 in dbfRecord2.Values)
                {
                    string mobile = "";
                    string email = "";
                    if (I == 2)
                    {
                        mobile = dbfValue2.ToString().Trim();

                        if (StringExtensions.ValidatePhoneNumber(mobile, true) && mobile.Length == 8)
                        {
                            _ravi2[I] = mobile;
                        }
                    }
                    else if (I == 12)
                    {
                        email = dbfValue2.ToString().Trim();

                        if (popclass.ValidateEmail(email))
                        {
                            _ravi2[I] = email;
                        }
                        else
                        {
                            _ravi2[I] = "";
                        }
                    }
                    else
                    {
                        _ravi2[I] = dbfValue2.ToString();
                    }

                    I++;
                }

                {
                    mcust_d_table.Rows.Add(_ravi2);
                }
            }


            dt.Merge(mcust_d_table);

            dt.Columns.Remove("EMAILSRH");
            dt.Columns.Remove("CABLESRH");
            dt.Columns.Remove("_NullFlags");
            //    label3.Text = dt.Rows.Count.ToString();
            table = dt;
            dataGridView1.DataSource = dt;
            // dataGridView3.Columns["CUST_ID"].Visible = false;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            for (int k = 0; k <= dataGridView1.ColumnCount - 1; k++)
            {
                string s = dataGridView1.Columns[k].HeaderText;
                listBox1.Items.Add(s);
                comboBox1.Items.Add(s);
            }
            dbfTable.Close();
            dbfTable2.Close();
            comboBox1.SelectedIndex = 0;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                textBox2.Text = folderPath;
            }
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                table.DefaultView.RowFilter = "[" + comboBox1.Text + "]" + " like '%" + textBox1.Text + "%'";
                dataGridView1.DataSource = table;  
            }
            catch
            {
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
           
            popclass.MoveListBoxItems(listBox1, listBox2); 
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            popclass.MoveListBoxItems(listBox2, listBox1);
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            popclass.MoveItem(-1, listBox2);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            popclass.MoveItem(1, listBox2);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            bool chkemail = false;
            bool chkmobile = false;

            string[] selectcolumn = listBox2.Items
           .OfType<object>()
           .Select(item => item.ToString())
           .ToArray();
            foreach (string b in selectcolumn)
            {
                if (b.ToUpper() == "EMAIL")
                {
                    chkemail = true;
                }
                if (b.ToUpper() == "MOBILE")
                {
                    chkmobile = true;
                }
            }

            if (selectcolumn.Length > 0)
            {
                //dataGridView2.DataSource = SelectedColumns(table, selectcolumn);
                //DownloadCSV((DataTable)(dataGridView2.DataSource), "");
                dataGridView2.DataSource =popclass. SelectedColumns(table, selectcolumn);
                //contain mobile column , email column
                if (checkBox2.Checked && chkmobile)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "mobile<>''";
                }
                if (checkBox3.Checked && chkemail)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "email<>''";
                }
                if (checkBox2.Checked && chkemail && checkBox3.Checked && chkmobile)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "email<>'' and mobile <>''";
                }
        
                DataTable dt = popclass.CloneAlteredDataTableSource(this.dataGridView2);

                popclass.DownloadCSV(dt, "csv_export.csv");
            }
            else

            {

                MessageBox.Show("no colum add to show");
            }
        }
    
    }
}

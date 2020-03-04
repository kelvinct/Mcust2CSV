using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
//using dBASE.NET;

using System.IO;
using System.Data.Odbc;
using System.Collections;

using System.Text.RegularExpressions;
using System.Web;
using DbfDataReader;

using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DataTable table = null;
        private string folderPath = "";
        private int minFormWidth = 900;
        private int minFormHeight = 700;

        private DataTable _dataTable;
        private string _filter = string.Empty;
        private string _sql = string.Empty;
        private string _database = string.Empty;

        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            comboBox2.SelectedIndex = 0;
          
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

            if (selectcolumn.Length >0 )
            {
                //dataGridView2.DataSource = SelectedColumns(table, selectcolumn);
                //DownloadCSV((DataTable)(dataGridView2.DataSource), "");
                dataGridView2.DataSource = SelectedColumns(table, selectcolumn);
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
                //  DataTable out = CloneAlteredDataTableSource(dataGridView4);

                DataTable dt = this.CloneAlteredDataTableSource(this.dataGridView2);

                DownloadCSV(dt, "");
            }
            else

            {

                MessageBox.Show("no colum add to show");
            }
          

        }


        private void Button2_Click(object sender, EventArgs e)
        {

         //   loaddata(textBox2.Text,comboBox3.Text);       

        }


        private void MoveListBoxItems(ListBox source, ListBox destination)
        {
            ListBox.SelectedObjectCollection sourceItems = source.SelectedItems;
            foreach (var item in sourceItems)
            {
                destination.Items.Add(item);
            }
            while (source.SelectedItems.Count > 0)
            {
                source.Items.Remove(source.SelectedItems[0]);
            }
        }
        //public static DataTable GetOdbcDbfDataTable(string Database, string OdbcString)
        //{
        //    DataTable myDataTable = new DataTable();
        //    OdbcConnection icn = OdbcDbfOpenConn(Database);
        //    OdbcDataAdapter da = new OdbcDataAdapter(OdbcString, icn);
        //    DataSet ds = new DataSet();
        //    ds.Clear();
        //    da.Fill(ds);
        //    myDataTable = ds.Tables[0];
        //    if (icn.State == ConnectionState.Open) icn.Close();
        //    return myDataTable;
        //}
   
        void loaddata2(String path)
        {            
           
           // comboBox1.SelectedIndex = 0;
            string target = @"c:\temp";  
            

            var dbfPath = path + "\\mcust.dbf";
            var dbfTable = new DbfTable(dbfPath, Encoding.GetEncoding(950));
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
                    _ravi[I] = dbfValue.ToString();
                    I++;
                }
                dt.Rows.Add(_ravi);
            }
            //   dataGridView1.DataSource = dt;

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
                //SQLstr = SQLstr + "MCUST_D.TELEX as \"Telex Number\", MCUST_D.CABLE as Mobile,";
                //SQLstr = SQLstr + "LTRIM(MCUST_D.ADD1) as Address1, MCUST_D.ADD2 as Address2, MCUST_D.ADD3 as Address3, MCUST_D.ADD4 as Address4,";
                //SQLstr = SQLstr + "MCUST_D.REMARKS as Remarks,MCUST_D.MARKS as MARKS,MCUST_D.TRADETERM as \"Trade Term\",MCUST_D.PAYTERM as \"Payment Term\",MCUST_D.CUR as CURRENCY,";
                //SQLstr = SQLstr + "MCUST_D.EMAIL,MCUST_D.HOMEPAGE,MCUST_D.ALIAS,MCUST_D.PASSWD as \"PASSWORD\" ,MCUST_D.BK_NO as \"BROKER NO\",MCUST_D.CCASS_ID as \"CCASS ID\",";
                //SQLstr = SQLstr + "MCUST_D.PHOTO as \"Picture File Name\",MCUST_D.ISWHATSAPP as WHATSAPP, MCUST_D.ISWECHAT as WECHAT ,MCUST_D.ISLINE as LINE ";
                //SQLstr = SQLstr + "FROM MCUST.DBF INNER JOIN MCUST_D.DBF ON MCUST.CUST_ID = MCUST_D.CUST_ID ";

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
                    _ravi2[I] = dbfValue2.ToString();
                    I++;
                }
                mcust_d_table.Rows.Add(_ravi2);
            }

            //   dataGridView2.DataSource = mcust_d_table;
            dt.Merge(mcust_d_table);

            dt.Columns.Remove("_NullFlags");
            table = dt;

            dataGridView1.AutoGenerateColumns = true;
        }
        //void loaddata(String path,String selectText)
        //{
        //     string cnstr = @"Provider=VFPOLEDB.1;Data Source=" + path + ";Collating Sequence=MACHINE;Mode=Read";
        //    //string cnstr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + "; Extended Properties=dBASE IV;");

        //  //  OleDbConnection oleDbCon = new OleDbConnection(cnstr);

        //    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + " ;Extended Properties=dBASE IV;";
        //    OleDbConnection oleDbCon = new OleDbConnection(connectionString);


        //    oleDbCon.Open();
        //    table = new DataTable();

        //    String SQLstr = "";

        //    SQLstr = "Select MCUST.CUST_ID as CUST_ID,MCUST.CUST_NO as \"Customer Code\",  MCUST.CUST_NAME as \"Company name\", MCUST.TYPE as Type, MCUST.CONTACT as \"Contact person\",";
        //    SQLstr = SQLstr + "MCUST.TEL as TEL, MCUST.FAX as FAX, MCUST.INPDATE as \"Create Date\",";
        //    SQLstr = SQLstr + "MCUST.INPUSER as \"Create Date By\",MCUST.UPDDATE as \"Last Update\",";
        //    SQLstr = SQLstr + "MCUST.UPDUSER as \"Last Update By\",MCUST.NAMETREE as \"Name Tree\",MCUST.JOBTITLE as \"Job Tile\",";
        //    SQLstr = SQLstr + "MCUST_D.TELEX as \"Telex Number\", MCUST_D.CABLE as Mobile,";
        //    SQLstr = SQLstr + "LTRIM(MCUST_D.ADD1) as Address1, MCUST_D.ADD2 as Address2, MCUST_D.ADD3 as Address3, MCUST_D.ADD4 as Address4,";
        //    SQLstr = SQLstr + "MCUST_D.REMARKS as Remarks,MCUST_D.MARKS as MARKS,MCUST_D.TRADETERM as \"Trade Term\",MCUST_D.PAYTERM as \"Payment Term\",MCUST_D.CUR as CURRENCY,";
        //    SQLstr = SQLstr + "MCUST_D.EMAIL,MCUST_D.HOMEPAGE,MCUST_D.ALIAS,MCUST_D.PASSWD as \"PASSWORD\" ,MCUST_D.BK_NO as \"BROKER NO\",MCUST_D.CCASS_ID as \"CCASS ID\",";
        //    SQLstr = SQLstr + "MCUST_D.PHOTO as \"Picture File Name\",MCUST_D.ISWHATSAPP as WHATSAPP, MCUST_D.ISWECHAT as WECHAT ,MCUST_D.ISLINE as LINE ";
        //    SQLstr = SQLstr + "FROM MCUST.DBF INNER JOIN MCUST_D.DBF ON MCUST.CUST_ID = MCUST_D.CUST_ID ";

        //    OleDbCommand oleDbcommand = new OleDbCommand("SELECT * FROM MCUST.DBF", oleDbCon);
        //    table.Load(oleDbcommand.ExecuteReader());
        //    oleDbCon.Close();
        //    dataGridView1.AutoGenerateColumns = true;
            
        //    for (int i = 0; i <= table.Rows.Count - 1; i++)
        //    {

        //        DataRow dr = table.Rows[i];
        //        // check mobile
        //        var mobile = table.Rows[i]["mobile"].ToString().Trim().RemoveNonNumeric();
        //        var email = table.Rows[i]["email"].ToString().Trim();
        //        var chkemail = false;
        //        var chkmobile = false;
        //        if (chkMobileEmty.Checked)
        //        {
        //            if (StringExtensions.ValidatePhoneNumber(mobile, true) == false)
        //            {
        //                dr.Delete();
        //            }
        //            else
        //            {
        //                table.Rows[i]["mobile"] = mobile;
        //            }
        //        }
        //        if (chkEmailEmty.Checked)
        //        {
        //            if (String.IsNullOrEmpty(email))
        //            {
        //                dr.Delete();
        //            }

        //        }

        //        if (chkMobileEmty.Checked && chkEmailEmty.Checked)
        //        {
        //            if (String.IsNullOrEmpty(email.Trim()))
        //            {
        //                if (StringExtensions.ValidatePhoneNumber(mobile.Trim(), true) == false)
        //                {
        //                    dr.Delete();
        //                }
        //            }
                   
        //        }
                

 

        //    }




        //    dataGridView1.DataSource = table;



        //    listBox1.Items.Clear();
        //    listBox2.Items.Clear();
        //    comboBox1.Items.Clear();
        //    for (int k = 0; k <= dataGridView1.ColumnCount - 1; k++)
        //    {
        //        string s = dataGridView1.Columns[k].HeaderText;
        //        listBox1.Items.Add(s);
        //        comboBox1.Items.Add(s);
        //    }

        //}
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

                        if (ValidateEmail(email))
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
        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox3.SelectedIndex = 0;
            textBox2.Text = Directory.GetCurrentDirectory();
            load(textBox2.Text);
        }

        public static DataTable SelectedColumns(DataTable RecordDT_, string[] colunmname)
        {
            DataTable TempTable = RecordDT_;

            System.Data.DataView view = new System.Data.DataView(TempTable);
            System.Data.DataTable selected = view.ToTable("Selected", false, colunmname);
            return selected;
        }



        private void Button3_Click(object sender, EventArgs e)
        {
            MoveListBoxItems(listBox1, listBox2);
        }

        private void Button4_Click(object sender, EventArgs e)
        {

        }
        private void Button5_Click_1(object sender, EventArgs e)
        {
            MoveListBoxItems(listBox2, listBox1);
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                table.DefaultView.RowFilter ="["+ comboBox1.Text+ "]"+ " like '%" + textBox1.Text + "%'";

                dataGridView1.DataSource = table;

                //BindingSource bs = new BindingSource();
                //bs.DataSource = dataGridView1.DataSource;
                //bs.Filter = comboBox1.Text + " like '%" + textBox1.Text + "%'";
                //dataGridView1.DataSource = bs;

            }
            catch
            {
            }
        }


        public void MoveItem(int direction, ListBox listBox)
        {
            // Checking selected item
            if (listBox.SelectedItem == null || listBox.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox.SelectedItem;

            // Removing removable element
            listBox.Items.Remove(selected);
            // Insert it in new position
            listBox.Items.Insert(newIndex, selected);
            // Restore selection
            listBox.SetSelected(newIndex, true);
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            MoveItem(-1, listBox2);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            MoveItem(1, listBox2);
        }

        private void Button6_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void Button7_Click(object sender, EventArgs e)
        {
       
            //string connection = "Data Source=192.168.0.217\\SQLEXPRESS;Initial Catalog=cust;User Id =cams; Password = cams";
            //SqlConnection con = new SqlConnection(connection);
            ////create object of SqlBulkCopy which help to insert  
            //SqlBulkCopy objbulk = new SqlBulkCopy(con);

            ////assign Destination table name  
            //objbulk.DestinationTableName = "mcust";

            //objbulk.ColumnMappings.Add("cust_id", "cust_id");
            //objbulk.ColumnMappings.Add("company name", "company name");
            //objbulk.ColumnMappings.Add("type", "type");
            //objbulk.ColumnMappings.Add("contact person", "contact person");
            //objbulk.ColumnMappings.Add("tel", "phone number");
            //objbulk.ColumnMappings.Add("FAX", "fax number");

            //objbulk.ColumnMappings.Add("create date", "create date");
            //objbulk.ColumnMappings.Add("create date by", "create date by");
            //objbulk.ColumnMappings.Add("last update", "last update");
            //objbulk.ColumnMappings.Add("last update by", "last update by");
            //objbulk.ColumnMappings.Add("name tree", "name tree");
            //objbulk.ColumnMappings.Add("job Tile", "job Tile");
            //objbulk.ColumnMappings.Add("telex Number", "telex Number");
            //objbulk.ColumnMappings.Add("mobile", "mobile");

            //objbulk.ColumnMappings.Add("address1", "address1");
            //objbulk.ColumnMappings.Add("address2", "address2");
            //objbulk.ColumnMappings.Add("address3", "address3");
            //objbulk.ColumnMappings.Add("address4", "address4");

            //objbulk.ColumnMappings.Add("remarks", "remarks");
            //objbulk.ColumnMappings.Add("marks", "marks");

            //objbulk.ColumnMappings.Add("trade term", "trade term");
            //objbulk.ColumnMappings.Add("payment term", "payment term");
            //objbulk.ColumnMappings.Add("currency", "currency");
            //objbulk.ColumnMappings.Add("email", "email");
            //objbulk.ColumnMappings.Add("homepage", "homepage");
            //objbulk.ColumnMappings.Add("alias", "alias");
            //objbulk.ColumnMappings.Add("password", "password");
            //objbulk.ColumnMappings.Add("broker no", "broker number");
            //objbulk.ColumnMappings.Add("ccass id", "ccass id");
            //objbulk.ColumnMappings.Add("picture file name", "picture file name");
            //objbulk.ColumnMappings.Add("whatsapp", "whatsapp");
            //objbulk.ColumnMappings.Add("wechat", "wechat");
            //objbulk.ColumnMappings.Add("line", "line");
            //con.Open();
            ////insert bulk Records into DataBase.  
   
            //// Delete old entries
            //SqlCommand truncate = new SqlCommand("TRUNCATE TABLE mcust", con);
            //truncate.ExecuteNonQuery();
           

            //objbulk.WriteToServer(table);
            //con.Close();

        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ////Form2 f2 = new Form2(table );
            ////f2.ShowDialog();


            //Form2 form2;

            //// in an appropriate EventHandler ... Form_Load ?
            //form2 = new Form2();
            //form2.SendDataTable += HandleDataTableresult;
            //form2.ShowDialog();

        }
        private void HandleDataTableresult(DataTable dataTable)
        {
            // do whatever with the data

            // Dispose, Close, or Hide secondary Form ?
            dataTable = table;
        }
    
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        public static void CopyStream(Stream source, Stream target)
        {
            if (source != null)
            {
                MemoryStream mstream = source as MemoryStream;
                if (mstream != null) mstream.WriteTo(target);
                else
                {
                    byte[] buffer = new byte[2048];
                    int length = buffer.Length, size;
                    while ((size = source.Read(buffer, 0, length)) != 0)
                        target.Write(buffer, 0, size);
                }
            }
        }
      
   
        private void Button6_Click_2(object sender, EventArgs e)
        {
    
        }
        // To search and replace content in a document part.
     

        private void SendToEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();

            f4.ShowDialog();
        }
        public static void DownloadCSV(DataTable Table, string FileName)
        {

            if (Table == null)
            {
                return;
            }
            if (Table.Columns.Count == 0)
            {
                return;
            }
            StringBuilder csv = new StringBuilder();
            for (int c = 0; c < Table.Columns.Count; c++)
            {
                if (c > 0)
                    csv.Append(",");
                DataColumn dc = Table.Columns[c];
                string columnTitleCleaned = CleanCSVString(dc.ColumnName);
                csv.Append(columnTitleCleaned);
            }
            csv.Append(Environment.NewLine);
            foreach (DataRow dr in Table.Rows)
            {
                StringBuilder csvRow = new StringBuilder();
                for (int c = 0; c < Table.Columns.Count; c++)
                {
                    if (c != 0)
                        csvRow.Append(",");

                    object columnValue = dr[c];
                    if (columnValue == null)
                        csvRow.Append("");
                    else
                    {
                        string columnStringValue = columnValue.ToString().Trim();


                        string cleanedColumnValue = CleanCSVString(columnStringValue);


                        csvRow.Append(cleanedColumnValue);
                    }
                }
                csv.AppendLine(csvRow.ToString());


            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"csv_mcust.csv"))
            {
                file.WriteLine(csv.ToString()); // "sb" is the StringBuilder
            }

            System.Diagnostics.Process.Start("notepad.exe", @"csv_mcust.csv");

        }
        protected static string CleanCSVString(string input)
        {
            string output = "\"" + input.Replace(",", ";").Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
            return output;
        }

        public static void ExportToCSVFile(DataTable dtTable)
        {
            StringBuilder sbldr = new StringBuilder();
            if (dtTable.Columns.Count != 0)
            {
                foreach (DataColumn col in dtTable.Columns)
                {
                    sbldr.Append(col.ColumnName + ',');
                }
                sbldr.Append("\r\n");
                foreach (DataRow row in dtTable.Rows)
                {
                    foreach (DataColumn column in dtTable.Columns)
                    {
                        sbldr.Append(row[column].ToString() + ',');
                    }
                    sbldr.Append("\r\n");
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form6 f6 = new Form6();

            this.Hide();
            f6.ShowDialog();
            this.Close();
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                //  loaddata(textBox2.Text, comboBox3.Text);
                loaddata2(textBox2.Text);
            }
            else

            {
                textBox2.Text = Directory.GetCurrentDirectory();

            }
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                textBox2.Text = folderPath;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Button6_Click_3(object sender, EventArgs e)
        {
            table.DefaultView.RowFilter = textBox3.Text;
            dataGridView1.DataSource = table;

          //  BindingSource bs = new BindingSource();
          //  bs.DataSource = dataGridView1.DataSource;
          //  bs.Filter = textBox3.Text;
           // dataGridView1.DataSource = bs;
        }

        private void ExtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();

            this.Hide();
            f6.ShowDialog();
            this.Close();
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button4_Click_1(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ChkMobileEmty_CheckedChanged(object sender, EventArgs e)
        {

        }
        private DataTable CloneAlteredDataTableSource(DataGridView dgv)
        {
            DataTable dt = dgv.DataSource as DataTable;

            if (dt == null)
            {
                return null;
            }

            DataTable clone = new DataTable();

            foreach (DataColumn col in dt.Columns)
            {
                clone.Columns.Add(col.ColumnName, col.DataType);
            }

            string order = string.Empty;

            switch (dgv.SortOrder)
            {
                case System.Windows.Forms.SortOrder.Ascending: order = "ASC"; break;
                case System.Windows.Forms.SortOrder.Descending: order = "DESC"; break;
            }

            string sort = dgv.SortedColumn == null ? string.Empty : string.Format("{0} {1}", dgv.SortedColumn.Name, order);

            DataRow[] rows = dt.Select(dt.DefaultView.RowFilter, sort);

            foreach (DataRow row in rows)
            {
                object[] items = (object[])row.ItemArray.Clone();
                clone.Rows.Add(items);
            }

            return clone;
        }
        public static bool ValidateEmail(string email)
        {
            System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            System.Text.RegularExpressions.Match emailMatch = emailRegex.Match(email);
            return emailMatch.Success;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            load(textBox2.Text);
        }

        public static OdbcConnection OdbcDbfOpenConn(string Database)
        {
            string cnstr = "Driver={Microsoft dBase Driver (*.dbf)}; SourceType=DBF; SourceDB=" + @"C:\c#\WindowsFormsApp1\WindowsFormsApp1\bin\Debug" + "; Exclusive=No; Collate=Machine; NULL=NO; DELETED=NO; BACKGROUNDFETCH=NO;";
            OdbcConnection icn = new OdbcConnection();
            icn.ConnectionString = cnstr;
            if (icn.State == ConnectionState.Open) icn.Close();
            icn.Open();
            return icn;
        }
        public static DataTable GetOdbcDbfDataTable(string Database, string OdbcString)
        {
            DataTable myDataTable = new DataTable();
            OdbcConnection icn = OdbcDbfOpenConn(Database);
            OdbcDataAdapter da = new OdbcDataAdapter(OdbcString, icn);
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            myDataTable = ds.Tables[0];
            if (icn.State == ConnectionState.Open) icn.Close();
            return myDataTable;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            GetOdbcDbfDataTable("select * from mcust.dbf", @"C:\c#\WindowsFormsApp1\WindowsFormsApp1\bin\Debug");
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();

            this.Hide();
            f6.ShowDialog();
            this.Close();
        }
        //void SetSql(string sqlString)
        //{
        //    using (var connect = new OleDbConnection())
        //    {
        //        connect.ConnectionString = GetConnectionString(_database);
        //        connect.Open();

        //        using (var cmd = connect.CreateCommand())
        //        {
        //            cmd.CommandText = sqlString;

        //            using (var da = new OleDbDataAdapter(cmd))
        //            {
        //                var dt = new DataTable();
        //                da.Fill(dt);

        //                SetDatabase(dt);

        //                _sql = sqlString;
        //            }
        //        }
        //    }
        //}

        //private string GetConnectionString(string directory)
        //{
        //    return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + directory + ";Extended Properties=dBase IV";
        //}
        //public void SetDatabase(DataTable data)
        //{
        //    _dataTable = data;
        //    dataGridView3.DataSource = data;
        //    _filter = string.Empty;
        //}

        //private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Form6 f6 = new Form6();

        //    this.Hide();
        //    f6.ShowDialog();
        //    this.Close();
        //}

        //private void Button9_Click(object sender, EventArgs e)
        //{
        //    GetConnectionString(@"C:\c#\WindowsFormsApp1\WindowsFormsApp1\bin\Debug");
        //}
    }
   
}
public static class StringExtensions
{

    /// <summary>
    /// Checks to be sure a phone number contains 10 digits as per American phone numbers.  
    /// If 'IsRequired' is true, then an empty string will return False. 
    /// If 'IsRequired' is false, then an empty string will return True.
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="IsRequired"></param>
    /// <returns></returns>
    public static bool ValidatePhoneNumber(this string phone, bool IsRequired)
    {
        if (string.IsNullOrEmpty(phone) & !IsRequired)
            return true;

        if (string.IsNullOrEmpty(phone) & IsRequired & int.TryParse(phone, out int n) == false)
            return false;

        var cleaned = phone.RemoveNonNumeric();

        if (IsRequired)
        {
            if (cleaned.Length == 8)
                return true;
            else
                return false;
        }
        else
        {
            if (cleaned.Length == 0)
                return true;
            else if (cleaned.Length > 0 & cleaned.Length < 8)
                return false;
            else if (cleaned.Length == 8)
                return true;
            else
                return false; // should never get here
        }
    }

    /// <summary>
    /// Removes all non numeric characters from a string
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static string RemoveNonNumeric(this string phone)
    {
        return Regex.Replace(phone, @"[^0-9]+", "");
    }

}
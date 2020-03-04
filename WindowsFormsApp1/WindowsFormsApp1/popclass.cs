using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApp1
{
    class popclass
    {
        public static DataTable readCSV(string filePath, out String result)
        {

            DataTable dt = new DataTable();


            try
            {

                StreamReader sr = new StreamReader(filePath);


                string strLine = sr.ReadLine().Replace("\"", "");
                //  Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                string[] strArray = System.Text.RegularExpressions.Regex.Split(strLine, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                foreach (string value in strArray)
                {
                    dt.Columns.Add(value.ToUpper().Trim());
                }
                DataRow dr = dt.NewRow();

                while (sr.Peek() >= 0)
                {
                    strLine = sr.ReadLine().Replace("\"", "");
                    strArray = System.Text.RegularExpressions. Regex.Split(strLine, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); ;
                    dt.Rows.Add(strArray);
                }
                result = "";
                 return dt;
            }

            catch (Exception)
            {

                result = ("Please select CSV file");

              return null;
            }
            finally
            {


            }

        }

        public static void DownloadCSV(DataTable Table, string FileName)
        {

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
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName))
            {
                file.WriteLine(csv.ToString()); // "sb" is the StringBuilder
            }

            System.Diagnostics.Process.Start("notepad.exe", FileName);

        }
        protected static string CleanCSVString(string input)
        {
            string output = "\"" + input.Replace(",", ";").Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", "") + "\"";
            return output;
        }
        public static bool ValidateEmail(string email)
        {
            System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            System.Text.RegularExpressions.Match emailMatch = emailRegex.Match(email);
            return emailMatch.Success;
        }
        public static void MoveListBoxItems(ListBox source, ListBox destination)
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
        public static void MoveItem(int direction, ListBox listBox)
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

        public static DataTable SelectedColumns(DataTable RecordDT_, string[] colunmname)
        {
            DataTable TempTable = RecordDT_;

            System.Data.DataView view = new System.Data.DataView(TempTable);
            System.Data.DataTable selected = view.ToTable("Selected", false, colunmname);
            return selected;
        }
        public static DataTable CloneAlteredDataTableSource(DataGridView dgv)
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
    }
}

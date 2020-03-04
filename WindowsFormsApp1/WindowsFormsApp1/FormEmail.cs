using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;

//using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using Spire.Doc;
using Document = Spire.Doc.Document;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
namespace WindowsFormsApp1
{
    public partial class FormEmail : Form
    {
        // C:\c#\WindowsFormsApp1\WindowsFormsApp1\bin\Debug
        String Folderpath = null;
        MySettings settings = MySettings.Load();
        public FormEmail()
        {
            InitializeComponent();
        }
        private void FormEmail_Load(object sender, EventArgs e)
        {
            txtHost.Text = settings.Host;
            txtForm.Text = settings.From;
            txtName.Text = settings.Name;
            txtPassword.Text = settings.Password;
            txtSubjec.Text = settings.EmailSubject;
            txtPort.Text = settings.Port;
            if (settings.Encryption == "SSL")

            {
                comboBox1.SelectedIndex = 1;

            }
            else if (settings.Encryption == "")
            {
                comboBox1.SelectedIndex = 0;
            }
            DirectoryInfo d = new DirectoryInfo(textBox2.Text + "\\");//Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles(".\\"+ "email*.dotx"); //Getting Text files
            if (Files.Length == 0)
            {
                //Create word document
                Document document = new Document();

                Paragraph p = document.AddSection().AddParagraph();
                TextRange txtRang = p.AppendText("H63TWX11072");
                txtRang.CharacterFormat.FontName = "C39HrP60DlTt";
                txtRang.CharacterFormat.FontSize = 80;
                txtRang.CharacterFormat.TextColor = Color.SeaGreen;
                Section section = document.AddSection();

                //Initialize a Header Instance
                HeaderFooter header = document.Sections[0].HeadersFooters.Header;
                //Add Header Paragraph and Format 
                Paragraph paragraph = header.AddParagraph();
                paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                //Append Picture for Header Paragraph and Format
                Spire.Doc.Fields.DocPicture headerimage = paragraph.AppendPicture(Image.FromFile(@"dackeasy_logo.png"));
                headerimage.VerticalAlignment = ShapeVerticalAlignment.Bottom;

                paragraph = section.AddParagraph();
                string str = "Dear <CONTACT PERSON>," + "\r\n";
                paragraph.AppendText(str);

                str = "As an independent Word .NET component, Spire.Doc for .NET doesn't need Microsoft Word to be installed on the machine. However, it can incorporate Microsoft Word document creation capabilities into any developers.NET applications.As an independent Word .NET component, Spire.Doc for .NET doesn't need Microsoft Word to be installed on the machine. However, it can incorporate Microsoft Word document creation capabilities into any developers’.NET applications." + "\r\n";
                paragraph.AppendText(str);

                document.SaveToFile("email.dotx", FileFormat.Dotx);
                d = new DirectoryInfo(Directory.GetCurrentDirectory());//Assuming Test is your Folder

                Files = d.GetFiles("email*.dotx");

            }


            comboBox2.DataSource = Files;
            comboBox2.DisplayMember = "demo";

            //Center(this);
        }
        private void LoadCSvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory =textBox2.Text + ".\\";
            dialog.Filter = "csv files (*.*)|*.csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = (dialog.FileName);
            }
            DataTable Table = new DataTable();
            String resulttext ="" ;
            Table = popclass.readCSV(dialog.FileName ,out resulttext);//,true);
            toolStripStatusLabel1.Text = resulttext;
            bindingSource1.DataSource = Table;
            dataGridView1.DataSource = bindingSource1.DataSource;
        }

        private void EmailSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;

            }
            else
            {
                panel1.Visible = true;
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Folderpath = folderBrowserDialog1.SelectedPath;
                textBox2.Text = Folderpath;
            }
        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            bool IsExistEmail = false;
            bool IsExistcontactperson = false;
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "EMAIL")
                {
                    IsExistEmail = true;
                }

            }
            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == "CONTACT PERSON")
                {
                    IsExistcontactperson = true;
                }
            }




            StringBuilder sb = new StringBuilder();
            if (dt != null && IsExistEmail == true && IsExistcontactperson == true)
            {
                try
                {
                    // listView1.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        var ResultName =textBox2.Text +"\\"+ string.Format(@"email{0}.docx", DateTime.Now.Ticks);

                        Document document = new Document();
                        document.LoadFromFile(  textBox2.Text + "\\"+comboBox2.Text);
                        String contactPerson = row["contact person".ToUpper()].ToString();
                        document.Replace("<contact person>", contactPerson, false, true);

                        document.SaveToFile(ResultName, FileFormat.Docx);
                        // System.Diagnostics.Process.Start("Replace.docx");

                        //     dotx2docx("demo.dotx", ResultName);


                        //    Mailmerge(ResultName, row, dt.Columns);                  
                        //  String contactPerson = row["contact person".ToUpper()].ToString();
                        String Toemail = row["email".ToUpper()].ToString();

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient(txtHost.Text);
                        System.Text.Encoding SystemEncoding;
                        SystemEncoding = System.Text.Encoding.UTF8;

                        mail.From = new MailAddress(txtForm.Text);
                        mail.To.Add(Toemail);
                        mail.Subject = txtSubjec.Text;

                        mail.IsBodyHtml = true;

                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(ResultName);
                        mail.Attachments.Add(attachment);

                        mail.Body = settings.Body; // GetTemplate("EmailDetial.txt").Replace("<contact person>", contactPerson);

                        SmtpServer.Port = Convert.ToInt32(settings.Port);
                        String Name = txtName.Text;
                        String password = txtPassword.Text;

                        SmtpServer.Credentials = new System.Net.NetworkCredential(Name, password);
                        if (settings.Encryption == "SSL")
                        {
                            SmtpServer.EnableSsl = true;
                        }
                        else if (settings.Encryption == "")
                        {
                            SmtpServer.EnableSsl = false;
                        }
                        SmtpServer.Send(mail);
                        sb.Append(contactPerson + "--> " + Toemail + " is sent" + System.Environment.NewLine);
                        //MessageBox.Show("mail Send");
                    }


                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    toolStripStatusLabel1.Text = (ex.ToString());
                }
            }
            else
            {
                if (IsExistcontactperson == false)
                {
                    toolStripStatusLabel1.Text = ("Missing contact person column in csv file");
                }
                if (IsExistEmail == false)
                {
                    toolStripStatusLabel1.Text = ("Missing contact email column in csv file");
                }
            }
            textBox1.Text = sb.ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }
    }
}

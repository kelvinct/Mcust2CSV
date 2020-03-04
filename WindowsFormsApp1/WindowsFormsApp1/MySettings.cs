using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

using System.IO;
using System.Reflection;

namespace WindowsFormsApp1
{
    class MySettings : AppSettings<MySettings>
    {
        //public string Host = "smtp.gmail.com";
        //public string From = "support.daceasy@gmail.com";
        //public string Subject = "Daceasy sample email";
        //public string Name = "support.daceasy@gmail.com";
        //public string Password = "23918816";
        //public string port = "587";
        //public string encryption = "SSL";

        private string accountno;
        public string Accountno   // property
        {
            get { return accountno; }
            set { accountno = value; }
        }

        private string pwd;
        public string Pwd   // sms password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string msg;
        public string Msg   // sms password
        {
            get { return msg; }
            set { msg = value; }
        }



        private string host;
        public string Host   // property
        {
            get { return host; }
            set { host = value; }
        }

        private string from;
        public string From   // property
        {
            get { return from; }
            set { from = value; }
        }


        private string esubject;
        public string EmailSubject   // property
        {
            get { return esubject; }
            set { esubject = value; }
        }
        private string Faxsubject;
        public string FaxSubject   // property
        {
            get { return Faxsubject; }
            set { Faxsubject = value; }
        }

        private string body;
        public string Body   // property
        {
            get { return body; }
            set { body = value; }
        }




        // public string Name;
        private string password;
        public string Password   // property
        {
            get { return password; }
            set { password = value; }
        }

        private string port;
        public string Port   // property
        {
            get { return port; }
            set { port = value; }
        }

        private string encryption;
        public string Encryption   // property
        {
            get { return encryption; }
            set { encryption = value; }
        }


        private string name;
        public string Name   // property
        {
            get { return name; }
            set { name = value; }
        }
        private string fax;
        public string Fax   // property
        {
            get { return fax; }
            set { fax = value; }
        }
    }


    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            T t = new T();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }

}

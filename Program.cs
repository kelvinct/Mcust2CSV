using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
namespace WindowsFormsApp1
{
    static class Program
    {



        [STAThread]
        static void Main()
        {
          //  var test = new Docu.Test();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           // Login();
            //};

            Application.Run(new Form6());


            //Form7 fLogin = new Form7();
            //if (fLogin.ShowDialog() == DialogResult.OK)
            //{
            //    Application.Run(new Form6());
            //}
            //else
            //{
            //    Application.Exit();
            //}


        }
         private static bool logOut;

        //private static void Login()
        //{
        //    Form7 login = new Form7();
        //    Form6 main = new Form6();
        //    main.FormClosed += new FormClosedEventHandler(main_FormClosed);
        //    if (login.ShowDialog(main) == DialogResult.OK)
        //    {
        //        Application.Run(main);
        //        if (logOut)
        //            Login();
        //    }
        //    else
        //        Application.Exit();
        //}

        //static void main_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    logOut = (sender as Form6).logOut;
        //}

    }
}

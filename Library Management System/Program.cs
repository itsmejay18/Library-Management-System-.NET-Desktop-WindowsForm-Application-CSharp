using Library_Management_System.Class;
using Library_Management_System.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormLogin());
            Application.Run(new AdminDashboard("Admin"));
            //Application.Run(new BorrowersDashboard());
            //Application.Run(new AdminDashboard());
            //Application.Run(new StaffDashboard());
        }
    }
}

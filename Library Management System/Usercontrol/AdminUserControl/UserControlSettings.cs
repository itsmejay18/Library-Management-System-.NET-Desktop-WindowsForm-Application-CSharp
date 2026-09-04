using Library_Management_System.Class;
using Library_Management_System.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.Usercontrol
{
    public partial class UserControlSettings : UserControl
    {
        private readonly string displayName;

        public UserControlSettings()
        {
            InitializeComponent();

            this.displayName = CurrentSession.Username ?? "User";
            lblWelcome.Text = $"Welcome, {displayName}!";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {            
                CurrentSession.Username = null;
                CurrentSession.Role = null;
  
                FormLogin formLogin = new FormLogin();
                formLogin.Show();
               
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Close();
                }
            }
        }
    }
}
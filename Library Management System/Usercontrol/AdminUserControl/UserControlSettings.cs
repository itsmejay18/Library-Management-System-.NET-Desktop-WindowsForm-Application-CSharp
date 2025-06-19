using Library_Management_System.Class;
using Library_Management_System.UI;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        private readonly string supabaseUrl = "https://mfvwlprqvbqqwhvpsxbr.supabase.co";
        private readonly string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1mdndscHJxdmJxcXdodnBzeGJyIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzNjMzNDAsImV4cCI6MjA2MTkzOTM0MH0.4KmpYTn08PeLbQwUmcHrvg2cDi35-cIjXA3jlp3CxDc";
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
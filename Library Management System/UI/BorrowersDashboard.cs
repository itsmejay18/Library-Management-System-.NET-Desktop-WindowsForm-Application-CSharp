using Library_Management_System.Borrower_UI;
using Library_Management_System.Class;
using Library_Management_System.Usercontrol;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Library_Management_System.UI
{
    public partial class BorrowersDashboard : Form
    {
        private string displayName;
        private string userRole;

        private readonly string supabaseUrl = "https://mfvwlprqvbqqwhvpsxbr.supabase.co";
        private readonly string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1mdndscHJxdmJxcXdodnBzeGJyIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzNjMzNDAsImV4cCI6MjA2MTkzOTM0MH0.4KmpYTn08PeLbQwUmcHrvg2cDi35-cIjXA3jlp3CxDc";

        // User controls
        readonly UCBorrowReturn ucBorrow = new UCBorrowReturn();
        readonly UCFindBook ucfindbook = new UCFindBook();
        readonly UCRequestInterloan ucRequestInterloan = new UCRequestInterloan();
        readonly EnterwhatyouRemember enterwhatyouRemember = new EnterwhatyouRemember();

        public BorrowersDashboard(string displayName)
        {
            InitializeComponent();

            this.displayName = displayName;
            this.userRole = CurrentSession.Role ?? "Guest";

            lblWelcome.Text = $"Welcome, {displayName} ({userRole})!";

            this.Load += BorrowersDashboard_Load;
        }

        private void BorrowersDashboard_Load(object sender, EventArgs e)
        {
            MovePanel(btnBooks);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(ucfindbook);
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                timer1.Stop();
                Application.Exit();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            MovePanel(btnSettings);
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.ExitThread(); // Ends the thread
                System.Diagnostics.Process.Start(Application.ExecutablePath); // Restarts app
            }
        }

        private void MovePanel(Control btn)
        {
            panelSlide.Top = btn.Top;
            panelSlide.Height = btn.Height;
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            MovePanel(btnBooks);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(ucfindbook);
        }

        private void btnBorrowReturn_Click(object sender, EventArgs e)
        {
            MovePanel(btnBorrowReturn);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(ucBorrow);
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            timer1.Start();
            lblWelcome.Text = $"Welcome, {displayName}!";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }

        private void btnAuthPub_Click(object sender, EventArgs e)
        {
            MovePanel(btnAuthPub);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(enterwhatyouRemember);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            MovePanel(btnReports);
            mainPanel.Controls.Clear();
            // Future implementation for reports goes here
        }
    }
}

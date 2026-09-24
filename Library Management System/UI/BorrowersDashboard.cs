using Library_Management_System.Borrower_UI;
using Library_Management_System.Class;
using Library_Management_System.Usercontrol;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Library_Management_System.UI
{
    public partial class BorrowersDashboard : Form
    {
        private string displayName;
        private string userRole;

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

            // Reset all nav buttons to the inactive style
            foreach (Control c in panel3.Controls)
            {
                if (c is Button && c != lblExit)
                {
                    ((Button)c).BackColor = System.Drawing.Color.White;
                    ((Button)c).ForeColor = System.Drawing.Color.FromArgb(26, 43, 70);
                    ((Button)c).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
                }
            }

            // Highlight the selected button
            if (btn is Button active)
            {
                active.BackColor = System.Drawing.Color.FromArgb(238, 243, 251);
                active.ForeColor = System.Drawing.Color.FromArgb(47, 107, 216);
                active.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            }
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

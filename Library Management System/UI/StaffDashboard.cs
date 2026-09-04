using Library_Management_System.Class;
using Library_Management_System.Usercontrol;
using Library_Management_System.Usercontrol.AdminUserControl;
using Library_Management_System.Usercontrol.StaffUserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Library_Management_System.UI
{
    public partial class StaffDashboard : Form
    {
        private readonly string displayName;
        private readonly string userRole;
        public StaffDashboard(string username)
        {
            InitializeComponent();
            this.displayName = username;
            this.userRole = CurrentSession.Role ?? "Guest";
            lblWelcome.Text = $"Welcome, {displayName} ({userRole})!";

        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private readonly UserControlDashboard userControlDashboard = new UserControlDashboard();
        private readonly FindBookUserControl findBookUserControl = new FindBookUserControl();
        private readonly FindStudentUserControl findStudentUserControl = new FindStudentUserControl();
        private readonly IssueReturnUserControl issueReturnUserControl = new IssueReturnUserControl();
        private readonly ViewLogUserControl viewLogUserControl = new ViewLogUserControl();
        readonly UserControlAuthPub ucAuthPub = new UserControlAuthPub(); 

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            MovePanel(btnDashboard);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(userControlDashboard);
        }

        private void btnBorrowReturn_Click(object sender, EventArgs e)
        {
            MovePanel(btnBorrowReturn);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(issueReturnUserControl);
        }
        private void MovePanel(Control btn)
        {
            panelSlide.Top = btn.Top;
            panelSlide.Height = btn.Height;
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

        private void btnBooks_Click(object sender, EventArgs e)
        {
            MovePanel(btnBooks);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(findBookUserControl);
        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
            MovePanel(btnMembers);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(findStudentUserControl);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            MovePanel(btnSettings);
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.ExitThread(); // Terminate current thread
                System.Diagnostics.Process.Start(Application.ExecutablePath); // Restart the application
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            MovePanel(btnReports);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(viewLogUserControl);
        }

        private void btnAuthPub_Click(object sender, EventArgs e)
        {
            MovePanel(btnAuthPub);
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(ucAuthPub);
        }

        private void lblDateTime_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            timer1.Start();

            // Set default panel to Dashboard on form load
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(userControlDashboard);
            MovePanel(btnDashboard); // Move panelSlide to Dashboard button
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
    }
}

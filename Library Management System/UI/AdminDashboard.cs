using Library_Management_System.Class;
using Library_Management_System.Usercontrol;
using Library_Management_System.Usercontrol.AdminUserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Library_Management_System.UI
{
   
    public partial class AdminDashboard : Form
    {

        private readonly string displayName;
        private readonly string userRole;
        public AdminDashboard(string username)
        {
            InitializeComponent();
            this.displayName = username;
            this.userRole = CurrentSession.Role ?? "Guest";
            lblWelcome.Text = $"Welcome, {displayName} ({userRole})!";


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
        readonly UserControlDashboard ucDash = new UserControlDashboard();
        readonly UserControlBooks ucBook = new UserControlBooks();
        readonly UserControlMembers ucMember = new UserControlMembers();
        readonly UserControlBorrowReturn ucBorrow = new UserControlBorrowReturn();
        readonly UserControlReports ucReport = new UserControlReports();
        readonly UserControlSettings ucSetting = new UserControlSettings();
        readonly UserControlAuthPub ucAuthPub = new UserControlAuthPub();

        private bool sidebarCollapsed = false;
        private readonly Dictionary<string, string> navTextStore = new Dictionary<string, string>();

        private void btnToggle_Click(object sender, EventArgs e)
        {
            sidebarCollapsed = !sidebarCollapsed;
            ToggleSidebar();
        }

        private void ToggleSidebar()
        {
            int w = sidebarCollapsed ? 82 : 286;

            panel3.Width = w;

            // Content area follows the sidebar width
            mainPanel.Location = new System.Drawing.Point(w, 100);
            mainPanel.Width = 1366 - w;
            panel6.Location = new System.Drawing.Point(w, 691);
            panel6.Width = 1366 - w;

            // Profile header elements
            lblWelcome.Visible = !sidebarCollapsed;
            labelRole.Visible = !sidebarCollapsed;
            label3.Visible = !sidebarCollapsed;
            panelMenuAccent.Visible = !sidebarCollapsed;
            label4.Visible = !sidebarCollapsed;
            btnToggle.Location = sidebarCollapsed ? new System.Drawing.Point(25, 26) : new System.Drawing.Point(236, 26);
            panel5Line.Width = w;
            panelLogoutSep.Width = sidebarCollapsed ? 70 : 286;

            // Nav buttons: icon-rail when collapsed, full text when expanded
            foreach (Control c in panel3.Controls)
            {
                if (!(c is Button b)) continue;

                if (sidebarCollapsed)
                {
                    if (!navTextStore.ContainsKey(b.Name)) navTextStore[b.Name] = b.Text;
                    b.Text = "";
                    b.Padding = new System.Windows.Forms.Padding(0);
                    b.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    b.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
                    b.Width = 70;
                }
                else
                {
                    if (navTextStore.ContainsKey(b.Name)) b.Text = navTextStore[b.Name];
                    b.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
                    b.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    b.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                    b.Width = 276;
                }
            }

            // Re-apply the active highlight on the current page button
            foreach (Control c in panel3.Controls)
            {
                if (c is Button active && active.BackColor == System.Drawing.Color.FromArgb(238, 243, 251))
                {
                    active.BackColor = System.Drawing.Color.FromArgb(238, 243, 251);
                    active.ForeColor = System.Drawing.Color.FromArgb(47, 107, 216);
                    active.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                }
            }
        }

        private void LoadControl(UserControl uc)
        {
            mainPanel.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(uc);
        }
        

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 1. Start your timer (likely for updating time display somewhere)
            timer1.Start();

            // 2. Set the welcome label text using the currently logged-in username from your session
            lblWelcome.Text = $"Welcome, {displayName}!";
            labelRole.Text = userRole;


            // 3. Clear any existing controls from your main panel before adding new content
            mainPanel.Controls.Clear();

            // 4. Add the UserControlDashboard (ucDash) as the default view on the main panel
            LoadControl(ucDash);

            // 5. Move the sliding panel indicator to the Dashboard button
            MovePanel(btnDashboard);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MovePanel(btnDashboard);
            LoadControl(ucDash);
        }

       

        private void btnBooks_Click(object sender, EventArgs e)
        {
            MovePanel(btnBooks);
            LoadControl(ucBook);
        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
            MovePanel(btnMembers);
            LoadControl(ucMember);
        }

        private void btnBorrowReturn_Click(object sender, EventArgs e)
        {
            MovePanel(btnBorrowReturn);
            LoadControl(ucBorrow);
        }

       

        private void btnReports_Click(object sender, EventArgs e)
        {
            MovePanel(btnReports);
            LoadControl(ucReport);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Clear the active session
                CurrentSession.Username = null;
                CurrentSession.Role = null;
                CurrentSession.FullName = null;
                CurrentSession.Email = null;

                // Return to the login screen
                FormLogin login = new FormLogin();
                login.Show();
                Hide();
            }
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

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }

       

private void btnAuthPub_Click(object sender, EventArgs e)
        {
            MovePanel(btnAuthPub);
            LoadControl(ucAuthPub);
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void lblDateTime_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (System.Drawing.Drawing2D.LinearGradientBrush brush =
                new System.Drawing.Drawing2D.LinearGradientBrush(
                    panel1.ClientRectangle,
                    System.Drawing.Color.FromArgb(36, 64, 107),
                    System.Drawing.Color.FromArgb(22, 50, 92),
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panel1.ClientRectangle);
            }
        }

        private void userControlBooks1_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        public interface IRefreshable
        {
            void Reload();
        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }
    }
}

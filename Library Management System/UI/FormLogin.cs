using Library_Management_System.Class;
using Library_Management_System.UI;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            ConfigureLook();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private static readonly Color ClrPrimary = Color.FromArgb(47, 107, 216);
        private static readonly Color ClrPrimaryDark = Color.FromArgb(37, 88, 180);
        private static readonly Color ClrNavy = Color.FromArgb(26, 43, 70);
        private static readonly Color ClrGray = Color.FromArgb(107, 122, 144);
        private static readonly Color ClrLightBg = Color.FromArgb(239, 243, 249);
        private static readonly Color ClrFocusBg = Color.FromArgb(230, 238, 250);

        private const string UserPlaceholder = "Username";
        private const string PassPlaceholder = "Password";

        private string selectedRole = "Student";
        private bool showPassword;
        private bool dragging;
        private Point dragStart;

        #region Setup / Styling

        private void ConfigureLook()
        {
            pictureBoxLogo.Image = LoadImage("Logo.png");
            pictureBoxUserIcon.Image = LoadImage("user.png");

            btnClose.Click += btnClose_Click;
            btnMinimize.Click += btnMinimize_Click;
            btnLogin.Click += btnLogin_Click;
            btnTogglePass.Click += btnTogglePass_Click;
            lblSignupLink.Click += lblSignupLink_Click;

            foreach (Button role in new[] { btnRoleAdmin, btnRoleStaff, btnRoleStudent })
            {
                role.Click += (s, e) =>
                {
                    Button b = (Button)s;
                    SelectRole(b == btnRoleAdmin ? "Admin" : b == btnRoleStaff ? "Staff" : "Student");
                };
                role.MouseEnter += (s, e) =>
                {
                    Button b = (Button)s;
                    if (b.BackColor != ClrPrimary) b.BackColor = Color.FromArgb(228, 235, 247);
                };
                role.MouseLeave += (s, e) => UpdateRoleSelection();
            }

            txtUsername.Enter += txtUser_Enter;
            txtUsername.Leave += txtUser_Leave;
            txtPassword.Enter += txtPass_Enter;
            txtPassword.Leave += txtPass_Leave;

            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = ClrPrimaryDark;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = ClrPrimary;
            btnClose.MouseEnter += (s, e) => { btnClose.BackColor = Color.FromArgb(232, 17, 35); btnClose.ForeColor = Color.White; };
            btnClose.MouseLeave += (s, e) => { btnClose.BackColor = Color.White; btnClose.ForeColor = Color.FromArgb(138, 155, 181); };
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.FromArgb(230, 236, 246);
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.White;

            WireDrag(this);
            WireDrag(panelBrand);
            WireDrag(pictureBoxLogo);
            WireDrag(labelBrandTitle);
            WireDrag(labelBrandSub);
            WireDrag(labelBrandTag);
            WireDrag(panelDivider);
            WireDrag(labelBrandFooter);
        }

        private Image LoadImage(string resourceName)
        {
            try
            {
                using (System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Library_Management_System.Static.Icons." + resourceName))
                {
                    return stream == null ? null : new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void RoundControl(Control c, int radius)
        {
            c.Region = new Region(RoundedRect(new Rectangle(0, 0, c.Width, c.Height), radius));
        }

        private void WireDrag(Control c)
        {
            c.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { dragging = true; dragStart = e.Location; } };
            c.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    Point p = PointToScreen(e.Location);
                    Location = new Point(p.X - dragStart.X, p.Y - dragStart.Y);
                }
            };
            c.MouseUp += (s, e) => dragging = false;
        }

        #endregion

        #region Interactions

        private void FormLogin_Load(object sender, EventArgs e)
        {
            RoundControl(this, 16);
            RoundControl(panelUser, 16);
            RoundControl(panelPass, 16);
            RoundControl(btnLogin, 24);
            RoundControl(btnRoleAdmin, 20);
            RoundControl(btnRoleStaff, 20);
            RoundControl(btnRoleStudent, 20);
            RoundControl(btnMinimize, 8);
            RoundControl(btnClose, 8);

            txtUsername.ForeColor = ClrGray;
            txtPassword.ForeColor = ClrGray;
            UpdateRoleSelection();
            UpdateMask();
        }

        private void panelBrand_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = panelBrand.ClientRectangle;
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(58, 102, 184), Color.FromArgb(22, 50, 92), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
            using (SolidBrush soft = new SolidBrush(Color.FromArgb(26, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(soft, -110, rect.Height - 330, 330, 330);
            }
            using (SolidBrush faint = new SolidBrush(Color.FromArgb(16, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(faint, rect.Width - 190, -140, 300, 300);
            }
        }

        private void SelectRole(string role)
        {
            selectedRole = role;
            UpdateRoleSelection();
        }

        private void UpdateRoleSelection()
        {
            StyleRole(btnRoleAdmin, selectedRole == "Admin");
            StyleRole(btnRoleStaff, selectedRole == "Staff");
            StyleRole(btnRoleStudent, selectedRole == "Student");
        }

        private void StyleRole(Button b, bool selected)
        {
            b.BackColor = selected ? ClrPrimary : ClrLightBg;
            b.ForeColor = selected ? Color.White : ClrNavy;
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == UserPlaceholder)
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = ClrNavy;
            }
            SetFieldBg(true, true);
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = UserPlaceholder;
                txtUsername.ForeColor = ClrGray;
            }
            SetFieldBg(true, false);
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == PassPlaceholder)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = ClrNavy;
            }
            SetFieldBg(false, true);
        }

        private void txtPass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = PassPlaceholder;
                txtPassword.ForeColor = ClrGray;
            }
            SetFieldBg(false, false);
        }

        private void SetFieldBg(bool isUser, bool focused)
        {
            Color bg = focused ? ClrFocusBg : ClrLightBg;
            if (isUser)
            {
                panelUser.BackColor = bg;
                txtUsername.BackColor = bg;
            }
            else
            {
                panelPass.BackColor = bg;
                txtPassword.BackColor = bg;
            }
        }

        private void btnTogglePass_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == PassPlaceholder) return;
            showPassword = !showPassword;
            UpdateMask();
        }

        private void UpdateMask()
        {
            txtPassword.UseSystemPasswordChar = !showPassword && txtPassword.Text != PassPlaceholder;
            btnTogglePass.Text = showPassword ? "Hide" : "Show";
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblSignupLink_Click(object sender, EventArgs e)
        {
            using (FormSignup signup = new FormSignup())
            {
                signup.ShowDialog();
            }
        }

        private string ComputeSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == UserPlaceholder) username = "";
            if (password == PassPlaceholder) password = "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Username, password, and role selection are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnLogin.Enabled = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT user_id, full_name, role FROM users_tbl WHERE username = @username AND password = @password AND role = @role";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", ComputeSha256(password));
                        cmd.Parameters.AddWithValue("@role", selectedRole);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string userRole = reader["role"].ToString();
                                string fullName = reader["full_name"].ToString();

                                CurrentSession.Username = username;
                                CurrentSession.Role = userRole;
                                CurrentSession.FullName = fullName;

                                MessageBox.Show($"Welcome {userRole}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                Form dashboardForm = null;
                                switch (userRole)
                                {
                                    case "Admin":
                                        dashboardForm = new AdminDashboard(fullName);
                                        break;
                                    case "Staff":
                                        dashboardForm = new StaffDashboard(fullName);
                                        break;
                                    case "Student":
                                        dashboardForm = new BorrowersDashboard(fullName);
                                        break;
                                }

                                if (dashboardForm != null)
                                {
                                    dashboardForm.Show();
                                    Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username, password, or role mismatch.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        #endregion
    }
}
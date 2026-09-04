using Library_Management_System.Class;
using Library_Management_System.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private string ComputeSha256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string selectedRole = null;
            if (rbtnAdmin.Checked) selectedRole = "Admin";
            else if (rbtnStaff.Checked) selectedRole = "Staff";
            else if (rbtnStudent.Checked) selectedRole = "Student";

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
                                    this.Hide();
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

        private void btnSignup_Click(object sender, EventArgs e)
        {
            using (FormSignup signUpForm = new FormSignup())
            {
                signUpForm.ShowDialog();
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            rbtnStudent.Checked = true;
        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
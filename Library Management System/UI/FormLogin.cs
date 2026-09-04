using Library_Management_System.Class;
using Library_Management_System.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
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

        private readonly string supabaseUrl = "https://nagljyrvhjkbxtjavfca.supabase.co";
        private readonly string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5hZ2xqeXJ2aGprYnh0amF2ZmNhIiwicm9sZSI6ImFub24iLCJpYXQiOjE3ODc3NzI0MTgsImV4cCI6MjEwMzM0ODQxOH0.k7vnbm-mgaQ7tCsRmWK-2hY1CTevnQh9jiGipHWI0rQ";

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string selectedRole = null;
            if (rbtnAdmin.Checked) selectedRole = "Admin";
            else if (rbtnStaff.Checked) selectedRole = "Staff";
            else if (rbtnStudent.Checked) selectedRole = "Student";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Email, password, and role selection are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnLogin.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", anonKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {anonKey}");

                    var loginPayload = new
                    {
                        email = email,
                        password = password
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(loginPayload), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{supabaseUrl}/auth/v1/token?grant_type=password", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Login failed: {responseString}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var loginJson = JObject.Parse(responseString);
                    var accessToken = loginJson["access_token"]?.ToString();
                    var userId = loginJson["user"]?["id"]?.ToString();

                    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(userId))
                    {
                        MessageBox.Show("Login failed: Could not retrieve access token or user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Fetch profile
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("apikey", anonKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                    var profileResponse = await client.GetAsync($"{supabaseUrl}/rest/v1/profiles?id=eq.{userId}&select=role,full_name,email");
                    var profileString = await profileResponse.Content.ReadAsStringAsync();

                    if (!profileResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Failed to fetch profile: {profileString}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var profiles = JArray.Parse(profileString);
                    if (profiles.Count == 0)
                    {
                        MessageBox.Show("User profile not found. Contact administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var profile = profiles[0];
                    string userRole = profile["role"]?.ToString();
                    string fullName = profile["full_name"]?.ToString();

                    if (string.IsNullOrEmpty(userRole))
                    {
                        MessageBox.Show("No role assigned to your account. Contact admin.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!string.Equals(userRole, selectedRole, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Access denied. You selected '{selectedRole}', but your account role is '{userRole}'.", "Role Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Save session data
                    CurrentSession.Email = email;
                    CurrentSession.Role = userRole;
                    CurrentSession.FullName = fullName;

                    MessageBox.Show($"Welcome {userRole}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open dashboard based on role
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
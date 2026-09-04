using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.UI
{
    public partial class FormSignup : Form
    {
        public FormSignup()
        {
            InitializeComponent();
        }
        private readonly string supabaseUrl = "https://nagljyrvhjkbxtjavfca.supabase.co";
        private readonly string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5hZ2xqeXJ2aGprYnh0amF2ZmNhIiwicm9sZSI6ImFub24iLCJpYXQiOjE3ODc3NzI0MTgsImV4cCI6MjEwMzM0ODQxOH0.k7vnbm-mgaQ7tCsRmWK-2hY1CTevnQh9jiGipHWI0rQ"; 

        private async void btnSignup_Click(object sender, EventArgs e)
        {
            string fullName = txtNewFullname.Text.Trim();
            string email = txtNewEmail.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string selectedRole = rbtnAdmin.Checked ? "Admin" :
                                  rbtnStaff.Checked ? "Staff" :
                                  rbtnStudent.Checked ? "Student" : "";

            // Input Validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select a role (Admin, Staff, or Student).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("apikey", anonKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {anonKey}");

                    // Step 1: Sign up the user with metadata
                    var signupPayload = new
                    {
                        email = email,
                        password = password,
                        data = new
                        {
                            full_name = fullName,
                            role = selectedRole
                        }
                    };

                    var signupJson = JsonConvert.SerializeObject(signupPayload);
                    var signupContent = new StringContent(signupJson, Encoding.UTF8, "application/json");

                    var signupResponse = await client.PostAsync($"{supabaseUrl}/auth/v1/signup", signupContent);
                    var signupResult = await signupResponse.Content.ReadAsStringAsync();

                    if (!signupResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Signup failed: {signupResult}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 2: Get User ID
                    var signupData = JObject.Parse(signupResult);
                    string userId = signupData["user"]?["id"]?.ToString();

                    if (string.IsNullOrEmpty(userId))
                    {
                        userId = signupData["id"]?.ToString();
                    }

                    if (string.IsNullOrEmpty(userId))
                    {
                        MessageBox.Show("Failed to retrieve user ID. Email confirmation may be required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Step 3: Insert into profiles table
                    var profilePayload = new
                    {
                        id = userId,
                        full_name = fullName,
                        email = email,
                        role = selectedRole
                    };

                    var profileJson = JsonConvert.SerializeObject(profilePayload);
                    var profileContent = new StringContent(profileJson, Encoding.UTF8, "application/json");

                    var profileResponse = await client.PostAsync($"{supabaseUrl}/rest/v1/profiles", profileContent);

                    if (profileResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Account created successfully for {selectedRole}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        var profileResult = await profileResponse.Content.ReadAsStringAsync();
                        MessageBox.Show($"Failed to create profile: {profileResult}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            this.Hide();
            formLogin.Show();
        }

        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtNewEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewFullname_TextChanged(object sender, EventArgs e)
        {

        }

        private void rbtnStudent_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtnStaff_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtnAdmin_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
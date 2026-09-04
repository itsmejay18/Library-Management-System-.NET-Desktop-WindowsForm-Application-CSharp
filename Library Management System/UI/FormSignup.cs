using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private string ComputeSha256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            string fullName = txtNewFullname.Text.Trim();
            string email = txtNewEmail.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string selectedRole = rbtnAdmin.Checked ? "Admin" :
                                  rbtnStaff.Checked ? "Staff" :
                                  rbtnStudent.Checked ? "Student" : "";

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
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM users_tbl WHERE username = @email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@email", email);
                        long exists = (long)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            MessageBox.Show("An account with this email already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO users_tbl (username, full_name, email, password, role) VALUES (@username, @fullname, @email, @password, @role)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", email);
                        cmd.Parameters.AddWithValue("@fullname", fullName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", ComputeSha256(password));
                        cmd.Parameters.AddWithValue("@role", selectedRole);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Account created successfully for {selectedRole}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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
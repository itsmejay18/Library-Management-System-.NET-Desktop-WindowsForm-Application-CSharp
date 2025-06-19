using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.Usercontrol.AdminUserControl.Input_Forms
{
    public partial class FormEditAuthor : Form
    {
        public FormEditAuthor(int authorId)
        {
            InitializeComponent();
            _authorId = authorId;
            LoadAuthorData();
        }
        private readonly int _authorId;
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void EditAuthor()
        {
            string authorName = txtAuthor.Text.Trim();
            string contactInfo = txtContactInfo.Text.Trim();
            string biography = rtbBiography.Text.Trim();

            if (string.IsNullOrWhiteSpace(authorName))
            {
                MessageBox.Show("Author Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    UPDATE authors_tbl 
                    SET author_name = @AuthorName, 
                        contact_info = @ContactInfo, 
                        biography = @Biography 
                    WHERE author_id = @AuthorId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorId", _authorId);
                        cmd.Parameters.AddWithValue("@AuthorName", authorName);
                        cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                        cmd.Parameters.AddWithValue("@Biography", biography);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Author updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the author. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadAuthorData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT author_name, contact_info, biography FROM authors_tbl WHERE author_id = @AuthorId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorId", _authorId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtAuthor.Text = reader["author_name"].ToString();
                                txtContactInfo.Text = reader["contact_info"]?.ToString();
                                rtbBiography.Text = reader["biography"]?.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading author data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EditAuthor();
        }
    }
}

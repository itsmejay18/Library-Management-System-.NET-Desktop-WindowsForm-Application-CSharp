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
    public partial class FormAddAuthor : Form
    {
        public FormAddAuthor()
        {
            InitializeComponent();
        }
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void AddAuthor()
        {
            string authorName = txtAuthor.Text.Trim();
            string contactInfo = txtContactInfo.Text.Trim();
            string biography = rtbBiography.Text.Trim();

            if (string.IsNullOrEmpty(authorName))
            {
                MessageBox.Show("Author Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO authors_tbl (author_name, contact_info, biography) VALUES (@AuthorName, @ContactInfo, @Biography)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorName", authorName);
                        cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);
                        cmd.Parameters.AddWithValue("@Biography", biography);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Author added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding author: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddAuthor();
        }
    }
}

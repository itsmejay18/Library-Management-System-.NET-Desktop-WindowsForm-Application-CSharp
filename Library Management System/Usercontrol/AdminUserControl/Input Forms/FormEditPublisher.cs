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
    public partial class FormEditPublisher : Form
    {
        public FormEditPublisher(int publisherId)
        {
            InitializeComponent();
            _publisherId = publisherId;
            LoadPublisherData();
        }
        private readonly int _publisherId;
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void EditPublisher()
        {
            string publisherName = txtPublisher.Text.Trim();
            string contactInfo = txtContactInfo.Text.Trim();

            if (string.IsNullOrWhiteSpace(publisherName))
            {
                MessageBox.Show("Publisher Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    UPDATE publishers_tbl 
                    SET publisher_name = @PublisherName, 
                        contact_info = @ContactInfo
                    WHERE publisher_id = @publisherId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PublisherId", _publisherId);
                        cmd.Parameters.AddWithValue("@PublisherName", publisherName);
                        cmd.Parameters.AddWithValue("@ContactInfo", contactInfo);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Publisher updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the publisher. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EditPublisher();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadPublisherData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT publisher_name, contact_info FROM publishers_tbl WHERE publisher_id = @PublisherId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PublisherId", _publisherId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtPublisher.Text = reader["publisher_name"].ToString();
                                txtContactInfo.Text = reader["contact_info"]?.ToString();
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
    }
}

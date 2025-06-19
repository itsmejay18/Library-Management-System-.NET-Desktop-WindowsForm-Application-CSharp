using Library_Management_System.Usercontrol.AdminUserControl.Input_Forms;
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

namespace Library_Management_System.Usercontrol.AdminUserControl
{
    public partial class UserControlAuthPub : UserControl
    {
        public UserControlAuthPub()
        {
            InitializeComponent();
            LoadAuthorsReport();
            LoadPublishersReport();
        }
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void LoadAuthorsReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    author_Id AS 'ID', 
                    author_name AS 'Author',
                    contact_info AS 'Contact Info',
                    biography AS 'Biography'
                FROM authors_tbl";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE author_id LIKE @Search 
                    OR author_name LIKE @Search";
            }

            PopulateDataGridView(query, dgvAuthors, searchQuery);
        }

        private void LoadPublishersReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    publisher_Id AS 'ID', 
                    publisher_name AS 'Publisher', 
                    contact_info AS 'Contact Info'
                FROM publishers_tbl";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE publisher_id LIKE @Search 
                    OR publisher_name LIKE @Search";
            }

            PopulateDataGridView(query, dgvPublishers, searchQuery);
        }
        private void PopulateDataGridView(string query, DataGridView dataGridView, string searchQuery = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // If there's a search query, bind the parameter
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");
                        }

                        // Use MySqlCommand with the DataAdapter
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabControAuthPub_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControAuthPub.SelectedIndex)
            {
                case 0: // Books Report
                    LoadAuthorsReport();
                    break;
                case 1: // Members Report
                    LoadPublishersReport();
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            switch (tabControAuthPub.SelectedIndex)
            {
                case 0: // Books Report
                    LoadAuthorsReport(searchQuery);
                    break;
                case 1: // Members Report
                    LoadPublishersReport(searchQuery);
                    break;
            }
        }

        private void btnAddPublishers_Click(object sender, EventArgs e)
        {
            FormAddPublisher formAddPublisher = new FormAddPublisher();
            formAddPublisher.ShowDialog();
        }

        private void btnAddAuthors_Click(object sender, EventArgs e)
        {
            FormAddAuthor formAddAuthor = new FormAddAuthor();
            formAddAuthor.ShowDialog();
        }

        private void btnEditAuthors_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAuthors.SelectedRows.Count > 0) // Ensure a row is selected
                {
                    // Retrieve the Author_Id from the DataGridView
                    DataGridViewRow selectedRow = dgvAuthors.SelectedRows[0];

                    // Check if the column exists before accessing
                    if (dgvAuthors.Columns.Contains("ID"))
                    {
                        int authorId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                        // Open the update author form with the selected Author_Id
                        FormEditAuthor updateAuthorForm = new FormEditAuthor(authorId);
                        updateAuthorForm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("The column 'ID' does not exist in the DataGridView. Ensure your query includes it.",
                                        "Column Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAuthors_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAuthors.SelectedRows.Count > 0) // Ensure a row is selected
                {
                    // Retrieve the Author_Id from the DataGridView
                    DataGridViewRow selectedRow = dgvAuthors.SelectedRows[0];

                    // Check if the column exists before accessing
                    if (dgvAuthors.Columns.Contains("ID"))
                    {
                        int authorId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                        // Confirm deletion
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete this author? This action cannot be undone.",
                            "Confirm Deletion",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            DeleteAuthorFromDatabase(authorId);
                            LoadAuthorsReport(); // Reload the DataGridView to reflect changes
                        }
                    }
                    else
                    {
                        MessageBox.Show("The column 'ID' does not exist in the DataGridView. Ensure your query includes it.",
                                        "Column Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteAuthorFromDatabase(int authorId)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the author is linked to any books
                    string checkQuery = "SELECT COUNT(*) FROM books_tbl WHERE author_id = @AuthorId";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@AuthorId", authorId);
                        int linkedBooksCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (linkedBooksCount > 0)
                        {
                            MessageBox.Show(
                                "This author is linked to one or more books. You must remove or update the linked books before deleting the author.",
                                "Cannot Delete Author",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Proceed to delete the author
                    string deleteQuery = "DELETE FROM authors_tbl WHERE author_id = @AuthorId";
                    using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@AuthorId", authorId);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Author deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the author. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditPublishers_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPublishers.SelectedRows.Count > 0) // Ensure a row is selected
                {
                    // Retrieve the Author_Id from the DataGridView
                    DataGridViewRow selectedRow = dgvPublishers.SelectedRows[0];

                    // Check if the column exists before accessing
                    if (dgvPublishers.Columns.Contains("ID"))
                    {
                        int publisherId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                        // Open the update author form with the selected Author_Id
                        FormEditPublisher updatePublisherForm= new FormEditPublisher(publisherId);
                        updatePublisherForm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("The column 'ID' does not exist in the DataGridView. Ensure your query includes it.",
                                        "Column Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeletePublisherFromDatabase(int publisherId)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the author is linked to any books
                    string checkQuery = "SELECT COUNT(*) FROM books_tbl WHERE publisher_id = @PublisherId";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@PublisherId", publisherId);
                        int linkedBooksCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (linkedBooksCount > 0)
                        {
                            MessageBox.Show(
                                "This publisher is linked to one or more books. You must remove or update the linked books before deleting the publisher.",
                                "Cannot Delete Publisher",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Proceed to delete the author
                    string deleteQuery = "DELETE FROM publishers_tbl WHERE publisher_id = @PublisherId";
                    using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@PublisherId", publisherId);
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Publisher deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the publisher. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeletePublishers_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPublishers.SelectedRows.Count > 0) // Ensure a row is selected
                {
                    // Retrieve the Author_Id from the DataGridView
                    DataGridViewRow selectedRow = dgvPublishers.SelectedRows[0];

                    // Check if the column exists before accessing
                    if (dgvPublishers.Columns.Contains("ID"))
                    {
                        int publisherId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                        // Confirm deletion
                        DialogResult result = MessageBox.Show(
                            "Are you sure you want to delete this publishers? This action cannot be undone.",
                            "Confirm Deletion",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.Yes)
                        {
                            DeletePublisherFromDatabase(publisherId);
                            LoadPublishersReport(); // Reload the DataGridView to reflect changes
                        }
                    }
                    else
                    {
                        MessageBox.Show("The column 'ID' does not exist in the DataGridView. Ensure your query includes it.",
                                        "Column Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

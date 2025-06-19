using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Library_Management_System.Borrower_UI
{
    public partial class EnterwhatyouRemember : UserControl
    {
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        public EnterwhatyouRemember()
        {
            InitializeComponent();
        }

        private void rbtnArticleorjournal_CheckedChanged(object sender, EventArgs e) { }
        private void rbtnBook_CheckedChanged(object sender, EventArgs e) { }
        private void rbtnDissertation_CheckedChanged(object sender, EventArgs e) { }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            string title = txtMaterialTitle.Text.Trim();
            string type = "";

            if (rbtnBook.Checked)
                type = "Book";
            else if (rbtnDissertation.Checked)
                type = "Dissertation";
            else if (rbtnArticleorjournal.Checked)
                type = "Article/Journal";

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Please enter the material title and select the type.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO interlibrary_loans 
                                     (ill_id, material_type, request_date) 
                                     VALUES (@title, @type, CURRENT_DATE)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Interlibrary loan request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show("Database error while submitting request:\n" + mysqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error while submitting request:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtMaterialTitle.Clear();
            rbtnBook.Checked = false;
            rbtnDissertation.Checked = false;
            rbtnArticleorjournal.Checked = false;
        }
    }
}

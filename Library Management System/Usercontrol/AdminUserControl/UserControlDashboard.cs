using Library_Management_System.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.Usercontrol
{
    public partial class UserControlDashboard : UserControl
    {
        public UserControlDashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }
        // Connection string (replace with your actual connection string)
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void LoadDashboardData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Fetch data from the database
                    int totalBooks = GetScalarValue(conn, "SELECT COUNT(*) FROM books_tbl");
                    //int borrowedBooks = GetScalarValue(conn, "SELECT COUNT(*) FROM transactions_tbl WHERE transaction_Status = 'Borrowed'");
                    int borrowedBooks = GetScalarValue(conn, "SELECT COUNT(*) FROM books_tbl WHERE book_status = 'Borrowed'");
                    //int overdueBooks = GetScalarValue(conn, "SELECT COUNT(*) FROM dues_tbl WHERE dues_Status = 'Borrowed' AND due_date < NOW()");
                    int overdueBooks = GetScalarValue(conn, "SELECT COUNT(*) FROM dues_tbl WHERE dues_Status = 'Unpaid'");
                    int totalMembers = GetScalarValue(conn, "SELECT COUNT(*) FROM members_tbl");

                    // Set the values to labels or other controls in your user control
                    lblTotalBooks.Text = totalBooks.ToString();
                    lblBorrowedBooks.Text = borrowedBooks.ToString();
                    lblOverdueBooks.Text = overdueBooks.ToString();
                    lblTotalMembers.Text = totalMembers.ToString();
                }
            }
            catch (Exception ex)
            {
                // Display a message box in case of errors
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetScalarValue(MySqlConnection conn, string query)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblOverdueBooks_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // Assuming this UserControlDashboard is inside a Form or a Panel
            var parent = this.Parent;

            if (parent != null)
            {
                // Clear existing controls in the parent container
                parent.Controls.Clear();

                // Create instance of the UserControl you want to show, e.g. Displayill
                var displayIllControl = new Library_Management_System.Borrower_UI.Displayill();

                // Dock to fill the container (optional)
                displayIllControl.Dock = DockStyle.Fill;

                // Add the new UserControl to the parent container
                parent.Controls.Add(displayIllControl);
            }
        }

    }
}

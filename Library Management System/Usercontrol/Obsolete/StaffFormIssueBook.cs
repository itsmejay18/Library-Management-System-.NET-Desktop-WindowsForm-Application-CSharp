using Library_Management_System.Class;
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

namespace Library_Management_System.Usercontrol.StaffUserControl
{
    public partial class StaffFormIssueBook : Form
    {
        // Connection string to the database
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        public StaffFormIssueBook()
        {
            InitializeComponent();
            LoadBooks();
            LoadMembers();
        }

        // Method to load available books into the ComboBox
        private void LoadBooks()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Book_Id, Title FROM Books_tbl WHERE book_Status = 'Available'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var books = new List<KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            books.Add(new KeyValuePair<string, int>(reader["Title"].ToString(), Convert.ToInt32(reader["Book_Id"])));
                        }
                        cmbBooks.DataSource = books;
                        cmbBooks.DisplayMember = "Key";  // Display the title of the book
                        cmbBooks.ValueMember = "Value";  // Use the BookId for selection
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to load active members into the ComboBox
        private void LoadMembers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Member_Id, Name FROM Members_tbl WHERE member_Status = 'Active'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var members = new List<KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            members.Add(new KeyValuePair<string, int>(reader["Name"].ToString(), Convert.ToInt32(reader["Member_Id"])));
                        }
                        cmbMembers.DataSource = members;
                        cmbMembers.DisplayMember = "Key";  // Display member name
                        cmbMembers.ValueMember = "Value";  // Use MemberId for selection
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Issue book when the "Issue" button is clicked
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedItem == null || cmbMembers.SelectedItem == null)
            {
                MessageBox.Show("Please select both a book and a member.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selectedBook = cmbBooks.SelectedItem;
            dynamic selectedMember = cmbMembers.SelectedItem;

            int bookId = selectedBook.Value;
            int memberId = selectedMember.Value;
            DateTime dueDate = dtpDueDate.Value;

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Insert transaction
                    string transactionQuery = "INSERT INTO Transactions_tbl (Member_Id, Book_Id, Borrow_Date, Due_Date, transaction_Status) " +
                                               "VALUES (@MemberId, @BookId, NOW(), @DueDate, 'Borrowed')";
                    MySqlCommand transactionCmd = new MySqlCommand(transactionQuery, conn);
                    transactionCmd.Parameters.AddWithValue("@MemberId", memberId);
                    transactionCmd.Parameters.AddWithValue("@BookId", bookId);
                    transactionCmd.Parameters.AddWithValue("@DueDate", dueDate);
                    transactionCmd.ExecuteNonQuery();

                    // Update book status
                    string updateBookQuery = "UPDATE Books_tbl SET book_Status = 'Borrowed' WHERE Book_Id = @BookId";
                    MySqlCommand updateBookCmd = new MySqlCommand(updateBookQuery, conn);
                    updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBookCmd.ExecuteNonQuery();

                    MessageBox.Show("Book issued successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cancel the operation when the "Cancel" button is clicked
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
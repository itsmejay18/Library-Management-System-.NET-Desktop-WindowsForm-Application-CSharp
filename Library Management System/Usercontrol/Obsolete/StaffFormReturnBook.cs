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
    public partial class StaffFormReturnBook : Form
    {
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        public StaffFormReturnBook()
        {
            InitializeComponent();
            LoadBooks();
            LoadMembers();
        }

        // Load Books that are borrowed (book status = 'Borrowed')
        private void LoadBooks()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Book_Id, Title FROM Books_tbl WHERE book_Status = 'Borrowed'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var books = new List<KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            books.Add(new KeyValuePair<string, int>(reader["Title"].ToString(), Convert.ToInt32(reader["Book_Id"])));
                        }
                        cmbBooks.DataSource = books;
                        cmbBooks.DisplayMember = "Key";  // Title
                        cmbBooks.ValueMember = "Value";  // BookId
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load Active Members
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
                        cmbMembers.DisplayMember = "Key";  // Name
                        cmbMembers.ValueMember = "Value";  // MemberId
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handle Book Return on Submit
        private void btnSubmit_Click(object sender, EventArgs e)
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
            DateTime returnDate = DateTime.Now;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Update the transaction to mark it as returned
                    string updateTransactionQuery = "UPDATE Transactions_tbl SET Return_Date = @ReturnDate, transaction_Status = 'Returned' WHERE Member_Id = @MemberId AND Book_Id = @BookId AND transaction_Status = 'Borrowed'";
                    MySqlCommand updateTransactionCmd = new MySqlCommand(updateTransactionQuery, conn);
                    updateTransactionCmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                    updateTransactionCmd.Parameters.AddWithValue("@MemberId", memberId);
                    updateTransactionCmd.Parameters.AddWithValue("@BookId", bookId);
                    int rowsAffected = updateTransactionCmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("No active borrow transaction found for this book and member.", "Return Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Update the book status to 'Available'
                    string updateBookQuery = "UPDATE Books_tbl SET book_Status = 'Available' WHERE Book_Id = @BookId";
                    MySqlCommand updateBookCmd = new MySqlCommand(updateBookQuery, conn);
                    updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBookCmd.ExecuteNonQuery();

                    MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();  // Close the form after successful return
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing return: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handle Cancel button click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();  // Close the form or hide it
        }
    }
}
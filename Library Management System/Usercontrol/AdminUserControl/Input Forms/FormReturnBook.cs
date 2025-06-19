 using Library_Management_System.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.UI
{
    public partial class FormReturnBook : Form
    {
        public FormReturnBook()
        {
            InitializeComponent();
            LoadMembers();
            timer1.Start();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private void LoadMembers()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Query to select the full name of the active members
                    string query = "SELECT Member_Id, CONCAT(first_name, ' ', last_name) AS FullName FROM Members_tbl WHERE member_Status = 'Active'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var members = new List<KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            members.Add(new KeyValuePair<string, int>(reader["FullName"].ToString(), Convert.ToInt32(reader["Member_Id"])));
                        }
                        cmbMembers.DataSource = members;
                        cmbMembers.DisplayMember = "Key"; // Display the full name (Key)
                        cmbMembers.ValueMember = "Value"; // Use Member_Id (Value)
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks()
        {
            try
            {
                if (cmbMembers.SelectedItem == null) return;

                dynamic selectedMember = cmbMembers.SelectedItem;
                int memberId = selectedMember.Value;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT b.Book_Id, b.Title 
                FROM Books_tbl b
                JOIN borrowers_tbl br ON b.Book_Id = br.Book_Id
                WHERE br.Member_Id = @MemberId AND b.book_status = 'Borrowed'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MemberId", memberId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var borrowedBooks = new List<KeyValuePair<string, int>>();

                        while (reader.Read())
                        {
                            borrowedBooks.Add(new KeyValuePair<string, int>(reader["Title"].ToString(), Convert.ToInt32(reader["Book_Id"])));
                        }

                        // Clear previous data before assigning new source
                        cmbBorrowedBooks.DataSource = null;
                        cmbBorrowedBooks.Items.Clear();

                        cmbBorrowedBooks.DataSource = borrowedBooks;
                        cmbBorrowedBooks.DisplayMember = "Key";
                        cmbBorrowedBooks.ValueMember = "Value";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading borrowed books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbBorrowedBooks.SelectedItem == null || cmbMembers.SelectedItem == null)
            {
                MessageBox.Show("Please select both a member and a borrowed book.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selectedBook = cmbBorrowedBooks.SelectedItem;
            dynamic selectedMember = cmbMembers.SelectedItem;

            int bookId = selectedBook.Value;
            int memberId = selectedMember.Value;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Fetch borrow_id and due date
                            string query = @"
                                SELECT Borrower_Id, Due_Date 
                                FROM Borrowers_tbl 
                                WHERE Book_Id = @BookId AND Member_Id = @MemberId AND Return_Date IS NULL";
                            MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
                            cmd.Parameters.AddWithValue("@BookId", bookId);
                            cmd.Parameters.AddWithValue("@MemberId", memberId);

                            int borrowId = -1;
                            DateTime dueDate = DateTime.MinValue;

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    borrowId = reader.GetInt32("Borrower_Id");
                                    dueDate = reader.GetDateTime("Due_Date");
                                }
                            }

                            if (borrowId == -1)
                            {
                                MessageBox.Show("No active borrowing record found for this book and member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            DateTime currentDate = DateTime.Now;
                            decimal fine = 0;

                            // Check for overdue and calculate fine
                            if (currentDate > dueDate)
                            {
                                int overdueDays = (currentDate - dueDate).Days;
                                fine = overdueDays * 5;

                                MessageBox.Show($"This book is overdue by {overdueDays} days. Fine: ${fine}", "Overdue Book", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                // Update dues_tbl fine and set status to 'Unpaid'
                                string updateFineQuery = @"
                                    UPDATE dues_tbl 
                                    SET Fine_amount = @Fine, dues_status = 'Unpaid' 
                                    WHERE Borrow_Id = @BorrowId";
                                MySqlCommand updateFineCmd = new MySqlCommand(updateFineQuery, conn, transaction);
                                updateFineCmd.Parameters.AddWithValue("@Fine", fine);
                                updateFineCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                                updateFineCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                // If no fine, mark dues as 'Paid'
                                string markPaidQuery = "UPDATE dues_tbl SET Fine_amount = 0, dues_status = 'Paid' WHERE Borrow_Id = @BorrowId";
                                MySqlCommand markPaidCmd = new MySqlCommand(markPaidQuery, conn, transaction);
                                markPaidCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                                markPaidCmd.ExecuteNonQuery();
                            }

                            // Update Borrowers_tbl: mark Return_Date = NOW()
                            string updateBorrowersQuery = "UPDATE Borrowers_tbl SET Return_Date = NOW() WHERE Borrower_Id = @BorrowId";
                            MySqlCommand updateBorrowersCmd = new MySqlCommand(updateBorrowersQuery, conn, transaction);
                            updateBorrowersCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                            updateBorrowersCmd.ExecuteNonQuery();

                            // Update Books_tbl status = 'Available'
                            string updateBookQuery = "UPDATE Books_tbl SET book_Status = 'Available' WHERE Book_Id = @BookId";
                            MySqlCommand updateBookCmd = new MySqlCommand(updateBookQuery, conn, transaction);
                            updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                            updateBookCmd.ExecuteNonQuery();

                            // Insert a transaction record into transactions_tbl for return
                            string insertTransactionQuery = @"
                                INSERT INTO transactions_tbl (borrower_id, transaction_date, transaction_status, fine)
                                VALUES (@BorrowerId, NOW(), 'Returned', @Fine)";
                            MySqlCommand insertTransactionCmd = new MySqlCommand(insertTransactionQuery, conn, transaction);
                            insertTransactionCmd.Parameters.AddWithValue("@BorrowerId", borrowId);
                            insertTransactionCmd.Parameters.AddWithValue("@Fine", fine);
                            insertTransactionCmd.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception exTrans)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Transaction failed: {exTrans.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing return: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateNow.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
        }
    }
}
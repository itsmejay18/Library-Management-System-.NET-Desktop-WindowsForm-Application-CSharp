using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.Class
{
    internal class Transaction
    {
        public int TransactionId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable for transactions without a return
        public decimal? Fine { get; set; } // Nullable for transactions without a fine
        public string TransactionStatus { get; set; } // E.g., "Borrowed", "Returned", "Overdue"

        // Constructor
        public Transaction(int transactionId, int memberId, int bookId, DateTime borrowDate, DateTime dueDate, DateTime returnDate, decimal fine)
        {
            TransactionId = transactionId;
            MemberId = memberId;
            BookId = bookId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            ReturnDate = null; // Will be assigned when the book is returned
            Fine = fine;
        }
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        // Add a new transaction
        public void AddTransaction()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Transactions_tbl 
                                (Member_Id, Book_Id, Borrow_Date, Due_Date, Return_Date, Fine, transaction_Status) 
                                VALUES (@MemberId, @BookId, @BorrowDate, @DueDate, @ReturnDate, @Fine, @TransactionStatus)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberId", MemberId);
                        cmd.Parameters.AddWithValue("@BookId", BookId);
                        cmd.Parameters.AddWithValue("@BorrowDate", BorrowDate);
                        cmd.Parameters.AddWithValue("@DueDate", DueDate);
                        cmd.Parameters.AddWithValue("@ReturnDate", (object)ReturnDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Fine", Fine);
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected > 0 ? "Transaction added successfully." : "Failed to add transaction.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        // Update an existing transaction
        public void UpdateTransaction()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE Transactions_tbl 
                                SET Return_Date = @ReturnDate, Fine = @Fine, transaction_Status = @TransactionStatus 
                                WHERE Transaction_Id = @TransactionId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);
                        cmd.Parameters.AddWithValue("@ReturnDate", (object)ReturnDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Fine", Fine);
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected > 0 ? "Transaction updated successfully." : "Failed to update transaction.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Delete a transaction
        public void DeleteTransaction()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Transactions_tbl WHERE Transaction_Id = @TransactionId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TransactionId", TransactionId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected > 0 ? "Transaction deleted successfully." : "Failed to delete transaction.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        // Display transactions in a DataGridView
        public void DisplayTransactions(DataGridView dataGridView)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                                    Transaction_Id AS 'Transaction ID', 
                                    Member_Id AS 'Member ID', 
                                    Book_Id AS 'Book ID', 
                                    Borrow_Date AS 'Borrow Date', 
                                    Due_Date AS 'Due Date', 
                                    Return_Date AS 'Return Date', 
                                    Fine AS 'Fine', 
                                    transaction_Status AS 'Status' 
                                FROM Transactions_tbl";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the data to the DataGridView
                        dataGridView.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying transactions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

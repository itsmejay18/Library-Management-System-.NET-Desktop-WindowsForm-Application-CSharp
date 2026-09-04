using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library_Management_System.Class;
using MySql.Data.MySqlClient;


namespace Library_Management_System.Borrower_UI
{
    public partial class BorrowBook : Form
    {
        public BorrowBook()
        {
            InitializeComponent();
            LoadBooks();
            LoadCurrentMember(); // Load only the logged-in user
            DisplayDueDate();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private void BorrowBook_Load(object sender, EventArgs e) { }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert into Borrowers_tbl
                    string borrowQuery = @"
                        INSERT INTO Borrowers_tbl (Book_Id, Member_Id, Borrow_Date, Due_Date)
                        VALUES (@BookId, @MemberId, NOW(), DATE_ADD(NOW(), INTERVAL 7 DAY));
                        SELECT LAST_INSERT_ID();";
                    MySqlCommand borrowCmd = new MySqlCommand(borrowQuery, conn);
                    borrowCmd.Parameters.AddWithValue("@BookId", bookId);
                    borrowCmd.Parameters.AddWithValue("@MemberId", memberId);
                    int borrowId = Convert.ToInt32(borrowCmd.ExecuteScalar());

                    // Insert into dues_tbl
                    string duesQuery = @"
                        INSERT INTO dues_tbl (Borrow_Id, Fine_amount, dues_status)
                        VALUES (@BorrowId, 0, 'Unpaid')";
                    MySqlCommand duesCmd = new MySqlCommand(duesQuery, conn);
                    duesCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                    duesCmd.ExecuteNonQuery();

                    // Update book status to 'Borrowed'
                    string updateBookQuery = "UPDATE Books_tbl SET book_Status = 'Borrowed' WHERE Book_Id = @BookId";
                    MySqlCommand updateBookCmd = new MySqlCommand(updateBookQuery, conn);
                    updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBookCmd.ExecuteNonQuery();

                    // ✅ Insert into transactions_tbl
                    string transactionQuery = @"
                        INSERT INTO transactions_tbl (borrower_id, transaction_date, transaction_status, fine)
                        VALUES (@BorrowId, NOW(), 'Borrowed', 0)";
                    MySqlCommand transCmd = new MySqlCommand(transactionQuery, conn);
                    transCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                    transCmd.ExecuteNonQuery();

                    MessageBox.Show("Book borrowed and transaction recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing transaction: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayDueDate()
        {
            DateTime dueDate = DateTime.Now.AddDays(7);
            lblDueDate.Text = $" {dueDate:MMMM dd, yyyy}";
        }

        private void LoadCurrentMember()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Member_Id, CONCAT(first_name, ' ', last_name) AS FullName FROM Members_tbl WHERE Email = @Email AND member_Status = 'Active'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", CurrentSession.Email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var member = new KeyValuePair<string, int>(reader["FullName"].ToString(), Convert.ToInt32(reader["Member_Id"]));
                            var members = new List<KeyValuePair<string, int>> { member };
                            cmbMembers.DataSource = members;
                            cmbMembers.DisplayMember = "Key";
                            cmbMembers.ValueMember = "Value";
                            cmbMembers.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Logged-in user is not found in the member list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading current member: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                        cmbBooks.DataSource = books.Distinct().ToList();
                        cmbBooks.DisplayMember = "Key";
                        cmbBooks.ValueMember = "Value";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
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

namespace Library_Management_System.Usercontrol
{
    public partial class UserControlReports : UserControl
    {
        public UserControlReports()
        {
            InitializeComponent();
        }
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private void LoadBooksReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    Book_Id AS 'Book ID', 
                    Title AS 'Title', 
                    Category AS 'Category', 
                    Genre AS 'Genre',
                    ISBN AS 'ISBN', 
                    Author_id AS 'Author ID', 
                    publisher_id AS 'Publisher ID',
                    year_published AS 'Year Published', 
                    book_Status AS 'Status' 
                FROM Books_tbl";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE book_ID LIKE @Search 
                    OR Title LIKE @Search 
                    OR Author LIKE @Search
                    OR Category LIKE @Search
                    OR Genre LIKE @Search
                    OR book_status LIKE @Search";
            }

            PopulateDataGridView(query, dataGridViewBooks, searchQuery);
        }

        private void LoadMembersReport(string searchQuery = "")
        {
            //string query = "SELECT Member_Id AS 'ID', Name AS 'Name', Email AS 'Email', Phone AS 'Phone', member_Status AS 'Status' FROM Members_tbl";
            string query = @"
                SELECT 
                    member_id AS 'Member ID', 
                    last_name AS 'Last Name',
                    first_name AS 'First Name',
                    Email AS 'Email', 
                    Phone AS 'Phone',
                    member_type AS 'Type',
                    membership_date AS 'Membership Date',
                    member_Status AS 'Member Status' 
                FROM members_tbl";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE member_ID LIKE @Search 
                    OR last_name LIKE @Search
                    OR first_name LIKE @Search
                    OR Email LIKE @Search
                    OR Phone LIKE @Search
                    OR member_type LIKE @Search
                    OR member_status LIKE @Search";
            }

            PopulateDataGridView(query, dataGridViewMembers, searchQuery);
        }

        private void LoadTransactionsReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    t.Transaction_Id AS 'Transaction ID',
                    t.Borrower_id AS 'Borrower ID',
                    br.book_id AS 'Book ID',
                    br.member_id AS 'Member ID',
                    br.Borrow_Date AS 'Borrow Date',
                    br.Due_Date AS 'Due Date',
                    br.Return_Date AS 'Return Date',
                    t.Fine AS 'Fine',
                    t.transaction_Status AS 'Status'
                FROM 
                    Transactions_tbl t
                JOIN Borrowers_tbl br ON t.Borrower_Id = br.Borrower_Id";
            // JOIN Members_tbl m ON t.Member_Id = m.Member_Id
            // JOIN Books_tbl b ON t.Book_Id = b.Book_Id

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE m.Name LIKE @Search 
                    OR b.Title LIKE @Search 
                    OR t.Transaction_Id LIKE @Search
                    OR t.transaction_Status LIKE @Search";
            }

            PopulateDataGridView(query, dataGridViewTransactions, searchQuery);
        }

        private void LoadOverdueBooksReport(string searchQuery = "")
        {
            /* using transaction tbl
            string query = @"
                SELECT 
                    t.Transaction_Id AS 'Transaction ID',
                    br.member_id AS 'Member ID',
                    br.book_id AS 'Book ID',
                    br.Due_Date AS 'Due Date',
                    DATEDIFF(CURDATE(), br.Due_Date) AS 'Overdue Days',
                    t.Fine AS 'Fine'
                FROM 
                    Transactions_tbl t
                JOIN borrowers_tbl br ON t.borrower_id
                WHERE 
                    t.transaction_Status = 'Borrowed' AND br.Due_Date < CURDATE()";
            // JOIN Members_tbl m ON t.Member_Id = m.Member_Id
            // JOIN Books_tbl b ON t.Book_Id = b.Book_Id
            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE t.transaction_ID LIKE @Search 
                    OR br.member_id LIKE @Search 
                    OR br.book_id LIKE @Search";
            }

            PopulateDataGridView(query, dataGridViewOverdueBooks, searchQuery);
            */
            //using dues_tbl
            string query = @"
                SELECT 
                    d.due_Id AS 'Due ID',
                    br.borrower_id AS 'Borrower ID',
                    br.borrow_date AS 'Borrow Date',
                    br.due_Date AS 'Due Date',
                    DATEDIFF(CURDATE(), br.Due_Date) AS 'Overdue Days',
                    d.Fine_amount AS 'Amount'
                FROM 
                    dues_tbl d
                JOIN borrowers_tbl br ON d.borrow_id
                WHERE 
                    d.dues_status = 'Unpaid' AND br.Due_Date < CURDATE()";
            // JOIN Members_tbl m ON t.Member_Id = m.Member_Id
            // JOIN Books_tbl b ON t.Book_Id = b.Book_Id
            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE d.due_id LIKE @Search 
                    OR br.borrower_id LIKE @Search";
            }

            PopulateDataGridView(query, dataGridViewOverdueBooks, searchQuery);
        }

        private void LoadAuthorsReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    author_Id AS 'ID', 
                    author_name AS 'Author', 
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
        private void LoadBorrowersReport(string searchQuery = "")
        {
            string query = @"
                SELECT 
                    borrower_Id AS 'ID', 
                    book_id AS 'Book ID', 
                    member_id AS 'Member ID',
                    borrow_date AS 'Borrow Date',
                    due_date AS 'Due Date',
                    return_date AS 'Return Date'
                FROM borrowers_tbl";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
                    WHERE borrower_id LIKE @Search 
                    OR book_id LIKE @Search
                    OR member_id LIKE @Search";
            }

            PopulateDataGridView(query, dgvBorrowers, searchQuery);
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

        private void UserControlReports_Load(object sender, EventArgs e)
        {
            LoadBooksReport();
        }

        private void tabControlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlReports.SelectedIndex)
            {
                case 0: // Books Report
                    LoadBooksReport();
                    break;
                case 1: // Members Report
                    LoadMembersReport();
                    break;
                case 2: // Transactions Report
                    LoadTransactionsReport();
                    break;
                case 3: // Overdue Books Report
                    LoadOverdueBooksReport();
                    break;
                case 4: // Authors
                    LoadAuthorsReport();
                    break;
                case 5: // Publishers
                    LoadPublishersReport();
                    break;
                case 6: // Borrowers
                    LoadBorrowersReport();
                    break;
            }
        }

        private void dataGridViewMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            switch (tabControlReports.SelectedIndex)
            {
                case 0: // Books Report
                    LoadBooksReport(searchQuery);
                    break;
                case 1: // Members Report
                    LoadMembersReport(searchQuery);
                    break;
                case 2: // Transactions Report
                    LoadTransactionsReport(searchQuery);
                    break;
                case 3: // Overdue Books Reports
                    LoadOverdueBooksReport(searchQuery);
                    break;
                case 4: // Authors
                    LoadAuthorsReport(searchQuery);
                    break;
                case 5: // Publishers
                    LoadPublishersReport(searchQuery);
                    break;
                case 6: // Borrowers
                    LoadBorrowersReport(searchQuery);
                    break;
            }
        }

        private void dataGridViewBooks_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridViewBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBorrowers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

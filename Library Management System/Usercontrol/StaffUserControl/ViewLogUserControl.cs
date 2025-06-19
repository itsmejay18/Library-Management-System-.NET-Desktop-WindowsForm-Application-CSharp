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
    public partial class ViewLogUserControl : UserControl
    {
        public ViewLogUserControl()
        {
            InitializeComponent();
            LoadBorrowersReport();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadBorrowersReport(string searchQuery = "")
        {
            string query = @"
        SELECT 
            b.Borrower_Id AS 'Borrow ID',
            m.last_Name AS 'Member',
            bk.Title AS 'Book',
            b.Borrow_Date AS 'Borrow Date',
            b.Due_Date AS 'Due Date',
            b.Return_Date AS 'Return Date',
            du.Fine_amount AS 'Fine',
            bk.Book_Status AS 'Status'
        FROM 
            Borrowers_tbl b
        JOIN Members_tbl m ON b.Member_Id = m.Member_Id
        JOIN Books_tbl bk ON b.Book_Id = bk.Book_Id
        JOIN dues_tbl du ON b.borrower_id = du.borrow_id";

            // Add a WHERE clause if there's a search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query += @"
            WHERE m.last_Name LIKE @Search 
            OR bk.Title LIKE @Search 
            OR b.Borrower_Id LIKE @Search
            OR b.Borrow_Status LIKE @Search";
            }

            PopulateDataGridView(query, searchQuery);
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        private void PopulateDataGridView(string query, string searchQuery = "")
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

                        // Populate the DataGridView with the result
                        dataGridViewTransactions.DataSource = dataTable;

                        // Format the columns
                        FormatDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Assuming dataGridViewTransactions is the name of your DataGridView

            // Set the width of each column according to the content or default setting
            dataGridViewTransactions.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Set the 'Book' column to fill the remaining space
            dataGridViewTransactions.Columns["Book"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Optionally, set other columns' AutoSizeMode (e.g., for 'Borrow ID' column to be fixed size)
            dataGridViewTransactions.Columns["Borrow ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

    }
}

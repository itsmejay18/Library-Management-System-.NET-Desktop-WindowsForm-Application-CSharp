using Library_Management_System.Class;
using Library_Management_System.UI;
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
    public partial class UserControlBooks : UserControl
    {
        public UserControlBooks()
        {
            InitializeComponent();
            InitializeFilterComboBox();
            LoadBooksData();
            this.Invalidate();
            this.Update();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        // Initialize your filter ComboBox items
        private void InitializeFilterComboBox()
        {
            filter.Items.Clear();
            filter.Items.Add("Id");
            filter.Items.Add("Title");
            filter.Items.Add("Category");
            filter.Items.Add("Genre");
            filter.Items.Add("ISBN");
            filter.Items.Add("Year Publish");
            filter.SelectedIndex = 1; // default to Title
        }

        // Load books with optional filterColumn and searchQuery
        private void LoadBooksData(string filterColumn = "", string searchQuery = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            b.Book_Id AS 'Book ID', 
                            b.Title AS 'Title', 
                            b.Category AS 'Category',
                            b.ISBN AS 'ISBN',
                            b.Genre AS 'Genre',
                            a.Author_Name AS 'Author Name', 
                            p.Publisher_Name AS 'Publisher Name', 
                            b.Year_Published AS 'Year Published',
                            b.Book_Status AS 'Status',
                            b.description AS 'Description'
                        FROM Books_tbl b
                        LEFT JOIN Authors_tbl a ON b.Author_Id = a.Author_Id
                        LEFT JOIN Publishers_tbl p ON b.Publisher_Id = p.Publisher_Id";

                    if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(searchQuery))
                    {
                        query += $" WHERE {filterColumn} LIKE @Search";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@Search", $"%{searchQuery}%");
                        }

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvBooks.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearchBooks_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearchBooks.Text.Trim();
            string filterColumn = "";

            if (filter.SelectedItem == null)
            {
                MessageBox.Show("Please select a filter category.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (filter.SelectedItem.ToString())
            {
                case "Id":
                    filterColumn = "b.Book_Id";
                    break;
                case "Title":
                    filterColumn = "b.Title";
                    break;
                case "Category":
                    filterColumn = "b.Category";
                    break;
                case "Genre":
                    filterColumn = "b.Genre";
                    break;
                case "ISBN":
                    filterColumn = "b.ISBN";
                    break;
                case "Year Publish":
                    filterColumn = "b.Year_Published";
                    break;
                default:
                    filterColumn = "b.Title";
                    break;
            }

            LoadBooksData(filterColumn, searchQuery);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormAddBook formAddBook = new FormAddBook();
            formAddBook.ShowDialog();
            LoadBooksData();
            this.Invalidate();
            this.Update();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBooks.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvBooks.SelectedRows[0];

                    if (dgvBooks.Columns.Contains("Book ID"))
                    {
                        int bookId = Convert.ToInt32(selectedRow.Cells["Book ID"].Value);
                        FormEditBook updateBookForm = new FormEditBook(bookId);
                        updateBookForm.ShowDialog();
                        LoadBooksData();
                        this.Invalidate();
                        this.Update();
                    }
                    else
                    {
                        MessageBox.Show("The column 'Book ID' does not exist in the DataGridView. Ensure your query includes it.",
                                        "Column Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Invalidate();
                    this.Update();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Invalidate();
                this.Update();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookId = Convert.ToInt32(dgvBooks.SelectedRows[0].Cells["Book ID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteBookFromDatabase(bookId);
                    LoadBooksData();
                    this.Invalidate();
                    this.Update();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Invalidate();
                this.Update();
            }
        }

        private void DeleteBookFromDatabase(int bookId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM books_tbl WHERE Book_Id = @BookId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the book. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBooks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvBooks.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                if (status == "Available")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.White;
                }
                else if (status == "Borrowed")
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.White;
                }
            }
        }

        private void UserControlBooks_Load(object sender, EventArgs e)
        {
            // Optional: reload data on load
            LoadBooksData();
        }
    }
}
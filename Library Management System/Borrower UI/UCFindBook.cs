using Library_Management_System.UI;
using Library_Management_System.Usercontrol;
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

namespace Library_Management_System.Borrower_UI
{
    public partial class UCFindBook : UserControl
    {
        public UCFindBook()
        {
            InitializeComponent();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private int bookId = -1;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtBookID.Text.Trim();
            string selectedFilter = filter.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(selectedFilter))
            {
                MessageBox.Show("Please enter a search term and select a filter.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string column = "";

            switch (selectedFilter)
            {
                case "Book ID":
                    column = "b.book_id";
                    break;
                case "Title":
                    column = "b.title";
                    break;
                case "Category":
                    column = "b.category";
                    break;
                case "Genre":
                    column = "b.genre";
                    break;
                case "ISBN":
                    column = "b.ISBN";
                    break;
                case "Year Published":
                    column = "b.year_published";
                    break;
                default:
                    MessageBox.Show("Invalid filter selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string bookQuery = $@"
                        SELECT 
                            b.book_id,
                            b.title,
                            b.category,
                            b.genre,
                            b.ISBN,
                            b.year_published,
                            b.book_status,
                            a.author_name,
                            b.description
                        FROM books_tbl b
                        LEFT JOIN authors_tbl a ON b.author_id = a.author_id
                        WHERE {column} LIKE @SearchTerm
                        LIMIT 1";

                    using (MySqlCommand bookCmd = new MySqlCommand(bookQuery, conn))
                    {
                        bookCmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = bookCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookId = Convert.ToInt32(reader["book_id"]);

                                lblTitle.Text = reader["title"].ToString();
                                lblAuthor.Text = reader["author_name"].ToString();
                                lblCategory.Text = reader["category"].ToString();
                                lblGenre.Text = reader["genre"].ToString();
                                lblISBN.Text = reader["ISBN"].ToString();
                                lblYear.Text = reader["year_published"].ToString();
                                lblStatus.Text = reader["book_status"].ToString();
                                lblDescription.Text = reader["description"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Book not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    // Borrower information fetching removed to eliminate errors caused by missing labels
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FindBookUserControl_Load(object sender, EventArgs e)
        {
            filter.Items.Clear();
            filter.Items.AddRange(new string[]
            {
                "Book ID",
                "Title",
                "Category",
                "Genre",
                "ISBN",
                "Year Published"
            });

            filter.SelectedIndex = 0;
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            FormAddBook formAddBook = new FormAddBook();
            formAddBook.ShowDialog();
        }

        private void btnEditBook_Click(object sender, EventArgs e)
        {
            if (bookId != -1)
            {
                FormEditBook formEditBook = new FormEditBook(bookId);
                formEditBook.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a book to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Empty event handlers (optional to remove)
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) { }
        private void UCFindBook_Load(object sender, EventArgs e) { }
        private void label29_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblAuthor_Click(object sender, EventArgs e) { }
        private void lblCategory_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void lblGenre_Click(object sender, EventArgs e) { }
        private void lblDescription_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label21_Click(object sender, EventArgs e) { }
    }
}
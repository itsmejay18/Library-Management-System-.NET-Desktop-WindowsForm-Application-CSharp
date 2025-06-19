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
    public partial class FormEditBook : Form
    {
        public FormEditBook()
        {
            InitializeComponent();
        }
        private readonly int _bookId; // Book ID to identify which record to edit
        private readonly string _connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        public FormEditBook(int bookId)
        {
            InitializeComponent();
            _bookId = bookId;
            SetupYearPicker();
            LoadBookData();
        }

        private void SetupYearPicker()
        {
            txtYear.Format = DateTimePickerFormat.Custom;
            txtYear.CustomFormat = "yyyy";
            txtYear.ShowUpDown = true;
        }
        private void LoadBookData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    b.Title, 
                    a.author_Name AS Author, 
                    p.publisher_Name AS Publisher,
                    b.Category, 
                    b.Genre, 
                    b.ISBN, 
                    b.Year_Published,
                    b.description
                FROM books_tbl b
                JOIN authors_tbl a ON b.Author_Id = a.Author_Id
                JOIN publishers_tbl p ON b.Publisher_Id = p.Publisher_Id
                WHERE b.Book_Id = @BookId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookId", _bookId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtTitle.Text = reader["Title"].ToString();
                                txtAuthor.Text = reader["Author"].ToString();
                                txtPublisher.Text = reader["Publisher"].ToString();
                                comboCategory.Text = reader["Category"].ToString();
                                comboGenre.Text = reader["Genre"].ToString();
                                txtISBN.Text = reader["ISBN"].ToString();
                                rtbDescription.Text = reader["Description"].ToString();

                                if (DateTime.TryParse(reader["Year_Published"].ToString(), out DateTime parsedYear))
                                {
                                    txtYear.Value = parsedYear;
                                }
                                else
                                {
                                    txtYear.Value = new DateTime(DateTime.Now.Year, 1, 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading book data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            UpdateBookInDatabase();
        }
        private void UpdateBookInDatabase()
        {
            string title = txtTitle.Text.Trim();
            string authorName = txtAuthor.Text.Trim();
            string publisherName = txtPublisher.Text.Trim();
            string category = comboCategory.Text.Trim();
            string genre = comboGenre.Text.Trim();
            string isbn = txtISBN.Text.Trim();
            string yearPublished = txtYear.Text.Trim();
            string description = rtbDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(authorName) || string.IsNullOrWhiteSpace(publisherName) ||
                string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(genre) || string.IsNullOrWhiteSpace(isbn) || string.IsNullOrWhiteSpace(yearPublished))
            {
                MessageBox.Show("Please fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    // Get or insert AuthorId
                    int authorId = GetOrInsertAuthor(conn, authorName);

                    // Get or insert PublisherId
                    int publisherId = GetOrInsertPublisher(conn, publisherName);

                    string query = @"
                UPDATE books_tbl 
                SET 
                    Title = @Title, 
                    Author_Id = @AuthorId, 
                    Publisher_Id = @PublisherId,
                    Category = @Category, 
                    Genre = @Genre, 
                    ISBN = @ISBN, 
                    Year_Published = @YearPublished,
                    description = @Description 
                WHERE Book_Id = @BookId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@AuthorId", authorId);
                        cmd.Parameters.AddWithValue("@PublisherId", publisherId);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        cmd.Parameters.AddWithValue("@ISBN", isbn);
                        cmd.Parameters.AddWithValue("@YearPublished", yearPublished);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@BookId", _bookId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the book. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int GetOrInsertAuthor(MySqlConnection conn, string authorName)
        {
            // Check if author exists
            string selectQuery = "SELECT Author_Id FROM Authors_tbl WHERE author_name = @Name";
            using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Name", authorName);
                object result = cmd.ExecuteScalar();

                if (result != null) // Author exists
                {
                    return Convert.ToInt32(result);
                }
            }

            // Insert new author
            string insertQuery = "INSERT INTO Authors_tbl (author_Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Name", authorName);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int GetOrInsertPublisher(MySqlConnection conn, string publisherName)
        {
            // Check if publisher exists
            string selectQuery = "SELECT Publisher_Id FROM Publishers_tbl WHERE publisher_Name = @Name";
            using (MySqlCommand cmd = new MySqlCommand(selectQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Name", publisherName);
                object result = cmd.ExecuteScalar();

                if (result != null) // Publisher exists
                {
                    return Convert.ToInt32(result);
                }
            }

            // Insert new publisher
            string insertQuery = "INSERT INTO Publishers_tbl (publisher_Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Name", publisherName);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}

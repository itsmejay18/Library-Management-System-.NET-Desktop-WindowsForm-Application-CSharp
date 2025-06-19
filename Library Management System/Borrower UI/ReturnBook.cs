using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Library_Management_System.Borrower_UI
{
    public partial class ReturnBook : Form
    {
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";
        private readonly string supabaseUrl = "https://mfvwlprqvbqqwhvpsxbr.supabase.co";
        private readonly string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1mdndscHJxdmJxcXdodnBzeGJyIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDYzNjMzNDAsImV4cCI6MjA2MTkzOTM0MH0.4KmpYTn08PeLbQwUmcHrvg2cDi35-cIjXA3jlp3CxDc";

        private readonly HttpClient httpClient = new HttpClient();

        public ReturnBook()
        {
            InitializeComponent();
            // Load members from Supabase (async)
            LoadMembersFromSupabaseAsync().ConfigureAwait(false);
            timer1.Start();
        }

        private async Task LoadMembersFromSupabaseAsync()
        {
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("apikey", anonKey);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {anonKey}");

                string url = $"{supabaseUrl}/rest/v1/Members_tbl?select=Member_Id,first_name,last_name&member_Status=eq.Active";

                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var members = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);

                var memberList = new List<KeyValuePair<string, int>>();

                foreach (var mem in members)
                {
                    string fullName = $"{mem.first_name} {mem.last_name}";
                    int id = mem.Member_Id;
                    memberList.Add(new KeyValuePair<string, int>(fullName, id));
                }

                // Set ComboBox datasource on UI thread
                if (cmbMembers.InvokeRequired)
                {
                    cmbMembers.Invoke(new Action(() =>
                    {
                        cmbMembers.DataSource = memberList;
                        cmbMembers.DisplayMember = "Key";
                        cmbMembers.ValueMember = "Value";
                        cmbMembers.DropDownStyle = ComboBoxStyle.DropDownList; // Non-editable
                    }));
                }
                else
                {
                    cmbMembers.DataSource = memberList;
                    cmbMembers.DisplayMember = "Key";
                    cmbMembers.ValueMember = "Value";
                    cmbMembers.DropDownStyle = ComboBoxStyle.DropDownList; // Non-editable
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members from Supabase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    // Fetch the borrow_id and due date
                    string query = @"
    SELECT Borrower_Id, Due_Date 
    FROM Borrowers_tbl 
    WHERE Book_Id = @BookId AND Member_Id = @MemberId AND Return_Date IS NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
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

                    if (borrowId != -1)
                    {
                        DateTime currentDate = DateTime.Now;

                        // Check overdue
                        if (currentDate > dueDate)
                        {
                            int overdueDays = (currentDate - dueDate).Days;
                            decimal fine = overdueDays * 5;

                            MessageBox.Show($"This book is overdue by {overdueDays} days. Fine: ${fine}", "Overdue Book", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            // Update dues_tbl with fine and set status Unpaid
                            string updateFineQuery = @"
            UPDATE dues_tbl 
            SET Fine_amount = @Fine, dues_status = 'Unpaid' 
            WHERE Borrow_Id = @BorrowId";
                            MySqlCommand updateFineCmd = new MySqlCommand(updateFineQuery, conn);
                            updateFineCmd.Parameters.AddWithValue("@Fine", fine);
                            updateFineCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                            updateFineCmd.ExecuteNonQuery();
                        }

                        // Mark dues as Paid on return
                        string markPaidQuery = "UPDATE dues_tbl SET dues_status = 'Paid' WHERE Borrow_Id = @BorrowId";
                        MySqlCommand markPaidCmd = new MySqlCommand(markPaidQuery, conn);
                        markPaidCmd.Parameters.AddWithValue("@BorrowId", borrowId);
                        markPaidCmd.ExecuteNonQuery();
                    }

                    // Update Borrowers_tbl to set Return_Date now
                    string updateBorrowersQuery = "UPDATE Borrowers_tbl SET Return_Date = NOW() WHERE Book_Id = @BookId AND Member_Id = @MemberId AND Return_Date IS NULL";
                    MySqlCommand updateBorrowersCmd = new MySqlCommand(updateBorrowersQuery, conn);
                    updateBorrowersCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBorrowersCmd.Parameters.AddWithValue("@MemberId", memberId);
                    updateBorrowersCmd.ExecuteNonQuery();

                    // Update Books_tbl to Available
                    string updateBookQuery = "UPDATE Books_tbl SET book_Status = 'Available' WHERE Book_Id = @BookId";
                    MySqlCommand updateBookCmd = new MySqlCommand(updateBookQuery, conn);
                    updateBookCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateBookCmd.ExecuteNonQuery();

                    MessageBox.Show("Book returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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
                    // Query borrowed books for selected member
                    string query = @"
                SELECT b.Book_Id, b.Title 
                FROM Books_tbl b
                JOIN Borrowers_tbl br ON b.Book_Id = br.Book_Id
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

        private void cmbMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBorrowedBooks();
        }
    }
}
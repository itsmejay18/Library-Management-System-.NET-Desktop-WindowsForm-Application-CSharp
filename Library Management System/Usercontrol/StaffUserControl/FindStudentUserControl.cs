using Google.Protobuf.WellKnownTypes;
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
    public partial class FindStudentUserControl : UserControl
    {
        private readonly string connectionString = "Server=localhost;Database=librarydb;Uid=root;Pwd=root;";

        public FindStudentUserControl()
        {
            InitializeComponent();
        }
        private int studentId; // Class-level variable to store Student ID
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string input = txtSearchMember.Text.Trim();
            string selectedFilter = filter.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(selectedFilter))
            {
                MessageBox.Show("Please enter a value and select a filter type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryCondition = "";
            if (selectedFilter == "Member ID")
            {
                queryCondition = "m.Member_Id = @Input";
            }
            else if (selectedFilter == "Full Name")
            {
                queryCondition = "CONCAT(m.first_name, ' ', m.last_name) LIKE @Input";
                input = "%" + input + "%";
            }

            string query = $@"
        SELECT 
            m.Member_Id AS 'Student ID',
            CONCAT(m.first_name, ' ', m.last_name) AS 'Name',
            m.Email AS 'Email',
            m.Phone AS 'Phone',
            m.member_type AS 'Type',
            m.Membership_Date AS 'Membership Date',
            m.Member_Status AS 'Status',
            br.Borrower_Id AS 'Borrow ID',
            bk.Book_Id AS 'Book ID',
            bk.Title AS 'Book Title',
            br.Borrow_Date AS 'Borrow Date',
            br.Due_Date AS 'Due Date',
            bk.Book_Status AS 'Book Status'
        FROM 
            Members_tbl m
        LEFT JOIN 
            Borrowers_tbl br ON m.Member_Id = br.Member_Id
        LEFT JOIN 
            Books_tbl bk ON br.Book_Id = bk.Book_Id
        WHERE 
            {queryCondition}
        ORDER BY 
            br.Borrow_Date DESC";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Input", input);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        ClearFields();

                        if (reader.Read())
                        {
                            studentId = Convert.ToInt32(reader["Student ID"]);

                            lblStudentID.Text = reader["Student ID"].ToString();
                            lblName.Text = reader["Name"].ToString();
                            lblEmail.Text = reader["Email"].ToString();
                            lblPhone.Text = reader["Phone"].ToString();
                            lblMemberType.Text = reader["Type"].ToString();
                            lblMembershipDate.Text = Convert.ToDateTime(reader["Membership Date"]).ToShortDateString();
                            lblStatus.Text = reader["Status"].ToString();

                            lblTransactionID.Text = reader["Borrow ID"].ToString();
                            lblBookID.Text = reader["Book ID"].ToString();
                            lblBookTitle.Text = reader["Book Title"].ToString();
                            lblBorrowDate.Text = reader["Borrow Date"].ToString();
                            lblDueDate.Text = reader["Due Date"].ToString();
                            lblBookStatus.Text = reader["Book Status"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No records found!", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void ClearFields()
        {
            lblStudentID.Text = "?";
            lblName.Text = "?";
            lblEmail.Text = "?";
            lblPhone.Text = "?";
            lblMemberType.Text = "?";
            lblMembershipDate.Text = "?";
            lblStatus.Text = "?";

            lblTransactionID.Text = "?";
            lblBookID.Text = "?";
            lblBookTitle.Text = "?";
            lblBorrowDate.Text = "?";
            lblDueDate.Text = "?";
            lblBookStatus.Text = "?";
        }

        private void btnEditMember_Click(object sender, EventArgs e)
        {
            if (studentId != 0)
            {

                // Open the edit form and pass the member ID
                FormEditMember formEditMember = new FormEditMember(studentId);
                formEditMember.ShowDialog();

            }
            else
            {
                MessageBox.Show("Please select a member to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            FormAddMember formAddMember = new FormAddMember();
            formAddMember.ShowDialog();
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if (studentId != 0)
            {

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this member?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteMemberFromDatabase(studentId);
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void DeleteMemberFromDatabase(int memberId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Members_tbl WHERE Member_Id = @MemberId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberId", memberId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Member deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the member. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting member: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FindStudentUserControl_Load(object sender, EventArgs e)
        {
            filter.Items.Clear();
            filter.Items.Add("Member ID");
            filter.Items.Add("Full Name");
            filter.SelectedIndex = 0; // Default selection
        }


    }
}

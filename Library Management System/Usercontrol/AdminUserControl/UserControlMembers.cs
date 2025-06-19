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
    public partial class UserControlMembers : UserControl
    {
        public UserControlMembers()
        {
            InitializeComponent();

            // Initialize filter ComboBox items
            filter.Items.Clear();
            filter.Items.Add("Member ID");
            filter.Items.Add("Fullname");
            filter.SelectedIndex = 1; // Default select Fullname

            LoadMembersData();
        }

        private readonly string connectionString = "Server=localhost;Database=librarydb;uid=root;Pwd=root;";

        private void LoadMembersData(string filterColumn = "", string searchQuery = "")
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Connection string is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dgvMembers == null)
                {
                    MessageBox.Show("DataGridView is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT 
                        Member_Id AS 'Member ID',
                        First_Name AS 'First Name',
                        Last_Name AS 'Last Name',
                        Email AS 'Email',
                        Phone AS 'Phone',
                        Member_Type AS 'Member Type',
                        Membership_Date AS 'Membership Date',
                        Member_Status AS 'Status'
                    FROM Members_tbl";

                    if (!string.IsNullOrEmpty(filterColumn) && !string.IsNullOrEmpty(searchQuery))
                    {
                        if (filterColumn == "Fullname")
                        {
                            query += " WHERE CONCAT(First_Name, ' ', Last_Name) LIKE @Search";
                        }
                        else
                        {
                            query += $" WHERE {filterColumn} LIKE @Search";
                        }
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
                        dgvMembers.DataSource = dataTable;
                        dgvMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserControlMembers_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormAddMember formAddMember = new FormAddMember();
            formAddMember.ShowDialog();
            LoadMembersData(); // Refresh after adding a member
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count > 0)
            {
                // Get the selected member's ID
                int memberId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Member ID"].Value);

                // Open the edit form and pass the member ID
                FormEditMember formEditMember = new FormEditMember(memberId);
                formEditMember.ShowDialog();

                // Refresh data after editing
                LoadMembersData();
            }
            else
            {
                MessageBox.Show("Please select a member to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count > 0)
            {
                // Get the selected member's ID
                int memberId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Member ID"].Value);

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete this member?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteMemberFromDatabase(memberId);
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
                            LoadMembersData(); // Refresh the DataGridView
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearchMembers_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearchMembers.Text.Trim();
            string filterColumn = "";

            if (filter.SelectedItem == null)
            {
                MessageBox.Show("Please select a filter.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (filter.SelectedItem.ToString())
            {
                case "Member ID":
                    filterColumn = "Member_Id";
                    break;
                case "Fullname":
                    filterColumn = "Fullname"; // special case handled in LoadMembersData
                    break;
                default:
                    filterColumn = "Fullname";
                    break;
            }

            LoadMembersData(filterColumn, searchQuery);
        }
    }
}
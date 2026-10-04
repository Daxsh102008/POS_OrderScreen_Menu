using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTAU
{
    public partial class UsersManagement : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;
        private int selectedUserId = 0; // Track the currently selected user for updates/deletions
        public UsersManagement()
        {
            InitializeComponent();
        }

        private void btnReturnTP_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void UsersManagement_Load(object sender, EventArgs e)
        {
            LoadUsersGrid();
        }

        private void LoadUsersGrid()
        {
            string query = "SELECT UserId, username, password FROM Users";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvUsers.DataSource = dt;

                    // Hide the password column in the grid for basic security
                    if (dgvUsers.Columns.Contains("Password"))
                        dgvUsers.Columns["Password"].Visible = false;
                    dgvUsers.AllowUserToAddRows = false;
                }
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["UserId"].Value);
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = row.Cells["Password"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all fields to create a user.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DoesUsernameExist(txtUsername.Text))
            {
                MessageBox.Show($"The username '{txtUsername.Text}' is already taken. Please choose a different one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            string query = "INSERT INTO Users (username, password) VALUES (@User, @Pass)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@User", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            //Tell the user the operation was successful and reset the form for the next entry
            MessageBox.Show($"User '{txtUsername.Text}' has been created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadUsersGrid();
        }
        private bool DoesUsernameExist(string username)
        {
            // COUNT(1) is lightning-fast for checking rows
            string query = "SELECT COUNT(1) FROM Users WHERE Username = @User";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Trim any accidental trailing spaces the user might have typed
                        cmd.Parameters.AddWithValue("@User", username.Trim());

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0; // If count > 0, it means the name is taken!
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security check failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true; // Err on the side of caution and block the insert if SQL breaks
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please select a user to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Users SET username = @User, password = @Pass WHERE UserId = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@User", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Id", selectedUserId);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"User '{txtUsername.Text}' has been updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
            LoadUsersGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0)
            {
                {
                    MessageBox.Show("Please select a user to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Always ask for confirmation before a DELETE! 
            DialogResult dialog = MessageBox.Show("Are you sure you want to sack this user? This cannot be undone.",
                                                  "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialog == DialogResult.Yes)
            {
                string query = "DELETE FROM Users WHERE UserId = @Id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", selectedUserId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"User '{txtUsername.Text}' has been deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadUsersGrid();
            }
        }
        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            selectedUserId = 0;
            txtUsername.Focus();
        }
    }
}

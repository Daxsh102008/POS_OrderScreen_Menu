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
    public partial class ManagerPanel : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;
        public ManagerPanel()
        {
            InitializeComponent();
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMenu.Rows[e.RowIndex];
                txtPriceUpdate.Text = row.Cells["Price"].Value.ToString();
            }
        }

        private void ManagerPanel_Load(object sender, EventArgs e)
        {
            LoadMenuGrid();
            StyleGrid();
            //Code to update the total cost of sales from orders
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string totalSalesQuery = "SELECT SUM(TotalAmount) FROM Orders";
                    SqlCommand cmd = new SqlCommand(totalSalesQuery, conn);
                    object result = cmd.ExecuteScalar();
                    decimal totalSales = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    lblTotalSales.Text = $"Total Sales: £{totalSales:F2}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error calculating total sales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void StyleGrid()
        {
            // Customize Header Style
            dgvMenu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 49, 67); // #2B3143
            dgvMenu.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMenu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvMenu.ColumnHeadersHeight = 40;

            // Customize Rows Style
            dgvMenu.DefaultCellStyle.BackColor = Color.FromArgb(30, 34, 48); // #1E2230
            dgvMenu.DefaultCellStyle.ForeColor = Color.White;
            dgvMenu.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvMenu.RowTemplate.Height = 35; // Gives rows some breathing space

            // Highlight color when a row is selected
            dgvMenu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(250, 116, 19); // Flame Orange
            dgvMenu.DefaultCellStyle.SelectionForeColor = Color.White;
        }
        //Code to retrieve items from food database
        private void LoadMenuGrid()
        {
            string query = "SELECT ItemId, ItemName, Price FROM MenuItems";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable menuTable = new DataTable();
                    adapter.Fill(menuTable);
                    dgvMenu.DataSource = menuTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading menu: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //dgvMenu.Columns["ItemId"].ReadOnly = true; // Make ItemId read-only
                //dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Adjust column widths to fill the grid
            }
        }

        private void btnUpdatePriceM_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null)
            {
                MessageBox.Show("Please select an item to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                //Get selected item ID and price
                int selectedItemId = Convert.ToInt32(dgvMenu.CurrentRow.Cells["ItemId"].Value);
                decimal newPrice = Convert.ToDecimal(txtPriceUpdate.Text);

                string updateQuery = "UPDATE MenuItems SET Price = @Price WHERE ItemId = @ItemId";

                using (SqlConnection conn = new SqlConnection(connectionString)) 
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn)) 
                    {
                        cmd.Parameters.AddWithValue("@Price", newPrice);
                        cmd.Parameters.AddWithValue("@ItemId", selectedItemId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Price updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadMenuGrid(); // Refresh the grid to show updated price
                        }
                        else
                        {
                            MessageBox.Show("Failed to update price. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //Refresh the grid to show updated price
                        LoadMenuGrid();
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid price.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            // Return to the main menu or previous form
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void btnViewAcc_Click(object sender, EventArgs e)
        {
            
            UsersManagement viewAcc = new UsersManagement();
            viewAcc.ShowDialog();
        }
    }
}

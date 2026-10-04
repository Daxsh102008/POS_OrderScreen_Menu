using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RESTAU
{
    public partial class OrderHistory : Form
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;

        private DataTable ordersTable;

        public OrderHistory()
        {
            InitializeComponent();
            ordersTable = new DataTable();
        }

        private void OrderHistory_Load(object sender, EventArgs e)
        {
            try
            {
                LoadOrderHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order history: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderHistory()
        {
            // Columns that actually exist in your Orders table
            string query = @"SELECT OrderID, OrderDate, OrderType, PaymentType, 
                                    TotalAmount, PaidAmount, ChangeAmount, Status
                             FROM Orders 
                             ORDER BY OrderDate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    ordersTable.Clear();
                    adapter.Fill(ordersTable);

                    dgvOrders.DataSource = ordersTable;
                    dgvOrders.ReadOnly = true;
                    dgvOrders.AllowUserToAddRows = false;
                    dgvOrders.AllowUserToDeleteRows = false;

                    // Nicer column headers
                    dgvOrders.Columns["OrderID"].HeaderText = "Order #";
                    dgvOrders.Columns["OrderDate"].HeaderText = "Date & Time";
                    dgvOrders.Columns["OrderType"].HeaderText = "Type";
                    dgvOrders.Columns["PaymentType"].HeaderText = "Payment";
                    dgvOrders.Columns["TotalAmount"].HeaderText = "Total (£)";
                    dgvOrders.Columns["PaidAmount"].HeaderText = "Paid (£)";
                    dgvOrders.Columns["ChangeAmount"].HeaderText = "Change (£)";
                    dgvOrders.Columns["Status"].HeaderText = "Status";

                    // Format money columns
                    dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "F2";
                    dgvOrders.Columns["PaidAmount"].DefaultCellStyle.Format = "F2";
                    dgvOrders.Columns["ChangeAmount"].DefaultCellStyle.Format = "F2";
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (ordersTable == null) return;

            string filterText = txtSearch.Text.Trim().Replace("'", "''"); // prevent injection

            if (string.IsNullOrEmpty(filterText))
            {
                ordersTable.DefaultView.RowFilter = "";
            }
            else
            {
                // Search across Order#, Type, Payment, and Status
                ordersTable.DefaultView.RowFilter =
                    $"CONVERT(OrderID, 'System.String') LIKE '%{filterText}%' OR " +
                    $"OrderType LIKE '%{filterText}%' OR " +
                    $"PaymentType LIKE '%{filterText}%' OR " +
                    $"Status LIKE '%{filterText}%'";
            }
        }
    }
}
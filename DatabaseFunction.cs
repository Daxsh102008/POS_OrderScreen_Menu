using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace RESTAU
{
    public static class DatabaseFunction
    {
        //DB runs locally
        private static string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;

        //Executing commands
        public static void ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SaveDineInOrder(decimal totalAmount, decimal paidAmount, decimal changeAmount, string paymentType, List<string> orderItems)
        {
            int orderId;

            // 1. Insert into Orders and get the new OrderID
            string insertOrder = @"INSERT INTO Orders (OrderDate, TotalAmount, PaidAmount, ChangeAmount, PaymentType, OrderType, Status)
                           OUTPUT INSERTED.OrderID
                           VALUES (@OrderDate, @Total, @Paid, @Change, @PaymentType, 'DineIn', 'Completed')";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(insertOrder, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Total", totalAmount);
                    cmd.Parameters.AddWithValue("@Paid", paidAmount);
                    cmd.Parameters.AddWithValue("@Change", changeAmount);
                    cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                    orderId = (int)cmd.ExecuteScalar();
                }

                // 2. Insert each item into OrderItems
                foreach (string line in orderItems)
                {
                    // Parse line format: "Chicken Burger       x2   £5.99  =  £11.98"
                    try
                    {
                        string itemName = line.Substring(0, 20).Trim();
                        int xIdx = line.IndexOf(" x");
                        int firstPound = line.IndexOf("£");
                        int equalIdx = line.IndexOf("=");
                        int secondPound = line.IndexOf("£", equalIdx);

                        int quantity = int.Parse(line.Substring(xIdx + 2, firstPound - xIdx - 2).Trim());
                        decimal unitPrice = decimal.Parse(line.Substring(firstPound + 1, equalIdx - firstPound - 1).Trim());
                        decimal totalPrice = decimal.Parse(line.Substring(secondPound + 1).Trim());

                        string insertItem = @"INSERT INTO OrderItems (OrderID, ItemName, Quantity, UnitPrice, TotalPrice)
                                      VALUES (@OrderID, @ItemName, @Qty, @Unit, @Total)";

                        using (SqlCommand itemCmd = new SqlCommand(insertItem, conn))
                        {
                            itemCmd.Parameters.AddWithValue("@OrderID", orderId);
                            itemCmd.Parameters.AddWithValue("@ItemName", itemName);
                            itemCmd.Parameters.AddWithValue("@Qty", quantity);
                            itemCmd.Parameters.AddWithValue("@Unit", unitPrice);
                            itemCmd.Parameters.AddWithValue("@Total", totalPrice);
                            itemCmd.ExecuteNonQuery();
                        }
                    }
                    catch { /* skip malformed lines */ }
                }
            }
        }

        //Code to fetch table data
        public static (bool status, decimal total, DateTime? seatedTime) GetTableState(int TableId)
        {
            bool status = false;
            decimal total = 0.00m;
            DateTime? seatedTime = null;

            string query = "SELECT Status, TotalAmount, TimeWaited FROM TableRestau WHERE TableId = @TableId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableId", TableId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            status = Convert.ToBoolean(reader["Status"]);
                            total = Convert.ToDecimal(reader["TotalAmount"]);
                            seatedTime = reader["TimeWaited"] as DateTime?;
                        }
                    }
                }
            }
            return (status, total, seatedTime);
        }

        public static void SaveCompletedOrder(string OrderType, string Platform, decimal TotalAmount)
        {
            // Automatically determine the initial status:
            // Dine-In is paid instantly when recorded -> 'Completed'
            // Takeaway needs preparation/pickup tracking -> 'Pending'
            string initialStatus = (OrderType == "DineIn") ? "Completed" : "Pending";

            string query = "INSERT INTO Orders (OrderType, Platform, TotalAmount, OrderDate, OrderStatus) VALUES (@OrderType, @Platform, @TotalAmount, @OrderDate, @OrderStatus)";

            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@OrderType", OrderType),
               new SqlParameter("@Platform", Platform),
               new SqlParameter("@TotalAmount", TotalAmount),
               new SqlParameter("@OrderDate", DateTime.Now),
               new SqlParameter("@OrderStatus", initialStatus) // Sets it dynamically!
            };

            ExecuteNonQuery(query, parameters);
        }
        public static Dictionary<int, (bool Status, decimal Total, DateTime? SeatedTime)> GetAllTableStates()
        {
            var tableStates = new Dictionary<int, (bool Status, decimal Total, DateTime? SeatedTime)>();
            string query = "SELECT TableId, Status, TotalAmount, TimeWaited FROM TableRestau";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int tableId = Convert.ToInt32(reader["TableId"]);
                            bool status = Convert.ToBoolean(reader["Status"]);
                            decimal total = Convert.ToDecimal(reader["TotalAmount"]);
                            DateTime? seatedTime = reader["TimeWaited"] as DateTime?;
                            tableStates[tableId] = (status, total, seatedTime);
                        }
                    }
                }
            }
            return tableStates;
        }
        // 1. Fetches all active/pending takeaway and online orders
        public static System.Data.DataTable GetPendingOrders()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string query = "SELECT OrderId, OrderType, TotalAmount, OrderDate FROM Orders WHERE OrderStatus = 'Pending' ORDER BY OrderDate ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 2. Clear an order out of the live queue once collected
        public static void CompleteOrder(int orderId)
        {
            string query = "UPDATE Orders SET OrderStatus = 'Completed' WHERE OrderId = @OrderId";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@OrderId", orderId)
            };
            ExecuteNonQuery(query, parameters);
        }
    }
}

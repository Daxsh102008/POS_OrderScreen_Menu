using Login_Registration;
using System.Configuration;
using RESTAU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payment__Software_
{
    public partial class Payment : Form
    {
       

        private void btn1_MouseEnter(object sender, EventArgs e)
        {
            btn1.BackColor = Color.MediumSpringGreen;
            btn1.ForeColor = Color.Black;
        }

        private void btn1_MouseLeave(object sender, EventArgs e)
        {
            btn1.BackColor = Color.White;
            btn1.ForeColor = Color.Black;
        }

        private void btn8_MouseEnter(object sender, EventArgs e)
        {
            btn8.BackColor = Color.MediumSpringGreen;
            btn8.ForeColor = Color.Black;
        }

        private void btn8_MouseLeave(object sender, EventArgs e)
        {
            btn8.BackColor = Color.White;
            btn8.ForeColor = Color.Black;
        }

        private void btn2_MouseEnter(object sender, EventArgs e)
        {
            btn2.BackColor = Color.MediumSpringGreen;
            btn2.ForeColor = Color.Black;
        }

        private void btn2_MouseLeave(object sender, EventArgs e)
        {
            btn2.BackColor = Color.White;
            btn2.ForeColor = Color.Black;
        }

        private void btn3_MouseEnter(object sender, EventArgs e)
        {
            btn3.BackColor = Color.MediumSpringGreen;
            btn3.ForeColor = Color.Black;
        }

        private void btn3_MouseLeave(object sender, EventArgs e)
        {
            btn3.BackColor = Color.White;
            btn3.ForeColor = Color.Black;
        }

        private void btn4_MouseEnter(object sender, EventArgs e)
        {
            btn4.BackColor = Color.MediumSpringGreen;
        }

        private void btn4_MouseLeave(object sender, EventArgs e)
        {
            btn4.BackColor = Color.White;
        }

        private void btn5_MouseEnter(object sender, EventArgs e)
        {
            btn5.BackColor = Color.MediumSpringGreen;
        }

        private void btn5_MouseLeave(object sender, EventArgs e)
        {
            btn5.BackColor = Color.White;
        }

        private void btn6_MouseEnter(object sender, EventArgs e)
        {
            btn6.BackColor = Color.MediumSpringGreen;
        }

        private void btn6_MouseLeave(object sender, EventArgs e)
        {
            btn6.BackColor = Color.White;
        }

        private void btn7_MouseEnter(object sender, EventArgs e)
        {
            btn7.BackColor = Color.MediumSpringGreen;
        }

        private void btn7_MouseLeave(object sender, EventArgs e)
        {
            btn7.BackColor = Color.White;
        }

        private void btn9_MouseEnter(object sender, EventArgs e)
        {
            btn9.BackColor = Color.MediumSpringGreen;
        }

        private void btn9_MouseLeave(object sender, EventArgs e)
        {
            btn9.BackColor = Color.White;
        }

        private void btn0_MouseEnter(object sender, EventArgs e)
        {
            btn0.BackColor = Color.MediumSpringGreen;
        }

        private void btn0_MouseLeave(object sender, EventArgs e)
        {
            btn0.BackColor = Color.White;
        }

        private void btn00_MouseEnter(object sender, EventArgs e)
        {
            btn00.BackColor = Color.MediumSpringGreen;
        }

        private void btn00_MouseLeave(object sender, EventArgs e)
        {
            btn00.BackColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       

        private void btnReceiptHis_MouseEnter(object sender, EventArgs e)
        {
            btnReceiptHis.BackColor = Color.White;
            
        }

        private void btnReceiptHis_MouseLeave(object sender, EventArgs e)
        {
            btnReceiptHis.BackColor = Color.Lime;
            
        }

       
        

        private string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;
        private List<string> orderItems;
        private decimal orderTotal;
        public bool PaymentComplete = false;
        private string orderType = "Table"; // Default

        public Payment(List<string> items, decimal total)
        {
            InitializeComponent();
            orderItems = items;
            orderTotal = total;

            // Display order total
            txtTotal.Text = "£" + orderTotal.ToString("F2");
            txtPaid.Text = "";
            txtChange.Text = "£0.00";

            // Load order into listbox
            lstOrder.Items.Clear();
            foreach (var item in orderItems)
                lstOrder.Items.Add(item);
        }

        // ── NUMPAD ──────────────────────────────────────────
        private void AppendToTotal(string value)
        {
            // Remove everything except digits
            string current = txtPaid.Text.Replace(".", "").Replace("£", "").TrimStart('0').Trim();

            current += value;

            // Pad to at least 3 digits
            current = current.PadLeft(3, '0');

            // Insert decimal 2 places from right
            string wholePart = current.Substring(0, current.Length - 2).TrimStart('0');
            string decimalPart = current.Substring(current.Length - 2);

            // Ensure whole part is never empty
            if (string.IsNullOrEmpty(wholePart)) wholePart = "0";

            txtPaid.Text = wholePart + "." + decimalPart;
            UpdateChange();
        }

        private void btn1_Click(object sender, EventArgs e) { AppendToTotal("1"); }
        private void btn2_Click(object sender, EventArgs e) { AppendToTotal("2"); }
        private void btn3_Click(object sender, EventArgs e) { AppendToTotal("3"); }
        private void btn4_Click(object sender, EventArgs e) { AppendToTotal("4"); }
        private void btn5_Click(object sender, EventArgs e) { AppendToTotal("5"); }
        private void btn6_Click(object sender, EventArgs e) { AppendToTotal("6"); }
        private void btn7_Click(object sender, EventArgs e) { AppendToTotal("7"); }
        private void btn8_Click(object sender, EventArgs e) { AppendToTotal("8"); }
        private void btn9_Click(object sender, EventArgs e) { AppendToTotal("9"); }
        private void btn0_Click(object sender, EventArgs e) { AppendToTotal("0"); }
        private void btn00_Click(object sender, EventArgs e) { AppendToTotal("00"); }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPaid.Text = "";
            txtChange.Text = "£0.00";
        }

        // ── QUICK CASH BUTTONS ───────────────────────────────
        private void btn5Pound_Click(object sender, EventArgs e) { SetPaid(5); }
        private void btn10Pound_Click(object sender, EventArgs e) { SetPaid(10); }
        private void btn20Pound_Click(object sender, EventArgs e) { SetPaid(20); }

        private void SetPaid(decimal amount)
        {
            decimal current = 0;
            decimal.TryParse(txtPaid.Text.Replace("£", ""), out current);
            txtPaid.Text = (current + amount).ToString("F2");
            UpdateChange();
        }

        private void UpdateChange()
        {
            if (decimal.TryParse(txtPaid.Text, out decimal paid))
            {
                decimal change = paid - orderTotal;
                txtChange.Text = change >= 0 ? "£" + change.ToString("F2") : "£0.00";
            }
            else
            {
                txtChange.Text = "£0.00";
            }
        }

        // ── ORDER TYPE ───────────────────────────────────────
        private void btnTakeaway_Click(object sender, EventArgs e)
        {
            orderType = "Takeaway";
            btnTakeaway.BackColor = System.Drawing.Color.Orange;
            MessageBox.Show("Order set to Takeaway.", "Order Type");
        }

        private bool ValidatePayment()
        {
            if (!decimal.TryParse(txtPaid.Text, out decimal paid))
            {
                MessageBox.Show("Please enter amount paid.", "No Amount");
                return false;
            }
            if (paid < orderTotal)
            {
                MessageBox.Show($"Amount paid is less than total.\nStill owed: £{(orderTotal - paid):F2}", "Insufficient Payment");
                return false;
            }
            return true;
        }

        private void BtnCash_Click(object sender, EventArgs e)
        {
            if (!ValidatePayment()) return;
            int orderID = SaveOrderToDB("Cash");
            if (orderID > 0)
            {
                PaymentComplete = true;
                ShowReceipt(orderID);
                this.DialogResult = DialogResult.OK;
                this.Close();

                Dashboard dashboard = new Dashboard();
                dashboard.Show();
            }
        }

        private void BtnCard_Click(object sender, EventArgs e)
        {
            txtPaid.Text = orderTotal.ToString("F2");
            txtChange.Text = "£0.00";

            int orderID = SaveOrderToDB("Card");
            if (orderID > 0)
            {
                PaymentComplete = true;
                ShowReceipt(orderID);
                this.DialogResult = DialogResult.OK;
                this.Close();

                Dashboard dashboard = new Dashboard();
                dashboard.Show();
            }
        }

        private void ShowReceipt(int orderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string orderQuery = @"SELECT OrderDate, TotalAmount, PaidAmount, 
                                         ChangeAmount, PaymentType, OrderType, Status 
                                  FROM Orders WHERE OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read()) return;

                            string date = Convert.ToDateTime(reader["OrderDate"]).ToString("dd/MM/yyyy");
                            string time = Convert.ToDateTime(reader["OrderDate"]).ToString("HH:mm:ss");
                            string total = "£" + Convert.ToDecimal(reader["TotalAmount"]).ToString("F2");
                            string paid = "£" + Convert.ToDecimal(reader["PaidAmount"]).ToString("F2");
                            string change = "£" + Convert.ToDecimal(reader["ChangeAmount"]).ToString("F2");
                            string payment = reader["PaymentType"].ToString();
                            string type = reader["OrderType"].ToString();
                            string status = reader["Status"].ToString();

                            reader.Close();

                            string line = "--------------------------------";
                            string receipt = "";

                            receipt += "          QUICKBITE POS\n";
                            receipt += "      Fast . Fresh . Delicious\n";
                            receipt += line + "\n";
                            receipt += $"Order No  : #{orderID}\n";
                            receipt += $"Date      : {date}\n";
                            receipt += $"Time      : {time}\n";
                            receipt += $"Type      : {type}\n";
                            receipt += $"Payment   : {payment}\n";
                            receipt += $"Status    : {status}\n";
                            receipt += line + "\n";
                            receipt += $"{"Item",-20} {"Qty",4}  {"Price",8}\n";
                            receipt += line + "\n";

                            string itemsQuery = @"SELECT ItemName, Quantity, UnitPrice, TotalPrice 
                                          FROM OrderItems WHERE OrderID = @OrderID";

                            using (SqlCommand itemCmd = new SqlCommand(itemsQuery, conn))
                            {
                                itemCmd.Parameters.AddWithValue("@OrderID", orderID);
                                using (SqlDataReader itemReader = itemCmd.ExecuteReader())
                                {
                                    while (itemReader.Read())
                                    {
                                        string name = itemReader["ItemName"].ToString();
                                        int qty = Convert.ToInt32(itemReader["Quantity"]);
                                        decimal unitPrice = Convert.ToDecimal(itemReader["UnitPrice"]);
                                        decimal lineTotal = Convert.ToDecimal(itemReader["TotalPrice"]);

                                        receipt += $"{name,-20} x{qty,-3} £{unitPrice:F2} = £{lineTotal:F2}\n";
                                    }
                                }
                            }

                            receipt += line + "\n";
                            receipt += $"{"TOTAL:",-20} {total,12}\n";
                            receipt += $"{"PAID:",-20} {paid,12}\n";
                            receipt += $"{"CHANGE:",-20} {change,12}\n";
                            receipt += line + "\n";
                            receipt += "      Thank you for your visit!\n";
                            receipt += "         Please come again!\n";

                            MessageBox.Show(receipt, $"Receipt - Order #{orderID}", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing receipt: {ex.Message}", "Error");
            }
        }

        // ── SAVE TO DATABASE ─────────────────────────────────
        private int SaveOrderToDB(string paymentType)
        {
            int orderID = 0;
            decimal paid = decimal.Parse(txtPaid.Text);
            decimal change = paid - orderTotal;
            if (change < 0) change = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // 1. Insert into Orders table
                        string orderQuery = @"INSERT INTO Orders 
                                         (TotalAmount, PaidAmount, ChangeAmount, PaymentType, OrderType) 
                                         VALUES (@Total, @Paid, @Change, @PaymentType, @OrderType);
                                         SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(orderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Total", orderTotal);
                            cmd.Parameters.AddWithValue("@Paid", paid);
                            cmd.Parameters.AddWithValue("@Change", change);
                            cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                            cmd.Parameters.AddWithValue("@OrderType", orderType);
                            orderID = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 2. Insert each item into OrderItems table
                        foreach (var item in orderItems)
                        {
                            // Parse: "Chicken Burger       x1   £4.50  =  £4.50"
                            string line = item.ToString();

                            string itemName = line.Substring(0, 20).Trim();

                            int xIdx = line.IndexOf(" x");
                            int firstPound = line.IndexOf("£");
                            int lastPound = line.LastIndexOf("£");

                            int qty = int.Parse(line.Substring(xIdx + 2, firstPound - xIdx - 2).Trim());
                            decimal unitPrice = decimal.Parse(line.Substring(firstPound + 1, lastPound - firstPound - 1).Trim().TrimEnd('=').Trim());
                            decimal totalPrice = decimal.Parse(line.Substring(lastPound + 1).Trim());

                            // Get ItemID from DB by name
                            int itemID = GetItemIDByName(itemName, conn, transaction);

                            string itemQuery = @"INSERT INTO OrderItems 
                                            (OrderID, ItemID, ItemName, Quantity, UnitPrice, TotalPrice)
                                            VALUES (@OrderID, @ItemID, @ItemName, @Qty, @UnitPrice, @TotalPrice)";

                            using (SqlCommand cmd = new SqlCommand(itemQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderID", orderID);
                                cmd.Parameters.AddWithValue("@ItemID", itemID);
                                cmd.Parameters.AddWithValue("@ItemName", itemName);
                                cmd.Parameters.AddWithValue("@Qty", qty);
                                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                                cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error saving order: {ex.Message}", "DB Error");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection Error: {ex.Message}", "Error");
                return 0;
            }

            return orderID;
        }

        private int GetItemIDByName(string itemName, SqlConnection conn, SqlTransaction transaction)
        {
            string query = "SELECT ItemID FROM MenuItems WHERE LTRIM(RTRIM(ItemName)) = LTRIM(RTRIM(@Name))";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Name", itemName);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void btnModifyOrder_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        // ── REFUND ───────────────────────────────────────────
        private void btnRefund_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Order ID to refund:", "Refund");
            if (int.TryParse(input, out int refundOrderID))
            {
                ProcessRefund(refundOrderID);
            }
        }

        private void ProcessRefund(int refundOrderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Orders SET Status = 'Refunded' WHERE OrderID = @OrderID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", refundOrderID);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show($"Order #{refundOrderID} has been refunded.", "Refund Success");
                        else
                            MessageBox.Show("Order ID not found.", "Refund Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Refund Error: {ex.Message}", "Error");
            }
        }


            private void btnReceiptHistory_Click(object sender, EventArgs e)
        {
            // Ask for order number
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter Order Number:", "Receipt History", "");

            if (string.IsNullOrEmpty(input)) return;

            if (!int.TryParse(input, out int orderID))
            {
                MessageBox.Show("Please enter a valid order number.", "Invalid Input");
                return;
            }

            string receipt = GetReceiptFromDB(orderID);

            if (receipt == null) return;

            // Show receipt in a simple MessageBox
            MessageBox.Show(receipt, $"Receipt - Order #{orderID}", MessageBoxButtons.OK);
        }

        private string GetReceiptFromDB(int orderID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get order header
                    string orderQuery = @"SELECT OrderDate, TotalAmount, PaidAmount, 
                                         ChangeAmount, PaymentType, OrderType, Status 
                                  FROM Orders WHERE OrderID = @OrderID";

                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show($"No order found with number: {orderID}", "Not Found");
                                return null;
                            }

                            string date = Convert.ToDateTime(reader["OrderDate"]).ToString("dd/MM/yyyy");
                            string time = Convert.ToDateTime(reader["OrderDate"]).ToString("HH:mm:ss");
                            string total = "£" + Convert.ToDecimal(reader["TotalAmount"]).ToString("F2");
                            string paid = "£" + Convert.ToDecimal(reader["PaidAmount"]).ToString("F2");
                            string change = "£" + Convert.ToDecimal(reader["ChangeAmount"]).ToString("F2");
                            string payment = reader["PaymentType"].ToString();
                            string type = reader["OrderType"].ToString();
                            string status = reader["Status"].ToString();

                            reader.Close();

                            // Get order items
                            string line = "--------------------------------";
                            string receipt = "";

                            receipt += "          QUICKBITE POS\n";
                            receipt += "      Fast . Fresh . Delicious\n";
                            receipt += line + "\n";
                            receipt += $"Order No  : #{orderID}\n";
                            receipt += $"Date      : {date}\n";
                            receipt += $"Time      : {time}\n";
                            receipt += $"Type      : {type}\n";
                            receipt += $"Payment   : {payment}\n";
                            receipt += $"Status    : {status}\n";
                            receipt += line + "\n";
                            receipt += $"{"Item",-20} {"Qty",4}  {"Price",8}\n";
                            receipt += line + "\n";

                            string itemsQuery = @"SELECT ItemName, Quantity, UnitPrice, TotalPrice 
                                          FROM OrderItems WHERE OrderID = @OrderID";

                            using (SqlCommand itemCmd = new SqlCommand(itemsQuery, conn))
                            {
                                itemCmd.Parameters.AddWithValue("@OrderID", orderID);
                                using (SqlDataReader itemReader = itemCmd.ExecuteReader())
                                {
                                    while (itemReader.Read())
                                    {
                                        string name = itemReader["ItemName"].ToString();
                                        int qty = Convert.ToInt32(itemReader["Quantity"]);
                                        decimal unitPrice = Convert.ToDecimal(itemReader["UnitPrice"]);
                                        decimal lineTotal = Convert.ToDecimal(itemReader["TotalPrice"]);

                                        receipt += $"{name,-20} x{qty,-3} £{unitPrice:F2} = £{lineTotal:F2}\n";
                                    }
                                }
                            }

                            receipt += line + "\n";
                            receipt += $"{"TOTAL:",-20} {total,12}\n";
                            receipt += $"{"PAID:",-20} {paid,12}\n";
                            receipt += $"{"CHANGE:",-20} {change,12}\n";
                            receipt += line + "\n";
                            receipt += "      Thank you for your visit!\n";
                            receipt += "         Please come again!\n";

                            return receipt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "DB Error");
                return null;
            }
        }

       

        
    }
    
}


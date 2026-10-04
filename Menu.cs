using Login_Registration;
using System.Configuration;
using Payment__Software_;
using RESTAU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_OrderScreen_Menu
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btnFriedChicken_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = false;
            btnChickenBurger.Visible = false;
            btnDoubleCheeseBurger.Visible = false;
            btnVegBurger.Visible = false;
            btnSpicyChickenBurger.Visible = false;
            btnBBQChickenPizza.Visible = false;
            btnChickenPizza.Visible = false;
            btnCheesePizza.Visible = false;
            btnPepperoniPizza.Visible = false;
            btnMargheritaPizza.Visible = false;
            btnVegPizza.Visible = false;
            btnChickenWings.Visible = true;
            btnChickenNuggets.Visible = true;
            btnCrispyStrip.Visible = true;
            btnOnionRings.Visible = false;
            btnFries.Visible = false;
            btnCocaCola.Visible = false;
            btnSprite.Visible = false;
            btnFanta.Visible = false;
            btnWaterbottle.Visible = false;
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;
        // Call this method when an item is added to the order
        private void AddItemToOrder(string itemName, decimal price, int quantity)
        {
            for (int i = 0; i < lstOrder.Items.Count; i++)
            {
                string existingItem = lstOrder.Items[i].ToString();

                if (existingItem.TrimStart().StartsWith(itemName.Trim()))
                {
                    // Parse qty: the number after "x"
                    int xIdx = existingItem.IndexOf(" x");
                    int poundIdx = existingItem.IndexOf("£");

                    if (xIdx >= 0 && poundIdx >= 0)
                    {
                        string qtyStr = existingItem.Substring(xIdx + 2, poundIdx - xIdx - 2).Trim();
                        if (int.TryParse(qtyStr, out int existingQty))
                        {
                            int newQty = existingQty + quantity;
                            decimal newTotal = price * newQty;
                            lstOrder.Items[i] = FormatOrderItem(itemName, newQty, price, newTotal);
                            UpdateTotals();
                            return;
                        }
                    }
                }
            }

            // New item
            decimal totalPrice = price * quantity;
            lstOrder.Items.Add(FormatOrderItem(itemName, quantity, price, totalPrice));
            UpdateTotals();
        }

           
        // Formats the display string for lstOrder
        private string FormatOrderItem(string name, int qty, decimal unitPrice, decimal totalPrice)
        {
            return $"{name,-20} x{qty,-4} £{unitPrice:F2}  =  £{totalPrice:F2}";
        }

        // Updates Total, Paid, and Change fields
        private void UpdateTotals()
        {
            decimal total = 0;

            foreach (var item in lstOrder.Items)
            {
                string line = item.ToString();
                int idx = line.LastIndexOf("£");
                if (idx >= 0)
                {
                    string totalPart = line.Substring(idx + 1).Trim();
                    if (decimal.TryParse(totalPart, out decimal lineTotal))
                        total += lineTotal;
                }
            }

            txtTotal.Text = "£" + total.ToString("F2");

            if (decimal.TryParse(txtPaid.Text, out decimal paid))
            {
                decimal change = paid - total;
                txtChange.Text = change >= 0 ? "£" + change.ToString("F2") : "£0.00";
            }
            else
            {
                txtChange.Text = "£0.00";
            }
        }

        // Gets item price from database when a menu button is clicked
        private decimal GetItemPriceFromDB(int itemID)
        {
            decimal price = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Price FROM MenuItems WHERE ItemID = @ItemID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemID);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                            price = Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB Error: {ex.Message}", "Error");
            }

            return price;
        }

        // Hook this up to each menu image/button click
        // Example: called when "Chicken Burger" button is clicked
        private void btnChicken_Click(object sender, EventArgs e)
        {

        }

        // Use X2, X3, X4, X6 buttons to set quantity multiplier
        private int selectedQuantity = 1;

        private void btnX2_Click(object sender, EventArgs e) { selectedQuantity = 2; MessageBox.Show("Quantity set to 2"); }
        private void btnX3_Click(object sender, EventArgs e) { selectedQuantity = 3; MessageBox.Show("Quantity set to 3"); }
        private void btnX4_Click(object sender, EventArgs e) { selectedQuantity = 4; MessageBox.Show("Quantity set to 4"); }
        private void btnX6_Click(object sender, EventArgs e) { selectedQuantity = 6; MessageBox.Show("Quantity set to 6"); }

        // Updated menu button click using selectedQuantity


        private void btnChickenBurger_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chicken Burger", _menuPrices[1], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnCheeseBurger_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Cheese Burger", _menuPrices[2], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnDoubleCheese_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Double Cheese", _menuPrices[3], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnVegBurger_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Veg Burger", _menuPrices[4], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnSpicyChicken_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Spicy Chicken Burger", _menuPrices[5], selectedQuantity);
            selectedQuantity = 1;
        }

        // Pizzas
        private void btnChickenPizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chicken Pizza", _menuPrices[6], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnCheesePizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Cheese Pizza", _menuPrices[7], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnPepperoniPizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Pepperoni Pizza", _menuPrices[8], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnBBQChickenPizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("BBQ Chicken Pizza", _menuPrices[9], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnMargherettaPizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Margheretta Pizza", _menuPrices[10], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnVegPizza_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Veg Pizza", _menuPrices[11], selectedQuantity);
            selectedQuantity = 1;
        }

        // Sides
        private void btnChrisyStrip_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chrispy Strip", _menuPrices[12], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnChickenNuggets_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chicken Nuggets", _menuPrices[13], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnChickenWings_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Chicken Wings", _menuPrices[14], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnFries_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Fries", _menuPrices[15], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnOnionRings_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Onion Rings", _menuPrices[16], selectedQuantity);
            selectedQuantity = 1;
        }

        // Drinks
        private void btnCocaCola_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Coca-Cola", _menuPrices[17], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnSprite_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Sprite", _menuPrices[18], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnFanta_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Fanta", _menuPrices[19], selectedQuantity);
            selectedQuantity = 1;
        }

        private void btnWaterBottle_Click(object sender, EventArgs e)
        {
            AddItemToOrder("Water Bottle", _menuPrices[20], selectedQuantity);
            selectedQuantity = 1;
        }

        // Remove selected item from lstOrder
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lstOrder.SelectedIndex >= 0)
            {
                lstOrder.Items.RemoveAt(lstOrder.SelectedIndex);
                UpdateTotals();
            }
        }

        // Clear entire order
        private void btnClearOrder_Click(object sender, EventArgs e)
        {
            lstOrder.Items.Clear();
            txtTotal.Text = "0.00";
            txtChange.Text = "0.00";
        }

        private void btnFUNC_Click(object sender, EventArgs e)
        {
            if (lstOrder.Items.Count == 0) { MessageBox.Show("No items in order.", "Empty Order"); return; }

            List<string> orderItems = new List<string>();
            foreach (var item in lstOrder.Items)
                orderItems.Add(item.ToString());

            decimal total = decimal.Parse(txtTotal.Text.Replace("£", "").Trim());

            Payment paymentForm = new Payment(orderItems, total);
            DialogResult result = paymentForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                
                

                // ── Clear the table in DB ──
                string clearQuery = "UPDATE TableRestau SET Status = 0, TotalAmount = 0.00, TimeWaited = NULL WHERE TableId = @TableId";
                DatabaseFunction.ExecuteNonQuery(clearQuery, new SqlParameter[] { new SqlParameter("@TableId", CurrentSession.SelectedTableId) });

                // ── Clear the UI ──
                lstOrder.Items.Clear();
                txtTotal.Text = "£0.00";
                txtPaid.Text = "0.00";
                txtChange.Text = "£0.00";

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSTable_Click(object sender, EventArgs e)
        {
            if (lstOrder.Items.Count == 0)
            {
                MessageBox.Show("No items in order to save.", "Empty Order");
                return;
            }

            try
            {
                decimal currentBillTotal = decimal.Parse(txtTotal.Text.Replace("£", "").Trim());

                string query = @"UPDATE TableRestau 
                         SET Status = 1, 
                             TotalAmount = @Total, 
                             TimeWaited = @Time 
                         WHERE TableId = @TableId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Total", currentBillTotal);
                        cmd.Parameters.AddWithValue("@Time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TableId", CurrentSession.SelectedTableId);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows == 0)
                        {
                            MessageBox.Show(
                                $"Table {CurrentSession.SelectedTableId} not found.",
                                "Not Found");
                            return;
                        }
                    }
                }

                    
                this.Hide();

                Dashboard dashboardForm = new Dashboard();
                dashboardForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            LoadMenuPrices();
        }

        private Dictionary<int, decimal> _menuPrices = new Dictionary<int, decimal>();
        private void LoadMenuPrices()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ItemID, Price FROM MenuItems", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        _menuPrices[Convert.ToInt32(reader["ItemID"])] = Convert.ToDecimal(reader["Price"]);
                }
            }
        }

        private void btnReturnTD_Click(object sender, EventArgs e)
        {
            //Return to table selection without saving
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            Dashboard dashboardForm = new Dashboard();
            dashboardForm.ShowDialog();
        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            this.Hide();

            Login loginForm = new Login();
            loginForm.ShowDialog();

        }

        private void btnAllItems_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = true;
            btnChickenBurger.Visible = true;
            btnDoubleCheeseBurger.Visible = true;
            btnVegBurger.Visible = true;
            btnSpicyChickenBurger.Visible = true;
            btnBBQChickenPizza.Visible = true;
            btnChickenPizza.Visible = true;
            btnCheesePizza.Visible = true;
            btnPepperoniPizza.Visible = true;
            btnMargheritaPizza.Visible = true;
            btnVegPizza.Visible = true;
            btnChickenWings.Visible = true;
            btnChickenNuggets.Visible = true;
            btnCrispyStrip.Visible = true;
            btnOnionRings.Visible = true;
            btnFries.Visible = true;
            btnCocaCola.Visible = true;
            btnSprite.Visible = true;
            btnFanta.Visible = true;
            btnWaterbottle.Visible = true;
        }

        private void btnBurger_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = true;
            btnChickenBurger.Visible = true;
            btnDoubleCheeseBurger.Visible = true;
            btnVegBurger.Visible = true;
            btnSpicyChickenBurger.Visible = true;
            btnBBQChickenPizza.Visible = false;
            btnChickenPizza.Visible = false;
            btnCheesePizza.Visible = false;
            btnPepperoniPizza.Visible = false;
            btnMargheritaPizza.Visible = false;
            btnVegPizza.Visible = false;
            btnChickenWings.Visible = false;
            btnChickenNuggets.Visible = false;
            btnCrispyStrip.Visible = false;
            btnOnionRings.Visible = false;
            btnFries.Visible = false;
            btnCocaCola.Visible = false;
            btnSprite.Visible = false;
            btnFanta.Visible = false;
            btnWaterbottle.Visible = false;
        }

        private void btnPizza_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = false;
            btnChickenBurger.Visible = false;
            btnDoubleCheeseBurger.Visible = false;
            btnVegBurger.Visible = false ;
            btnSpicyChickenBurger.Visible = false;
            btnBBQChickenPizza.Visible = true;
            btnChickenPizza.Visible = true;
            btnCheesePizza.Visible = true;
            btnPepperoniPizza.Visible = true;
            btnMargheritaPizza.Visible = true;
            btnVegPizza.Visible = true;
            btnChickenWings.Visible = false;
            btnChickenNuggets.Visible = false;
            btnCrispyStrip.Visible = false;
            btnOnionRings.Visible = false;
            btnFries.Visible = false;
            btnCocaCola.Visible = false;
            btnSprite.Visible = false;
            btnFanta.Visible = false;
            btnWaterbottle.Visible = false;
        }

        private void btnSides_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = false;
            btnChickenBurger.Visible = false;
            btnDoubleCheeseBurger.Visible = false;
            btnVegBurger.Visible = false;
            btnSpicyChickenBurger.Visible = false;
            btnBBQChickenPizza.Visible = false;
            btnChickenPizza.Visible = false;
            btnCheesePizza.Visible = false;
            btnPepperoniPizza.Visible = false;
            btnMargheritaPizza.Visible = false;
            btnVegPizza.Visible = false;
            btnChickenWings.Visible = false;
            btnChickenNuggets.Visible = false;
            btnCrispyStrip.Visible = false;
            btnOnionRings.Visible = true;
            btnFries.Visible = true;
            btnCocaCola.Visible = false;
            btnSprite.Visible = false;
            btnFanta.Visible = false;
            btnWaterbottle.Visible = false;
        }

        private void btnDrinks_Click(object sender, EventArgs e)
        {
            btnCheeseBurger.Visible = false;
            btnChickenBurger.Visible = false;
            btnDoubleCheeseBurger.Visible = false;
            btnVegBurger.Visible = false;
            btnSpicyChickenBurger.Visible = false;
            btnBBQChickenPizza.Visible = false;
            btnChickenPizza.Visible = false;
            btnCheesePizza.Visible = false;
            btnPepperoniPizza.Visible = false;
            btnMargheritaPizza.Visible = false;
            btnVegPizza.Visible = false;
            btnChickenWings.Visible = false;
            btnChickenNuggets.Visible = false;
            btnCrispyStrip.Visible = false;
            btnOnionRings.Visible = false;
            btnFries.Visible = false;
            btnCocaCola.Visible = true;
            btnSprite.Visible = true;
            btnFanta.Visible = true;
            btnWaterbottle.Visible = true;
        }
    }

}

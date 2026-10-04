using Login_Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace RESTAU
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            this.Load += Dashboard_Load;
        }

        // ── LOAD ──────────────────────────────────────────
        private void Dashboard_Load(object sender, EventArgs e)
        {
            trackTimer.Interval = 1000;
            trackTimer.Start();
            trackTimer.Tick += trackTimer_Tick;
            RefreshFromDB();
        }

        // ── TABLE BUTTON CLICK ────────────────────────────
        private void TableButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Tag == null || !int.TryParse(clickedButton.Tag.ToString(), out int tableId))
            {
                MessageBox.Show("Error identifying table number.", "Error");
                return;
            }

            CurrentSession.SelectedTableId = tableId;
            var (status, total, seatedTime) = DatabaseFunction.GetTableState(tableId);

            if (status == false)
            {
                using (POS_OrderScreen_Menu.Menu orderScreen = new POS_OrderScreen_Menu.Menu())
                {
                    this.Hide();
                    orderScreen.ShowDialog();
                    RefreshFromDB(); // ← refresh cache after menu closes
                }
            }
            else
            {
                string message = $"Table {tableId} currently has a balance of £{total:F2}.\nDo you want to settle the bill and clear this table?";
                if (MessageBox.Show(message, "Settle Bill", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Ask payment type
                    string paymentType = "Cash"; // default
                    DialogResult cardResult = MessageBox.Show("Was payment by card?", "Payment Type", MessageBoxButtons.YesNo);
                    if (cardResult == DialogResult.Yes) paymentType = "Card";

                    // Get order items from cached state isn't enough — you need the item lines
                    // So fetch them from OrderItems where they were saved, OR pass them from Menu
                    // Simplest: just save a summary order with the total
                    DatabaseFunction.SaveDineInOrder(total, total, 0, paymentType, new List<string>());

                    // Clear the table
                    string updateQuery = "UPDATE TableRestau SET Status = 0, TotalAmount = 0.00, TimeWaited = NULL WHERE TableId = @TableId";
                    DatabaseFunction.ExecuteNonQuery(updateQuery, new SqlParameter[] { new SqlParameter("@TableId", tableId) });

                    MessageBox.Show($"Table {tableId} has been cleared.", "Table Cleared");
                    RefreshFromDB();
                    
                }
            }
        }

        // ── UPDATE BUTTONS ────────────────────────────────
        private void UpdateButtonTab()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button btn && btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int tableId))
                {
                    if (_cachedTableStates.TryGetValue(tableId, out var data))
                    {
                        if (!data.Status)
                        {
                            btn.BackColor = Color.Gray;
                            btn.Text = $"Table {tableId}\n£0.00";
                        }
                        else
                        {
                            btn.BackColor = Color.Yellow;
                            string time = data.SeatedTime.HasValue
                                ? (DateTime.Now - data.SeatedTime.Value).ToString(@"hh\:mm\:ss")
                                : "00:00:00";
                            btn.Text = $"Table {tableId}\n£{data.Total:F2}\n{time}";
                        }
                        btn.ForeColor = Color.Black;
                    }
                }
            }
        }

        // ── TIMER TICK ────────────────────────────────────
        private Dictionary<int, (bool Status, decimal Total, DateTime? SeatedTime)> _cachedTableStates = new Dictionary<int, (bool Status, decimal Total, DateTime? SeatedTime)>();

        private void trackTimer_Tick(object sender, EventArgs e)
        {
            // Just update the displayed time using cached data — no DB call
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button btn && btn.Tag != null && int.TryParse(btn.Tag.ToString(), out int tableId))
                {
                    if (_cachedTableStates.TryGetValue(tableId, out var data) && data.Status && data.SeatedTime.HasValue)
                    {
                        TimeSpan duration = DateTime.Now - data.SeatedTime.Value;
                        btn.Text = $"Table {tableId}\n£{data.Total:F2}\n{duration:hh\\:mm\\:ss}";
                    }
                }
            }
        }

        private void RefreshFromDB()
        {
            _cachedTableStates = DatabaseFunction.GetAllTableStates();
            UpdateButtonTab(); // uses the cache
        }

        // ── REST OF YOUR BUTTONS ──────────────────────────

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private int managerAuthAttempt = 0;
        private void btnManagerFxn_Click(object sender, EventArgs e)
        {
            using (PasscodeVerify prompt = new PasscodeVerify())
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    string enteredcode = prompt.EnteredPasscode;

                    if (VerifyManagerPasscode(enteredcode))
                    {
                        managerAuthAttempt = 0;
                        MessageBox.Show("Access granted. Welcome, manager!", "Access Granted");
                        using (ManagerPanel managerForm = new ManagerPanel())
                        {
                            managerForm.ShowDialog();
                            this.Hide();
                        }
                    }
                    else
                    {
                        managerAuthAttempt++;
                        if (managerAuthAttempt >= 3)
                        {
                            MessageBox.Show("Too many failed attempts. Returning to login.", "Security Alert");
                            managerAuthAttempt = 0;
                            this.Hide();
                            Login loginf = new Login();
                            loginf.Show();

                        }
                        else
                        {
                            int attemptsLeft = 3 - managerAuthAttempt;
                            MessageBox.Show($"Incorrect passcode. {attemptsLeft} attempt(s) left.", "Warning");
                        }
                    }
                }
            }
        }

        // The manager passcode lives in the database (AppSettings, stored hashed)
        // rather than in this file. Returns false on any DB problem so a failure
        // can never be mistaken for a successful login.
        private bool VerifyManagerPasscode(string entered)
        {
            try
            {
                return DatabaseFunction.VerifyManagerPasscode(entered);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not check the manager passcode: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnOrderHistory_Click(object sender, EventArgs e)
        {
            
            using (PasscodeVerify prompt = new PasscodeVerify())
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    string enteredcode = prompt.EnteredPasscode;

                    if (VerifyManagerPasscode(enteredcode))
                    {
                        managerAuthAttempt = 0;
                        MessageBox.Show("Access granted. Welcome, manager!", "Access Granted");
                        OrderHistory managerForm = new OrderHistory();
                        managerForm.ShowDialog();
                        
                    }
                    else
                    {
                        managerAuthAttempt++;
                        if (managerAuthAttempt >= 3)
                        {
                            MessageBox.Show("Too many failed attempts. Returning to login.", "Security Alert");
                            managerAuthAttempt = 0;
                            this.Hide();
                            Login loginf = new Login();
                            loginf.Show();
                        }
                        else
                        {
                            int attemptsLeft = 3 - managerAuthAttempt;
                            MessageBox.Show($"Incorrect passcode. {attemptsLeft} attempt(s) left.", "Warning");
                        }
                    }
                }
            }
        }
    }
}
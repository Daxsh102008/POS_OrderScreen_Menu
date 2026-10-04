using RESTAU;
using System.Configuration;
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

namespace Login_Registration
{
    public partial class Login : Form
    {
        

        public Login()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registration = new RegisterForm();
            registration.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both your username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string connectionString = ConfigurationManager.ConnectionStrings["PosDb"].ConnectionString;

         
            // Passwords are stored as salted PBKDF2 hashes, so the comparison cannot happen
            // in SQL. Fetch the stored hash for this username and verify it here.
            string query = "SELECT password FROM Users WHERE username = @Username";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());

                        connection.Open();

                        object storedPassword = command.ExecuteScalar();

                        bool authenticated =
                            storedPassword != null &&
                            storedPassword != DBNull.Value &&
                            PasswordHasher.Verify(txtPassword.Text, Convert.ToString(storedPassword));

                        if (authenticated)
                        {
                            MessageBox.Show("Login successful! Welcome.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Dashboard dashboard = new Dashboard();
                            this.Hide();
                            dashboard.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

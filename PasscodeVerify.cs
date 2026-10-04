using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RESTAU
{
    public partial class PasscodeVerify : Form
    {
        public PasscodeVerify()
        {
            InitializeComponent();
        }
        public string EnteredPasscode { get; private set; }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            //Save the entered passcode to a property
            EnteredPasscode = txtPasscode.Text;
            this.DialogResult = DialogResult.OK; // Indicate that the passcode was entered and verified
            this.Close();
        }

        private void PasscodeVerify_Load(object sender, EventArgs e)
        {
            //Drops cursor in the passcode textbox when the form loads
            txtPasscode.Focus();
        }

        private void btnReturnTD_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Indicate that the user wants to return to the table display
            this.Close();
        }
    }
}

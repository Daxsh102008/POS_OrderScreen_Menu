namespace RESTAU
{
    partial class PasscodeVerify
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasscodeVerify));
            this.label1 = new System.Windows.Forms.Label();
            this.txtPasscode = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnReturnTD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(51, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(486, 76);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manager Authentication\r\nPlease enter the reserved password:\r\n";
            // 
            // txtPasscode
            // 
            this.txtPasscode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.txtPasscode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPasscode.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasscode.ForeColor = System.Drawing.Color.White;
            this.txtPasscode.Location = new System.Drawing.Point(58, 178);
            this.txtPasscode.Name = "txtPasscode";
            this.txtPasscode.Size = new System.Drawing.Size(464, 45);
            this.txtPasscode.TabIndex = 1;
            this.txtPasscode.UseSystemPasswordChar = true;
            // 
            // btnVerify
            // 
            this.btnVerify.BackColor = System.Drawing.Color.DarkOrange;
            this.btnVerify.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerify.FlatAppearance.BorderSize = 0;
            this.btnVerify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerify.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Location = new System.Drawing.Point(58, 273);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(156, 48);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Verify Code";
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnReturnTD
            // 
            this.btnReturnTD.BackColor = System.Drawing.Color.DarkRed;
            this.btnReturnTD.FlatAppearance.BorderSize = 0;
            this.btnReturnTD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturnTD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturnTD.ForeColor = System.Drawing.Color.White;
            this.btnReturnTD.Location = new System.Drawing.Point(389, 256);
            this.btnReturnTD.Name = "btnReturnTD";
            this.btnReturnTD.Size = new System.Drawing.Size(212, 74);
            this.btnReturnTD.TabIndex = 3;
            this.btnReturnTD.Text = "Cancel and return to the dashboard";
            this.btnReturnTD.UseVisualStyleBackColor = false;
            this.btnReturnTD.Click += new System.EventHandler(this.btnReturnTD_Click);
            // 
            // PasscodeVerify
            // 
            this.AcceptButton = this.btnVerify;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(653, 360);
            this.Controls.Add(this.btnReturnTD);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.txtPasscode);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PasscodeVerify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Admin Authentication";
            this.Load += new System.EventHandler(this.PasscodeVerify_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPasscode;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnReturnTD;
    }
}
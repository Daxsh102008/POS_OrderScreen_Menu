namespace RESTAU
{
    partial class ManagerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerPanel));
            this.dgvMenu = new System.Windows.Forms.DataGridView();
            this.btnUpdatePriceM = new System.Windows.Forms.Button();
            this.txtPriceUpdate = new System.Windows.Forms.TextBox();
            this.pnlFinances = new System.Windows.Forms.Panel();
            this.btnViewAcc = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlMenuEditor = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).BeginInit();
            this.pnlFinances.SuspendLayout();
            this.pnlMenuEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMenu
            // 
            this.dgvMenu.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.dgvMenu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMenu.EnableHeadersVisualStyles = false;
            this.dgvMenu.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(49)))), ((int)(((byte)(67)))));
            this.dgvMenu.Location = new System.Drawing.Point(3, 64);
            this.dgvMenu.Name = "dgvMenu";
            this.dgvMenu.RowHeadersVisible = false;
            this.dgvMenu.RowHeadersWidth = 62;
            this.dgvMenu.RowTemplate.Height = 28;
            this.dgvMenu.Size = new System.Drawing.Size(530, 294);
            this.dgvMenu.TabIndex = 0;
            this.dgvMenu.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMenu_CellContentClick);
            // 
            // btnUpdatePriceM
            // 
            this.btnUpdatePriceM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(116)))), ((int)(((byte)(19)))));
            this.btnUpdatePriceM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdatePriceM.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdatePriceM.ForeColor = System.Drawing.Color.White;
            this.btnUpdatePriceM.Location = new System.Drawing.Point(134, 447);
            this.btnUpdatePriceM.Name = "btnUpdatePriceM";
            this.btnUpdatePriceM.Size = new System.Drawing.Size(186, 52);
            this.btnUpdatePriceM.TabIndex = 1;
            this.btnUpdatePriceM.Text = "Update Price";
            this.btnUpdatePriceM.UseVisualStyleBackColor = false;
            this.btnUpdatePriceM.Click += new System.EventHandler(this.btnUpdatePriceM_Click);
            // 
            // txtPriceUpdate
            // 
            this.txtPriceUpdate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriceUpdate.Location = new System.Drawing.Point(259, 383);
            this.txtPriceUpdate.Name = "txtPriceUpdate";
            this.txtPriceUpdate.Size = new System.Drawing.Size(150, 34);
            this.txtPriceUpdate.TabIndex = 2;
            // 
            // pnlFinances
            // 
            this.pnlFinances.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.pnlFinances.Controls.Add(this.btnViewAcc);
            this.pnlFinances.Controls.Add(this.btnReturn);
            this.pnlFinances.Controls.Add(this.label2);
            this.pnlFinances.Controls.Add(this.lblTotalSales);
            this.pnlFinances.Controls.Add(this.label1);
            this.pnlFinances.Location = new System.Drawing.Point(12, 12);
            this.pnlFinances.Name = "pnlFinances";
            this.pnlFinances.Size = new System.Drawing.Size(459, 524);
            this.pnlFinances.TabIndex = 3;
            // 
            // btnViewAcc
            // 
            this.btnViewAcc.BackColor = System.Drawing.Color.Turquoise;
            this.btnViewAcc.FlatAppearance.BorderSize = 0;
            this.btnViewAcc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewAcc.ForeColor = System.Drawing.Color.White;
            this.btnViewAcc.Location = new System.Drawing.Point(20, 425);
            this.btnViewAcc.Name = "btnViewAcc";
            this.btnViewAcc.Size = new System.Drawing.Size(114, 60);
            this.btnViewAcc.TabIndex = 5;
            this.btnViewAcc.Text = "View Accounts";
            this.btnViewAcc.UseVisualStyleBackColor = false;
            this.btnViewAcc.Click += new System.EventHandler(this.btnViewAcc_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.DarkRed;
            this.btnReturn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.ForeColor = System.Drawing.Color.White;
            this.btnReturn.Location = new System.Drawing.Point(260, 425);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(173, 60);
            this.btnReturn.TabIndex = 4;
            this.btnReturn.Text = "Return to Dashboard";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(268, 114);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total Sales \r\nrecorded across all \r\nterminals:";
            // 
            // lblTotalSales
            // 
            this.lblTotalSales.AutoSize = true;
            this.lblTotalSales.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalSales.ForeColor = System.Drawing.Color.Orange;
            this.lblTotalSales.Location = new System.Drawing.Point(11, 285);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = new System.Drawing.Size(257, 54);
            this.lblTotalSales.TabIndex = 1;
            this.lblTotalSales.Text = "lblTotalSales";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(148)))), ((int)(((byte)(165)))));
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Financial Overview";
            // 
            // pnlMenuEditor
            // 
            this.pnlMenuEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(34)))), ((int)(((byte)(48)))));
            this.pnlMenuEditor.Controls.Add(this.label4);
            this.pnlMenuEditor.Controls.Add(this.label3);
            this.pnlMenuEditor.Controls.Add(this.dgvMenu);
            this.pnlMenuEditor.Controls.Add(this.txtPriceUpdate);
            this.pnlMenuEditor.Controls.Add(this.btnUpdatePriceM);
            this.pnlMenuEditor.Location = new System.Drawing.Point(477, 12);
            this.pnlMenuEditor.Name = "pnlMenuEditor";
            this.pnlMenuEditor.Size = new System.Drawing.Size(533, 524);
            this.pnlMenuEditor.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(28, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(225, 32);
            this.label4.TabIndex = 4;
            this.label4.Text = "List of Food Items:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(29, 361);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 56);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter new \r\nitem value(£):";
            // 
            // ManagerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(24)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1022, 548);
            this.Controls.Add(this.pnlMenuEditor);
            this.Controls.Add(this.pnlFinances);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManagerPanel";
            this.Text = "Management Hub";
            this.Load += new System.EventHandler(this.ManagerPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMenu)).EndInit();
            this.pnlFinances.ResumeLayout(false);
            this.pnlFinances.PerformLayout();
            this.pnlMenuEditor.ResumeLayout(false);
            this.pnlMenuEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMenu;
        private System.Windows.Forms.Button btnUpdatePriceM;
        private System.Windows.Forms.TextBox txtPriceUpdate;
        private System.Windows.Forms.Panel pnlFinances;
        private System.Windows.Forms.Panel pnlMenuEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnViewAcc;
        private System.Windows.Forms.Label label4;
    }
}
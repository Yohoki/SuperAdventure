
namespace SuperAdventure
{
    partial class TradingScreen
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
            this.lblMyInventory = new System.Windows.Forms.Label();
            this.lblVendorInventory = new System.Windows.Forms.Label();
            this.dgvMyInventory = new System.Windows.Forms.DataGridView();
            this.dgvVendorInventory = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMyInventory
            // 
            this.lblMyInventory.AutoSize = true;
            this.lblMyInventory.Location = new System.Drawing.Point(99, 13);
            this.lblMyInventory.Name = "lblMyInventory";
            this.lblMyInventory.Size = new System.Drawing.Size(94, 20);
            this.lblMyInventory.TabIndex = 0;
            this.lblMyInventory.Text = "My Inventory";
            // 
            // lblVendorInventory
            // 
            this.lblVendorInventory.AutoSize = true;
            this.lblVendorInventory.Location = new System.Drawing.Point(335, 13);
            this.lblVendorInventory.Name = "lblVendorInventory";
            this.lblVendorInventory.Size = new System.Drawing.Size(121, 20);
            this.lblVendorInventory.TabIndex = 1;
            this.lblVendorInventory.Text = "Vendor Inventory";
            // 
            // dgvMyInventory
            // 
            this.dgvMyInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyInventory.Location = new System.Drawing.Point(13, 43);
            this.dgvMyInventory.Name = "dgvMyInventory";
            this.dgvMyInventory.RowHeadersWidth = 51;
            this.dgvMyInventory.RowTemplate.Height = 29;
            this.dgvMyInventory.Size = new System.Drawing.Size(240, 216);
            this.dgvMyInventory.TabIndex = 2;
            // 
            // dgvVendorInventory
            // 
            this.dgvVendorInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendorInventory.Location = new System.Drawing.Point(276, 43);
            this.dgvVendorInventory.Name = "dgvVendorInventory";
            this.dgvVendorInventory.RowHeadersWidth = 51;
            this.dgvVendorInventory.RowTemplate.Height = 29;
            this.dgvVendorInventory.Size = new System.Drawing.Size(240, 216);
            this.dgvVendorInventory.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(441, 265);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TradingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 302);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvVendorInventory);
            this.Controls.Add(this.dgvMyInventory);
            this.Controls.Add(this.lblVendorInventory);
            this.Controls.Add(this.lblMyInventory);
            this.Name = "TradingScreen";
            this.Text = "Trade";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMyInventory;
        private System.Windows.Forms.Label lblVendorInventory;
        private System.Windows.Forms.DataGridView dgvMyInventory;
        private System.Windows.Forms.DataGridView dgvVendorInventory;
        private System.Windows.Forms.Button btnClose;
    }
}
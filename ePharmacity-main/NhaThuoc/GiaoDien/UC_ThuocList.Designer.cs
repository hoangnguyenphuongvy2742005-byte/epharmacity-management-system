namespace NhaThuoc
{
    partial class UC_ThuocList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.dgvThuocList = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuocList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimKiem.Location = new System.Drawing.Point(20, 18);
            this.lblTimKiem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(91, 20);
            this.lblTimKiem.TabIndex = 0;
            this.lblTimKiem.Text = "Tìm kiếm:";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTimKiem.Location = new System.Drawing.Point(133, 15);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(399, 26);
            this.txtTimKiem.TabIndex = 1;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            // 
            // dgvThuocList
            // 
            this.dgvThuocList.AllowUserToAddRows = false;
            this.dgvThuocList.AllowUserToDeleteRows = false;
            this.dgvThuocList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvThuocList.BackgroundColor = System.Drawing.Color.White;
            this.dgvThuocList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThuocList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThuocList.Location = new System.Drawing.Point(0, 0);
            this.dgvThuocList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvThuocList.MultiSelect = false;
            this.dgvThuocList.Name = "dgvThuocList";
            this.dgvThuocList.ReadOnly = true;
            this.dgvThuocList.RowHeadersVisible = false;
            this.dgvThuocList.RowHeadersWidth = 51;
            this.dgvThuocList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThuocList.Size = new System.Drawing.Size(1333, 492);
            this.dgvThuocList.TabIndex = 0;
            this.dgvThuocList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThuocList_CellClick);
            this.dgvThuocList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThuocList_CellContentClick);
            this.dgvThuocList.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvThuocList_RowPrePaint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvThuocList);
            this.panel1.Location = new System.Drawing.Point(20, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1333, 492);
            this.panel1.TabIndex = 2;
            // 
            // UC_ThuocList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtTimKiem);
            this.Controls.Add(this.lblTimKiem);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UC_ThuocList";
            this.Size = new System.Drawing.Size(1373, 578);
            this.Load += new System.EventHandler(this.UC_ThuocList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThuocList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.DataGridView dgvThuocList;
        private System.Windows.Forms.Panel panel1;
    }
}

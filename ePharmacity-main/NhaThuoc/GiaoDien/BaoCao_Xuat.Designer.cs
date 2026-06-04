namespace NhaThuoc
{
    partial class BaoCao_Xuat
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoTongHop = new System.Windows.Forms.RadioButton();
            this.rdoTheoNhom = new System.Windows.Forms.RadioButton();
            this.rdoSapHetHan = new System.Windows.Forms.RadioButton();
            this.rdoTonKho = new System.Windows.Forms.RadioButton();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnXuatPDF = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.dgvBaoCao = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lblTieuDe);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 123);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoTongHop);
            this.groupBox1.Controls.Add(this.rdoTheoNhom);
            this.groupBox1.Controls.Add(this.rdoSapHetHan);
            this.groupBox1.Controls.Add(this.rdoTonKho);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(27, 55);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(800, 62);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn loại báo cáo:";
            // 
            // rdoTongHop
            // 
            this.rdoTongHop.AutoSize = true;
            this.rdoTongHop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rdoTongHop.Location = new System.Drawing.Point(600, 25);
            this.rdoTongHop.Margin = new System.Windows.Forms.Padding(4);
            this.rdoTongHop.Name = "rdoTongHop";
            this.rdoTongHop.Size = new System.Drawing.Size(147, 22);
            this.rdoTongHop.TabIndex = 3;
            this.rdoTongHop.Text = "Báo cáo tổng hợp";
            this.rdoTongHop.UseVisualStyleBackColor = true;
            this.rdoTongHop.CheckedChanged += new System.EventHandler(this.rdoBaoCao_CheckedChanged);
            // 
            // rdoTheoNhom
            // 
            this.rdoTheoNhom.AutoSize = true;
            this.rdoTheoNhom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rdoTheoNhom.Location = new System.Drawing.Point(400, 25);
            this.rdoTheoNhom.Margin = new System.Windows.Forms.Padding(4);
            this.rdoTheoNhom.Name = "rdoTheoNhom";
            this.rdoTheoNhom.Size = new System.Drawing.Size(160, 22);
            this.rdoTheoNhom.TabIndex = 2;
            this.rdoTheoNhom.Text = "Báo cáo theo nhóm";
            this.rdoTheoNhom.UseVisualStyleBackColor = true;
            this.rdoTheoNhom.CheckedChanged += new System.EventHandler(this.rdoBaoCao_CheckedChanged);
            // 
            // rdoSapHetHan
            // 
            this.rdoSapHetHan.AutoSize = true;
            this.rdoSapHetHan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rdoSapHetHan.Location = new System.Drawing.Point(200, 25);
            this.rdoSapHetHan.Margin = new System.Windows.Forms.Padding(4);
            this.rdoSapHetHan.Name = "rdoSapHetHan";
            this.rdoSapHetHan.Size = new System.Drawing.Size(165, 22);
            this.rdoSapHetHan.TabIndex = 1;
            this.rdoSapHetHan.Text = "Báo cáo sắp hết hạn";
            this.rdoSapHetHan.UseVisualStyleBackColor = true;
            this.rdoSapHetHan.CheckedChanged += new System.EventHandler(this.rdoBaoCao_CheckedChanged);
            // 
            // rdoTonKho
            // 
            this.rdoTonKho.AutoSize = true;
            this.rdoTonKho.Checked = true;
            this.rdoTonKho.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.rdoTonKho.Location = new System.Drawing.Point(27, 25);
            this.rdoTonKho.Margin = new System.Windows.Forms.Padding(4);
            this.rdoTonKho.Name = "rdoTonKho";
            this.rdoTonKho.Size = new System.Drawing.Size(139, 22);
            this.rdoTonKho.TabIndex = 0;
            this.rdoTonKho.TabStop = true;
            this.rdoTonKho.Text = "Báo cáo tồn kho";
            this.rdoTonKho.UseVisualStyleBackColor = true;
            this.rdoTonKho.CheckedChanged += new System.EventHandler(this.rdoBaoCao_CheckedChanged);
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTieuDe.Location = new System.Drawing.Point(27, 18);
            this.lblTieuDe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(529, 36);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "📊 XUẤT BÁO CÁO KHO THUỐC";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDong);
            this.panel2.Controls.Add(this.btnXuatExcel);
            this.panel2.Controls.Add(this.btnXuatPDF);
            this.panel2.Controls.Add(this.btnIn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 615);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1200, 74);
            this.panel2.TabIndex = 1;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(1000, 18);
            this.btnDong.Margin = new System.Windows.Forms.Padding(4);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(133, 43);
            this.btnDong.TabIndex = 3;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnXuatExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatExcel.ForeColor = System.Drawing.Color.White;
            this.btnXuatExcel.Location = new System.Drawing.Point(667, 18);
            this.btnXuatExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(133, 43);
            this.btnXuatExcel.TabIndex = 2;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = false;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnXuatPDF
            // 
            this.btnXuatPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnXuatPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatPDF.ForeColor = System.Drawing.Color.White;
            this.btnXuatPDF.Location = new System.Drawing.Point(827, 18);
            this.btnXuatPDF.Margin = new System.Windows.Forms.Padding(4);
            this.btnXuatPDF.Name = "btnXuatPDF";
            this.btnXuatPDF.Size = new System.Drawing.Size(133, 43);
            this.btnXuatPDF.TabIndex = 1;
            this.btnXuatPDF.Text = "Xuất PDF";
            this.btnXuatPDF.UseVisualStyleBackColor = false;
            this.btnXuatPDF.Click += new System.EventHandler(this.btnXuatPDF_Click);
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnIn.ForeColor = System.Drawing.Color.White;
            this.btnIn.Location = new System.Drawing.Point(507, 18);
            this.btnIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(133, 43);
            this.btnIn.TabIndex = 0;
            this.btnIn.Text = "In";
            this.btnIn.UseVisualStyleBackColor = false;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // dgvBaoCao
            // 
            this.dgvBaoCao.AllowUserToAddRows = false;
            this.dgvBaoCao.AllowUserToDeleteRows = false;
            this.dgvBaoCao.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBaoCao.BackgroundColor = System.Drawing.Color.White;
            this.dgvBaoCao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoCao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBaoCao.Location = new System.Drawing.Point(0, 123);
            this.dgvBaoCao.Margin = new System.Windows.Forms.Padding(4);
            this.dgvBaoCao.Name = "dgvBaoCao";
            this.dgvBaoCao.ReadOnly = true;
            this.dgvBaoCao.RowHeadersVisible = false;
            this.dgvBaoCao.RowHeadersWidth = 51;
            this.dgvBaoCao.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBaoCao.Size = new System.Drawing.Size(1200, 492);
            this.dgvBaoCao.TabIndex = 2;
            // 
            // BaoCao_Xuat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 689);
            this.Controls.Add(this.dgvBaoCao);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaoCao_Xuat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Xuất Báo Cáo Kho Thuốc";
            this.Load += new System.EventHandler(this.BaoCao_Xuat_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoTongHop;
        private System.Windows.Forms.RadioButton rdoTheoNhom;
        private System.Windows.Forms.RadioButton rdoSapHetHan;
        private System.Windows.Forms.RadioButton rdoTonKho;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnXuatPDF;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.DataGridView dgvBaoCao;
    }
}
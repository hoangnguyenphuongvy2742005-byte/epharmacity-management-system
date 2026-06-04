namespace NhaThuoc
{
    partial class UC_ThongKeNhanh
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
            this.panelThongKe = new System.Windows.Forms.Panel();
            this.lblThuocTonKhoThap = new System.Windows.Forms.Label();
            this.lblThuocSapHetHan = new System.Windows.Forms.Label();
            this.lblTongGiaTriKho = new System.Windows.Forms.Label();
            this.lblTongSoThuoc = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.panelThongKe.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelThongKe
            // 
            this.panelThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.panelThongKe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelThongKe.Controls.Add(this.lblThuocTonKhoThap);
            this.panelThongKe.Controls.Add(this.lblThuocSapHetHan);
            this.panelThongKe.Controls.Add(this.lblTongGiaTriKho);
            this.panelThongKe.Controls.Add(this.lblTongSoThuoc);
            this.panelThongKe.Controls.Add(this.lblTieuDe);
            this.panelThongKe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelThongKe.Location = new System.Drawing.Point(0, 0);
            this.panelThongKe.Margin = new System.Windows.Forms.Padding(4);
            this.panelThongKe.Name = "panelThongKe";
            this.panelThongKe.Size = new System.Drawing.Size(533, 246);
            this.panelThongKe.TabIndex = 0;
            this.panelThongKe.Paint += new System.Windows.Forms.PaintEventHandler(this.panelThongKe_Paint);
            // 
            // lblThuocTonKhoThap
            // 
            this.lblThuocTonKhoThap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblThuocTonKhoThap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblThuocTonKhoThap.Location = new System.Drawing.Point(27, 166);
            this.lblThuocTonKhoThap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThuocTonKhoThap.Name = "lblThuocTonKhoThap";
            this.lblThuocTonKhoThap.Size = new System.Drawing.Size(420, 30);
            this.lblThuocTonKhoThap.TabIndex = 4;
            this.lblThuocTonKhoThap.Text = "📦 Thuốc tồn kho thấp: 0 sản phẩm";
            // 
            // lblThuocSapHetHan
            // 
            this.lblThuocSapHetHan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblThuocSapHetHan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblThuocSapHetHan.Location = new System.Drawing.Point(27, 129);
            this.lblThuocSapHetHan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThuocSapHetHan.Name = "lblThuocSapHetHan";
            this.lblThuocSapHetHan.Size = new System.Drawing.Size(420, 21);
            this.lblThuocSapHetHan.TabIndex = 3;
            this.lblThuocSapHetHan.Text = "⏰ Thuốc sắp hết hạn: 0 sản phẩm";
            // 
            // lblTongGiaTriKho
            // 
            this.lblTongGiaTriKho.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongGiaTriKho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblTongGiaTriKho.Location = new System.Drawing.Point(27, 92);
            this.lblTongGiaTriKho.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTongGiaTriKho.Name = "lblTongGiaTriKho";
            this.lblTongGiaTriKho.Size = new System.Drawing.Size(463, 22);
            this.lblTongGiaTriKho.TabIndex = 2;
            this.lblTongGiaTriKho.Text = "💰 Tổng giá trị kho: 0 VNĐ";
            // 
            // lblTongSoThuoc
            // 
            this.lblTongSoThuoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongSoThuoc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTongSoThuoc.Location = new System.Drawing.Point(27, 55);
            this.lblTongSoThuoc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTongSoThuoc.Name = "lblTongSoThuoc";
            this.lblTongSoThuoc.Size = new System.Drawing.Size(420, 24);
            this.lblTongSoThuoc.TabIndex = 1;
            this.lblTongSoThuoc.Text = "💊 Tổng số thuốc: 0 sản phẩm";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblTieuDe.Location = new System.Drawing.Point(13, 12);
            this.lblTieuDe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(315, 25);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "📊 THỐNG KÊ NHANH";
            // 
            // UC_ThongKeNhanh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelThongKe);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UC_ThongKeNhanh";
            this.Size = new System.Drawing.Size(533, 246);
            this.Load += new System.EventHandler(this.UC_ThongKeNhanh_Load);
            this.panelThongKe.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelThongKe;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTongSoThuoc;
        private System.Windows.Forms.Label lblTongGiaTriKho;
        private System.Windows.Forms.Label lblThuocSapHetHan;
        private System.Windows.Forms.Label lblThuocTonKhoThap;
    }
}

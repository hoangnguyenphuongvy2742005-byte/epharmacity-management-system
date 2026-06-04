namespace NhaThuoc
{
    partial class UC_CanhBaoTonKho
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
            this.panelCanhBao = new System.Windows.Forms.Panel();
            this.btnBaoCaoTonKho = new System.Windows.Forms.Button();
            this.lblDaHetHan = new System.Windows.Forms.Label();
            this.lblSapHetHan = new System.Windows.Forms.Label();
            this.lblTonKhoThap = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.panelCanhBao.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCanhBao
            // 
            this.panelCanhBao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelCanhBao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCanhBao.Controls.Add(this.btnBaoCaoTonKho);
            this.panelCanhBao.Controls.Add(this.lblDaHetHan);
            this.panelCanhBao.Controls.Add(this.lblSapHetHan);
            this.panelCanhBao.Controls.Add(this.lblTonKhoThap);
            this.panelCanhBao.Controls.Add(this.lblTieuDe);
            this.panelCanhBao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCanhBao.Location = new System.Drawing.Point(0, 0);
            this.panelCanhBao.Margin = new System.Windows.Forms.Padding(4);
            this.panelCanhBao.Name = "panelCanhBao";
            this.panelCanhBao.Size = new System.Drawing.Size(533, 185);
            this.panelCanhBao.TabIndex = 0;
            this.panelCanhBao.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCanhBao_Paint);
            // 
            // btnBaoCaoTonKho
            // 
            this.btnBaoCaoTonKho.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnBaoCaoTonKho.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnBaoCaoTonKho.ForeColor = System.Drawing.Color.White;
            this.btnBaoCaoTonKho.Location = new System.Drawing.Point(333, 49);
            this.btnBaoCaoTonKho.Margin = new System.Windows.Forms.Padding(4);
            this.btnBaoCaoTonKho.Name = "btnBaoCaoTonKho";
            this.btnBaoCaoTonKho.Size = new System.Drawing.Size(160, 37);
            this.btnBaoCaoTonKho.TabIndex = 4;
            this.btnBaoCaoTonKho.Text = "Báo cáo tồn kho";
            this.btnBaoCaoTonKho.UseVisualStyleBackColor = false;
            this.btnBaoCaoTonKho.Click += new System.EventHandler(this.btnBaoCaoTonKho_Click);
            // 
            // lblDaHetHan
            // 
            this.lblDaHetHan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblDaHetHan.ForeColor = System.Drawing.Color.Red;
            this.lblDaHetHan.Location = new System.Drawing.Point(27, 111);
            this.lblDaHetHan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDaHetHan.Name = "lblDaHetHan";
            this.lblDaHetHan.Size = new System.Drawing.Size(298, 20);
            this.lblDaHetHan.TabIndex = 3;
            this.lblDaHetHan.Text = "❌ Đã hết hạn: 0 sản phẩm";
            // 
            // lblSapHetHan
            // 
            this.lblSapHetHan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSapHetHan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblSapHetHan.Location = new System.Drawing.Point(27, 80);
            this.lblSapHetHan.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSapHetHan.Name = "lblSapHetHan";
            this.lblSapHetHan.Size = new System.Drawing.Size(298, 20);
            this.lblSapHetHan.TabIndex = 2;
            this.lblSapHetHan.Text = "⏰ Sắp hết hạn: 0 sản phẩm";
            // 
            // lblTonKhoThap
            // 
            this.lblTonKhoThap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTonKhoThap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTonKhoThap.Location = new System.Drawing.Point(27, 49);
            this.lblTonKhoThap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTonKhoThap.Name = "lblTonKhoThap";
            this.lblTonKhoThap.Size = new System.Drawing.Size(298, 20);
            this.lblTonKhoThap.TabIndex = 1;
            this.lblTonKhoThap.Text = "📦 Tồn kho thấp: 0 sản phẩm";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTieuDe.Location = new System.Drawing.Point(13, 12);
            this.lblTieuDe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(407, 25);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "⚠️ CẢNH BÁO KHO THUỐC";
            // 
            // UC_CanhBaoTonKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelCanhBao);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UC_CanhBaoTonKho";
            this.Size = new System.Drawing.Size(533, 185);
            this.Load += new System.EventHandler(this.UC_CanhBaoTonKho_Load);
            this.panelCanhBao.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCanhBao;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTonKhoThap;
        private System.Windows.Forms.Label lblSapHetHan;
        private System.Windows.Forms.Label lblDaHetHan;
        private System.Windows.Forms.Button btnBaoCaoTonKho;
    }
}

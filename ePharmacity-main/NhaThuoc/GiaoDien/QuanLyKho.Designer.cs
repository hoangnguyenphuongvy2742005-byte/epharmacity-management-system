namespace NhaThuoc.GiaoDien
{
    partial class QuanLyKho
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
            this.btn_QuanLyKho = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_QuanLyKho
            // 
            this.btn_QuanLyKho.Location = new System.Drawing.Point(121, 54);
            this.btn_QuanLyKho.Name = "btn_QuanLyKho";
            this.btn_QuanLyKho.Size = new System.Drawing.Size(142, 52);
            this.btn_QuanLyKho.TabIndex = 0;
            this.btn_QuanLyKho.Text = "Quản lý kho thuốc";
            this.btn_QuanLyKho.UseVisualStyleBackColor = true;
            this.btn_QuanLyKho.Click += new System.EventHandler(this.btn_QuanLyKho_Click);
            // 
            // QuanLyKho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 173);
            this.Controls.Add(this.btn_QuanLyKho);
            this.Name = "QuanLyKho";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuanLyKho";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_QuanLyKho;
    }
}
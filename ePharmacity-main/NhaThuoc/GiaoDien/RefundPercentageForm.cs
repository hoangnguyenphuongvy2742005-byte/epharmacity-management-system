using System;
using System.Windows.Forms;

namespace NhaThuoc.GiaoDien
{
    public partial class RefundPercentageForm : Form
    {
        public int SelectedPercentage { get; private set; }

        public RefundPercentageForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblReason = new Label();
            this.lblPercentage = new Label();
            this.cmbPercentage = new ComboBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chọn phần trăm hoàn trả";

            // lblReason
            this.lblReason.AutoSize = true;
            this.lblReason.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblReason.Location = new System.Drawing.Point(20, 60);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(300, 19);
            this.lblReason.TabIndex = 1;
            this.lblReason.Text = "Dựa vào lý do hoàn trả, hãy chọn phần trăm phù hợp:";

            // lblPercentage
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPercentage.Location = new System.Drawing.Point(20, 100);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(120, 19);
            this.lblPercentage.TabIndex = 2;
            this.lblPercentage.Text = "Phần trăm hoàn trả:";

            // cmbPercentage
            this.cmbPercentage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPercentage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPercentage.FormattingEnabled = true;
            this.cmbPercentage.Items.AddRange(new object[] {
                "50% - Hàng lỗi nhẹ, có thể sử dụng một phần",
                "75% - Hàng lỗi trung bình, ảnh hưởng đến chất lượng",
                "100% - Hàng lỗi nghiêm trọng, không thể sử dụng"
            });
            this.cmbPercentage.Location = new System.Drawing.Point(20, 130);
            this.cmbPercentage.Name = "cmbPercentage";
            this.cmbPercentage.Size = new System.Drawing.Size(400, 25);
            this.cmbPercentage.TabIndex = 3;

            // btnOK
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnOK.FlatStyle = FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(20, 180);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 40);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Xác nhận";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new EventHandler(this.BtnOK_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(140, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);

            // RefundPercentageForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 250);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbPercentage);
            this.Controls.Add(this.lblPercentage);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefundPercentageForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chọn phần trăm hoàn trả";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblTitle;
        private Label lblReason;
        private Label lblPercentage;
        private ComboBox cmbPercentage;
        private Button btnOK;
        private Button btnCancel;

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (cmbPercentage.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phần trăm hoàn trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy phần trăm từ lựa chọn
            string selectedText = cmbPercentage.SelectedItem.ToString();
            if (selectedText.Contains("50%"))
            {
                SelectedPercentage = 50;
            }
            else if (selectedText.Contains("75%"))
            {
                SelectedPercentage = 75;
            }
            else if (selectedText.Contains("100%"))
            {
                SelectedPercentage = 100;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

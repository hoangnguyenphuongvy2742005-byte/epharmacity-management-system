using NhaThuoc.GiaoDien;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using NhaThuoc;

namespace NhaThuoc
{
    public partial class NhanVienBanHang : Form
    {
        private Button btnOrderHistory;
        private Button btnCreateOrder;
        private Button btnExit;
        private Label lblTitle;
        private Panel pnlMain;
        private Panel pnlHeader;
        private Panel pnlMenu;
        public string Sodienthoai;
        public string Manhanvien;
        public string Hovaten;
        public NhanVienBanHang()
        {
            InitializeComponent();
            SetupHoverEffects();
            TestDatabaseConnection();
        }

        public NhanVienBanHang(string hoten, string sdt, string manv)
        {
            InitializeComponent();
            Sodienthoai = sdt;
            Manhanvien = manv;
            Hovaten = hoten;
        }

        private void TestDatabaseConnection()
        {
            try
            {
                var db = new DatabaseHelper.DatabaseConnection();
                if (!db.TestConnection())
                {
                    // Connection failed - show warning but allow app to continue
                    // User can still use the app, but database features won't work
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối database: {ex.Message}\n\nỨng dụng vẫn có thể chạy nhưng các chức năng database sẽ không hoạt động.", 
                              "Cảnh báo Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupHoverEffects()
        {
            AddHoverEffect(btnOrderHistory, Color.FromArgb(52, 152, 219));
            AddHoverEffect(btnCreateOrder, Color.FromArgb(230, 126, 34));
            AddHoverEffect(btnReturnRequest, Color.FromArgb(155, 89, 182));
            AddHoverEffect(btnExit, Color.FromArgb(231, 76, 60));
        }

        private void AddHoverEffect(Button button, Color originalColor)
        {
            button.MouseEnter += (s, e) => {
                button.BackColor = ControlPaint.Light(originalColor, 0.2f);
            };
            
            button.MouseLeave += (s, e) => {
                button.BackColor = originalColor;
            };
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string useCaseId = button.Tag?.ToString();
                
                try
                {
                    switch (useCaseId)
                    {
                        case "UC001":
                            ShowOrderHistoryForm();
                            break;
                        case "UC003":
                            ShowCreateOrderForm();
                            break;
                        case "UC005":
                            ShowReturnRequestForm();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowOrderHistoryForm()
        {
            using (OrderHistoryForm form = new OrderHistoryForm())
            {
                form.ShowDialog();
            }
        }

        private void ShowCreateOrderForm()
        {
            using (CreateOrderForm form = new CreateOrderForm(Manhanvien))
            {
                form.ShowDialog();
            }
        }

        private void ShowReturnRequestForm()
        {
            try
            {
                // Sử dụng ReturnRequestForm thực
                ReturnRequestForm form = new ReturnRequestForm(Manhanvien);
                form.ShowDialog();
                form.Dispose();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form hoàn trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowReturnRequestFormDemo()
        {
            try
            {
                // Form demo với chức năng đầy đủ
                Form demoForm = new Form();
                demoForm.Text = "Xử Lý Yêu Cầu Hoàn Trả - DEMO";
                demoForm.Size = new Size(1000, 700);
                demoForm.StartPosition = FormStartPosition.CenterParent;
                demoForm.BackColor = Color.FromArgb(245, 248, 250);

                // Header
                Panel headerPanel = new Panel();
                headerPanel.BackColor = Color.FromArgb(52, 73, 94);
                headerPanel.Dock = DockStyle.Top;
                headerPanel.Height = 80;
                demoForm.Controls.Add(headerPanel);

                Label titleLabel = new Label();
                titleLabel.Text = "XỬ LÝ YÊU CẦU HOÀN TRẢ";
                titleLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                titleLabel.ForeColor = Color.White;
                titleLabel.Dock = DockStyle.Fill;
                titleLabel.TextAlign = ContentAlignment.MiddleCenter;
                headerPanel.Controls.Add(titleLabel);

                // Main content
                Panel mainPanel = new Panel();
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.Padding = new Padding(20);
                demoForm.Controls.Add(mainPanel);

                // DataGridView để hiển thị dữ liệu
                DataGridView dgv = new DataGridView();
                dgv.Location = new Point(20, 20);
                dgv.Size = new Size(940, 300);
                dgv.ReadOnly = true;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.BackgroundColor = Color.White;
                dgv.BorderStyle = BorderStyle.Fixed3D;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                mainPanel.Controls.Add(dgv);

                // Tạo dữ liệu demo
                DataTable demoData = new DataTable();
                demoData.Columns.Add("Mã đơn hàng", typeof(int));
                demoData.Columns.Add("SĐT khách hàng", typeof(string));
                demoData.Columns.Add("Tên khách hàng", typeof(string));
                demoData.Columns.Add("Lý do", typeof(string));
                demoData.Columns.Add("Yêu cầu", typeof(string));
                demoData.Columns.Add("Ngày tạo", typeof(DateTime));

                // Thêm dữ liệu demo
                demoData.Rows.Add(1, "0123456789", "Nguyễn Văn A", "Sản phẩm bị hỏng", "Hoàn tiền", DateTime.Now.AddDays(-1));
                demoData.Rows.Add(2, "0987654321", "Trần Thị B", "Mua nhầm sản phẩm", "Đổi sản phẩm", DateTime.Now.AddDays(-2));
                demoData.Rows.Add(3, "0123456789", "Nguyễn Văn A", "Sản phẩm hết hạn", "Hoàn tiền", DateTime.Now.AddDays(-3));
                demoData.Rows.Add(4, "0987654321", "Trần Thị B", "Dị ứng với thuốc", "Hoàn tiền", DateTime.Now.AddDays(-4));
                demoData.Rows.Add(5, "0123456789", "Nguyễn Văn A", "Sản phẩm không đúng mô tả", "Đổi sản phẩm", DateTime.Now.AddDays(-5));

                dgv.DataSource = demoData;

                // Panel chi tiết
                Panel detailPanel = new Panel();
                detailPanel.Location = new Point(20, 340);
                detailPanel.Size = new Size(940, 150);
                detailPanel.BackColor = Color.White;
                detailPanel.BorderStyle = BorderStyle.FixedSingle;
                mainPanel.Controls.Add(detailPanel);

                Label detailTitle = new Label();
                detailTitle.Text = "Chi tiết yêu cầu:";
                detailTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                detailTitle.Location = new Point(20, 10);
                detailTitle.Size = new Size(200, 30);
                detailPanel.Controls.Add(detailTitle);

                Label detailInfo = new Label();
                detailInfo.Text = "Click vào một dòng trong bảng để xem chi tiết...";
                detailInfo.Font = new Font("Segoe UI", 10);
                detailInfo.Location = new Point(20, 40);
                detailInfo.Size = new Size(900, 100);
                detailPanel.Controls.Add(detailInfo);

                // Event handler cho DataGridView
                dgv.CellClick += (s, e) => {
                    if (e.RowIndex >= 0)
                    {
                        DataRow row = demoData.Rows[e.RowIndex];
                        detailInfo.Text = $"Mã đơn hàng: {row[0]}\n" +
                                        $"SĐT: {row[1]}\n" +
                                        $"Tên khách hàng: {row[2]}\n" +
                                        $"Lý do: {row[3]}\n" +
                                        $"Yêu cầu: {row[4]}\n" +
                                        $"Ngày tạo: {Convert.ToDateTime(row[5]):dd/MM/yyyy HH:mm}";
                    }
                };

                // Panel buttons
                Panel buttonPanel = new Panel();
                buttonPanel.Location = new Point(20, 500);
                buttonPanel.Size = new Size(940, 60);
                mainPanel.Controls.Add(buttonPanel);

                // Button Chấp nhận
                Button btnApprove = new Button();
                btnApprove.Text = "Chấp nhận";
                btnApprove.BackColor = Color.FromArgb(46, 204, 113);
                btnApprove.ForeColor = Color.White;
                btnApprove.FlatStyle = FlatStyle.Flat;
                btnApprove.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnApprove.Location = new Point(20, 10);
                btnApprove.Size = new Size(120, 40);
                btnApprove.Click += (s, e) => {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        MessageBox.Show($"Đã chấp nhận yêu cầu hoàn trả cho đơn hàng {dgv.SelectedRows[0].Cells[0].Value}!", 
                                      "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn một yêu cầu để xử lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                buttonPanel.Controls.Add(btnApprove);

                // Button Từ chối
                Button btnReject = new Button();
                btnReject.Text = "Từ chối";
                btnReject.BackColor = Color.FromArgb(231, 76, 60);
                btnReject.ForeColor = Color.White;
                btnReject.FlatStyle = FlatStyle.Flat;
                btnReject.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnReject.Location = new Point(160, 10);
                btnReject.Size = new Size(120, 40);
                btnReject.Click += (s, e) => {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        MessageBox.Show($"Đã từ chối yêu cầu hoàn trả cho đơn hàng {dgv.SelectedRows[0].Cells[0].Value}!", 
                                      "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn một yêu cầu để xử lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                buttonPanel.Controls.Add(btnReject);

                // Button In
                Button btnPrint = new Button();
                btnPrint.Text = "In danh sách";
                btnPrint.BackColor = Color.FromArgb(52, 152, 219);
                btnPrint.ForeColor = Color.White;
                btnPrint.FlatStyle = FlatStyle.Flat;
                btnPrint.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnPrint.Location = new Point(300, 10);
                btnPrint.Size = new Size(120, 40);
                btnPrint.Click += (s, e) => {
                    try
                    {
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrintPage += (sender, printArgs) => {
                            Graphics g = printArgs.Graphics;
                            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                            Font contentFont = new Font("Arial", 10);

                            int yPos = 50;
                            int leftMargin = 50;

                            // Tiêu đề
                            g.DrawString("DANH SÁCH YÊU CẦU HOÀN TRẢ", titleFont, Brushes.Black, leftMargin, yPos);
                            yPos += 40;

                            // Header
                            g.DrawString("Mã đơn hàng", headerFont, Brushes.Black, leftMargin, yPos);
                            g.DrawString("SĐT KH", headerFont, Brushes.Black, leftMargin + 100, yPos);
                            g.DrawString("Tên KH", headerFont, Brushes.Black, leftMargin + 200, yPos);
                            g.DrawString("Lý do", headerFont, Brushes.Black, leftMargin + 350, yPos);
                            g.DrawString("Yêu cầu", headerFont, Brushes.Black, leftMargin + 500, yPos);
                            yPos += 30;

                            // Dữ liệu
                            foreach (DataRow row in demoData.Rows)
                            {
                                g.DrawString(row[0].ToString(), contentFont, Brushes.Black, leftMargin, yPos);
                                g.DrawString(row[1].ToString(), contentFont, Brushes.Black, leftMargin + 100, yPos);
                                g.DrawString(row[2].ToString(), contentFont, Brushes.Black, leftMargin + 200, yPos);
                                g.DrawString(row[3].ToString(), contentFont, Brushes.Black, leftMargin + 350, yPos);
                                g.DrawString(row[4].ToString(), contentFont, Brushes.Black, leftMargin + 500, yPos);
                                yPos += 20;
                            }

                            // Footer
                            yPos += 20;
                            g.DrawString($"Tổng số yêu cầu: {demoData.Rows.Count}", contentFont, Brushes.Black, leftMargin, yPos);
                        };

                        PrintDialog printDialog = new PrintDialog();
                        printDialog.Document = printDoc;
                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            printDoc.Print();
                            MessageBox.Show("In thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi in: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                buttonPanel.Controls.Add(btnPrint);

                // Button Xuất Excel
                Button btnExport = new Button();
                btnExport.Text = "Xuất Excel";
                btnExport.BackColor = Color.FromArgb(155, 89, 182);
                btnExport.ForeColor = Color.White;
                btnExport.FlatStyle = FlatStyle.Flat;
                btnExport.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnExport.Location = new Point(440, 10);
                btnExport.Size = new Size(120, 40);
                btnExport.Click += (s, e) => {
                    try
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                        saveDialog.FileName = $"DanhSachHoanTra_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            StringBuilder csv = new StringBuilder();
                            
                            // Header
                            csv.AppendLine("Mã đơn hàng,SĐT khách hàng,Tên khách hàng,Lý do,Yêu cầu,Ngày tạo");
                            
                            // Data
                            foreach (DataRow row in demoData.Rows)
                            {
                                csv.AppendLine($"\"{row[0]}\",\"{row[1]}\",\"{row[2]}\",\"{row[3]}\",\"{row[4]}\",\"{Convert.ToDateTime(row[5]):dd/MM/yyyy HH:mm}\"");
                            }
                            
                            System.IO.File.WriteAllText(saveDialog.FileName, csv.ToString(), Encoding.UTF8);
                            MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                buttonPanel.Controls.Add(btnExport);

                // Button Đóng
                Button btnClose = new Button();
                btnClose.Text = "Đóng";
                btnClose.BackColor = Color.FromArgb(149, 165, 166);
                btnClose.ForeColor = Color.White;
                btnClose.FlatStyle = FlatStyle.Flat;
                btnClose.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnClose.Location = new Point(800, 10);
                btnClose.Size = new Size(120, 40);
                btnClose.Click += (s, e) => demoForm.Close();
                buttonPanel.Controls.Add(btnClose);

                demoForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form hoàn trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Notice notice = new Notice(Sodienthoai);
            notice.ShowDialog();
            this.Close();
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Homepage homepage = new Homepage();
            homepage.Show();
            this.Close();
        }

        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
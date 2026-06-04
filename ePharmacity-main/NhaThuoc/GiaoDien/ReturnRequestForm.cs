using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NhaThuoc.GiaoDien;

namespace NhaThuoc
{
    public partial class ReturnRequestForm : Form
    {
        private DatabaseHelper.DatabaseConnection db;
        private string currentEmployeeId;
        private int selectedRequestId = -1;
        private PrintDocument printDocument;
        private DataTable printData;

        public ReturnRequestForm(string employeeId)
        {
            InitializeComponent();
            db = new DatabaseHelper.DatabaseConnection();
            currentEmployeeId = employeeId;
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void ReturnRequestForm_Load(object sender, EventArgs e)
        {
            LoadReturnRequests();
        }

        private void LoadReturnRequests()
        {
            try
            {
                // Load dữ liệu từ database thực tế - Chỉ load những yêu cầu chưa được xử lý
                string query = @"
                    SELECT 
                        y.ID,
                        y.Madonhang,
                        y.Sodienthoaikhachhang,
                        y.Manhanvien,
                        y.Lydo,
                        y.Mota,
                        y.YeuCau,
                        y.Ngaytao
                    FROM YeuCauHoanDon y
                    LEFT JOIN Thongtinkhachhang kh ON y.Sodienthoaikhachhang = kh.Sodienthoai
                    WHERE y.Manhanvien IS NULL OR y.Manhanvien = ''
                    ORDER BY y.Ngaytao DESC";

                DataTable dt = db.GetData(query);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvReturnRequests.DataSource = dt;
                    printData = dt;
                }
                else
                {
                    // Nếu không có dữ liệu, hiển thị thông báo
                    MessageBox.Show("Không có yêu cầu hoàn trả nào trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Tạo DataTable rỗng
                    DataTable emptyData = new DataTable();
                    emptyData.Columns.Add("ID", typeof(int));
                    emptyData.Columns.Add("Madonhang", typeof(int));
                    emptyData.Columns.Add("Sodienthoaikhachhang", typeof(string));
                    emptyData.Columns.Add("Manhanvien", typeof(string));
                    emptyData.Columns.Add("Lydo", typeof(string));
                    emptyData.Columns.Add("Mota", typeof(string));
                    emptyData.Columns.Add("YeuCau", typeof(string));
                    emptyData.Columns.Add("Ngaytao", typeof(DateTime));
                    
                    dgvReturnRequests.DataSource = emptyData;
                    printData = emptyData;
                }

                if (dgvReturnRequests.Columns.Count > 0)
                {
                    // Ẩn cột ID
                    dgvReturnRequests.Columns["ID"].Visible = false;
                    
                    // Đặt tên cột
                    dgvReturnRequests.Columns["Madonhang"].HeaderText = "Mã đơn hàng";
                    dgvReturnRequests.Columns["Sodienthoaikhachhang"].HeaderText = "SĐT khách hàng";
                    dgvReturnRequests.Columns["Manhanvien"].HeaderText = "Mã nhân viên";
                    dgvReturnRequests.Columns["Lydo"].HeaderText = "Lý do";
                    dgvReturnRequests.Columns["Mota"].HeaderText = "Mô tả";
                    dgvReturnRequests.Columns["YeuCau"].HeaderText = "Yêu cầu";
                    dgvReturnRequests.Columns["Ngaytao"].HeaderText = "Ngày tạo";

                    // Định dạng ngày
                    dgvReturnRequests.Columns["Ngaytao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                    // Thiết lập độ rộng cột
                    dgvReturnRequests.Columns["Madonhang"].Width = 100;
                    dgvReturnRequests.Columns["Sodienthoaikhachhang"].Width = 120;
                    dgvReturnRequests.Columns["Manhanvien"].Width = 120;
                    dgvReturnRequests.Columns["Lydo"].Width = 150;
                    dgvReturnRequests.Columns["Mota"].Width = 200;
                    dgvReturnRequests.Columns["YeuCau"].Width = 150;
                    dgvReturnRequests.Columns["Ngaytao"].Width = 120;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách yêu cầu hoàn trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            if (selectedRequestId == -1)
            {
                MessageBox.Show("Vui lòng chọn yêu cầu hoàn trả cần xử lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị form chọn phần trăm hoàn trả
            using (var refundForm = new RefundPercentageForm())
            {
                if (refundForm.ShowDialog() == DialogResult.OK)
                {
                    int refundPercentage = refundForm.SelectedPercentage;
                    
                    // Xác nhận hoàn trả
                    string confirmMessage = $"Bạn có chắc chắn muốn hoàn trả {refundPercentage}% tổng tiền đơn hàng này?\n\n" +
                                          "Sau khi xác nhận, hệ thống sẽ:\n" +
                                          "- Cập nhật số lượng hàng trong kho\n" +
                                          "- Cập nhật doanh thu\n" +
                                          "- Ghi nhận hoàn trả";

                    DialogResult result = MessageBox.Show(confirmMessage, "Xác nhận hoàn trả", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // Hiển thị loading để người dùng biết hệ thống đang xử lý
                            this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;
                            
                            // Xử lý hoàn trả trực tiếp (không dùng async/await)
                            ProcessRefund(refundPercentage);
                            
                            MessageBox.Show($"Đã hoàn trả {refundPercentage}% tổng tiền đơn hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            LoadReturnRequests();
                            ClearSelection();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi khi xử lý hoàn trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            // Khôi phục UI
                            this.Cursor = Cursors.Default;
                            this.Enabled = true;
                        }
                    }
                }
            }
        }

        private void ProcessRefund(int refundPercentage)
        {
            // Lấy thông tin đơn hàng
            string getOrderQuery = @"
                SELECT y.Madonhang, y.Sodienthoaikhachhang
                FROM YeuCauHoanDon y
                WHERE y.ID = @ID";
            
            SqlParameter[] getParams = { new SqlParameter("@ID", selectedRequestId) };
            DataTable orderData = db.GetData(getOrderQuery, getParams);
            
            if (orderData == null || orderData.Rows.Count == 0)
            {
                throw new Exception("Không tìm thấy thông tin đơn hàng");
            }
            
            int orderId = Convert.ToInt32(orderData.Rows[0]["Madonhang"]);
            string customerPhone = orderData.Rows[0]["Sodienthoaikhachhang"].ToString();
            
            // Kiểm tra xem đơn hàng đã được hoàn trả chưa
            string checkRefundQuery = @"
                SELECT COUNT(*) FROM DanhSachHoanTra 
                WHERE Madonhang = @Madonhang";
            
            SqlParameter[] checkParams = { new SqlParameter("@Madonhang", orderId) };
            int refundCount = Convert.ToInt32(db.ExecuteScalar(checkRefundQuery, checkParams));
            
            if (refundCount > 0)
            {
                throw new Exception($"Đơn hàng {orderId} đã được hoàn trả trước đó. Không thể hoàn trả lại!");
            }
            
            // Kiểm tra xem yêu cầu đã được xử lý chưa (đã có mã nhân viên)
            string checkProcessedQuery = @"
                SELECT Manhanvien FROM YeuCauHoanDon 
                WHERE ID = @ID";
            
            SqlParameter[] checkProcessedParams = { new SqlParameter("@ID", selectedRequestId) };
            DataTable processedData = db.GetData(checkProcessedQuery, checkProcessedParams);
            
            if (processedData != null && processedData.Rows.Count > 0)
            {
                string manhanvien = processedData.Rows[0]["Manhanvien"]?.ToString();
                if (!string.IsNullOrEmpty(manhanvien))
                {
                    throw new Exception($"Yêu cầu hoàn trả này đã được xử lý bởi nhân viên {manhanvien}. Không thể hoàn trả lại!");
                }
            }
            
            // Lấy chi tiết đơn hàng để tính toán
            string getOrderDetailsQuery = @"
                SELECT ct.Masanpham, ct.Soluong, ct.DonGia, ct.Tongtiensanpham
                FROM Chitietdonhang ct
                WHERE ct.Madonhang = @Madonhang";
            
            SqlParameter[] detailParams = { new SqlParameter("@Madonhang", orderId) };
            DataTable orderDetails = db.GetData(getOrderDetailsQuery, detailParams);
            
            if (orderDetails == null || orderDetails.Rows.Count == 0)
            {
                throw new Exception("Không tìm thấy chi tiết đơn hàng");
            }
            
            // Bắt đầu transaction
            db.BeginTransaction();
            
            try
            {
                // Cập nhật số lượng hàng trong kho và tính tổng tiền hoàn trả
                decimal totalRefundAmount = 0;
                
                foreach (DataRow row in orderDetails.Rows)
                {
                    string masanpham = row["Masanpham"].ToString();
                    int soluong = Convert.ToInt32(row["Soluong"]);
                    decimal tongtiensanpham = Convert.ToDecimal(row["Tongtiensanpham"]);
                    
                    // Tính số lượng hoàn trả
                    int refundQuantity = (int)Math.Ceiling(soluong * refundPercentage / 100.0);
                    
                    // Tính tiền hoàn trả cho sản phẩm này
                    decimal refundAmount = tongtiensanpham * refundPercentage / 100;
                    totalRefundAmount += refundAmount;
                    
                    // Cập nhật số lượng trong kho - đơn giản chỉ cộng vào
                    string updateStockQuery = @"
                        UPDATE Sanphamthuoc
                        SET Kho = Kho + @Soluong
                        WHERE ID = @ID";
                    
                    SqlParameter[] stockParams = {
                        new SqlParameter("@Soluong", refundQuantity),
                        new SqlParameter("@ID", masanpham)
                    };
                    
                    db.ExecuteNonQueryWithTransaction(updateStockQuery, stockParams);
                }
                
                // Tính tổng tiền đơn hàng gốc từ Chitietdonhang
                string getTotalOrderQuery = @"
                    SELECT SUM(Tongtiensanpham) FROM Chitietdonhang 
                    WHERE Madonhang = @Madonhang";
                
                SqlParameter[] totalParams = { new SqlParameter("@Madonhang", orderId) };
                decimal originalTotal = Convert.ToDecimal(db.ExecuteScalar(getTotalOrderQuery, totalParams));
                
                // Hiển thị thông tin để debug
                MessageBox.Show($"💰 Tổng tiền đơn hàng gốc: {originalTotal:N0} VNĐ\n💸 Số tiền hoàn trả: {totalRefundAmount:N0} VNĐ", "Debug Revenue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Lưu thông tin hoàn trả vào bảng DanhSachHoanTra
                try
                {
                    string insertRefundQuery = @"
                        INSERT INTO DanhSachHoanTra 
                        (Madonhang, Sodienthoaikhachhang, Manhanvien, PhantramHoanTra, TongTienDonHang, SoTienHoanTra)
                        VALUES (@Madonhang, @Sodienthoaikhachhang, @Manhanvien, @PhantramHoanTra, @TongTienDonHang, @SoTienHoanTra)";
                    
                    SqlParameter[] insertParams = {
                        new SqlParameter("@Madonhang", orderId),
                        new SqlParameter("@Sodienthoaikhachhang", customerPhone),
                        new SqlParameter("@Manhanvien", currentEmployeeId),
                        new SqlParameter("@PhantramHoanTra", refundPercentage),
                        new SqlParameter("@TongTienDonHang", originalTotal),
                        new SqlParameter("@SoTienHoanTra", totalRefundAmount)
                    };
                    
                    int rowsInserted = db.ExecuteNonQueryWithTransaction(insertRefundQuery, insertParams);
                    
                    // Debug: Kiểm tra xem có insert thành công không
                    if (rowsInserted > 0)
                    {
                        MessageBox.Show($"✅ Đã lưu thành công vào DanhSachHoanTra:\nĐơn hàng: {orderId}\nSố tiền: {totalRefundAmount:N0} VNĐ", "Debug Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"❌ Lỗi: Không thể lưu vào DanhSachHoanTra\nrowsInserted = {rowsInserted}", "Debug Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception insertEx)
                {
                    MessageBox.Show($"❌ Lỗi INSERT vào DanhSachHoanTra:\n{insertEx.Message}", "Debug Insert Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception($"Lỗi khi lưu thông tin hoàn trả: {insertEx.Message}");
                }
                
                // Cập nhật trạng thái yêu cầu hoàn trả
                string updateRequestQuery = @"
                    UPDATE YeuCauHoanDon 
                    SET Trangthai = 'Đã chấp nhận', 
                        Ngaycapnhat = GETDATE(),
                        Manhanvien = @Manhanvien
                    WHERE ID = @ID";
                
                SqlParameter[] requestParams = {
                    new SqlParameter("@ID", selectedRequestId),
                    new SqlParameter("@Manhanvien", currentEmployeeId)
                };
                
                db.ExecuteNonQueryWithTransaction(updateRequestQuery, requestParams);
                
                // Commit transaction
                db.CommitTransaction();
                MessageBox.Show($"✅ Transaction đã được commit thành công!", "Debug Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Rollback transaction nếu có lỗi
                MessageBox.Show($"❌ Lỗi trong ProcessRefund:\n{ex.Message}", "Debug Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                db.RollbackTransaction();
                throw;
            }
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (selectedRequestId == -1)
            {
                MessageBox.Show("Vui lòng chọn yêu cầu hoàn trả cần xử lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn từ chối yêu cầu hoàn trả này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string updateQuery = @"
                        UPDATE YeuCauHoanDon 
                        SET Trangthai = 'Đã từ chối', 
                            Ngaycapnhat = GETDATE(),
                            Manhanvien = @Manhanvien
                        WHERE ID = @ID";

                    SqlParameter[] parameters = {
                        new SqlParameter("@ID", selectedRequestId),
                        new SqlParameter("@Manhanvien", currentEmployeeId)
                    };

                    db.ExecuteNonQuery(updateQuery, parameters);
                    MessageBox.Show("Đã từ chối yêu cầu hoàn trả!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadReturnRequests();
                    ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi từ chối yêu cầu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadReturnRequests();
            ClearSelection();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (printData == null || printData.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (printData == null || printData.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.FileName = "DanhSachYeuCauHoanTra.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder csv = new StringBuilder();
                    
                    // Header
                    csv.AppendLine("Mã đơn hàng,SĐT khách hàng,Mã nhân viên,Lý do,Mô tả,Yêu cầu,Ngày tạo");

                    // Data
                    foreach (DataRow row in printData.Rows)
                    {
                        csv.AppendLine($"{row["Madonhang"]},{row["Sodienthoaikhachhang"]},{row["Manhanvien"]},{row["Lydo"]},{row["Mota"]},{row["YeuCau"]},{row["Ngaytao"]:dd/MM/yyyy HH:mm}");
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file CSV thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvReturnRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvReturnRequests.Rows[e.RowIndex];
                selectedRequestId = Convert.ToInt32(row.Cells["ID"].Value);
                
                // Hiển thị chi tiết
                txtOrderId.Text = row.Cells["Madonhang"].Value.ToString();
                txtCustomerPhone.Text = row.Cells["Sodienthoaikhachhang"].Value.ToString();
                txtReason.Text = row.Cells["Lydo"].Value.ToString();
                txtDescription.Text = row.Cells["Mota"].Value.ToString();
                txtRequest.Text = row.Cells["YeuCau"].Value.ToString();
                txtCreatedDate.Text = Convert.ToDateTime(row.Cells["Ngaytao"].Value).ToString("dd/MM/yyyy HH:mm");
            }
        }

        private void dgvReturnRequests_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Empty method to resolve CS1061 error
        }

        private void ClearSelection()
        {
            selectedRequestId = -1;
            txtOrderId.Clear();
            txtCustomerPhone.Clear();
            txtReason.Clear();
            txtDescription.Clear();
            txtRequest.Clear();
            txtCreatedDate.Clear();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (printData == null || printData.Rows.Count == 0)
                return;

            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;

            float yPosition = 50;
            float xPosition = 50;
            float lineHeight = 25;

            // Title
            g.DrawString("DANH SÁCH YÊU CẦU HOÀN TRẢ", headerFont, brush, xPosition, yPosition);
            yPosition += lineHeight * 2;

            // Headers
            string[] headers = { "Mã đơn hàng", "SĐT khách hàng", "Mã nhân viên", "Lý do", "Mô tả", "Yêu cầu", "Ngày tạo" };
            float[] columnWidths = { 100, 120, 120, 150, 200, 150, 120 };

            for (int i = 0; i < headers.Length; i++)
            {
                g.DrawString(headers[i], headerFont, brush, xPosition, yPosition);
                xPosition += columnWidths[i];
            }
            yPosition += lineHeight;
            xPosition = 50;

            // Data
            foreach (DataRow row in printData.Rows)
            {
                if (yPosition > e.MarginBounds.Height - 100)
                {
                    e.HasMorePages = true;
                    return;
                }

                string[] values = {
                    row["Madonhang"].ToString(),
                    row["Sodienthoaikhachhang"].ToString(),
                    row["Manhanvien"].ToString(),
                    row["Lydo"].ToString(),
                    row["Mota"].ToString(),
                    row["YeuCau"].ToString(),
                    Convert.ToDateTime(row["Ngaytao"]).ToString("dd/MM/yyyy HH:mm")
                };

                for (int i = 0; i < values.Length; i++)
                {
                    g.DrawString(values[i], font, brush, xPosition, yPosition);
                    xPosition += columnWidths[i];
                }
                yPosition += lineHeight;
                xPosition = 50;
            }
        }

        private void pnlDetails_Paint(object sender, PaintEventArgs e)
        {
            // Empty method to resolve CS1061 error
        }

        private void txtRequest_TextChanged(object sender, EventArgs e)
        {
            // Empty method to resolve CS1061 error
        }
    }
}

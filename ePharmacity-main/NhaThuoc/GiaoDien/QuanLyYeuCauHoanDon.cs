using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhaThuoc.GiaoDien
{
    public partial class QuanLyYeuCauHoanDon : Form
    {
        private string maNVHienTai;
        DataConnection dc = new DataConnection();
        public QuanLyYeuCauHoanDon(string maNV)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
        }

        public QuanLyYeuCauHoanDon()
        {
            InitializeComponent();
        }
        private void LoadDanhSachYeuCau(string keyword = "", DateTime? from = null, DateTime? to = null)
        {
            string sql = @"
            SELECT yc.ID,
                   yc.Madonhang AS [Mã đơn hàng],
                   yc.Sodienthoaikhachhang AS [Số điện thoại],
                   yc.Lydo AS [Lý do],
                   yc.Mota AS [Mô tả chi tiết],
                   yc.Trangthai AS [Trạng thái],
                   yc.Yeucau AS [Yêu cầu],
                   yc.Manhanvien AS [Mã NV yêu cầu],
                   yc.Ngaytao AS [Ngày tạo]
            FROM Yeucauhoandon yc
            WHERE 1=1";

            List<SqlParameter> prms = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (yc.Sodienthoai LIKE @kw + '%' OR yc.Madonhang LIKE @kw + '%' OR yc.Lydo LIKE N'%' + @kw + '%')";
                prms.Add(new SqlParameter("@kw", keyword));
            }

            if (from.HasValue && to.HasValue)
            {
                sql += " AND yc.Ngaytao BETWEEN @from AND @to";
                prms.Add(new SqlParameter("@from", from.Value));
                prms.Add(new SqlParameter("@to", to.Value));
            }

            sql += " ORDER BY yc.Ngaytao DESC";

            DataTable dt = dc.GetData(sql, prms.ToArray());
            dgvYeuCau.DataSource = dt;
        }

        private void QuanLyYeuCauHoanDon_Load(object sender, EventArgs e)
        {
            dgvYeuCau.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvYeuCau.MultiSelect = false;
            dgvYeuCau.AutoGenerateColumns = true;
            dgvYeuCau.AllowUserToAddRows = false;

            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;

            LoadDanhSachYeuCau();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1);
            LoadDanhSachYeuCau(keyword, fromDate, toDate);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Taoyeucauhoandon frm = new Taoyeucauhoandon(maNVHienTai);
            frm.ShowDialog();
            LoadDanhSachYeuCau();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvYeuCau.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn yêu cầu cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvYeuCau.SelectedRows[0];

            int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            string trangThai = selectedRow.Cells["Trạng thái"].Value?.ToString() ?? "";

            // ✅ Chỉ cho phép xóa khi trạng thái là "Đã gửi yêu cầu"
            if (!trangThai.Equals("Đã gửi yêu cầu", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("❌ Chỉ có thể xóa các yêu cầu ở trạng thái 'Đã gửi yêu cầu'.",
                                "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa yêu cầu này?",
                                              "Xác nhận",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                string sql = "DELETE FROM Yeucauhoandon WHERE ID = @id";
                SqlParameter p = new SqlParameter("@id", id);
                dc.GetData(sql, p);

                MessageBox.Show("✅ Đã xóa yêu cầu thành công!",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDanhSachYeuCau();
            }
        }

        private void dgvYeuCau_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void dgvYeuCau_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được nhấn đúp
                DataGridViewRow row = dgvYeuCau.Rows[e.RowIndex];

                // Lấy ID yêu cầu từ cột "ID" trong DataGridView
                int idYeuCau = Convert.ToInt32(row.Cells["ID"].Value);

                // Mở form Tạo yêu cầu ở chế độ Sửa
                Taoyeucauhoandon frm = new Taoyeucauhoandon(maNVHienTai, idYeuCau); // maNVHienTai đã truyền từ frmCSKH
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();

                // Sau khi đóng form, load lại danh sách để cập nhật thay đổi
                LoadDanhSachYeuCau();
            }
        }
    }
}

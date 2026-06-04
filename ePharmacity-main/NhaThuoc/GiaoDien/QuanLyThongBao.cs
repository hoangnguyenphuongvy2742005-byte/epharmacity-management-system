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
    public partial class QuanLyThongBao : Form
    {
        DataConnection dc = new DataConnection();
        private string maNVHienTai;
        public QuanLyThongBao(string maNV)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
        }


        public QuanLyThongBao()
        {
            InitializeComponent();
        }

        private void LoadTatCaThongBao(string keyword = "", DateTime? from = null, DateTime? to = null)
        {
            string sql = @"
        SELECT 
            tb.ID,
            tb.Tieude AS [Tiêu đề],
            tb.Noidung AS [Nội dung],
            tb.Sodienthoai AS [Số điện thoại KH],
            kh.Hovaten AS [Họ tên KH],
            tb.Ngaygui AS [Ngày gửi],
            tb.Nguoigui AS [Mã NV gửi]
        FROM Thongbaokhachhang tb
        LEFT JOIN Thongtinkhachhang kh ON tb.Sodienthoai = kh.Sodienthoai
        WHERE 1=1";

            List<SqlParameter> prms = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " AND (tb.Sodienthoai LIKE @kw + '%' OR tb.Tieude LIKE N'%' + @kw + '%' OR kh.Hovaten LIKE N'%' + @kw + '%')";
                prms.Add(new SqlParameter("@kw", keyword));
            }

            if (from.HasValue && to.HasValue)
            {
                sql += " AND tb.Ngaygui BETWEEN @from AND @to";
                prms.Add(new SqlParameter("@from", from.Value));
                prms.Add(new SqlParameter("@to", to.Value));
            }

            sql += " ORDER BY tb.Ngaygui DESC";

            DataTable dt = dc.GetData(sql, prms.ToArray());
            dgvThongBao.DataSource = dt;
        }
        private void QuanLyThongBao_Load(object sender, EventArgs e)
        {
            dgvThongBao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongBao.MultiSelect = false;
            dgvThongBao.AutoGenerateColumns = true;
            dgvThongBao.AllowUserToAddRows = false;

            // 🟡 Thêm cột checkbox để chọn xóa nếu chưa có
            if (!dgvThongBao.Columns.Contains("Chon"))
            {
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "Chon";
                chkCol.HeaderText = "";
                chkCol.Width = 30;
                chkCol.Frozen = true;  // giữ cố định khi cuộn ngang
                dgvThongBao.Columns.Insert(0, chkCol);
            }

            // 📅 Đặt mặc định ngày tìm kiếm
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;

            LoadTatCaThongBao();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            GuiThongBaoChoKhachHang frm = new GuiThongBaoChoKhachHang(maNVHienTai);
            frm.ShowDialog();
            LoadTatCaThongBao();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> idsToDelete = new List<int>();

            foreach (DataGridViewRow row in dgvThongBao.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["Chon"].Value ?? false);
                if (isChecked)
                {
                    int id = Convert.ToInt32(row.Cells["ID"].Value);
                    idsToDelete.Add(id);
                }
            }

            if (idsToDelete.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một thông báo để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Xác nhận trước khi xóa
            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa {idsToDelete.Count} thông báo?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(dc.connection))
                {
                    conn.Open();
                    foreach (int id in idsToDelete)
                    {
                        string sql = "DELETE FROM Thongbaokhachhang WHERE ID = @id"; // 👈 Sửa đúng tên bảng
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("✅ Đã xóa thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ Load lại danh sách sau khi xóa
                LoadTatCaThongBao();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1);
            LoadTatCaThongBao(keyword, fromDate, toDate);
        }

        private void dgvThongBao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvThongBao.Columns["Chon"].Index)
            {
                dgvThongBao.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkSelectAll.Checked;
            foreach (DataGridViewRow row in dgvThongBao.Rows)
            {
                row.Cells["Chon"].Value = isChecked;
            }
        }
    }
}

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
    public partial class GuiThongBaoChoKhachHang : Form
    {
        DataConnection dc = new DataConnection();
        public GuiThongBaoChoKhachHang()
        {
            InitializeComponent();
        }
        private string maNhanVien;

        public GuiThongBaoChoKhachHang(string maNV)
        {
            InitializeComponent();
            this.maNhanVien = maNV;
        }
        private void GuiThongBaoChoKhachHang_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableDragByControl(this, panel1);
            LoadKhachHang();

            // ✅ Thêm cột checkbox nếu chưa có
            if (!dgvKhachHang.Columns.Contains("Chon"))
            {
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "Chon";
                chkCol.HeaderText = "";
                chkCol.Width = 30;
                chkCol.ReadOnly = false;
                chkCol.Frozen = true;  // ✅ Cố định khi kéo ngang
                dgvKhachHang.Columns.Insert(0, chkCol);
            }

            dgvKhachHang.AllowUserToAddRows = false;
        }
        private void LoadKhachHang(string keyword = "")
        {
            string sql = @"
                SELECT 
                    Hovaten AS [Họ và tên], 
                    Sodienthoai AS [Số điện thoại], 
                    Diemtichluy AS [Điểm tích lũy]
                FROM Thongtinkhachhang";

            List<SqlParameter> prms = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += " WHERE Sodienthoai LIKE @kw + '%' OR Hovaten LIKE N'%' + @kw + '%'";
                prms.Add(new SqlParameter("@kw", keyword));
            }

            DataTable dt = dc.GetData(sql, prms.ToArray());
            dgvKhachHang.DataSource = dt;
        }

        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvKhachHang.Columns["Chon"].Index)
            {
                dgvKhachHang.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void chkSeclectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool checkAll = chkSeclectAll.Checked;
            foreach (DataGridViewRow row in dgvKhachHang.Rows)
            {
                row.Cells["Chon"].Value = checkAll;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string tieuDe = txtTieuDe.Text.Trim();
            string noiDung = txtNoiDung.Text.Trim();

            if (string.IsNullOrEmpty(tieuDe) || string.IsNullOrEmpty(noiDung))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề và nội dung thông báo!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> danhSachSDT = new List<string>();

            // ✅ Nếu chọn "Chọn tất cả" → gửi cho toàn bộ dòng đang hiển thị
            if (chkSeclectAll.Checked)
            {
                foreach (DataGridViewRow row in dgvKhachHang.Rows)
                {
                    string sdt = row.Cells["Số điện thoại"].Value?.ToString();
                    if (!string.IsNullOrEmpty(sdt))
                        danhSachSDT.Add(sdt);
                }
            }
            else
            {
                // ✅ Nếu không → gửi cho các dòng có tick
                foreach (DataGridViewRow row in dgvKhachHang.Rows)
                {
                    bool isChecked = Convert.ToBoolean(row.Cells["Chon"].Value ?? false);
                    if (isChecked)
                    {
                        string sdt = row.Cells["Số điện thoại"].Value?.ToString();
                        if (!string.IsNullOrEmpty(sdt))
                            danhSachSDT.Add(sdt);
                    }
                }

                if (danhSachSDT.Count == 0)
                {
                    MessageBox.Show("Vui lòng tick chọn ít nhất một khách hàng để gửi!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 📝 Lưu vào bảng Thongbaohethong (ví dụ)
            using (SqlConnection conn = new SqlConnection(dc.connection))
            {
                conn.Open();
                foreach (string sdt in danhSachSDT)
                {
                    string sql = @"INSERT INTO Thongbaokhachhang (Tieude, Noidung, Sodienthoai, Ngaygui, Nguoigui)
               VALUES (@td, @nd, @sdt, GETDATE(), @nv)";


                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@td", tieuDe);
                        cmd.Parameters.AddWithValue("@nd", noiDung);
                        cmd.Parameters.AddWithValue("@sdt", sdt);
                        cmd.Parameters.AddWithValue("@nv", maNhanVien);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show($"✅ Đã gửi thông báo cho {danhSachSDT.Count} khách hàng.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtTieuDe.Clear();
            txtNoiDung.Clear();
            chkSeclectAll.Checked = false;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim(); // ✅ Lấy từ ô tìm kiếm

            string sql = @"
                        SELECT 
                        Hovaten AS [Họ và tên], 
                        Sodienthoai AS [Số điện thoại], 
                        Diemtichluy AS [Điểm tích lũy]
                        FROM Thongtinkhachhang
                        WHERE Sodienthoai LIKE @kw + '%' OR Hovaten LIKE N'%' + @kw + '%'";

            List<SqlParameter> prms = new List<SqlParameter>
            {
                new SqlParameter("@kw", keyword)
            };

            DataTable dt = dc.GetData(sql, prms.ToArray());
            dgvKhachHang.DataSource = dt;

            // ✅ Nếu chưa có cột checkbox thì thêm vào đầu
            if (!dgvKhachHang.Columns.Contains("Chon"))
            {
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "Chon";
                chkCol.HeaderText = "";
                chkCol.Width = 30;
                chkCol.ReadOnly = false;
                chkCol.Frozen = true;  // cố định khi cuộn ngang
                dgvKhachHang.Columns.Insert(0, chkCol);
            }

            // ✅ Reset checkbox chọn tất cả mỗi lần tìm kiếm
            chkSeclectAll.Checked = false;
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

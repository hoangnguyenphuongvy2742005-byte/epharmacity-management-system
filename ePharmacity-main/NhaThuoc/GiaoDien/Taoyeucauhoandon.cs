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
    public partial class Taoyeucauhoandon : Form
    {
        private string maNVHienTai;
        private int yeuCauId = -1; // nếu -1 => tạo mới, ngược lại => sửa
        DataConnection dc = new DataConnection();

        // 👉 Constructor tạo mới
        public Taoyeucauhoandon(string maNV)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
        }
        // 👉 Constructor sửa
        public Taoyeucauhoandon(string maNV, int idYeuCau)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
            this.yeuCauId = idYeuCau;
        }
        public void SetOrderData(string maDH, string sdt)
        {
            txtMaDonHang.Text = maDH;
            txtSDT.Text = sdt;
        }
        private void Taoyeucauhoandon_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableDragByControl(this, panel1);
            txtNVYeuCau.Text = maNVHienTai;

            // Load các combobox lý do, trạng thái, yêu cầu
            cbbTrangThai.Items.Clear();
            cbbTrangThai.Items.AddRange(new object[]
            {
                "Đã gửi yêu cầu",
                "Đã tiếp nhận yêu cầu",
                "Đã hoàn hàng"
            });
            cbbTrangThai.SelectedIndex = 0;

            // nếu sửa thì load dữ liệu chi tiết
            if (yeuCauId != -1)
            {
                LoadYeuCauChiTiet(yeuCauId);
            }
        }
        private void LoadYeuCauChiTiet(int id)
        {
            string sql = "SELECT * FROM Yeucauhoandon WHERE ID = @id";
            SqlParameter p = new SqlParameter("@id", id);
            DataTable dt = dc.GetData(sql, p);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                txtMaDonHang.Text = r["Madonhang"].ToString();
                txtSDT.Text = r["Sodienthoaikhachhang"].ToString();
                txtNVYeuCau.Text = r["Manhanvien"].ToString();
                cbbLyDo.Text = r["Lydo"].ToString();
                txtMoTa.Text = r["Mota"].ToString();
                cbbTrangThai.Text = r["Trangthai"].ToString();
                cbbYeuCau.Text = r["Yeucau"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDonHang.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã đơn hàng trước khi xem chi tiết!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMaDonHang.Text.Trim(), out int madonhang))
            {
                MessageBox.Show("Mã đơn hàng không hợp lệ!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mở form chi tiết đơn hàng
            frmLSDHChiTiet frm = new frmLSDHChiTiet(madonhang);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string madon = txtMaDonHang.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string lydo = cbbLyDo.Text.Trim();
            string mota = txtMoTa.Text.Trim();
            string trangthai = cbbTrangThai.Text.Trim();
            string yeucau = cbbYeuCau.Text.Trim();
            string manv = maNVHienTai;

            if (string.IsNullOrEmpty(madon) || string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã đơn hàng và SĐT!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(dc.connection))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (yeuCauId == -1)
                {
                    // Tạo mới - KHÔNG tự động thêm mã nhân viên
                    cmd.CommandText = @"
                        INSERT INTO Yeucauhoandon (Madonhang,Sodienthoaikhachhang, Lydo, Mota, Trangthai, Yeucau, Ngaytao)
                        VALUES (@madon, @sdt, @lydo, @mota, @trangthai, @yeucau, GETDATE())";
                }
                else
                {
                    // Cập nhật
                    cmd.CommandText = @"
                        UPDATE Yeucauhoandon
                        SET Madonhang = @madon,
                            Sodienthoaikhachhang = @sdt,
                            Manhanvien = @manv,
                            Lydo = @lydo,
                            Mota = @mota,
                            Trangthai = @trangthai,
                            Yeucau = @yeucau
                        WHERE ID = @id";
                    cmd.Parameters.AddWithValue("@id", yeuCauId);
                }

                cmd.Parameters.AddWithValue("@madon", madon);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                
                if (yeuCauId != -1) // Chỉ thêm mã nhân viên khi cập nhật, không thêm khi tạo mới
                {
                    if (string.IsNullOrWhiteSpace(manv))
                        cmd.Parameters.AddWithValue("@manv", txtNVYeuCau.Text.Trim());
                    else
                        cmd.Parameters.AddWithValue("@manv", manv);
                }
                
                cmd.Parameters.AddWithValue("@lydo", lydo);
                cmd.Parameters.AddWithValue("@mota", mota);
                cmd.Parameters.AddWithValue("@trangthai", trangthai);
                cmd.Parameters.AddWithValue("@yeucau", yeucau);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("✅ Lưu yêu cầu thành công!", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Không có bản ghi nào được lưu.", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cbbYeuCau_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

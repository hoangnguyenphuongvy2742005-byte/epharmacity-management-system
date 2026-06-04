using NhaThuoc.GiaoDien;
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
using static NhaThuoc.frmInfoKH;

namespace NhaThuoc
{
    public partial class frmInfoChiTietKH : Form
    {
        DataConnection dc = new DataConnection();
        private string sdt;
        private FormMode mode;
        private string maNhanVienHienTai;
        public frmInfoChiTietKH(string sodienthoai, FormMode mode = FormMode.View, string maNV = null)
        {
            InitializeComponent();
            this.sdt = sodienthoai;
            this.mode = mode;
            this.maNhanVienHienTai = maNV;
        }

        private void LoadThongTinKhachHang()
        {
            string sql = "SELECT * FROM Thongtinkhachhang WHERE Sodienthoai = @sdt";
            SqlParameter param = new SqlParameter("@sdt", sdt);
            DataTable dt = dc.GetData(sql, param);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                txtPhone.Text = r["Sodienthoai"].ToString();
                txtName.Text = r["Hovaten"].ToString();
                txtDiemTichLuy.Text = r["Diemtichluy"].ToString();
                txtMaNV.Text = r["Manhanvien"].ToString();
            }
        }
        private void frmInfoChiTietKH_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableDragByControl(this, panel1);

            if (mode == FormMode.View)
            {
                LoadThongTinKhachHang();
                btnLuu.Visible = false;
                txtPhone.Enabled = false;
                txtDiemTichLuy.Enabled = false;
                txtName.Enabled = false;
                txtMaNV.Enabled = false;
            }
            else if (mode == FormMode.Add)
            {
                txtPhone.Clear();
                txtName.Clear();
                txtDiemTichLuy.Text = "0";
                txtMaNV.Text = maNhanVienHienTai;

                btnLuu.Visible = true;
                btnLSDH.Visible = false;

                txtPhone.Enabled = true;
            }
            else if (mode == FormMode.Edit)
            {
                LoadThongTinKhachHang();
                btnLuu.Visible = true;
                btnLSDH.Visible = false;
                txtPhone.Enabled = false;
                txtDiemTichLuy.Enabled = false;
                txtMaNV.Text = maNhanVienHienTai;
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLSDH_Click(object sender, EventArgs e)
        {
            string sdt = txtPhone.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng chọn khách hàng trước khi xem lịch sử mua hàng!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form lịch sử đơn hàng, truyền số điện thoại để lọc sẵn
            frmLichSuDonHang frm = new frmLichSuDonHang(sdt);
            frm.ShowDialog();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sdt = txtPhone.Text.Trim();
            string hoTen = txtName.Text.Trim();
            int diem = 0;
            int.TryParse(txtDiemTichLuy.Text.Trim(), out diem);

            string maNV = (this.mode == FormMode.Add)
                ? maNhanVienHienTai
                : txtMaNV.Text.Trim();

            // 🧠 1. Kiểm tra số điện thoại
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập Số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Bắt buộc đúng 10 chữ số
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🧠 2. Kiểm tra họ tên
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập Họ tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(dc.connection))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    if (this.mode == FormMode.Add)
                    {
                        // 🔎 Kiểm tra SDT đã tồn tại chưa
                        string checkSql = "SELECT COUNT(*) FROM Thongtinkhachhang WHERE Sodienthoai = @sdt";
                        using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@sdt", sdt);
                            int count = (int)checkCmd.ExecuteScalar();
                            if (count > 0)
                            {
                                MessageBox.Show("Số điện thoại này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        cmd.CommandText = @"
                    INSERT INTO Thongtinkhachhang (Sodienthoai, Hovaten, Diemtichluy, Manhanvien)
                    VALUES (@sdt, @ten, @diem, @manv)";
                    }
                    else if (this.mode == FormMode.Edit)
                    {
                        cmd.CommandText = @"
                    UPDATE Thongtinkhachhang
                    SET Hovaten = @ten,
                        Diemtichluy = @diem,
                        Manhanvien = @manv
                    WHERE Sodienthoai = @sdt";
                    }
                    else
                    {
                        MessageBox.Show("Chế độ không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@sdt", sdt);
                    cmd.Parameters.AddWithValue("@ten", hoTen);
                    cmd.Parameters.AddWithValue("@diem", diem);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("✅ Lưu thông tin khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Không có bản ghi nào được lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLSDH_Click_1(object sender, EventArgs e)
        {
            string sdt = txtPhone.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng chọn khách hàng trước khi xem lịch sử mua hàng!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form lịch sử đơn hàng, truyền số điện thoại để lọc sẵn
            frmLichSuDonHang frm = new frmLichSuDonHang(sdt);
            frm.ShowDialog();
        }
    }
}

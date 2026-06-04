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

namespace NhaThuoc
{
    public partial class frmInfoKH : Form
    {
        DataConnection dc = new DataConnection();
        private string maNVHienTai;

        public frmInfoKH(string maNV)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
        }
        public enum FormMode
        {
            View,
            Add,
            Edit,
            Delete
        }
        private void LoadAllCustomers()
        {
            string sql = @"
                        SELECT  kh.Hovaten AS HoTen,
                                kh.Sodienthoai AS SoDienThoai,
                                kh.Diemtichluy AS DiemTichLuy
                        FROM Thongtinkhachhang kh";

            DataTable dt = dc.GetData(sql);
            dgvInfoKH.DataSource = dt;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadAllCustomers();
                return;
            }

            string sql = @"
                        SELECT  kh.Hovaten AS HoTen,
                                kh.Sodienthoai AS SoDienThoai,
                                kh.Diemtichluy AS DiemTichLuy
                        FROM Thongtinkhachhang kh
                        WHERE kh.Sodienthoai LIKE @kw + '%'
                            OR kh.Hovaten LIKE N'%' + @kw + '%'";

            SqlParameter param = new SqlParameter("@kw", keyword);
            DataTable dt = dc.GetData(sql, param);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("❌ Không tìm thấy thông tin khách hàng phù hợp.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            dgvInfoKH.DataSource = dt;
        }

        private void frmInfoKH_Load(object sender, EventArgs e)
        {
            LoadAllCustomers();
        }


        private void dgvInfoKH_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string sodt = dgvInfoKH.Rows[e.RowIndex].Cells["SoDienThoai"].Value.ToString();
                frmInfoChiTietKH frm = new frmInfoChiTietKH(sodt, FormMode.View);
                frm.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInfoKH.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sdt = dgvInfoKH.SelectedRows[0].Cells["SoDienThoai"].Value.ToString();

            DialogResult dr = MessageBox.Show(
                $"Xóa khách hàng {sdt} sẽ đồng thời xóa:\n- Tất cả đơn hàng\n- Chi tiết đơn hàng\n- Phản hồi\n- Yêu cầu hoàn đơn\n- Thông báo liên quan\n\nBạn có chắc chắn muốn xóa?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(dc.connection))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        // 1️⃣ Xóa chi tiết đơn hàng (liên quan tới đơn hàng của KH)
                        string sqlCTDH = @"
                DELETE FROM Chitietdonhang 
                WHERE Madonhang IN (
                    SELECT Madonhang FROM Donhang WHERE Sodienthoaikhachhang = @sdt
                )";
                        using (SqlCommand cmd = new SqlCommand(sqlCTDH, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        // 2️⃣ Xóa yêu cầu hoàn đơn
                        string sqlYCHD = "DELETE FROM YeuCauHoanDon WHERE Sodienthoaikhachhang = @sdt";
                        using (SqlCommand cmd = new SqlCommand(sqlYCHD, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        // 3️⃣ Xóa đơn hàng
                        string sqlDH = "DELETE FROM Donhang WHERE Sodienthoaikhachhang = @sdt";
                        using (SqlCommand cmd = new SqlCommand(sqlDH, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        // 4️⃣ Xóa phản hồi KH
                        string sqlPH = "DELETE FROM Phanhoikhachhang WHERE Sodienthoai = @sdt";
                        using (SqlCommand cmd = new SqlCommand(sqlPH, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        // 5️⃣ Xóa thông báo KH
                        string sqlTB = "DELETE FROM Thongbaokhachhang WHERE Sodienthoai = @sdt";
                        using (SqlCommand cmd = new SqlCommand(sqlTB, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        // 6️⃣ Cuối cùng xóa khách hàng
                        string sqlKH = "DELETE FROM Thongtinkhachhang WHERE Sodienthoai = @sdt";
                        using (SqlCommand cmd = new SqlCommand(sqlKH, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@sdt", sdt);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        MessageBox.Show("Xóa khách hàng và toàn bộ dữ liệu liên quan thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllCustomers();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInfoKH.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sdt = dgvInfoKH.SelectedRows[0].Cells["SoDienThoai"].Value.ToString();
            frmInfoChiTietKH frm = new frmInfoChiTietKH(sdt, FormMode.Edit, maNVHienTai);
            frm.ShowDialog();
            LoadAllCustomers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmInfoChiTietKH frm = new frmInfoChiTietKH(null, FormMode.Add, maNVHienTai);
            frm.ShowDialog();
            LoadAllCustomers();
        }
    }
}

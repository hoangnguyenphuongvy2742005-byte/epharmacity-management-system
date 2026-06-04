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

namespace NhaThuoc
{
    public partial class frmChiTietPhanHoi : Form
    {
        private string sdt;
        DataConnection dc = new DataConnection();
        private int phanHoiId = -1;

        private string maNVHienTai;

        public frmChiTietPhanHoi(int idPhanHoi, string sdt, string maNV)
        {
            InitializeComponent();
            this.phanHoiId = idPhanHoi;
            this.sdt = sdt;
            this.maNVHienTai = maNV;
        }

        private void frmChiTietPhanHoi_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableDragByControl(this, panel1);
            if (phanHoiId == -1)
            {
                MessageBox.Show("Không xác định được phản hồi cần xem!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadThongTinVaPhanHoi();
            txtNgPhanHoi.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtNVPhanHoi.Text = maNVHienTai;
        }

        private void LoadThongTinVaPhanHoi()
        {
            string sql = @"
                    SELECT kh.Sodienthoai, kh.Hovaten, ph.ID, ph.Phanhoi, ph.Traloi
                    FROM Thongtinkhachhang kh
                    JOIN Phanhoikhachhang ph ON kh.Sodienthoai = ph.Sodienthoai
                    WHERE ph.ID = @id";

            SqlParameter p = new SqlParameter("@id", phanHoiId);
            DataTable dt = dc.GetData(sql, p);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                txtPhone.Text = r["Sodienthoai"].ToString();
                txtName.Text = r["Hovaten"].ToString();
                txtPHKH.Text = r["Phanhoi"] != DBNull.Value ? r["Phanhoi"].ToString() : "";
                txtTraLoi.Text = r["Traloi"] != DBNull.Value ? r["Traloi"].ToString() : "";
            }
            else
            {
                MessageBox.Show("Không tìm thấy phản hồi này!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sdt = txtPhone.Text.Trim();
            string traLoi = txtTraLoi.Text.Trim();
            string maNV = string.IsNullOrWhiteSpace(txtNVPhanHoi.Text) ? null : txtNVPhanHoi.Text.Trim();

            if (string.IsNullOrEmpty(traLoi))
            {
                MessageBox.Show("Vui lòng nhập nội dung trả lời!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phanHoiId == -1)
            {
                MessageBox.Show("Không xác định được phản hồi để cập nhật!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = @"
                    UPDATE Phanhoikhachhang
                    SET Traloi = @traloi,
                    Manhanvien = @manv
                    WHERE ID = @id";

            using (SqlConnection conn = new SqlConnection(dc.connection))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@traloi", traLoi);
                        if (maNV != null)
                            cmd.Parameters.AddWithValue("@manv", maNV);
                        else
                            cmd.Parameters.AddWithValue("@manv", DBNull.Value);

                        cmd.Parameters.AddWithValue("@id", phanHoiId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Cập nhật trả lời thành công!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Lỗi khi cập nhật trả lời: " + ex.Message,
                                    "Lỗi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHoanDon_Click(object sender, EventArgs e)
        {
            Taoyeucauhoandon frmYeuCau = new Taoyeucauhoandon(maNVHienTai);
            frmYeuCau.StartPosition = FormStartPosition.CenterParent;
        }
    }
}

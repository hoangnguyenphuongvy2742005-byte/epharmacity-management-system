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
    public partial class frmLichSuDonHang : Form
    {
        DataConnection dc = new DataConnection();
        private string sdtFilter;
        public frmLichSuDonHang()
        {
            InitializeComponent();
        }
        public frmLichSuDonHang(string sdt)
        {
            InitializeComponent();
            this.sdtFilter = sdt;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void LoadDonHang()
        {
            string keyword = txtSearch.Text.Trim();
            string madon = txtMDH.Text.Trim();
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

            string sql = @"
                        SELECT dh.Madonhang,
                        kh.Sodienthoai AS SoDienThoai,
                        kh.Hovaten AS HoTen,
                        dh.Ngaytaodon,
                        SUM(ct.Tongtiensanpham) AS TongTien
                        FROM Donhang dh
                        JOIN Thongtinkhachhang kh ON dh.Sodienthoaikhachhang = kh.Sodienthoai
                        JOIN Chitietdonhang ct ON dh.Madonhang = ct.Madonhang
                        WHERE dh.Ngaytaodon BETWEEN @fromDate AND @toDate";

            List<SqlParameter> prms = new List<SqlParameter>
            {
                    new SqlParameter("@fromDate", fromDate),
                    new SqlParameter("@toDate", toDate)
            };

            // 🔸 Lọc theo KH (SĐT hoặc Họ tên)
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND (kh.Sodienthoai LIKE @kw + '%' OR kh.Hovaten LIKE N'%' + @kw + '%')";
                prms.Add(new SqlParameter("@kw", keyword));
            }

            // 🔸 Lọc theo Mã đơn hàng
            if (!string.IsNullOrEmpty(madon))
            {
                sql += " AND dh.Madonhang = @mdh";
                prms.Add(new SqlParameter("@mdh", madon));
            }

            sql += @"
                    GROUP BY dh.Madonhang, kh.Sodienthoai, kh.Hovaten, dh.Ngaytaodon
                    ORDER BY dh.Ngaytaodon DESC";

            DataTable dt = dc.GetData(sql, prms.ToArray());
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("❌ Không tìm thấy thông tin đơn hàng phù hợp.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            dgvLichSuDH.DataSource = dt;
        }

        private void frmLichSuDonHang_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableFormDrag(this);
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;

            if (!string.IsNullOrEmpty(sdtFilter))
            {
                txtSearch.Text = sdtFilter;
            }

            LoadDonHang();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDonHang();
        }

        private void dgvLichSuDH_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int madonhang = Convert.ToInt32(dgvLichSuDH.Rows[e.RowIndex].Cells["Madonhang"].Value);
                frmLSDHChiTiet frm = new frmLSDHChiTiet(madonhang);
                frm.ShowDialog();
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

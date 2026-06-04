using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NhaThuoc.GiaoDien;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace NhaThuoc
{
    public partial class frmPhanHoiKH : Form
    {
        DataConnection dc = new DataConnection();

        private string maNVHienTai;

        public frmPhanHoiKH(string maNV)
        {
            InitializeComponent();
            this.maNVHienTai = maNV;
        }
        public frmPhanHoiKH()
        {
            InitializeComponent();

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void frmPhanHoiKH_Load(object sender, EventArgs e)
        {
            FormMoveHelper.EnableFormDrag(this);
            dtpTuNgay.Value = new DateTime(1900, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            cbbRating.Items.Clear();
            cbbRating.Items.Add("Tất cả");
            cbbRating.Items.Add("5");
            cbbRating.Items.Add("4");
            cbbRating.Items.Add("3");
            cbbRating.Items.Add("2");
            cbbRating.Items.Add("1");
            cbbRating.SelectedIndex = 0;
            cbbTrangThai.SelectedIndex = 0;

            LoadPhanHoi();
        }
        private void LoadPhanHoi()
        {
            string keyword = txtTimKiem.Text.Trim();
            DateTime fromDate = dtpTuNgay.Value.Date;
            DateTime toDate = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
            string rating = cbbRating.SelectedItem?.ToString();
            string trangThaiLoc = cbbTrangThai.SelectedItem?.ToString();

            string sql = @"
        SELECT ID,
               Hovaten AS [Họ và tên KH],
               Sodienthoai AS [Số điện thoại],
               Phanhoi AS [Phản hồi KH],
               Traloi AS [Trả lời],
               Trangthai AS [Trạng thái],
               Rating AS [Đánh giá],
               Ngaytao AS [Ngày phản hồi],
               Manhanvien AS [Nhân viên phản hồi]
        FROM Phanhoikhachhang
        WHERE Ngaytao BETWEEN @from AND @to";

            List<SqlParameter> prms = new List<SqlParameter>
    {
        new SqlParameter("@from", fromDate),
        new SqlParameter("@to", toDate)
    };

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += " AND (Sodienthoai = @kw OR Hovaten LIKE N'%' + @kw + '%')";
                prms.Add(new SqlParameter("@kw", keyword));
            }

            // Lọc theo rating
            if (!string.IsNullOrEmpty(rating) && rating != "Tất cả")
            {
                sql += " AND Rating = @rating";
                prms.Add(new SqlParameter("@rating", rating));
            }

            // 🔸 Lọc theo trạng thái trả lời
            if (!string.IsNullOrEmpty(trangThaiLoc) && trangThaiLoc != "Tất cả")
            {
                if (trangThaiLoc == "Chưa trả lời")
                {
                    sql += " AND (Traloi IS NULL OR LTRIM(RTRIM(Traloi)) = '')";
                }
                else if (trangThaiLoc == "Đã trả lời")
                {
                    sql += " AND (Traloi IS NOT NULL AND LTRIM(RTRIM(Traloi)) <> '')";
                }
            }

            sql += " ORDER BY Ngaytao DESC";

            DataTable dt = dc.GetData(sql, prms.ToArray());
            dgvPhanHoi.DataSource = dt;
        }


        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadPhanHoi();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvPhanHoi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phản hồi để xử lý!", "Thông báo");
                return;
            }

            var row = dgvPhanHoi.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string sdt = row.Cells["Số điện thoại"].Value.ToString();

            frmChiTietPhanHoi frm = new frmChiTietPhanHoi(id, sdt, maNVHienTai);
            frm.ShowDialog();

            LoadPhanHoi();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPhanHoi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phản hồi cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvPhanHoi.SelectedRows[0].Cells["ID"].Value);
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa phản hồi này?",
                                              "Xác nhận",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                string sql = "DELETE FROM Phanhoikhachhang WHERE ID = @id";
                SqlParameter param = new SqlParameter("@id", id);
                dc.GetData(sql, param);
            }
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpTuNgay.Value.Date;
            DateTime toDate = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);

            string sql = @"
        SELECT kh.Hovaten, ph.Sodienthoai, ph.Phanhoi, ph.Traloi, ph.Rating, ph.Ngaytao
        FROM Phanhoikhachhang ph
        JOIN Thongtinkhachhang kh ON kh.Sodienthoai = ph.Sodienthoai
        WHERE ph.Ngaytao BETWEEN @fromDate AND @toDate
        ORDER BY ph.Ngaytao DESC";

            SqlParameter[] prms = new SqlParameter[]
            {
        new SqlParameter("@fromDate", fromDate),
        new SqlParameter("@toDate", toDate)
            };

            DataTable dt = new DataConnection().GetData(sql, prms);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("⚠️ Không có dữ liệu trong khoảng thời gian đã chọn.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Chọn nơi lưu file
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel File|*.xlsx";
            saveFile.Title = "Chọn nơi lưu báo cáo";
            saveFile.FileName = $"BaoCaoPhanHoi_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            if (saveFile.ShowDialog() != DialogResult.OK)
                return;

            FileInfo fileInfo = new FileInfo(saveFile.FileName);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                // --- Sheet 1: Danh sách phản hồi ---
                var ws = package.Workbook.Worksheets.Add("DanhSachPhanHoi");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                ws.Cells.AutoFitColumns();

                // --- Sheet 2: Tổng hợp rating ---
                var ws2 = package.Workbook.Worksheets.Add("TongHopRating");

                // Thống kê theo rating
                var groups = dt.AsEnumerable()
                               .GroupBy(r => r["Rating"].ToString())
                               .Select(g => new { Rating = g.Key, SoLuong = g.Count() })
                               .OrderBy(g => g.Rating)
                               .ToList();

                ws2.Cells[1, 1].Value = "Rating";
                ws2.Cells[1, 2].Value = "Số lượng";

                int row = 2;
                foreach (var g in groups)
                {
                    ws2.Cells[row, 1].Value = g.Rating;
                    ws2.Cells[row, 2].Value = g.SoLuong;
                    row++;
                }

                ws2.Cells.AutoFitColumns();

                // --- Thêm biểu đồ cột ---
                var chart = ws2.Drawings.AddChart("chartRating", eChartType.ColumnClustered);
                chart.Title.Text = "Thống kê phản hồi theo Rating";
                chart.SetPosition(1, 0, 3, 0);   // dòng 1, cột D
                chart.SetSize(600, 400);

                var series = chart.Series.Add(ws2.Cells[2, 2, row - 1, 2], ws2.Cells[2, 1, row - 1, 1]);
                series.Header = "Số lượng";

                // Lưu file
                package.Save();
            }

            MessageBox.Show("Tạo báo cáo Excel thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvPhanHoi_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvPhanHoi.Rows)
            {
                string ratingStr = row.Cells["Đánh giá"].Value?.ToString();
                if (int.TryParse(ratingStr, out int ratingVal))
                {
                    switch (ratingVal)
                    {
                        case 5: row.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        case 4: row.DefaultCellStyle.BackColor = Color.LightBlue; break;
                        case 3: row.DefaultCellStyle.BackColor = Color.LightYellow; break;
                        case 2: row.DefaultCellStyle.BackColor = Color.LightSalmon; break;
                        case 1: row.DefaultCellStyle.BackColor = Color.LightCoral; break;
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray; // chưa có rating
                }

                string traLoi = row.Cells["Trả lời"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(traLoi))
                {
                    row.DefaultCellStyle.Font = new Font(dgvPhanHoi.Font, FontStyle.Bold);
                }
                else
                {
                    row.DefaultCellStyle.Font = new Font(dgvPhanHoi.Font, FontStyle.Regular);
                }
            }
        }

        private void Exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

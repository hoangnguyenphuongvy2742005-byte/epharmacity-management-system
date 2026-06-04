using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NhaThuoc.GiaoDien
{
    public partial class Revenue : Form
    {
        DatabaseConnection db = new DatabaseConnection();
        public Revenue()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtDonviloc.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đơn vị lọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại lọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.Text == "Theo tháng")
            {
                int nam = int.Parse(txtDonviloc.Text);
                string s = "exec ThongKeDoanhThuTheoNam @nam";
                SqlParameter[] parameter1 = new SqlParameter[]
                    {
                new SqlParameter("@nam",nam)
                    };
                DataTable dt = db.GiveData(s, parameter1);
                chart.Series.Clear();
                chart.Series.Add("Doanh thu");
                chart.Series["Doanh thu"].ChartType = SeriesChartType.Column;
                chart.Series["Doanh thu"].IsValueShownAsLabel = true;
                chart.Series["Doanh thu"].Color = Color.SteelBlue;
                chart.Series["Doanh thu"].Font = new Font("Times New Roman", 10, FontStyle.Bold);

                foreach (DataRow row in dt.Rows)
                {
                    string thang = "Tháng " + row["Thang"].ToString();
                    decimal tong = Convert.ToDecimal(row["TongDoanhThu"]);
                    chart.Series["Doanh thu"].Points.AddXY(thang, tong);
                }

                chart.ChartAreas[0].AxisX.Title = "Tháng";
                chart.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
                chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);

                chart.Titles.Clear();
                chart.Titles.Add("THỐNG KÊ DOANH THU NĂM " + nam);
                chart.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);
            }
            else
                 if (comboBox1.Text   == "Theo quý")
            {
                // 📊 Vẽ biểu đồ thống kê doanh thu theo quý
                try
                {
                    chart.Series.Clear();
                    chart.Series.Add("Doanh thu");
                    chart.Series["Doanh thu"].ChartType = SeriesChartType.Column;
                    chart.Series["Doanh thu"].IsValueShownAsLabel = true;
                    chart.Series["Doanh thu"].Color = Color.SteelBlue;
                    chart.Series["Doanh thu"].Font = new Font("Times New Roman", 10, FontStyle.Bold);

                    // 🧮 Lấy năm từ textbox
                    int nam;
                    if (!int.TryParse(txtDonviloc.Text, out nam))
                    {
                        MessageBox.Show("Vui lòng nhập năm hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 🧾 Gọi stored procedure
                    string s = "exec ThongKeDoanhThuTheoQuy @Nam";
                    SqlParameter[] parameter1 = new SqlParameter[]
                    {
                        new SqlParameter("@Nam", nam)
                    };
                    DataTable dt = db.GiveData(s, parameter1);

                    // 🧠 Kiểm tra dữ liệu trả về
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu doanh thu trong năm " + nam, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 🔁 Thêm dữ liệu vào biểu đồ
                    foreach (DataRow row in dt.Rows)
                    {
                        string quy = row["Quy"].ToString();
                        decimal tong = Convert.ToDecimal(row["TongDoanhThu"]);
                        chart.Series["Doanh thu"].Points.AddXY(quy, tong);
                    }

                    // 🎨 Cấu hình trục và tiêu đề
                    chart.ChartAreas[0].AxisX.Title = "Quý";
                    chart.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
                    chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
                    chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
                    chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                    chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                    chart.ChartAreas[0].AxisX.Interval = 1;
                    chart.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0"; // Định dạng tiền tệ có dấu phẩy

                    chart.Titles.Clear();
                    chart.Titles.Add("THỐNG KÊ DOANH THU THEO QUÝ NĂM " + nam);
                    chart.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);
                    chart.Titles[0].ForeColor = Color.Navy;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoc1_Click(object sender, EventArgs e)
        {
            string s = "exec DanhSachDoanhThu @ndb,@nkt";
            SqlParameter[] parameter1 = new SqlParameter[]
                {
                new SqlParameter("@ndb",dateTimePicker1.Value)
                ,new SqlParameter("@nkt",dateTimePicker2.Value)
                };
            DataTable dt = db.GiveData(s, parameter1);
            chart.Series.Clear();
            chart.Series.Add("Doanh thu");
            chart.Series["Doanh thu"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart.Series["Doanh thu"].IsValueShownAsLabel = true;

            foreach (DataRow row in dt.Rows)
            {
                string ngay = Convert.ToDateTime(row["Ngaytaodon"]).ToString("dd/MM/yyyy");
                decimal tong = Convert.ToDecimal(row["TongTienDonHang"]);
                chart.Series["Doanh thu"].Points.AddXY(ngay, tong);
            }

            chart.ChartAreas[0].AxisX.Title = "Ngày tạo đơn";
            chart.ChartAreas[0].AxisY.Title = "Tổng tiền (VNĐ)";
            chart.Titles.Clear();
            chart.Titles.Add("BIỂU ĐỒ DOANH THU ĐƠN HÀNG");

            chart.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
            chart.Series["Doanh thu"].Font = new Font("Times New Roman", 10, FontStyle.Bold);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Chọn nơi lưu biểu đồ";
                    saveDialog.Filter = "Ảnh PNG (*.png)|*.png|Ảnh JPEG (*.jpg)|*.jpg|Ảnh BMP (*.bmp)|*.bmp";
                    saveDialog.FileName = "BieuDo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string extension = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();
                        ImageFormat format = ImageFormat.Png;

                        if (extension == ".jpg") format = ImageFormat.Jpeg;
                        else if (extension == ".bmp") format = ImageFormat.Bmp;

                        // Lưu biểu đồ ra file
                        chart.SaveImage(saveDialog.FileName, format);

                        MessageBox.Show("Đã lưu biểu đồ thành công!\n" + saveDialog.FileName,
                                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // (Tuỳ chọn) Mở folder chứa ảnh
                        System.Diagnostics.Process.Start("explorer.exe", "/select," + saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu biểu đồ: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Chọn nơi lưu biểu đồ";
                    saveDialog.Filter = "Ảnh PNG (*.png)|*.png|Ảnh JPEG (*.jpg)|*.jpg|Ảnh BMP (*.bmp)|*.bmp";
                    saveDialog.FileName = "BieuDo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string extension = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();
                        ImageFormat format = ImageFormat.Png;

                        if (extension == ".jpg") format = ImageFormat.Jpeg;
                        else if (extension == ".bmp") format = ImageFormat.Bmp;

                        // Lưu biểu đồ ra file
                        chart.SaveImage(saveDialog.FileName, format);

                        MessageBox.Show("Đã lưu biểu đồ thành công!\n" + saveDialog.FileName,
                                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // (Tuỳ chọn) Mở folder chứa ảnh
                        System.Diagnostics.Process.Start("explorer.exe", "/select," + saveDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu biểu đồ: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    }
    


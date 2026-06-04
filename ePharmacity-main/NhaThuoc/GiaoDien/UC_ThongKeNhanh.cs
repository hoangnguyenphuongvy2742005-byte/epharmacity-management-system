using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NhaThuoc.DatabaseHelper;

namespace NhaThuoc
{
    public partial class UC_ThongKeNhanh : UserControl
    {
        public UC_ThongKeNhanh()
        {
            InitializeComponent();
            //EnableUnicodeRendering(this);
        }

        private void UC_ThongKeNhanh_Load(object sender, EventArgs e)
        {
            LoadThongKeData();
        }

        private void LoadThongKeData()
        {
            try
            {
                Dictionary<string, int> thongKe = DatabaseConnection.GetThongKeNhanh();
                
                lblTongSoThuoc.Text = $"💊 Tổng số thuốc: {thongKe["TongSoThuoc"]} sản phẩm";
                lblTongGiaTriKho.Text = $"💰 Tổng giá trị kho: {thongKe["TongGiaTriKho"]:N0} VNĐ";
                lblThuocSapHetHan.Text = $"⏰ Thuốc sắp hết hạn: {thongKe["ThuocSapHetHan"]} sản phẩm";
                lblThuocTonKhoThap.Text = $"📦 Thuốc tồn kho thấp: {thongKe["ThuocTonKhoThap"]} sản phẩm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thống kê: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshData()
        {
            LoadThongKeData();
        }

        private void EnableUnicodeRendering(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Label lbl) lbl.UseCompatibleTextRendering = true;
                if (c is Button btn) btn.UseCompatibleTextRendering = true;
                if (c is GroupBox gb) gb.UseCompatibleTextRendering = true;
                // gọi đệ quy để áp dụng cho tất cả control con
                EnableUnicodeRendering(c);
            }
        }
        private void panelThongKe_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

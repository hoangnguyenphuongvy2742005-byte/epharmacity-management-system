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
    public partial class UC_CanhBaoTonKho : UserControl
    {
        public event EventHandler BaoCaoTonKhoClicked;

        private int previousLowStockCount = -1;
        private int previousNearExpiryCount = -1;
        private int previousExpiredCount = -1;
        private bool popupsEnabled = true;

        public UC_CanhBaoTonKho()
        {
            InitializeComponent();
        }

        private void UC_CanhBaoTonKho_Load(object sender, EventArgs e)
        {
            LoadCanhBaoData();
        }

        private void LoadCanhBaoData()
        {
            try
            {
                DataTable tonKhoThap = DatabaseConnection.GetThuocTonKhoThap();
                DataTable sapHetHan = DatabaseConnection.GetThuocSapHetHan();
                DataTable daHetHan = DatabaseConnection.GetThuocDaHetHan();
                
                int soTonKhoThap = tonKhoThap.Rows.Count;
                int soSapHetHan = sapHetHan.Rows.Count;
                int soDaHetHan = daHetHan.Rows.Count;
                
                lblTonKhoThap.Text = $"📦 Tồn kho thấp: {soTonKhoThap} sản phẩm";
                lblSapHetHan.Text = $"⏰ Sắp hết hạn: {soSapHetHan} sản phẩm";
                lblDaHetHan.Text = $"❌ Đã hết hạn: {soDaHetHan} sản phẩm";
                
                if (soTonKhoThap > 0 || soSapHetHan > 0 || soDaHetHan > 0)
                {
                    panelCanhBao.BackColor = Color.FromArgb(255, 255, 192);
                }
                else
                {
                    panelCanhBao.BackColor = Color.FromArgb(192, 255, 192);
                }
                
                bool countsChanged = soTonKhoThap != previousLowStockCount 
                    || soSapHetHan != previousNearExpiryCount 
                    || soDaHetHan != previousExpiredCount;

                if (popupsEnabled && countsChanged)
                {
                    if (soTonKhoThap > 0)
                    {
                        MessageBox.Show($"Có {soTonKhoThap} sản phẩm tồn kho thấp cần nhập thêm", 
                            "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                    if (soDaHetHan > 0)
                    {
                        MessageBox.Show($"Có {soDaHetHan} sản phẩm đã hết hạn cần xử lý", 
                            "Cảnh báo hết hạn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                previousLowStockCount = soTonKhoThap;
                previousNearExpiryCount = soSapHetHan;
                previousExpiredCount = soDaHetHan;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cảnh báo: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaoCaoTonKho_Click(object sender, EventArgs e)
        {
            BaoCaoTonKhoClicked?.Invoke(this, e);
        }

        public void RefreshData()
        {
            LoadCanhBaoData();
        }

        public void SetPopupsEnabled(bool enabled)
        {
            popupsEnabled = enabled;
        }

        private void panelCanhBao_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

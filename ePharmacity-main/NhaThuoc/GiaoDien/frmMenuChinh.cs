using System;
using System.Windows.Forms;

namespace NhaThuoc
{
    public partial class frmMenuChinh : Form
    {
        private int maNguoiDung = 1; // Mã người dùng đăng nhập (mặc định là Kế toán)
        public string Sodienthoai;
        public string Manhanvien;
        public string Hovaten;

        public frmMenuChinh()
        {
            InitializeComponent();
        }
        public frmMenuChinh(string hoten, string sdt, string manv)
        {
            InitializeComponent();
            Sodienthoai = sdt;
            Manhanvien = manv;
            Hovaten = hoten;
        }

        public frmMenuChinh(int maNguoiDung)
        {
            InitializeComponent();
            this.maNguoiDung = maNguoiDung;
        }

        private void frmMenuChinh_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Chào mừng bạn đến với hệ thống quản lý kế toán";
        }

        // === QUẢN LÝ BẢNG LƯƠNG ===
        private void btnQuanLyBangLuong_Click(object sender, EventArgs e)
        {
            frmQuanLyBangLuongMoi frm = new frmQuanLyBangLuongMoi();
            frm.ShowDialog();
        }

        private void btnChinhSuaThongSoLuong_Click(object sender, EventArgs e)
        {
            frmThongSoLuong frm = new frmThongSoLuong();
            frm.ShowDialog();
        }

        // === QUẢN LÝ CÔNG NỢ ===
        private void btnThemCongNo_Click(object sender, EventArgs e)
        {
            frmThemCongNo frm = new frmThemCongNo(maNguoiDung);
            frm.ShowDialog();
        }

        private void btnXoaCongNo_Click(object sender, EventArgs e)
        {
            frmXoaCongNo frm = new frmXoaCongNo();
            frm.ShowDialog();
        }

        // === BÁO CÁO & THỐNG KÊ ===
        private void btnChiPhiNhapHang_Click(object sender, EventArgs e)
        {
            frmChiPhiNhapHang frm = new frmChiPhiNhapHang();
            frm.ShowDialog();
        }

        private void btnChiPhiBanHang_Click(object sender, EventArgs e)
        {
            frmChiPhiBanHang frm = new frmChiPhiBanHang();
            frm.ShowDialog();
        }

        // === THOÁT ===
        private void btnThoat_Click(object sender, EventArgs e)
        {
           Homepage homepage = new Homepage();
           homepage.Show();
            this.Hide();
        }
    }
}

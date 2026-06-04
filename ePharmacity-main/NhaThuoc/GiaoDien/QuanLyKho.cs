using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhaThuoc.GiaoDien
{
    public partial class QuanLyKho : Form
    {
        public string Sodienthoai;
        public string Manhanvien;
        public string Hovaten;
        public QuanLyKho(string hoten, string sdt, string manv)
        {
            InitializeComponent();
            Sodienthoai = sdt;
            Manhanvien = manv;
            Hovaten = hoten;
        }

        private void btn_QuanLyKho_Click(object sender, EventArgs e)
        {
            QuanLyKhoThuoc_Main quanLyKhoThuoc_Main = new QuanLyKhoThuoc_Main(Hovaten, Sodienthoai, Manhanvien);
            quanLyKhoThuoc_Main.Show();
            this.Hide();
        }
    }
}

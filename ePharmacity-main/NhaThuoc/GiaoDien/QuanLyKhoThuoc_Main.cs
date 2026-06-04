using Nhathuoc;
using NhaThuoc.GiaoDien;
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
    public partial class QuanLyKhoThuoc_Main : Form
    {
        public string Sodienthoai;
        public string Manhanvien;
        public string Hovaten;
        public QuanLyKhoThuoc_Main()
        {
            InitializeComponent();
        }

        public QuanLyKhoThuoc_Main(string hoten, string sdt, string manv)
        {
            InitializeComponent();
            Sodienthoai = sdt;
            Manhanvien = manv;
            Hovaten = hoten;
        }

        private void QuanLyKhoThuoc_Main_Load(object sender, EventArgs e)
        {
            RefreshAllData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Thuoc_AddEdit formThem = new Thuoc_AddEdit();
                if (formThem.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form thêm thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedId = ucThuocList.GetSelectedThuocId();
                if (selectedId == -1)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Thuoc_AddEdit formSua = new Thuoc_AddEdit(selectedId);
                if (formSua.ShowDialog() == DialogResult.OK)
                {
                    RefreshAllData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form sửa thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedId = ucThuocList.GetSelectedThuocId();
                if (selectedId == -1)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = DatabaseHelper.DatabaseConnection.DeleteThuoc(selectedId);
                    if (success)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshAllData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCao_Xuat formBaoCao = new BaoCao_Xuat();
                formBaoCao.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form báo cáo: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ucCanhBaoTonKho_BaoCaoTonKhoClicked(object sender, EventArgs e)
        {
            try
            {
                BaoCao_Xuat formBaoCao = new BaoCao_Xuat();
                formBaoCao.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form báo cáo: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshAllData()
        {
            try
            {
                ucThuocList.RefreshData();
                ucThongKeNhanh.RefreshData();
                ucCanhBaoTonKho.SetPopupsEnabled(false);
                ucCanhBaoTonKho.RefreshData();
                ucCanhBaoTonKho.SetPopupsEnabled(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi refresh dữ liệu: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThongBao_Click(object sender, EventArgs e)
        {
            Notice notice = new Notice(Sodienthoai);
            this.Hide();
            notice.ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
              Homepage homepage = new Homepage();
                homepage.Show();
                this.Hide();
        }

        private void ucThongKeNhanh_Load(object sender, EventArgs e)
        {

        }
    }
}

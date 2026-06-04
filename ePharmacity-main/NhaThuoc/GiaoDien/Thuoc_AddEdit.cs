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

namespace Nhathuoc
{
    public partial class Thuoc_AddEdit : Form
    {
        private bool isEditMode = false;
        private int thuocId = -1;
        private DataTable loaiThuocData;
        private DataTable nhomThuocData;
        private bool isBindingCombo = false;

        public Thuoc_AddEdit()
        {
            InitializeComponent();
            isEditMode = false;
            this.Text = "Thêm thuốc mới";
            btnLuu.Text = "Lưu";
        }

        public Thuoc_AddEdit(int id) : this()
        {
            isEditMode = true;
            thuocId = id;
            this.Text = "Cập nhật thông tin thuốc";
            btnLuu.Text = "Cập nhật";
        }

        private void Thuoc_AddEdit_Load(object sender, EventArgs e)
        {
            LoadNhomThuoc();
            
            if (isEditMode)
            {
                LoadThuocData();
            }
        }

        private void LoadNhomThuoc()
        {
            try
            {
                nhomThuocData = DatabaseConnection.GetNhomThuoc();
                isBindingCombo = true;
                cboNhomHang.DataSource = nhomThuocData;
                cboNhomHang.DisplayMember = "TenNhom";
                cboNhomHang.ValueMember = "MaNhom";
                isBindingCombo = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nhóm thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoaiThuoc(int maNhom)
        {
            try
            {
                if (loaiThuocData == null)
                {
                    loaiThuocData = DatabaseConnection.GetLoaiThuoc();
                }
                
                DataView dv = new DataView(loaiThuocData);
                dv.RowFilter = $"MaNhom = {maNhom}";
                
                isBindingCombo = true;
                cboKieuHang.DataSource = dv.ToTable();
                cboKieuHang.DisplayMember = "TenLoai";
                cboKieuHang.ValueMember = "MaLoai";
                isBindingCombo = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThuocData()
        {
            try
            {
                DataRow thuocData = DatabaseConnection.GetThuocById(thuocId);
                if (thuocData != null)
                {
                    int maNhom = Convert.ToInt32(thuocData["MaNhom"]);
                    isBindingCombo = true;
                    cboNhomHang.SelectedValue = maNhom;
                    isBindingCombo = false;
                    LoadLoaiThuoc(maNhom);
                    
                    isBindingCombo = true;
                    cboKieuHang.SelectedValue = thuocData["MaLoai"];
                    isBindingCombo = false;
                    txtMaHang.Text = thuocData["Mahanghoa"] != DBNull.Value ? thuocData["Mahanghoa"].ToString() : "";
                    txtTenHang.Text = thuocData["Tenhang"].ToString();
                    txtHoatChat.Text = thuocData["Thanhphan"] != DBNull.Value ? thuocData["Thanhphan"].ToString() : "";
                    txtNhaSanXuat.Text = thuocData["Nhasanxuat"] != DBNull.Value ? thuocData["Nhasanxuat"].ToString() : "";
                    txtDongGoi.Text = thuocData["Donggoi"] != DBNull.Value ? thuocData["Donggoi"].ToString() : "";
                    numGiaNhap.Value = Convert.ToDecimal(thuocData["Gianhap"]);
                    numSoLuong.Value = Convert.ToDecimal(thuocData["Soluong"] ?? 0);
                    numTonKho.Value = Convert.ToDecimal(thuocData["Kho"]);
                    numGiaBan.Value = Convert.ToDecimal(thuocData["Giaban"]);
                    
                    if (thuocData["Ngayhethan"] != DBNull.Value)
                    {
                        dtpNgayHetHan.Value = Convert.ToDateTime(thuocData["Ngayhethan"]);
                    }
                    else
                    {
                        dtpNgayHetHan.Value = DateTime.Now.AddDays(30);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm để cập nhật", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thuốc: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboNhomHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isBindingCombo) return;
            if (cboNhomHang.SelectedValue == null) return;

            int maNhom = GetComboSelectedInt(cboNhomHang, "MaNhom");
            if (maNhom > 0)
            {
                LoadLoaiThuoc(maNhom);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    Dictionary<string, object> thuocData = GetFormData();
                    
                    bool success = false;
                    if (isEditMode)
                    {
                        success = DatabaseConnection.UpdateThuoc(thuocId, thuocData);
                        if (success)
                        {
                            MessageBox.Show("Cập nhật sản phẩm thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        success = DatabaseConnection.AddThuoc(thuocData);
                        if (success)
                        {
                            MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    
                    if (success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại sau: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (cboKieuHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn kiểu hàng", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKieuHang.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin - Tên hàng không được để trống", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenHang.Focus();
                return false;
            }

            if (numGiaNhap.Value <= 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin - Giá nhập phải lớn hơn 0", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numGiaNhap.Focus();
                return false;
            }

            if (numSoLuong.Value < 0)
            {
                MessageBox.Show("Số lượng nhập phải >= 0", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSoLuong.Focus();
                return false;
            }

            if (numTonKho.Value < 0)
            {
                MessageBox.Show("Tồn kho phải >= 0", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numTonKho.Focus();
                return false;
            }

            if (numGiaBan.Value <= 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin - Giá bán phải lớn hơn 0", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numGiaBan.Focus();
                return false;
            }

            if (dtpNgayHetHan.Value <= DateTime.Now)
            {
                MessageBox.Show("Định dạng ngày không hợp lệ - Ngày hết hạn phải sau ngày hiện tại", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayHetHan.Focus();
                return false;
            }

            return true;
        }

        private Dictionary<string, object> GetFormData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            
            data["MaLoai"] = GetComboSelectedInt(cboKieuHang, "MaLoai");
            data["Mahanghoa"] = string.IsNullOrWhiteSpace(txtMaHang.Text) ? null : txtMaHang.Text.Trim();
            data["Mavach"] = null; // Có thể thêm field này nếu cần
            data["Tenhang"] = txtTenHang.Text.Trim();
            data["Mathuoc"] = null;
            data["Thanhphan"] = string.IsNullOrWhiteSpace(txtHoatChat.Text) ? null : txtHoatChat.Text.Trim();
            data["Nhasanxuat"] = string.IsNullOrWhiteSpace(txtNhaSanXuat.Text) ? null : txtNhaSanXuat.Text.Trim();
            data["Donggoi"] = string.IsNullOrWhiteSpace(txtDongGoi.Text) ? null : txtDongGoi.Text.Trim();
            data["Gianhap"] = Convert.ToDecimal(numGiaNhap.Value);
            int soLuongNhap = Convert.ToInt32(numSoLuong.Value);
            int tonKho = Convert.ToInt32(numTonKho.Value);
            if (!isEditMode && tonKho == 0)
            {
                tonKho = soLuongNhap;
            }
            data["Soluong"] = soLuongNhap;
            data["Kho"] = tonKho;
            data["Mota"] = null;
            data["Lohang"] = null;
            data["Ngayhethan"] = dtpNgayHetHan.Value;
            data["Giaban"] = Convert.ToDecimal(numGiaBan.Value); 
            
            return data;
        }

        private int GetComboSelectedInt(ComboBox combo, string valueMember)
        {
            if (combo.SelectedValue == null)
            {
                return -1;
            }

            if (combo.SelectedValue is DataRowView drv)
            {
                object raw = drv[valueMember];
                return raw == null || raw == DBNull.Value ? -1 : Convert.ToInt32(raw);
            }

            return Convert.ToInt32(combo.SelectedValue);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

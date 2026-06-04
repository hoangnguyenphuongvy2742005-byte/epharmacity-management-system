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
    public partial class UC_ThuocList : UserControl
    {
        private DataTable originalData;
        private DataTable filteredData;

        public UC_ThuocList()
        {
            InitializeComponent();
        }

        private void UC_ThuocList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                originalData = DatabaseConnection.GetAllThuoc();
                filteredData = originalData.Copy();
                dgvThuocList.DataSource = filteredData;
                
                if (originalData.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có sản phẩm nào trong kho", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                filteredData = originalData.Copy();
                dgvThuocList.DataSource = filteredData;
            }
            else
            {
                try
                {
                    filteredData = DatabaseConnection.SearchThuoc(keyword);
                    dgvThuocList.DataSource = filteredData;
                    
                    if (filteredData.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm phù hợp", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvThuocList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int id = Convert.ToInt32(dgvThuocList.Rows[e.RowIndex].Cells["ID"].Value);
                    DataRow thuocInfo = DatabaseConnection.GetThuocById(id);
                    
                    if (thuocInfo != null)
                    {
                        string message = $"Thông tin chi tiết thuốc:\n\n" +
                                       $"ID: {thuocInfo["ID"]}\n" +
                                       $"Tên hàng: {thuocInfo["Tenhang"]}\n" +
                                       $"Mã hàng: {thuocInfo["Mahanghoa"]}\n" +
                                       $"Loại: {thuocInfo["TenLoai"]}\n" +
                                       $"Nhóm: {thuocInfo["TenNhom"]}\n" +
                                       $"Số lượng: {thuocInfo["Soluong"]}\n" +
                                       $"Hoạt chất: {thuocInfo["Thanhphan"]}\n" +
                                       $"Nhà sản xuất: {thuocInfo["Nhasanxuat"]}\n" +
                                       $"Đóng gói: {thuocInfo["Donggoi"]}\n" +
                                       $"Giá nhập: {thuocInfo["Gianhap"]:N0} VNĐ\n" +
                                       $"Giá bán: {thuocInfo["Giaban"]:N0} VNĐ\n" +
                                       $"Tồn kho: {thuocInfo["Kho"]}\n" +
                                       $"Ngày hết hạn: {thuocInfo["Ngayhethan"]}\n" +
                                       $"Mô tả: {thuocInfo["Mota"]}";
                        
                        MessageBox.Show(message, "Chi tiết thuốc", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị thông tin: " + ex.Message, "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvThuocList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvThuocList.Rows[e.RowIndex].DataBoundItem != null)
            {
                DataRowView row = (DataRowView)dgvThuocList.Rows[e.RowIndex].DataBoundItem;
                
                if (row["Tồn kho"] != DBNull.Value && Convert.ToInt32(row["Tồn kho"]) < 10)
                {
                    dgvThuocList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else if (row["Ngày hết hạn"] != DBNull.Value && row["Ngày hết hạn"].ToString() != "")
                {
                    try
                    {
                        DateTime ngayHetHan = Convert.ToDateTime(row["Ngày hết hạn"]);
                        int soNgayConLai = (ngayHetHan - DateTime.Now).Days;
                        
                        if (soNgayConLai < 0)
                        {
                            dgvThuocList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        }
                        else if (soNgayConLai < 30)
                        {
                            dgvThuocList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void RefreshData()
        {
            LoadData();
        }

        public DataRow GetSelectedThuoc()
        {
            if (dgvThuocList.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvThuocList.SelectedRows[0].Cells["ID"].Value);
                return DatabaseConnection.GetThuocById(id);
            }
            return null;
        }

        public int GetSelectedThuocId()
        {
            if (dgvThuocList.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(dgvThuocList.SelectedRows[0].Cells["ID"].Value);
            }
            return -1;
        }

        private void dgvThuocList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

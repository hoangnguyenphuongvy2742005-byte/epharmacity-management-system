using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static NhaThuoc.DatabaseHelper;

namespace NhaThuoc
{
    public partial class BaoCao_Xuat : Form
    {
        private DataTable currentData;

        public BaoCao_Xuat()
        {
            InitializeComponent();
        }

        private void BaoCao_Xuat_Load(object sender, EventArgs e)
        {
            LoadBaoCaoTonKho();
        }

        private void rdoBaoCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTonKho.Checked)
            {
                LoadBaoCaoTonKho();
            }
            else if (rdoSapHetHan.Checked)
            {
                LoadBaoCaoSapHetHan();
            }
            else if (rdoTheoNhom.Checked)
            {
                LoadBaoCaoTheoNhom();
            }
            else if (rdoTongHop.Checked)
            {
                LoadBaoCaoTongHop();
            }
        }

        private void LoadBaoCaoTonKho()
        {
            try
            {
                currentData = DatabaseConnection.GetBaoCaoTonKho();
                dgvBaoCao.DataSource = currentData;
                
                if (currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phù hợp để tạo báo cáo", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo tồn kho: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBaoCaoSapHetHan()
        {
            try
            {
                currentData = DatabaseConnection.GetBaoCaoSapHetHan();
                dgvBaoCao.DataSource = currentData;
                
                if (currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phù hợp để tạo báo cáo", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo sắp hết hạn: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBaoCaoTheoNhom()
        {
            try
            {
                currentData = DatabaseConnection.GetBaoCaoTheoNhom();
                dgvBaoCao.DataSource = currentData;
                
                if (currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phù hợp để tạo báo cáo", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo theo nhóm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBaoCaoTongHop()
        {
            try
            {
                currentData = DatabaseConnection.GetBaoCaoTongHop();
                dgvBaoCao.DataSource = currentData;
                
                if (currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu phù hợp để tạo báo cáo", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo tổng hợp: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentData == null || currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để in", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Đã gửi lệnh in thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentData == null || currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = GetFileName() + ".pdf";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string content = GenerateReportContent();
                    var utf8Bom = new System.Text.UTF8Encoding(true);
                    File.WriteAllText(saveDialog.FileName.Replace(".pdf", ".txt"), content, utf8Bom);
                    
                    MessageBox.Show("Xuất báo cáo thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất PDF: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentData == null || currentData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.csv)|*.csv";
                saveDialog.FileName = GetFileName() + ".csv";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(currentData, saveDialog.FileName);
                    
                    MessageBox.Show("Xuất báo cáo thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetFileName()
        {
            string fileName = "BaoCao_";
            if (rdoTonKho.Checked) fileName += "TonKho";
            else if (rdoSapHetHan.Checked) fileName += "SapHetHan";
            else if (rdoTheoNhom.Checked) fileName += "TheoNhom";
            else if (rdoTongHop.Checked) fileName += "TongHop";
            
            fileName += "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return fileName;
        }

        private string GenerateReportContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BÁO CÁO KHO THUỐC");
            sb.AppendLine("Ngày tạo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.AppendLine("Loại báo cáo: " + GetReportType());
            sb.AppendLine(new string('=', 50));
            sb.AppendLine();

            if (currentData != null && currentData.Rows.Count > 0)
            {
            foreach (DataColumn column in currentData.Columns)
                {
                    sb.Append(column.ColumnName + "\t");
                }
                sb.AppendLine();

                foreach (DataRow row in currentData.Rows)
                {
                    foreach (object item in row.ItemArray)
                    {
                        sb.Append(item.ToString() + "\t");
                    }
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private string GetReportType()
        {
            if (rdoTonKho.Checked) return "Báo cáo tồn kho";
            else if (rdoSapHetHan.Checked) return "Báo cáo sắp hết hạn";
            else if (rdoTheoNhom.Checked) return "Báo cáo theo nhóm";
            else if (rdoTongHop.Checked) return "Báo cáo tổng hợp";
            return "";
        }

        private void ExportToCSV(DataTable dataTable, string filePath)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                sb.Append(dataTable.Columns[i].ColumnName);
                if (i < dataTable.Columns.Count - 1)
                    sb.Append(",");
            }
            sb.AppendLine();

            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string value = row[i].ToString();
                    if (value.Contains(",") || value.Contains("\""))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }
                    sb.Append(value);
                    if (i < dataTable.Columns.Count - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }

            var utf8Bom = new UTF8Encoding(true);
            File.WriteAllText(filePath, sb.ToString(), utf8Bom);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

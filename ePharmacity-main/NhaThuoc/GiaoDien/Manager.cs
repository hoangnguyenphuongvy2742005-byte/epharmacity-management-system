using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace NhaThuoc.GiaoDien
{
    public partial class Manager : Form
    {
        public string tam="-1";
        public string Hovaten;
        public string Sodienthoai;
        public string Manhanvien;
        public string chucnang=null;
        public string chon;
        public string tam1, tam2, tam3,  tam5;
        public DateTime tam4;
        DatabaseConnection db = new DatabaseConnection();
        DataTable dt;
        public Manager()
        {
            InitializeComponent();
            //HienThiTaiKhoan();
        }

        public Manager(string hoten,string sdt,string manv)
        {
            InitializeComponent();
            Sodienthoai = sdt;
            Manhanvien = manv;
            Hovaten = hoten;
            //HienThiTaiKhoan();
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            BeautifyComboBox(comboBox1);
            BeautifyButton(btnGui);
            BeautifyButton(btnThem);
            BeautifyButton(btnSua);
            BeautifyButton(btnXoa);
            MakeAvatarRound(pictureBox1);
            panel1.Visible = false;
            dataGridView.Visible = true;
            txtSodienthoai.Visible=false;
        }

        private void BeautifyComboBox(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;                          // phẳng, hiện đại
            cb.Font = new Font("Segoe UI", 10, FontStyle.Regular);  // đồng bộ font
            cb.ForeColor = Color.Black;                             // màu chữ
            cb.BackColor = Color.White;                             // nền trắng sạch
            cb.DropDownStyle = ComboBoxStyle.DropDownList;          // không cho nhập tay
            cb.IntegralHeight = false;                              // tránh che khuất
            cb.DropDownHeight = 120;                                // độ cao menu
        }


        private void BeautifyButton(Button btn)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.BackColor = Color.FromArgb(56, 182, 255); // #38B6FF
        btn.ForeColor = Color.White;
        btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
        btn.Cursor = Cursors.Hand;

        // Tạo vùng bo tròn
        using (GraphicsPath path = CreateRoundedRect(new Rectangle(0, 0, btn.Width, btn.Height), 20))
        {
            btn.Region = new Region(path);
        }
    }
        private GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            // Góc trên trái
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            // Góc trên phải
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            // Góc dưới phải
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            // Góc dưới trái
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }


        private void AddHoverEffect(Button btn)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(36, 145, 211); // #2491D3
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(56, 182, 255); // #38B6FF
        }


        private void MakeAvatarRound(PictureBox pic)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, pic.Width - 1, pic.Height - 1);
            pic.Region = new Region(path);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            HienThiTaiKhoan();
        }

        public void HienThiTaiKhoan()
        {
            string s1 = "select * from Taikhoan";
            dt = db.GiveDataNoParameter(s1);
            dataGridView.DataSource = dt;
        }

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            //ResetTatCaControl();
            chucnang = "QuanLyNhanVien";
            txtSodienthoai.Visible= false;
            btnThem.Enabled = true;
            btnThem.Visible = true;
            btnSua.Visible = true;
            btnXoa.Visible = true;
            panel1.Visible = false;
            dataGridView.Visible = true;
            HienThiTaiKhoan();



        }

        private void btnQuanLyDoanhThu_Click(object sender, EventArgs e)
        {
            this.Hide();
            Revenue revenue = new Revenue();
            revenue.ShowDialog(); // Show dạng modal
            this.Show(); // Khi Add đóng, quay lại Manager
            HienThiTaiKhoan(); // Refresh lại danh sách
        }

        private void btnQuanLyTruyCap_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            dataGridView.Visible = true;

            // Kiểm tra nếu chưa chọn dòng nào
            if (string.IsNullOrEmpty(tam) || tam == "-1")
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xem quyền truy cập", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int m;
            if (!int.TryParse(tam, out m) || m < 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu có chọn dòng hợp lệ
            this.Hide();
            Role role = new Role(tam2);
            role.ShowDialog();
            this.Show();
        }

        private Dictionary<TextBox, bool> trangThaiNhap = new Dictionary<TextBox, bool>();

        private void DatPlaceholderHover(TextBox txt, string placeholder)
        {
            // Hiển thị placeholder ban đầu
            txt.Text = placeholder;
            txt.ForeColor = Color.DimGray;
            trangThaiNhap[txt] = false;

            // Khi focus hoặc rê chuột vào
            txt.Enter += (s, e) =>
            {
                if (!trangThaiNhap[txt] && txt.Text == placeholder)
                {
                    txt.Clear();
                    txt.ForeColor = Color.Black;
                }
            };
            txt.MouseEnter += (s, e) =>
            {
                if (!trangThaiNhap[txt] && txt.Text == placeholder)
                {
                    txt.Clear();
                    txt.ForeColor = Color.Black;
                }
            };

            // ✅ Khi người dùng bắt đầu gõ (bất kỳ ký tự nào)
            txt.KeyPress += (s, e) =>
            {
                if (!trangThaiNhap[txt] && txt.Text == placeholder)
                {
                    txt.Clear();
                    txt.ForeColor = Color.Black;
                }
            };

            // Khi mất focus hoặc rời chuột ra
            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    trangThaiNhap[txt] = false;
                    txt.Text = placeholder;
                    txt.ForeColor = Color.DimGray;
                }
            };
            txt.MouseLeave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    trangThaiNhap[txt] = false;
                    txt.Text = placeholder;
                    txt.ForeColor = Color.DimGray;
                }
            };

            // Khi nội dung thay đổi (để đánh dấu là đã nhập)
            txt.TextChanged += (s, e) =>
            {
                trangThaiNhap[txt] = !string.IsNullOrWhiteSpace(txt.Text) && txt.Text != placeholder;
            };
        }
        private void btnThongBaoNhanVien_Click(object sender, EventArgs e)
        {
            chucnang = "Thongbao";
            DatPlaceholderHover(txtChude, "Nhập chủ đề...");
            DatPlaceholderHover(txtThongbao, "Nhập nội dung thông báo...");
            panel1.Visible = true;
            dataGridView.Visible = false;

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Homepage homepage = new Homepage();
            homepage.Show();
            this.Hide();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (chucnang == "QuanLyNhanVien")
            {
                this.Hide(); 
                Add add = new Add();
                add.ShowDialog();
                this.Show(); 
                HienThiTaiKhoan(); 
            }
        }

        private void btnHoantac_Click(object sender, EventArgs e)
        {
            HienThiTaiKhoan();
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            if (chucnang == null)
                return;
            if (chucnang == "Thongbao")
            {
                string chude = txtChude.Text;
                string noidung = txtThongbao.Text;
                string nguoinhan = comboBox1.Text;
                if (chude == "Nhập chủ đề..." || noidung == "Nhập nội dung thông báo...")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (chon == "Cá nhân")
                    nguoinhan = txtSodienthoai.Text;
                string s = "exec Luuthongbao @chude,@noidung,@nguoinhan";
                SqlParameter[] parameter = new SqlParameter[]
                   {
                    new SqlParameter("@chude",chude)
                    ,new SqlParameter("@noidung",noidung)
                    ,new SqlParameter("@nguoinhan",nguoinhan)
                   };
                db.GiveData(s, parameter);
                MessageBox.Show("Gửi thông báo thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtThongbao_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chon = comboBox1.SelectedItem.ToString();
            if (chon == "Cá nhân")
            {
                btnThem.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
                txtSodienthoai.Visible = true;
            }
            else
            {
                btnThem.Visible = true;
                btnSua.Visible = true;
                btnXoa.Visible = true;
                txtSodienthoai.Visible = false;
            }
        }

        private void btnThem_Enter(object sender, EventArgs e) => HienLoi(btnThem, "Thongbao");
        private void btnThem_Leave(object sender, EventArgs e) => TatLoi(btnThem);

        private void btnSua_Enter(object sender, EventArgs e) => HienLoi(btnSua, "Thongbao");
        private void btnSua_Leave(object sender, EventArgs e) => TatLoi(btnSua);

        private void btnGui_Enter(object sender, EventArgs e) => HienLoi(btnGui, "QuanLyNhanVien");
        private void btnGui_Leave(object sender, EventArgs e) => TatLoi(btnGui);

        private void comboBox1_Enter(object sender, EventArgs e) => HienLoi(comboBox1, "QuanLyNhanVien");
        private void comboBox1_Leave(object sender, EventArgs e) => TatLoi(comboBox1);



        
        private void panelChucnang_Paint(object sender, PaintEventArgs e)
        {

        }
        

        private void btnThem_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void btnSua_MouseLeave(object sender, EventArgs e)
        {
            
        }



        private void HienLoi(Control ctrl, string requiredFunction)
        {
            errorProvider1.Clear();
            if (chucnang == requiredFunction || chucnang == null)
            {
                // Chỉ hiển thị cảnh báo, không vô hiệu hóa nút
                errorProvider1.SetError(ctrl, "Không có đặc quyền khi thực hiện chức năng!");
            }
        }

        private void TatLoi(Control ctrl)
        {
            // Xóa cảnh báo khi rời khỏi
            errorProvider1.Clear();
        }


        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tam = e.RowIndex.ToString();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                tam1 = row.Cells["Hovaten"].Value.ToString();
                tam2 = row.Cells["Sodienthoai"].Value.ToString();
                tam3 = row.Cells["Matkhau"].Value.ToString();
                //tam4 = Convert.ToDateTime(row.Cells["Ngaysinh"].Value);
                tam5 = row.Cells["Chucvu"].Value.ToString();
                // ✅ Kiểm tra NULL hoặc giá trị rỗng cho Ngaysinh
                    if (row.Cells["Ngaysinh"].Value == DBNull.Value || string.IsNullOrWhiteSpace(row.Cells["Ngaysinh"].Value?.ToString()))
                {
                    tam4 = DateTime.MinValue; // hoặc DateTime.Today nếu bạn muốn mặc định là hôm nay
                }
                else
                {
                    DateTime temp;
                    if (DateTime.TryParse(row.Cells["Ngaysinh"].Value.ToString(), out temp))
                        tam4 = temp;
                    else
                        tam4 = DateTime.MinValue;
                }
            }

            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (chucnang == "QuanLyNhanVien")
            {
                if (tam=="-1")
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.Hide(); 
                Add add = new Add(tam1,tam2,tam3,tam4,tam5);
                add.ShowDialog(); // Show dạng modal
                this.Show(); // Khi Add đóng, quay lại Manager
                HienThiTaiKhoan(); // Refresh lại danh sách
            }
        }
    }
}

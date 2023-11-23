using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSoThu
{
    public partial class Chuong : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        public Chuong()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            string sql = "Select Chuong.MaChuong, TrangThai.TenTT, NhanVien.TenNV, Loai.TenLoai, " +
                "Khu.TenKhu, DienTich, ChieuCao, SLThu " +
                "From Chuong Left join TrangThai On Chuong.MaTT = TrangThai.MaTT " +
                "Left join NhanVien on Chuong.MaNV = NhanVien.MaNV " +
                "Left join Khu on Chuong.MaKhu = Khu.MaKhu " +
                "Left join Loai on Chuong.MaLoai = Loai.MaLoai";

            dgvChuong.DataSource = pd.DocBang(sql);

            dgvChuong.Columns[0].HeaderText = "Mã chuồng";
            dgvChuong.Columns[1].HeaderText = "Trạng thái";
            dgvChuong.Columns[2].HeaderText = "Nhân viên";
            dgvChuong.Columns[3].HeaderText = "Loài";
            dgvChuong.Columns[4].HeaderText = "Khu";
            dgvChuong.Columns[5].HeaderText = "Diện tích";
            dgvChuong.Columns[6].HeaderText = "Chiều cao";
            dgvChuong.Columns[7].HeaderText = "Số lượng thú";

            foreach (DataGridViewColumn column in dgvChuong.Columns)
            {
                column.Width = 300; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvChuong.RowTemplate.Height = 100;
            LoadDataToComboBox();
            ResetValue();
        }

        // Load dữ liệu lên combobox
        private void LoadDataToComboBox()
        {
            // Lấy dữ liệu từ cơ sở dữ liệu cho ComboBox
            DataTable dtLoai = pd.DocBang("SELECT * FROM Loai");
            // Gán nguồn dữ liệu cho ComboBox
            cbLoai.DataSource = dtLoai;
            cbLoai.DisplayMember = "TenLoai"; // Hiển thị tên loài
            cbLoai.ValueMember = "MaLoai";    // Giữ giá trị mã loài

            DataTable dtTrangThai = pd.DocBang("Select * From TrangThai");
            cbTrangThai.DataSource = dtTrangThai;
            cbTrangThai.DisplayMember = "TenTT";
            cbTrangThai.ValueMember = "MaTT";

            DataTable dtKhu = pd.DocBang("Select * From Khu");
            cbKhu.DataSource = dtKhu;
            cbKhu.DisplayMember = "TenKhu";
            cbKhu.ValueMember = "MaKhu";

            DataTable dtNhanVien = pd.DocBang("Select * From NhanVien");
            cbTenNV.DataSource = dtNhanVien;
            cbTenNV.DisplayMember = "TenNV";
            cbTenNV.ValueMember = "MaNV";
        }

        private void Chuong_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        // Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaChuong.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã chuồng, hãy nhập mã chuồng !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (!Kiemtradltrong())
                {
                    MessageBox.Show("Hãy nhập đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DataTable dt = pd.DocBang("Select * From Chuong Where MaChuong = N'" + (txtMaChuong.Text).Trim() + "'");

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã chuồng này đã tồn tại mời nhập mã khác !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaChuong.Focus();
                    }

                    else
                    {
                        string sql = "Insert into Chuong Values(@MaChuong, @TrangThai, " +
                            "@NhanVien, @Loai, @Khu, @DienTich, @ChieuCao, @SoLuong)";

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@MaChuong", txtMaChuong.Text),
                            new SqlParameter("@TrangThai", cbTrangThai.SelectedValue.ToString()),
                            new SqlParameter("@NhanVien", cbTenNV.SelectedValue.ToString()),
                            new SqlParameter("@Loai", cbLoai.SelectedValue.ToString()),
                            new SqlParameter("@Khu", cbKhu.SelectedValue.ToString()),
                            new SqlParameter("@DienTich", txtDienTich.Text),
                            new SqlParameter("@ChieuCao", txtChieuCao.Text),
                            new SqlParameter("@SoLuong", txtSoLuong.Text)
                        };

                        pd.CapNhatTS(sql, parameters);

                        MessageBox.Show("Thêm mới thành công !", "Thông báo", MessageBoxButtons.OK);

                        LoadDL();
                        ResetValue();
                    }
                }
            }
        }

        private bool Kiemtradltrong()
        {
            return !string.IsNullOrEmpty(txtChieuCao.Text.Trim()) &&
                !string.IsNullOrEmpty(cbKhu.Text.Trim()) &&
                !string.IsNullOrEmpty(cbTrangThai.Text.Trim()) &&
                !string.IsNullOrEmpty(cbTenNV.Text.Trim()) &&
                !string.IsNullOrEmpty(txtDienTich.Text.Trim());
        }

        private void ResetValue()
        {
            txtDienTich.Text = txtChieuCao.Text = txtMaChuong.Text = cbKhu.Text
                = cbLoai.Text = cbTrangThai.Text = cbTenNV.Text = "";
        }

        // Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!Kiemtradltrong())
            {
                MessageBox.Show("Hãy chọn dữ liệu cần sửa !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "Update Chuong " +
                    "Set MaTT = @MaTT, MaNV = @MaNV, " +
                    "MaLoai = @MaLoai, MaKhu = @MaKhu, DienTich = @DTich, ChieuCao = @ChieuCao, " +
                    "SLThu = @SoLuong " +
                    "Where MaChuong = N'" + txtMaChuong.Text + "'";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                    new SqlParameter("@MaTT", cbTrangThai.SelectedValue.ToString()),
                    new SqlParameter("@MaNV", cbTenNV.SelectedValue.ToString()),
                    new SqlParameter("@MaLoai", cbLoai.SelectedValue.ToString()),
                    new SqlParameter("@MaKhu", cbKhu.SelectedValue.ToString()),
                    new SqlParameter("@DTich", txtDienTich.Text),
                    new SqlParameter("@ChieuCao", txtChieuCao.Text),
                    new SqlParameter("@SoLuong", txtSoLuong.Text)
                    };

                    pd.CapNhatTS(sql, parameters);

                    MessageBox.Show("Cập nhật dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK);

                    LoadDL();
                    ResetValue();
                }
            }
        }

        // Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            
        }

        // Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        private void dgvChuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaChuong.Text = dgvChuong.CurrentRow.Cells[0].Value.ToString();
            cbTrangThai.Text = dgvChuong.CurrentRow.Cells[1].Value .ToString();
            cbTenNV.Text = dgvChuong.CurrentRow.Cells[2].Value .ToString();
            cbLoai.Text = dgvChuong.CurrentRow.Cells[3].Value .ToString();
            cbKhu.Text = dgvChuong.CurrentRow.Cells[4].Value .ToString();
            txtDienTich.Text = dgvChuong.CurrentRow.Cells[5].Value .ToString();
            txtChieuCao.Text = dgvChuong.CurrentRow.Cells[6].Value .ToString();
            txtSoLuong.Text = dgvChuong.CurrentRow.Cells[7].Value .ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            string tukhoa = txtTimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên thú hoặc nguồn gốc thú cần tìm !", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            string sql = "Select Chuong.MaChuong, TrangThai.TenTT, NhanVien.TenNV, Loai.TenLoai, " +
               "Khu.TenKhu, DienTich, ChieuCao, SLThu " +
               "From Chuong Left join TrangThai On Chuong.MaTT = TrangThai.MaTT " +
               "Left join NhanVien on Chuong.MaNV = NhanVien.MaNV " +
               "Left join Khu on Chuong.MaKhu = Khu.MaKhu " +
               "Left join Loai on Chuong.MaLoai = Loai.MaLoai " +
               "Where SLThu Like @TuKhoa Or TenNV Like @TuKhoa Or TenLoai Like @TuKhoa";

            DataTable dt = pd.LayDuLieu(sql, new SqlParameter("@TuKhoa", "%" + tukhoa + "%"));

            // Kiểm tra kết quả
            if (dt.Rows.Count > 0)
            {
                // Hiển thị kết quả tìm kiếm trong DataGridView hoặc ListBox
                dgvChuong.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin ứng với từ khóa !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

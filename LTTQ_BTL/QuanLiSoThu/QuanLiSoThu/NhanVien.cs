using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSoThu
{
    public partial class NhanVien : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public NhanVien()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            DataTable dt = db.DocBang("Select * From NhanVien");
            dgvNhanVien.DataSource = dt;

            dgvNhanVien.Columns[0].HeaderText = "Mã nhân viên";
            dgvNhanVien.Columns[1].HeaderText = "Tên nhân viên";
            dgvNhanVien.Columns[2].HeaderText = "Ngày sinh";
            dgvNhanVien.Columns[3].HeaderText = "Giới tính";
            dgvNhanVien.Columns[4].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns[5].HeaderText = "Số điện thoại";
            dgvNhanVien.Columns[6].HeaderText = "Tên đăng nhập";
            dgvNhanVien.Columns[7].HeaderText = "Mật khẩu";

            foreach (DataGridViewColumn column in dgvNhanVien.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvNhanVien.RowTemplate.Height = 100;
        }

        private void ResetValue()
        {
            txtDiaChi.Text = txtMatKhau.Text = txtTenDN.Text = txtTenNV.Text 
                = txtmaNV.Text = txtSoDT.Text = "";
            rdbNam.Checked = rdbNu.Checked = false;
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtmaNV.Text == "" & txtTenNV.Text == "" & txtDiaChi.Text == "" & txtSoDT.Text == "" & txtTenDN.Text == "" & txtMatKhau.Text == "")
            {
                MessageBox.Show("Dữ liệu chưa được nhập đầy đủ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT MaNV FROM NhanVien WHERE MaNV = '" + txtmaNV.Text + "'";

            System.Data.DataTable table = db.DocBang(query);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Mã nhập vào đã tồn tại trong dữ liệu. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string gt;
                if (rdbNam.Checked == true)
                {
                    gt = "Nam";
                }
                else
                {
                    gt = "Nữ";
                }
                string a = "Insert Into NhanVien Values ('" + txtmaNV.Text + "', N'" + txtTenNV.Text + "', '" + dtpNgaySinh.Value.Date.ToString() + "', " +
                        gt + ", N'" + txtDiaChi.Text + "', '" + txtSoDT.Text + "', '" + txtTenDN.Text + "', '" + txtMatKhau.Text + "')";
                db.CapNhat(a);
                MessageBox.Show("Dữ liệu của bạn đã được thêm mới thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDL();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtmaNV.Text == "")
            {
                MessageBox.Show("Hãy chọn thông tin cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string gt;
                if (rdbNam.Checked == true)
                {
                    gt = "Nam";
                }
                else
                {
                    gt = "Nữ";
                }
                db.CapNhat("Update NhanVien Set TenNV = N'" + txtTenNV.Text + "', NTNS = '" + dtpNgaySinh.Value.Date.ToString() + "', GTinh = " +
                        gt + ", DiaChi = N'" + txtDiaChi.Text + "', SDT = '" + txtSoDT.Text + "', TenDN = '" + txtTenDN.Text + "',MatKhau = '" + txtMatKhau.Text +
                        "' Where MaNV = '" + txtmaNV.Text + "'");
                dgvNhanVien.DataSource = db.DocBang("Select * From NhanVien");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtmaNV.Text == "")
            {
                MessageBox.Show("Hãy chọn thông tin cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                db.CapNhat("Delete NhanVien Where MaNV = '" + txtmaNV.Text + "'");
                DataTable dt = db.DocBang("Select * From NhanVien");
                dgvNhanVien.DataSource = dt;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmaNV.Text = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();
            txtTenNV.Text = dgvNhanVien.CurrentRow.Cells[1].Value.ToString();
            dtpNgaySinh.Text = dgvNhanVien.CurrentRow.Cells[2].Value.ToString();
            txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells[4].Value.ToString();
            txtSoDT.Text = dgvNhanVien.CurrentRow.Cells[5].Value.ToString();
            txtTenDN.Text = dgvNhanVien.CurrentRow.Cells[6].Value.ToString();
            txtMatKhau.Text = dgvNhanVien.CurrentRow.Cells[7].Value.ToString();

            string gioiTinh = dgvNhanVien.CurrentRow.Cells[3].Value.ToString();

            if (gioiTinh == "Nam")
            {
                rdbNam.Checked = true;
                rdbNu.Checked = false;
            }
            else
            {
                rdbNam.Checked = false;
                rdbNu.Checked = true;
            }
        }
    }
}

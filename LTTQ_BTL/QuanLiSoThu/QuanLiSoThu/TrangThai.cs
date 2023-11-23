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
    public partial class TrangThai : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public TrangThai()
        {
            InitializeComponent();
        }
        private void LoadDL()
        {
            DataTable dt = db.DocBang("Select * From TrangThai");
            dgvTrangThai.DataSource = dt;

            dgvTrangThai.Columns[0].HeaderText = "Mã trạng thái";
            dgvTrangThai.Columns[1].HeaderText = "Tên trạng thái";
            dgvTrangThai.Columns[2].HeaderText = "Ghi chú";

            foreach (DataGridViewColumn column in dgvTrangThai.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvTrangThai.RowTemplate.Height = 100;
            dgvTrangThai.Columns[1].Width = 400;
        }

        private void TrangThai_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaTT.Text.Trim().Equals(""))
            {
                MessageBox.Show("Hãy nhập mã trạng thái", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaTT.Focus();
            }
            else
            {
                if (txtTenTT.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Hãy nhập tên trạng thái", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenTT.Focus();
                }
                else
                {
                    DataTable tb = db.DocBang("Select * From TrangThai Where MaTT = N'" + txtMaTT.Text + "'");
                    if (tb.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã trạng thái này đã tồn tại, hãy nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaTT.Focus();
                    }
                    else
                    {
                        db.CapNhat("Insert Into TrangThai Values (N'" + txtMaTT.Text + "', N'" + txtTenTT.Text + "', N'" + txtGhiChu.Text + "')");
                        MessageBox.Show("Dữ liệu của bạn đã được thêm mới thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDL();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTT.Text.Trim().Equals(""))
            {
                MessageBox.Show("Hãy chọn trạng thái cần sửa trước");
            }
            else
            {
                db.CapNhat("Update TrangThai Set TenTT = N'" + txtTenTT.Text + "', GhiChu = N'" + txtGhiChu.Text + "' Where MaTT = N'" + txtMaTT.Text + "'");
                LoadDL() ;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaTT.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã để xóa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    db.KetNoi();

                    // Delete records in Thu_SuKien first
                    string deleteThuSuKienQuery = "DELETE FROM Chuong WHERE MaTT = N'" + txtMaTT.Text.ToString() + "'";
                    db.CapNhat(deleteThuSuKienQuery);

                    // Update MaSK in Thu_SK table (if needed)
                    string updateThuSKQuery = "UPDATE Chuong SET MaTT = '0' WHERE MaTT = N'" + txtMaTT.Text.ToString() + "'";
                    db.CapNhat(updateThuSKQuery);

                    // Delete the event record in SuKien
                    string deleteSuKienQuery = "DELETE FROM TrangThai WHERE MaTT = N'" + txtMaTT.Text.ToString() + "'";
                    db.CapNhat(deleteSuKienQuery);

                    LoadDL();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTT.Text = " ";
            txtTenTT.Text = "";
            txtGhiChu.Text = "";
            txtMaTT.Focus();
        }

        private void dgvTrangThai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaTT.Text = dgvTrangThai.CurrentRow.Cells[0].Value.ToString();
            txtTenTT.Text = dgvTrangThai.CurrentRow.Cells[1].Value.ToString();
            txtGhiChu.Text = dgvTrangThai.CurrentRow.Cells[2].Value.ToString();
        }
    }
}

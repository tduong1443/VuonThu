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
    public partial class Khu : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public Khu()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            DataTable dt = db.DocBang("Select * From Khu");
            dgvKhu.DataSource = dt;

            dgvKhu.Columns[0].HeaderText = "Mã khu";
            dgvKhu.Columns[1].HeaderText = "Tên khu";
            dgvKhu.Columns[2].HeaderText = "Ghi chú";

            foreach (DataGridViewColumn column in dgvKhu.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvKhu.RowTemplate.Height = 100;
            dgvKhu.Columns[1].Width = 400;
        }

        private void ResetValue()
        {
            txtMaKhu.Text = txtTenKhu.Text = txtGhiChu.Text = "";
        }

        private void Khu_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaKhu.Text.Trim().Equals(""))
            {
                MessageBox.Show("Hãy nhập mã khu");
                txtMaKhu.Focus();
            }
            else
            {
                if (txtTenKhu.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Hãy nhập tên khu");
                    txtTenKhu.Focus();
                }
                else
                {
                    DataTable tb = db.DocBang("Select * From Khu Where MaKhu = N'" + txtMaKhu.Text + "'");
                    if (tb.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã khu này đã tồn tại, hãy nhập mã khác!");
                        txtMaKhu.Focus();
                    }
                    else
                    {
                        db.CapNhat("Insert Into Khu Values (N'" + txtMaKhu.Text + "', N'" + txtTenKhu.Text + "', N'" + txtGhiChu.Text + "')");
                        LoadDL();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKhu.Text.Trim().Equals(""))
            {
                MessageBox.Show("Hãy chọn khu cần sửa trước");
            }
            else
            {
                db.CapNhat("Update Khu Set TenKhu = N'" + txtTenKhu.Text + "', GhiChu = N'" + txtGhiChu.Text + "' Where MaKhu = N'" + txtMaKhu.Text + "'");
                LoadDL() ;
                ResetValue();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaKhu.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã để xóa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    db.KetNoi();

                    // Delete records in Thu_SuKien first
                    string deleteThuSuKienQuery = "DELETE FROM Chuong WHERE MaKhu = N'" + txtMaKhu.Text.ToString() + "'";
                    db.CapNhat(deleteThuSuKienQuery);

                    // Update MaSK in Thu_SK table (if needed)
                    string updateThuSKQuery = "UPDATE Chuong SET MaKhu = '0' WHERE MaKhu = N'" + txtMaKhu.Text.ToString() + "'";
                    db.CapNhat(updateThuSKQuery);

                    // Delete the event record in SuKien
                    string deleteSuKienQuery = "DELETE FROM Khu WHERE MaKhu = N'" + txtMaKhu.Text.ToString() + "'";
                    db.CapNhat(deleteSuKienQuery);

                    dgvKhu.DataSource = db.DocBang("select * from Khu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        private void dgvKhu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaKhu.Text = dgvKhu.CurrentRow.Cells[0].Value.ToString();
            txtTenKhu.Text = dgvKhu.CurrentRow.Cells[1].Value.ToString();
            txtGhiChu.Text = dgvKhu.CurrentRow.Cells[2].Value.ToString();
        }
    }
}

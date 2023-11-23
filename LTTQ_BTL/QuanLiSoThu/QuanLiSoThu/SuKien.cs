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
    public partial class SuKien : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        public SuKien()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            dgvSuKien.DataSource = pd.DocBang("Select * From SuKien");
            dgvSuKien.Columns[0].HeaderText = "Mã sự kiện";
            dgvSuKien.Columns[1].HeaderText = "Tên sự kiện";
            dgvSuKien.Columns[2].HeaderText = "Ghi chú";

            foreach (DataGridViewColumn column in dgvSuKien.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvSuKien.RowTemplate.Height = 100;
            dgvSuKien.Columns[1].Width = 400;
        }

        private void ResetValue()
        {
            txtMaSK.Text = " ";
            txtTenSK.Text = "";
            txtGhiChu.Text = "";
            txtMaSK.Focus();
        }

        private void SuKien_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaSK.Text == "" || txtTenSK.Text == "")
            {
                MessageBox.Show("Dữ liệu chưa được nhập đầy đủ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT MaSK FROM SuKien WHERE MaSK = '" + txtMaSK.Text + "'";

            System.Data.DataTable table = pd.DocBang(query);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Mã nhập vào đã tồn tại trong dữ liệu. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string a = "INSERT INTO SuKien VALUES ('" + txtMaSK.Text + "','" + txtTenSK.Text + "','" + txtGhiChu.Text + "')";
                pd.CapNhat(a);
                MessageBox.Show("Dữ liệu của bạn đã được thêm mới thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvSuKien.DataSource = pd.DocBang("Select * From SuKien");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra Mã SK có không
            if (txtMaSK.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string query = "Update SuKien set TenSK = '" + txtTenSK.Text + "', GhiChu = '" + txtGhiChu.Text + "' WHERE MaSK = '" + txtMaSK.Text + "'";
                pd.CapNhat(query);
                dgvSuKien.DataSource = pd.DocBang("Select * From SuKien");
            }

            // Kiểm tra xem Mã SK có tồn tại không
            string queryCheckExist = "SELECT COUNT(*) FROM SuKien WHERE MaSK = @MaSK";
            SqlParameter parameter = new SqlParameter("@MaSK", txtMaSK.Text);

            int count = (int)pd.LayDuLieu(queryCheckExist, parameter).Rows[0][0];

            if (count == 0)
            {
                MessageBox.Show("Mã sự kiện bạn chọn không tồn tại. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSK.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã để xóa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    pd.KetNoi();

                    // Delete records in Thu_SuKien first
                    string deleteThuSuKienQuery = "DELETE FROM Thu_SuKien WHERE MaSK = N'" + txtMaSK.Text.ToString() + "'";
                    pd.CapNhat(deleteThuSuKienQuery);

                    // Update MaSK in Thu_SK table (if needed)
                    string updateThuSKQuery = "UPDATE Thu_SuKien SET MaSK = '0' WHERE MASK = N'" + txtMaSK.Text.ToString() + "'";
                    pd.CapNhat(updateThuSKQuery);

                    // Delete the event record in SuKien
                    string deleteSuKienQuery = "DELETE FROM SuKien WHERE MaSK = N'" + txtMaSK.Text.ToString() + "'";
                    pd.CapNhat(deleteSuKienQuery);

                    dgvSuKien.DataSource = pd.DocBang("select * from SuKien");
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

        private void dgvSuKien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSK.Text = dgvSuKien.CurrentRow.Cells[0].Value.ToString();
            txtTenSK.Text = dgvSuKien.CurrentRow.Cells[1].Value.ToString();
            txtGhiChu.Text = dgvSuKien.CurrentRow.Cells[2].Value.ToString();
        }
    }
}

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
    public partial class ThuSuKien : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        public ThuSuKien()
        {
            InitializeComponent();

            foreach (string i in ListCombobox("SELECT MaSK FROM SuKien"))
            {
                cboxMaSK.Items.Add(i);
            }
            cboxMaSK.SelectedIndex = -1;

            foreach (string i in ListCombobox("SELECT MaThu FROM Thu"))
            {
                cboxMaThu.Items.Add(i);
            }
            cboxMaThu.SelectedIndex = -1;
        }

        public List<String> ListCombobox(string _query)
        {
            SqlConnection connection = new SqlConnection(pd.getStringConncet());
            List<string> dataList = new List<string>();
            connection.Open();
            string query = _query;
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string columValue = reader.GetString(0);
                    dataList.Add(columValue);
                }
            }
            connection.Close();
            return dataList;

        }

        private void LoadDL()
        {
            dgvThuSK.DataSource = pd.DocBang("Select * From Thu_SuKien");
            dgvThuSK.Columns[0].HeaderText = "Mã sự kiện";
            dgvThuSK.Columns[1].HeaderText = "Mã thú";
            dgvThuSK.Columns[2].HeaderText = "Ngày BĐ";
            dgvThuSK.Columns[3].HeaderText = "Lý do";
            dgvThuSK.Columns[4].HeaderText = "Cách khắc phục";
            dgvThuSK.Columns[5].HeaderText = "Ngày KT";

            foreach (DataGridViewColumn column in dgvThuSK.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvThuSK.RowTemplate.Height = 100;
        }

        private void ThuSuKien_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ các control trên form
            string maSK = cboxMaSK.Text.Trim();
            string maThu = cboxMaThu.Text.Trim();
            DateTime ngayBD = dtpNgayBD.Value;
            DateTime ngayKT = dtpNgayKT.Value;
            string maLD = txtMaLD.Text.Trim();
            string maKP = txtMaKP.Text.Trim();

            // Kiểm tra xem đã nhập đủ dữ liệu chưa
            if (!KiemTraDuLieuNhap())
            {
                MessageBox.Show("Dữ liệu chưa được nhập đầy đủ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem mã nhập vào đã tồn tại trong DataGridView chưa
            if (KiemTraTrungMaDataGridView(maSK, maThu))
            {
                MessageBox.Show("Mã nhập vào đã tồn tại trong dữ liệu. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm dữ liệu vào cơ sở dữ liệu
            ThemDuLieu(maSK, maThu, ngayBD, ngayKT, maLD, maKP);

            // Hiển thị thông báo thành công
            MessageBox.Show("Dữ liệu của bạn đã được thêm mới thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Load lại dữ liệu cho DataGridView
            LoadDL();
        }

        private bool KiemTraDuLieuNhap()
        {
            return !string.IsNullOrEmpty(cboxMaSK.Text.Trim()) &&
                   !string.IsNullOrEmpty(cboxMaThu.Text.Trim()) &&
                   !string.IsNullOrEmpty(txtMaLD.Text.Trim()) &&
                   !string.IsNullOrEmpty(txtMaKP.Text.Trim());
        }

        private bool KiemTraTrungMaDataGridView(string maSK, string maThu)
        {
            foreach (DataGridViewRow row in dgvThuSK.Rows)
            {
                if (row.Cells["MaSK"].Value != null && row.Cells["MaThu"].Value != null)
                {
                    if (row.Cells["MaSK"].Value.ToString() == maSK && row.Cells["MaThu"].Value.ToString() == maThu)
                    {
                        return true; // Mã đã tồn tại trong DataGridView
                    }
                }
            }
            return false;
        }

        private void ThemDuLieu(string maSK, string maThu, DateTime ngayBD, DateTime ngayKT, string maLD, string maKP)
        {
            string queryInsert = $"INSERT INTO Thu_SuKien (MaSK, MaThu, NgayBD, NgayKT, TenLD, CachKP) VALUES ('{maSK}', '{maThu}', '{ngayBD}', '{ngayKT}', '{maLD}', '{maKP}')";
            pd.CapNhat(queryInsert);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSK = cboxMaSK.Text.Trim();
            string maThu = cboxMaThu.Text.Trim();

            if (cboxMaSK.Text == "" & cboxMaThu.Text == "")
            {
                MessageBox.Show("Bạn phải chọn mã để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra mã nhập vào có tồn tại không
            string queryCheckExistence = "SELECT COUNT(*) FROM Thu_SuKien WHERE MaSK = @MaSK AND MaThu = @MaThu";
            SqlParameter[] parameters = {
                new SqlParameter("@MaSK", SqlDbType.NVarChar),
                new SqlParameter("@MaThu", SqlDbType.NVarChar)
            };
            parameters[0].Value = cboxMaSK.Text;
            parameters[1].Value = cboxMaThu.Text;

            int rowCount = (int)pd.LayDuLieu(queryCheckExistence, parameters).Rows[0][0];

            if (rowCount == 0)
            {
                MessageBox.Show("Mã bạn chọn không tồn tại trong dữ liệu hệ thống. Vui lòng thử lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thực hiện cập nhật dữ liệu
            string query = $"UPDATE Thu_SuKien SET NgayBD = '{dtpNgayBD.Value}', NgayKT = '{dtpNgayKT.Value}'," +
                $" TenLD = '{txtMaLD.Text}', CachKP = '{txtMaKP.Text}' WHERE MaSK = '{cboxMaSK.Text}' AND MaThu = '{cboxMaThu.Text}'";

            pd.CapNhat(query);

            MessageBox.Show("Dữ liệu đã được cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Load lại dữ liệu từ database lên DataGridView
            LoadDL();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any cell is selected
                if (dgvThuSK.CurrentCell == null)
                {
                    MessageBox.Show("Bạn phải chọn một ô để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy MaSK và MaThu của ô được chọn
                int selectedRowIndex = dgvThuSK.CurrentCell.RowIndex;
                string maSK = dgvThuSK.Rows[selectedRowIndex].Cells["MaSK"].Value.ToString();
                string maThu = dgvThuSK.Rows[selectedRowIndex].Cells["MaThu"].Value.ToString();

                // Hiển thị hộp thoại xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Xóa dữ liệu từ bảng Thu_SuKien
                    XoaDuLieu(maSK, maThu);

                    // Hiển thị thông báo xóa thành công
                    MessageBox.Show("Dữ liệu đã được xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại dữ liệu cho DataGridView
                    dgvThuSK.DataSource = pd.DocBang("SELECT * FROM Thu_SuKien");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaDuLieu(string maSK, string maThu)
        {
            string queryDeleteThuSuKien = $"DELETE FROM Thu_SuKien WHERE MaSK = '{maSK}' AND MaThu = '{maThu}'";
            pd.CapNhat(queryDeleteThuSuKien);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboxMaSK.Text = " ";
            cboxMaThu.Text = "";
            txtMaLD.Text = "";
            txtMaKP.Text = "";
            cboxMaSK.Focus();
            cboxMaThu.Focus();
        }

        private void dgvThuSK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cboxMaSK.Text = dgvThuSK.CurrentRow.Cells[0].Value.ToString();
            cboxMaThu.Text = dgvThuSK.CurrentRow.Cells[1].Value.ToString();
            dtpNgayBD.Text = dgvThuSK.CurrentRow.Cells[2].Value.ToString();
            txtMaLD.Text = dgvThuSK.CurrentRow.Cells[3].Value.ToString();
            txtMaKP.Text = dgvThuSK.CurrentRow.Cells[4].Value.ToString();
            dtpNgayKT.Text = dgvThuSK.CurrentRow.Cells[5].Value.ToString();
        }
    }
}

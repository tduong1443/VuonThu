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
    public partial class Loai : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        public Loai()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            dgvLoai.DataSource = pd.DocBang("Select * From Loai");

            dgvLoai.Columns[0].HeaderText = "Mã loài";
            dgvLoai.Columns[1].HeaderText = "Tên loài";
            dgvLoai.Columns[2].HeaderText = "Ghi chú";

            foreach (DataGridViewColumn column in dgvLoai.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvLoai.RowTemplate.Height = 100;
        }

        private void Loai_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void dgvLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLoai.Text = dgvLoai.CurrentRow.Cells[0].Value.ToString();
            txtTenLoai.Text = dgvLoai.CurrentRow.Cells[1].Value.ToString();
            txtGhiChu.Text = dgvLoai.CurrentRow.Cells[2].Value.ToString();
        }

        private void ResetValue()
        {
            txtTenLoai.Text = txtMaLoai.Text = txtGhiChu.Text = "";
        }

        // Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaLoai.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã loài, hãy nhập mã loài !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if(txtTenLoai.Text == "")
                {
                    MessageBox.Show("Hãy nhập đủ thông tin !", "Thông báo",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    DataTable dt = pd.DocBang("Select * From Loai Where MaLoai = N'" + (txtMaLoai.Text).Trim() + "'");

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã thú này đã tồn tại mời nhập mã khác !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaLoai.Focus();
                    }

                    else
                    {
                        string sql = "Insert into Loai (MaLoai, TenLoai, GhiChu) " +
                            "Values (@MaLoai, @TenLoai, @GhiChu)";

                        SqlParameter[] sqlParameter = new SqlParameter[]
                        {
                            new SqlParameter("@MaLoai", txtMaLoai.Text),
                            new SqlParameter("@TenLoai", txtTenLoai.Text),
                            new SqlParameter("@GhiChu", txtGhiChu.Text)
                        };

                        pd.CapNhatTS(sql, sqlParameter);

                        MessageBox.Show("Thêm mới thành công !", "Thông báo", MessageBoxButtons.OK);

                        LoadDL();
                    }
                }
            }
        }

        // Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLoai.Text == "")
            {
                MessageBox.Show("Hãy chọn dữ liệu cần sửa !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "Update Loai Set TenLoai = @TenLoai, GhiChu = @GhiChu " +
                        "Where MaLoai = N'" + txtMaLoai.Text + "'";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@TenLoai", txtTenLoai.Text),
                        new SqlParameter("@GhiChu", txtGhiChu.Text)
                    };
                    pd.CapNhatTS(sql, parameters);
                    MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK);
                    LoadDL();
                    ResetValue();
                }
            }
        }

        // Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaLoai.Text == "")
                {
                    MessageBox.Show("Hãy chọn dữ liệu cần xóa !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    pd.KetNoi();

                    // Check if there are any records in the Thu table referencing the current Loai
                    string checkReferencesQuery = $"SELECT COUNT(*) FROM Thu WHERE MaLoai = N'{txtMaLoai.Text}'";
                    int referenceCount = (int)pd.LayDuLieu(checkReferencesQuery).Rows[0][0];

                    if (referenceCount > 0)
                    {
                        MessageBox.Show("Không thể xóa loại này vì có thú đang tham chiếu đến nó.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Delete records in Thu table first
                    string deleteThuQuery = $"DELETE FROM Thu WHERE MaLoai = N'{txtMaLoai.Text}'";
                    pd.CapNhat(deleteThuQuery);

                    // Delete the record in Loai table
                    string deleteLoaiQuery = $"DELETE FROM Loai WHERE MaLoai = N'{txtMaLoai.Text}'";
                    pd.CapNhat(deleteLoaiQuery);

                    LoadDL();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }
    }
}

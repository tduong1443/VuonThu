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
    public partial class ThucAn : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public ThucAn()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            dgvThucAn.DataSource = db.DocBang("Select * From ThucAn");

            dgvThucAn.Columns[0].HeaderText = "Mã Thức ăn";
            dgvThucAn.Columns[1].HeaderText = "Tên";
            dgvThucAn.Columns[2].HeaderText = "Công dụng";
            dgvThucAn.Columns[3].HeaderText = "Đơn Vị";
            dgvThucAn.Columns[4].HeaderText = "Đơn Giá";
            dgvThucAn.Columns[5].HeaderText = "Ngày AD";

            foreach (DataGridViewColumn column in dgvThucAn.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvThucAn.RowTemplate.Height = 100;

        }

        private void ThucAn_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaTA.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã Thức ăn, hãy nhập mã !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (txtTen.Text == "" || txtDonGia.Text == "")
                {
                    MessageBox.Show("Hãy nhập đủ thông tin !", "Thông báo",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    DataTable dt = db.DocBang("Select * From ThucAn Where MaTA = N'" + (txtMaTA.Text).Trim() + "'");

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã thức ăn này đã tồn tại mời nhập mã khác !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaTA.Focus();
                    }

                    else
                    {
                        string sql = "Insert into ThucAn (MaTA, Ten, CongDung, MaDV, DonGia, NgayAD) " +
                            "Values (@MaTA, @Ten, @CongDung, @DonVi, @DonGia, @NgayAD)";

                        SqlParameter[] sqlParameter = new SqlParameter[]
                        {
                            new SqlParameter("@MaTA", txtMaTA.Text),
                            new SqlParameter("@Ten", txtTen.Text),
                            new SqlParameter("@CongDung", txtCongDung.Text),
                            new SqlParameter("@DonVi", txtMaDV.Text),
                            new SqlParameter("@DonGia", txtDonGia.Text),
                            new SqlParameter("@NgayAD", dtpNgayAD.Value.ToString("yyyy-MM-dd"))
                        };

                        db.CapNhatTS(sql, sqlParameter);
                        LoadDL();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaTA.Text == "")
            {
                MessageBox.Show("Hãy chọn dữ liệu cần sửa !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sql = "Update ThucAn Set MaTA = @MaTA, Ten = @Ten, CongDung = @CongDung, MaDV = @DonVi, DonGia = @DonGia," +
                        " NgayAD = @NgayAD " +
                        "Where MaTA = N'" + txtMaTA.Text + "'";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MaTA", txtMaTA.Text),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@CongDung", txtCongDung.Text),
                        new SqlParameter("@DonVi", txtMaDV.Text),
                        new SqlParameter("@DonGia", txtDonGia.Text),
                        new SqlParameter("@NgayAD", dtpNgayAD.Text),
                    };
                    db.CapNhatTS(sql, parameters);
                    MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK);
                    LoadDL();
                    //ResetValue();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaTA.Text == "")
                {
                    MessageBox.Show("Bạn phải chọn mã để xóa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    db.KetNoi();

                    // Delete records in Thu_SuKien first
                    string deleteThuSuKienQuery = "DELETE FROM Thu_ThucAn WHERE MaTA = N'" + txtMaTA.Text.ToString() + "'";
                    db.CapNhat(deleteThuSuKienQuery);

                    // Update MaSK in Thu_SK table (if needed)
                    string updateThuSKQuery = "UPDATE Thu_ThucAn SET MaTA = '0' WHERE MaTA = N'" + txtMaTA.Text.ToString() + "'";
                    db.CapNhat(updateThuSKQuery);

                    // Delete the event record in SuKien
                    string deleteSuKienQuery = "DELETE FROM ThucAn WHERE MaTA = N'" + txtMaTA.Text.ToString() + "'";
                    db.CapNhat(deleteSuKienQuery);

                    dgvThucAn.DataSource = db.DocBang("select * from ThucAn");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTA.Text = " ";
            txtTen.Text = "";
            txtCongDung.Text = "";
            txtMaDV.Text = "";
            txtDonGia.Text = "";
            txtMaTA.Focus();
        }

        private void dgvThucAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaTA.Text = dgvThucAn.CurrentRow.Cells[0].Value.ToString();
            txtTen.Text = dgvThucAn.CurrentRow.Cells[1].Value.ToString();
            txtCongDung.Text = dgvThucAn.CurrentRow.Cells[2].Value.ToString();
            txtMaDV.Text = dgvThucAn.CurrentRow.Cells[3].Value.ToString();
            txtDonGia.Text = dgvThucAn.CurrentRow.Cells[4].Value.ToString();
            dtpNgayAD.Text = dgvThucAn.CurrentRow.Cells[5].Value.ToString();
        }
    }
}

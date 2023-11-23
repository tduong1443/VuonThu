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
    public partial class ThuChuong : Form
    {
        ProccessDatabase pd = new ProccessDatabase();
        public ThuChuong()
        {
            InitializeComponent();
        }

        private void LoadDL()
        {
            string sql = "Select * From ThuChuong";

            dgvThuChuong.DataSource = pd.DocBang(sql);

            dgvThuChuong.Columns[0].HeaderText = "Mã chuồng";
            dgvThuChuong.Columns[1].HeaderText = "Mã thú";
            dgvThuChuong.Columns[2].HeaderText = "Ngày vào";
            dgvThuChuong.Columns[3].HeaderText = "Lý do vào";

            foreach (DataGridViewColumn column in dgvThuChuong.Columns)
            {
                column.Width = 300; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvThuChuong.RowTemplate.Height = 100;
            LoadDataToComboBox();
            ResetValue();
        }

        // Load dữ liệu lên combobox
        private void LoadDataToComboBox()
        {
            DataTable dtChuong = pd.DocBang("Select * From Chuong");
            cbMaChuong.DataSource = dtChuong;
            cbMaChuong.DisplayMember = "MaChuong";
            cbMaChuong.ValueMember = "MaChuong";

            // Gán sự kiện cho ComboBox cbMaChuong
            cbMaChuong.SelectedIndexChanged += new EventHandler(cbMaChuong_SelectedIndexChanged);
        }

        private void cbMaChuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaChuong.SelectedValue != null)
            {
                // Lấy Mã Chuồng từ ComboBox cbMaChuong
                string maChuong = cbMaChuong.SelectedValue.ToString();

                // Lấy Mã Loài từ cột MaLoai của dòng hiện tại trong bảng Chuong
                DataTable dtChuong = pd.DocBang($"SELECT MaLoai FROM Chuong WHERE MaChuong = '{maChuong}'");

                if (dtChuong.Rows.Count > 0)
                {
                    string maLoai = dtChuong.Rows[0]["MaLoai"].ToString();

                    // Lấy danh sách Mã Thú thuộc loài của mã chuồng tương ứng
                    string sql = $"SELECT MaThu FROM Thu WHERE MaLoai = '{maLoai}'";

                    // Load và gán danh sách Mã Thú vào ComboBox cbMaThu
                    DataTable dtThu = pd.DocBang(sql);
                    cbMaThu.DataSource = dtThu;
                    cbMaThu.DisplayMember = "MaThu";
                    cbMaThu.ValueMember = "MaThu";
                }
            }
        }

        private void ThuChuong_Load(object sender, EventArgs e)
        {
            LoadDL();
        }

        // Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!Kiemtradltrong())
            {
                MessageBox.Show("Hãy nhập đủ thông tin !", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                DataTable dt = pd.DocBang("Select * From ThuChuong Where MaThu = N'" + (cbMaThu.SelectedValue.ToString()).Trim() + "' " +
                    "And MaChuong = N'" + (cbMaChuong.SelectedValue.ToString()).Trim() + "'");

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Cặp mã này đã tồn tại mời nhập mã khác !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    string sqlGetSL = "SELECT SLThu FROM Chuong WHERE MaChuong = @maChuong";

                    int slCu = (int)pd.LayGiaTri(sqlGetSL,
                      new SqlParameter("@maChuong", cbMaChuong.SelectedValue.ToString()));

                    string sql = "Insert into ThuChuong Values(@MaChuong, @MaThu, @NgayVao, @LyDoVao)";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MaChuong", cbMaChuong.SelectedValue.ToString()),
                        new SqlParameter("@MaThu", cbMaThu.SelectedValue.ToString()),
                        new SqlParameter("@NgayVao", dtpNgayvao.Value.ToString("yyyy-MM-dd")),
                        new SqlParameter("@LyDoVao", txtLyDo.Text)
                    };

                    pd.CapNhatTS(sql, parameters);

                    int slMoi = slCu + 1;

                    // Cập nhật số lượng trong bảng Chuong
                    string sqlCapNhatSoLuong = "UPDATE Chuong SET SLThu = @slMoi WHERE MaChuong = @MaChuong";

                    SqlParameter[] parametersCapNhatSoLuong = new SqlParameter[]
                    {
                        new SqlParameter("@slMoi", slMoi),
                        new SqlParameter("@MaChuong", cbMaChuong.SelectedValue.ToString())
                    };

                    pd.CapNhatTS(sqlCapNhatSoLuong, parametersCapNhatSoLuong);

                    MessageBox.Show("Thêm mới thành công !", "Thông báo", MessageBoxButtons.OK);

                    LoadDL();
                    ResetValue();
                }
            }
        }

        private bool Kiemtradltrong()
        {
            return !string.IsNullOrEmpty(cbMaChuong.Text.Trim()) &&
                !string.IsNullOrEmpty(cbMaThu.Text.Trim()) &&
                !string.IsNullOrEmpty(txtLyDo.Text.Trim());
        }

        private void ResetValue()
        {
            cbMaThu.Text = cbMaChuong.Text = txtLyDo.Text = ""; 
        }

        // Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!Kiemtradltrong())
            {
                MessageBox.Show("Hãy chọn thông tin cần sửa !", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Lấy giá trị mới từ ComboBox
                    string newMaChuong = cbMaChuong.SelectedValue.ToString();
                    string newMaThu = cbMaThu.SelectedValue.ToString();

                    // Lấy dòng đang được chọn trong DataGridView
                    DataGridViewRow selectedRow = dgvThuChuong.CurrentRow;

                    // Kiểm tra nếu có dòng được chọn
                    if (selectedRow != null)
                    {
                        // Lấy MaThu và MaChuong từ dòng đang được chọn
                        string maThu = selectedRow.Cells["MaThu"].Value.ToString();
                        string maChuong = selectedRow.Cells["MaChuong"].Value.ToString();

                        // Lấy số lượng hiện tại từ bảng Chuong cho mã chuồng cũ
                        string sqlGetSLCu = "Select SLThu From Chuong Where MaChuong = @maChuongCu";
                        int slCu = (int)pd.LayGiaTri(sqlGetSLCu, new SqlParameter("@maChuongCu", maChuong));

                        // Lấy số lượng hiện tại từ bảng Chuong cho mã chuồng mới
                        string sqlGetSLMoi = "Select SLThu From Chuong Where MaChuong = @maChuongMoi";
                        int slMoi = (int)pd.LayGiaTri(sqlGetSLMoi, new SqlParameter("@maChuongMoi", newMaChuong));

                        // Kiểm tra xem bản ghi có tồn tại hay không
                        string sqlCheckExist = "Select Count(*) From ThuChuong Where MaChuong = @maChuongMoi And MaThu = @maThu";
                        SqlParameter[] parametersCheckExist = new SqlParameter[]
                        {
                            new SqlParameter("@maChuongMoi", newMaChuong),
                            new SqlParameter("@maThu", maThu)
                        };
                        int count = (int)pd.LayGiaTri(sqlCheckExist, parametersCheckExist);

                        if (count > 0)
                        {
                            // Bản ghi đã tồn tại
                            // Thực hiện cập nhật trong cơ sở dữ liệu chỉ cho các trường cần cập nhật
                            string sqlUpdate = "Update ThuChuong Set NgayVao = @Ngayvao, LyDoVao = @LyDo " +
                                " Where MaThu = @MaThuOld And MaChuong = @MaChuongOld";

                            SqlParameter[] parameters = new SqlParameter[]
                            {
                                new SqlParameter("@NgayVao", dtpNgayvao.Value.ToString("yyyy-MM-dd")),
                                new SqlParameter("@LyDo", txtLyDo.Text),
                                new SqlParameter("@MaChuongOld", maChuong),
                                new SqlParameter("@MaThuOld", maThu)
                            };

                            pd.CapNhatTS(sqlUpdate, parameters);

                            MessageBox.Show("Cập nhật dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK);

                            LoadDL();
                            ResetValue();
                        }
                        else
                        {
                            // Thực hiện cập nhật trong cơ sở dữ liệu
                            string sqlUpdate = "Update ThuChuong Set MaChuong = @MaChuong, MaThu = @MaThu, NgayVao = @Ngayvao, LyDoVao = @LyDo " +
                            " Where MaThu = @MaThuOld And MaChuong = @MaChuongOld";

                            SqlParameter[] parameters = new SqlParameter[]
                            {
                                new SqlParameter("@MaChuong", newMaChuong),
                                new SqlParameter("@MaThu", newMaThu),
                                new SqlParameter("@NgayVao", dtpNgayvao.Value.ToString("yyyy-MM-dd")),
                                new SqlParameter("@LyDo", txtLyDo.Text),
                                new SqlParameter("@MaChuongOld", maChuong),
                                new SqlParameter("@MaThuOld", maThu)
                            };

                            pd.CapNhatTS(sqlUpdate, parameters);

                            // Cập nhật số lượng trong bảng Chuong cho mã chuồng mới
                            string sqlUpdateChuongMoi = "Update Chuong Set SLThu = @slMoi Where MaChuong = @maChuongMoi";
                            SqlParameter[] parametersChuongMoi = new SqlParameter[]
                            {
                                new SqlParameter("@slMoi", slMoi + 1),
                                new SqlParameter("@maChuongMoi", newMaChuong)
                            };
                            pd.CapNhatTS(sqlUpdateChuongMoi, parametersChuongMoi);

                            // Cập nhật số lượng trong bảng Chuong cho mã chuồng cũ
                            string sqlUpdateChuongCu = "Update Chuong Set SLThu = @slCu Where MaChuong = @maChuongCu";
                            SqlParameter[] parametersChuongCu = new SqlParameter[]
                            {
                                new SqlParameter("@slCu", slCu - 1),
                                new SqlParameter("@maChuongCu", maChuong)
                            };
                            pd.CapNhatTS(sqlUpdateChuongCu, parametersChuongCu);
                            MessageBox.Show("Cập nhật dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK);

                            LoadDL();
                            ResetValue();
                        }
                        
                    }
                }
            }
        }

        // Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!Kiemtradltrong())
            {
                MessageBox.Show("Hãy chọn thông tin cần xóa !", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Lấy số lượng cũ từ bảng Chuong
                    string sqlGetSL = "SELECT SLThu FROM Chuong WHERE MaChuong = @maChuong";

                    int slCu = (int)pd.LayGiaTri(sqlGetSL,
                      new SqlParameter("@maChuong", cbMaChuong.SelectedValue.ToString()));

                    string sql = "Delete ThuChuong Where MaThu = N'" + cbMaThu.SelectedValue.ToString() + "' " +
                    "And MaChuong = N'" + cbMaChuong.SelectedValue.ToString() + "'";

                    pd.CapNhat(sql);

                    int slMoi = slCu - 1;

                    string sqlSoLuong = "Update Chuong Set SLThu = @slMoi Where MaChuong = @MaChuong";

                    SqlParameter[] parameters1 = new SqlParameter[]
                    {
                        new SqlParameter("@slMoi", slMoi),
                        new SqlParameter("@MaChuong", cbMaChuong.SelectedValue.ToString())
                    };
                    pd.CapNhatTS(sqlSoLuong, parameters1);

                    MessageBox.Show("Xóa dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK);

                    LoadDL();
                    ResetValue();
                }
            }
        }

        // Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        private void dgvThuChuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbMaChuong.Text = dgvThuChuong.CurrentRow.Cells[0].Value.ToString();
            cbMaThu.Text = dgvThuChuong.CurrentRow.Cells[1].Value.ToString();
            dtpNgayvao.Text = dgvThuChuong.CurrentRow.Cells[2].Value.ToString();
            txtLyDo.Text = dgvThuChuong.CurrentRow.Cells[3].Value.ToString();
        }
    }
}

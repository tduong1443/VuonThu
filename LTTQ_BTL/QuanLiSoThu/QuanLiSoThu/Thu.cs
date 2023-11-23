using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSoThu
{
    public partial class Thu : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        private string appDirectory;
        public Thu()
        {
            InitializeComponent();
            appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        }

        private void LoadData()
        {
            dgvThu.DataSource = db.DocBang("SELECT Thu.MaThu, TenNguonGoc, TenThu, Loai.TenLoai," +
                "SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, " +
                "NgaySinh, Anh, TuoiTho FROM Thu LEFT JOIN Loai ON Thu.MaLoai = Loai.MaLoai");

            //dgvThu.DataSource = db.DocBang("Select * From Thu");

            //dgvThu.DataSource = db.DocBang("Select * From DanhSachThu");
            
            dgvThu.Columns[0].HeaderText = "Mã thú";
            dgvThu.Columns[1].HeaderText = "Nguồn gốc";
            dgvThu.Columns[2].HeaderText = "Tên thú";
            dgvThu.Columns[3].HeaderText = "Tên loài";
            dgvThu.Columns[4].HeaderText = "Số lượng";
            dgvThu.Columns[5].HeaderText = "Sách đỏ";
            dgvThu.Columns[6].HeaderText = "Tên khoa học";
            dgvThu.Columns[7].HeaderText = "Tên tiếng anh";
            dgvThu.Columns[8].HeaderText = "Tên tiếng việt";
            dgvThu.Columns[9].HeaderText = "Kiểu sinh";
            dgvThu.Columns[10].HeaderText = "Ngày vào";
            dgvThu.Columns[11].HeaderText = "Đặc điểm";
            dgvThu.Columns[12].HeaderText = "Ngày sinh";
            dgvThu.Columns[13].HeaderText = "Ảnh";
            dgvThu.Columns[14].HeaderText = "Tuổi thọ";

            DataGridViewImageColumn pic = new DataGridViewImageColumn();
            pic = (DataGridViewImageColumn)dgvThu.Columns[13];
            pic.ImageLayout = DataGridViewImageCellLayout.Stretch;

            foreach (DataGridViewColumn column in dgvThu.Columns)
            {
                column.Width = 250; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvThu.RowTemplate.Height = 100; // Đặt độ cao mặc định cho tất cả các dòng

            LoadDataToComboBox();
            ResetValue();
        }

        // Load dữ liệu lên datagridview
        private void Thu_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        // Combobox hiển thị tên loài
        private void LoadDataToComboBox()
        {
            // Lấy dữ liệu từ cơ sở dữ liệu cho ComboBox
            DataTable dtLoai = db.DocBang("SELECT * FROM Loai");
            // Gán nguồn dữ liệu cho ComboBox
            cbLoai.DataSource = dtLoai;
            cbLoai.DisplayMember = "TenLoai"; // Hiển thị tên loài
            cbLoai.ValueMember = "MaLoai";    // Giữ giá trị mã loài
        }

        // reset giá trị của text box
        public void ResetValue()
        {
            txtMaThu.Text = txtNguonGoc.Text = txtTenThu.Text
                = txtTenKH.Text = txtTenTA.Text = txtTenTV.Text
                = txtDacDiem.Text = cbKieuSinh.Text = cbLoai.Text
                = txtTuoiTho.Text = txtSoLuong.Text = "";
            pbThu.Image = null;
            cbSachDo.Text = null;
        }

        // Thêm
        private bool KiemTraDL(string value)
        {
            return int.TryParse(value, out _);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if(txtMaThu.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã thú, hãy nhập mã thú !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if(txtNguonGoc.Text == "" || txtTenThu.Text == ""
                    || txtSoLuong.Text == "" ||  cbLoai.Text == "" || cbSachDo.Text == ""
                    || cbKieuSinh.Text == "" || txtTenKH.Text == "" || txtTenTA.Text == "" || txtTenTV.Text == ""|| pbThu.Image == null)
                {
                    MessageBox.Show("Hãy nhập đủ thông tin !", "Thông báo",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    if (!KiemTraDL(txtSoLuong.Text))
                    {
                        MessageBox.Show("Số lượng là 1 số nguyên, hãy nhập lại !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (!KiemTraDL(txtTuoiTho.Text))
                    {
                        MessageBox.Show("Tuổi thọ là 1 số, hãy nhập lại !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else
                    {
                        DataTable dt = db.DocBang("Select * From Thu Where MaThu = N'" + (txtMaThu.Text).Trim() + "'");

                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Mã thú này đã tồn tại mời nhập mã khác !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMaThu.Focus();
                        }

                        else
                        {
                            if (!KiemTraDL(txtSoLuong.Text))
                            {
                                MessageBox.Show("Số lượng là 1 số, hãy nhập lại !", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtSoLuong.Focus();
                            }

                            if (!KiemTraDL(txtTuoiTho.Text))
                            {
                                MessageBox.Show("Tuổi thọ là 1 số, hãy nhập lại !", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtTuoiTho.Focus();
                            }

                            else
                            {
                                byte[] imageData = ChuyenDoiDLAnh(pbThu);// Lấy giá trị từ ô được click

                                string sql = "Insert into Thu (MaThu, TenNguonGoc, TenThu, MaLoai, SoLuong," +
                                    "SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, NgaySinh, Anh, TuoiTho)" +
                                    "Values (@MaThu, @TenNguonGoc, @TenThu, @MaLoai, @SoLuong, @SachDo, @TenKhoaHoc, @TenTA," +
                                    "@TenTV, @KieuSinh, @NgayVao, @DacDiem, @NgaySinh, @Anh, @TuoiTho)";

                                // Thực hiện cập nhật
                                SqlParameter[] parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@MaThu", txtMaThu.Text),
                                    new SqlParameter("@TenNguonGoc", txtNguonGoc.Text),
                                    new SqlParameter("@TenThu", txtTenThu.Text),
                                    new SqlParameter("@MaLoai", cbLoai.SelectedValue.ToString()),
                                    new SqlParameter("@SoLuong", txtSoLuong.Text),
                                    new SqlParameter("@SachDo", cbSachDo.Text),
                                    new SqlParameter("@TenKhoaHoc", txtTenKH.Text),
                                    new SqlParameter("@TenTA", txtTenTA.Text),
                                    new SqlParameter("@TenTV", txtTenTV.Text),
                                    new SqlParameter("@KieuSinh", cbKieuSinh.Text),
                                    new SqlParameter("@NgayVao", dtpNgayvao.Value.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@DacDiem", txtDacDiem.Text),
                                    new SqlParameter("@NgaySinh", dtpNgaySinh.Value.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@Anh", imageData),
                                    new SqlParameter("@TuoiTho", txtTuoiTho.Text)
                                };
                                db.CapNhatTS(sql, parameters);

                                MessageBox.Show("Thêm mới thành công !", "Thông báo", MessageBoxButtons.OK);

                                LoadData();
                            }
                        }
                    }
                }
            }
        }

        // Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if(txtMaThu.Text == "")
            {
                MessageBox.Show("Hãy chọn dữ liệu cần sửa !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!KiemTraDL(txtSoLuong.Text))
                    {
                        MessageBox.Show("Số lượng là 1 số, hãy nhập lại !", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //if (!KiemTraDL(txtTuoiTho.Text))
                    //{
                    //    MessageBox.Show("Tuổi thọ là 1 số, hãy nhập lại !", "Thông báo",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    else
                    {
                        byte[] imageData = ChuyenDoiDLAnh(pbThu);// Lấy giá trị từ ô được click

                        string sql = "Update Thu " +
                        "Set TenNguonGoc = N'" + txtNguonGoc.Text + "', " +
                        "TenThu = N'" + txtTenThu.Text + "', " +
                        "MaLoai = N'" + cbLoai.SelectedValue.ToString() + "', " +
                        "SoLuong = '" + txtSoLuong.Text + "', " +
                        "SachDo = N'" + cbSachDo.Text + "', " +
                        "TenKhoaHoc = N'" + txtTenKH.Text + "', " +
                        "TenTA = N'" + txtTenTA.Text + "', " +
                        "TenTV = N'" + txtTenTV.Text + "', " +
                        "KieuSinh = N'" + cbKieuSinh.Text + "', " +
                        "NgayVao = '" + dtpNgayvao.Value.ToString("yyyy-MM-dd") + "', " +
                        "DacDiem = N'" + txtDacDiem.Text + "', " +
                        "NgaySinh = '" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "', " +
                        "Anh = @Anh, " +
                        "TuoiTho = '" + txtTuoiTho.Text + "' " +
                        "Where MaThu = N'" + txtMaThu.Text + "'";

                        // Thực hiện cập nhật
                        SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Anh", imageData) };
                        db.CapNhatTS(sql, parameters);

                        MessageBox.Show("Cập nhật thành công !", "Thông báo", MessageBoxButtons.OK);

                        LoadData();
                    }
                }
            }
        }

        private byte[] ChuyenDoiDLAnh(PictureBox pictureBox)
        {
            if(pbThu.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                return ms.ToArray();
            }
            
            else
            {
                return null;
            }
        }

        // Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any cell is selected
                if (dgvThu.CurrentCell == null)
                {
                    MessageBox.Show("Hãy chọn thông tin cần xóa !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedRowIndex = dgvThu.CurrentCell.RowIndex;
                string maloai = dgvThu.Rows[selectedRowIndex].Cells["MaLoai"].Value.ToString();
                string maThu = dgvThu.Rows[selectedRowIndex].Cells["MaThu"].Value.ToString();

                // Hiển thị hộp thoại xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    XoaDuLieu(maloai, maThu);

                    // Hiển thị thông báo xóa thành công
                    MessageBox.Show("Dữ liệu đã được xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Load lại dữ liệu cho DataGridView
                    dgvThu.DataSource = db.DocBang("SELECT * FROM Thu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaDuLieu(string maloai, string maThu)
        {
            string queryDeleteThuSuKien = $"DELETE FROM Thu WHERE Maloai = '{maloai}' AND MaThu = '{maThu}'";
            db.CapNhat(queryDeleteThuSuKien);
        }

        // Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetValue();
        }

        // Chọn file ảnh
        private void btnChonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbThu.ImageLocation = openFileDialog.FileName;
                //Lấy danh sách đường dẫn đầy đủ của các file hình ảnh được chọn
                    List<string> imagePaths = new List<string>();
                foreach (string imagePath in openFileDialog.FileNames)
                {
                    string absolutePath = Path.GetFullPath(imagePath);
                    imagePaths.Add(absolutePath);
                }

                // Đọc dữ liệu hình ảnh từ các file và thêm vào DataGridView
                foreach (string imagePath in imagePaths)
                {
                    ChenAnh(imagePath);
                }
            }
        }

        private void ChenAnh(string imagePath)
        {
            // Kiểm tra xem hình ảnh có tồn tại không
            if (File.Exists(imagePath))
            {
                // Đọc dữ liệu hình ảnh từ file
                Image image = Image.FromFile(imagePath);

                // Thêm hình ảnh vào ô DataGridViewImageCell
                dgvThu.CurrentRow.Cells[13].Value = image;

                // Hiển thị hình ảnh trong PictureBox
                pbThu.Image = image;
            }
            else
            {
                // Hiển thị hình ảnh mặc định hoặc thông báo lỗi
                MessageBox.Show($"Hình ảnh không tồn tại: {imagePath}", "Lỗi");
            }
        }

        private void dgvThu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaThu.Text = dgvThu.CurrentRow.Cells[0].Value.ToString();
            txtNguonGoc.Text = dgvThu.CurrentRow.Cells[1].Value.ToString();
            txtTenThu.Text = dgvThu.CurrentRow.Cells[2].Value.ToString();
            cbLoai.Text = dgvThu.CurrentRow.Cells[3].Value.ToString();
            txtSoLuong.Text = dgvThu.CurrentRow.Cells[4].Value.ToString();
            cbSachDo.Text = dgvThu.CurrentRow.Cells[5].Value.ToString();
            txtTenKH.Text = dgvThu.CurrentRow.Cells[6].Value.ToString();
            txtTenTA.Text = dgvThu.CurrentRow.Cells[7].Value.ToString();
            txtTenTV.Text = dgvThu.CurrentRow.Cells[8].Value.ToString();
            cbKieuSinh.Text = dgvThu.CurrentRow.Cells[9].Value.ToString();
            dtpNgayvao.Text = dgvThu.CurrentRow.Cells[10].Value.ToString();
            txtDacDiem.Text = dgvThu.CurrentRow.Cells[11].Value.ToString();
            dtpNgaySinh.Text = dgvThu.CurrentRow.Cells[12].Value.ToString();
            txtTuoiTho.Text = dgvThu.CurrentRow.Cells[14].Value.ToString();

            // Kiểm tra giá trị của ô cần hiển thị
            if (dgvThu.CurrentRow.Cells[13].Value != null && dgvThu.CurrentRow.Cells[13].Value != DBNull.Value)
            {
                if (dgvThu.CurrentRow.Cells[13].Value.ToString() != "")
                {
                    MemoryStream ms = new MemoryStream(
                        (byte[])dgvThu.CurrentRow.Cells[13].Value);
                    pbThu.Image = Image.FromStream(ms);
                }
                else
                {
                    // Xử lý nếu giá trị không phải là mảng byte (nếu cột không chứa hình ảnh)
                    pbThu.Image = null;
                }
            }
            else
            {
                // Xử lý nếu giá trị của ô là null hoặc DBNull
                pbThu.Image = null;
            }

        }

        // Tìm kiếm theo mã thú || tên thú || nguồn gốc
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            string tukhoa = txtTimKiem.Text.Trim();

            if(string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập mã hoặc tên thú hoặc nguồn gốc thú cần tìm !", "Thông báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            string sql = "SELECT Thu.MaThu, TenNguonGoc, TenThu, Loai.TenLoai," +
                "SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, " +
                "NgaySinh, Anh, TuoiTho FROM Thu LEFT JOIN Loai ON Thu.MaLoai = Loai.MaLoai " +
                "Where MaThu Like @tukhoa OR TenThu Like @tukhoa OR TenNguonGoc Like @tukhoa " +
                "OR Loai.TenLoai Like @tukhoa OR KieuSinh Like @tukhoa OR SachDo Like @TuKhoa";

            DataTable dt = db.LayDuLieu(sql, new SqlParameter("@tukhoa", "%" + tukhoa + "%"));

            // Kiểm tra kết quả
            if (dt.Rows.Count > 0)
            {
                // Hiển thị kết quả tìm kiếm trong DataGridView hoặc ListBox
                dgvThu.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin ứng với từ khóa !", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbSachDo_DropDown(object sender, EventArgs e)
        {
            cbSachDo.Items.Add("Có trong sách đỏ");
            cbSachDo.Items.Add("Không trong sách đỏ");
        }

        private void cbKieuSinh_DropDown(object sender, EventArgs e)
        {
            cbKieuSinh.Items.Add("Đẻ trứng");
            cbKieuSinh.Items.Add("Đẻ con");
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            LoadData();
        }
    }

}

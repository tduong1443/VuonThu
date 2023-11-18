using System;
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

        // Load dữ liệu lên datagridview
        private void Thu_Load(object sender, EventArgs e)
        {
            dgvThu.DataSource = db.DocBang("SELECT Thu.MaThu, TenNguonGoc, TenThu, Loai.TenLoai," +
                "SoLuong, SachDo, TenKhoaHoc, TenTA, TenTV, KieuSinh, NgayVao, DacDiem, " +
                "NgaySinh, Anh, TuoiTho FROM Thu LEFT JOIN Loai ON Thu.MaLoai = Loai.MaLoai");

            //dgvThu.DataSource = db.DocBang("Select * From Thu");

            dgvThu.Columns[0].HeaderText = "Mã thú";
            dgvThu.Columns[1].HeaderText = "Nguồng gốc";
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
            pic.ImageLayout = DataGridViewImageCellLayout.Zoom;

            foreach (DataGridViewColumn column in dgvThu.Columns)
            {
                column.Width = 200; // Đặt độ rộng của mỗi cột là 200px
            }

            dgvThu.RowTemplate.Height = 100; // Đặt độ cao mặc định cho tất cả các dòng

            LoadDataToComboBox();
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
                = txtDacDiem.Text = txtKieuSinh.Text = cbLoai.Text
                = txtTuoiTho.Text = "";
            pbThu.Image = null;
        }

        // Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        // Sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn sửa không", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                byte[] imageData = ChuanBiDuLieuAnh();

                string sql = "Update Thu " +
                "Set TenNguonGoc = N'" + txtNguonGoc.Text + "', " +
                "TenThu = N'" + txtTenThu.Text + "', " +
                "MaLoai = N'" + cbLoai.SelectedValue.ToString() + "', " +
                "SoLuong = '" + txtSoLuong.Text + "', " +
                "SachDo = N'" + (rdbCo.Checked ? "Có" : "Không") + "OR SachDo IS NULL', " +
                "TenKhoaHoc = N'" + txtTenKH.Text + "', " +
                "TenTA = N'" + txtTenTA.Text + "', " +
                "TenTV = N'" + txtTenTV.Text + "', " +
                "KieuSinh = N'" + txtKieuSinh.Text + "', " +
                "NgayVao = '" + dtpNgayvao.Value.ToString("yyyy-MM-dd") + "', " +
                "DacDiem = N'" + txtDacDiem.Text + "', " +
                "NgaySinh = '" + dtpNgaySinh.Value.ToString("yyyy-MM-dd") + "', " +
                "Anh = @Anh, " +
                "TuoiTho = '" + txtTuoiTho.Text + "' " +
                "Where MaThu = N'" + txtMaThu.Text + "'";

                // Thực hiện cập nhật
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Anh", imageData) };
                db.CapNhatTS(sql, parameters);
            }
        }

        private byte[] ChuanBiDuLieuAnh()
        {
            // Lấy dữ liệu ảnh từ PictureBox và chuyển đổi thành mảng byte
            if (pbThu.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pbThu.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }

            return null; // Trả về null nếu không có ảnh
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

        // Chọn file ảnh
        private void btnChonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn của hình ảnh đầu tiên từ danh sách đường dẫn được chọn
                string firstImagePath = openFileDialog.FileNames.FirstOrDefault();

                // Gọi hàm AddImageToDataGridView với hình ảnh đầu tiên
                AddImage(firstImagePath);
            }
        }

        private void AddImage(string imageName)
        {
            // Xây dựng đường dẫn tương đối từ thư mục chứa ứng dụng
            string imagePath = Path.Combine(Application.StartupPath, "image-pet", imageName);

            // Kiểm tra xem hình ảnh có tồn tại không
            if (File.Exists(imagePath))
            {
                // Đọc dữ liệu hình ảnh từ file
                Image image = Image.FromFile(imagePath);

                pbThu.Image = image;

                dgvThu.CurrentRow.Cells[13].Value = image;
            }
            else
            {
                // Hiển thị hình ảnh mặc định hoặc thông báo lỗi
                MessageBox.Show($"Hình ảnh không tồn tại: {imageName}", "Lỗi");
            }
        }

        private void dgvThu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaThu.Text = dgvThu.CurrentRow.Cells[0].Value.ToString();
            txtNguonGoc.Text = dgvThu.CurrentRow.Cells[1].Value.ToString();
            txtTenThu.Text = dgvThu.CurrentRow.Cells[2].Value.ToString();
            cbLoai.Text = dgvThu.CurrentRow.Cells[3].Value.ToString();
            txtSoLuong.Text = dgvThu.CurrentRow.Cells[4].Value.ToString();
            txtTenKH.Text = dgvThu.CurrentRow.Cells[6].Value.ToString();
            txtTenTA.Text = dgvThu.CurrentRow.Cells[7].Value.ToString();
            txtTenTV.Text = dgvThu.CurrentRow.Cells[8].Value.ToString();
            txtKieuSinh.Text = dgvThu.CurrentRow.Cells[9].Value.ToString();
            dtpNgayvao.Text = dgvThu.CurrentRow.Cells[10].Value.ToString();
            txtDacDiem.Text = dgvThu.CurrentRow.Cells[11].Value.ToString();
            dtpNgaySinh.Text = dgvThu.CurrentRow.Cells[12].Value.ToString();
            txtTuoiTho.Text = dgvThu.CurrentRow.Cells[14].Value.ToString();

            // Kiểm tra giá trị của ô cần hiển thị
            if (dgvThu.CurrentRow.Cells[13].Value != null && dgvThu.CurrentRow.Cells[13].Value != DBNull.Value)
            {
                // Kiểm tra xem giá trị có phải là mảng byte hay không
                if (dgvThu.CurrentRow.Cells[13].Value is byte[] imageData)
                {
                    // Chuyển đổi mảng byte thành hình ảnh và hiển thị trong PictureBox
                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        pbThu.Image = Image.FromStream(memoryStream);
                    }
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

            if (e.RowIndex >= 0)
            {
                // Lấy giá trị từ ô được click
                object cellValue = dgvThu.CurrentRow.Cells[5].Value;

                // Kiểm tra giá trị có tồn tại không
                if (cellValue != null && cellValue != DBNull.Value)
                {
                    // Chuyển giá trị về kiểu dữ liệu mong muốn
                    string giaTriRadioButton = cellValue.ToString();

                    // Dựa vào giá trị lấy được, thực hiện việc set giá trị cho RadioButton
                    if (giaTriRadioButton == "Có")
                    {
                        rdbCo.Checked = true;
                        rdbKhong.Checked = false;
                    }
                    else if (giaTriRadioButton == "Không")
                    {
                        rdbCo.Checked = false;
                        rdbKhong.Checked = true;
                    }
                }
            }

        }
    }

}

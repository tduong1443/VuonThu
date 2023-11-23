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
    public partial class DangNhap : Form
    {
        private List<Image> imagePaths;
        private int currentIndex = 0;
        private Timer timer;
        ProccessDatabase pd = new ProccessDatabase();
        public static string tenNV;
        private Label lblMK = new Label();
        private string placeholderTenDN = "Tên đăng nhập";
        private string placeholderMatKhau = "Mật khẩu";

        public DangNhap()
        {
            InitializeComponent();

            txtTenDN.Text = placeholderTenDN;
            txtTenDN.ForeColor = System.Drawing.Color.Gray;

            txtMatKhau.Text = placeholderMatKhau;
            txtMatKhau.ForeColor = System.Drawing.Color.Gray;

            //AnMatKhau();

            LoadImagePaths(); // Khởi tạo danh sách đường dẫn hình ảnh
            // Khởi tạo Timer
            timer = new Timer();
            timer.Interval = 3000; // Đặt khoảng thời gian giữa các frame (đơn vị là mili giây)
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        // Load hình ảnh
        private void LoadImagePaths()
        {
            imagePaths = new List<Image>
            {
                Properties.Resources.zoo,
                Properties.Resources.zoopet,
                Properties.Resources.pet
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Nạp hình ảnh từ đường dẫn và thay đổi hình ảnh của PictureBox
            Zoopb.Image = imagePaths[currentIndex];

            // Di chuyển đến hình ảnh tiếp theo trong danh sách
            currentIndex = (currentIndex + 1) % imagePaths.Count;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Đăng nhập
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            pd.KetNoi();

            string tenDN = txtTenDN.Text;
            string matKhau = txtMatKhau.Text;

            string query = "SELECT * FROM NhanVien WHERE TenDN = @TenDN AND MatKhau = @MatKhau";

            DataTable dt = pd.LayDuLieu(query, new SqlParameter("@TenDN", tenDN), new SqlParameter("@MatKhau", matKhau));

            // Kiểm tra kết quả truy vấn
            if (dt.Rows.Count > 0)
            {
                tenNV = dt.Rows[0]["TenDN"].ToString();

                // Đăng nhập thành công, chuyển đến Form1 và truyền tên nhân viên
                Form1 fm = new Form1();
                fm.Show();
                this.Hide();
            }
            else
            {
                // Đăng nhập thất bại
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Thông báo");
            }
        }

        // Ẩn mật khẩu khi nhập vào
        private void cbShow_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !cbShow.Checked;
            txtMatKhau.PasswordChar = '\0';
        }

        private void AnMatKhau()
        {
            cbShow.CheckedChanged += cbShow_CheckedChanged;
            txtMatKhau.PasswordChar = '*';
            txtMatKhau.UseSystemPasswordChar = true;
        }

        private void txtTenDN_Enter(object sender, EventArgs e)
        {
            if (txtTenDN.Text == placeholderTenDN)
            {
                txtTenDN.Text = "";
                txtTenDN.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void txtTenDN_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDN.Text))
            {
                txtTenDN.Text = placeholderTenDN;
                txtTenDN.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void txtMatKhau_Enter(object sender, EventArgs e)
        {
            if (txtMatKhau.Text == placeholderMatKhau)
            {
                txtMatKhau.Text = "";
                txtMatKhau.ForeColor = System.Drawing.Color.Black;
                AnMatKhau();
            }
        }

        private void txtMatKhau_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                txtMatKhau.Text = placeholderMatKhau;
                txtMatKhau.ForeColor = System.Drawing.Color.Gray;
                HienMatKhau();
            }
        }

        private void HienMatKhau()
        {
            txtMatKhau.UseSystemPasswordChar = false; // Hiển thị văn bản thường (không ẩn)
            txtMatKhau.PasswordChar = '\0'; // Đặt PasswordChar thành ký tự null để không ẩn văn bản thay thế cho mật khẩu
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
            panel1.Select();
        }
    }
}

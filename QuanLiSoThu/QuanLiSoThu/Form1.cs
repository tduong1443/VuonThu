using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiSoThu
{
    public partial class Form1 : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;

        public Form1()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panel2.Controls.Add(leftBorderBtn);

            // form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            HomeMenu();
        }

        private void HomeMenu()
        {
            panelThuMenu.Visible = false;
            panelChuongMenu.Visible = false;
            panelThucAnMenu.Visible = false;
        }

        private void HideMenu()
        {
            if (panelThuMenu.Visible)
            {
                panelThuMenu.Visible = false;
            }
            if (panelChuongMenu.Visible)
            {
                panelChuongMenu.Visible = false;
            }
            if (panelThucAnMenu.Visible)
            {
                panelThucAnMenu.Visible = false;
            }
        }

        private void ShowMenu(Panel panel)
        {
            if (panel.Visible == false)
            {
                HideMenu();
                panel.Visible = true;
            }
            else panel.Visible = false;
        }

        //Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }
        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
                //iconCurrentChildForm.IconChar = currentBtn.IconChar;
                //iconCurrentChildForm.IconColor = color;
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(35, 41, 70);
                currentBtn.ForeColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.White;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        // Mở các form con
        private Form activeForm = null;

        private void MoFile(Form form)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(form);
            panelChildForm.Tag = form;
            form.BringToFront();
            form.Show();
        }

        // Quản lí thú
        private void btnQLThu_Click(object sender, EventArgs e)
        {
            ShowMenu(panelThuMenu);
            ActivateButton(sender, RGBColors.color2);
        }

        private void btnThu_Click(object sender, EventArgs e)
        {
            MoFile(new Thu());
            HideMenu();
        }

        private void btnLoai_Click(object sender, EventArgs e)
        {
            MoFile(new Loai());
            HideMenu();
        }

        private void btnSuKien_Click(object sender, EventArgs e)
        {
            MoFile(new SuKien());
            HideMenu();
        }

        private void btnThuSK_Click(object sender, EventArgs e)
        {
            MoFile(new ThuSuKien());
            HideMenu();
        }

        // Quản lí chuồng
        private void btnQLChuong_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            ShowMenu(panelChuongMenu);
        }

        private void btnChuong_Click(object sender, EventArgs e)
        {
            MoFile(new Chuong());
            HideMenu();
        }

        private void btnThuChuong_Click(object sender, EventArgs e)
        {

        }

        private void btnKhu_Click(object sender, EventArgs e)
        {
            MoFile(new Khu());
            HideMenu();
        }

        private void btnTrangThai_Click(object sender, EventArgs e)
        {
            MoFile(new TrangThai());
            HideMenu();
        }

        // Quản lí thức ăn
        private void btnQLThucAn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
            ShowMenu(panelThucAnMenu);
        }

        private void btnThucAn_Click(object sender, EventArgs e)
        {
            MoFile(new ThucAn());
            HideMenu();
        }

        private void btnThuTA_Click(object sender, EventArgs e)
        {
            MoFile(new ThuThucAn());
            HideMenu();
        }

        // Quản lí nhân viên
        private void btnQLNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            MoFile(new NhanVien());
            HideMenu();
        }

        // Đăng xuất
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            if (MessageBox.Show("Bạn có muốn đăng xuất không", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DangNhap lg = new DangNhap();
                lg.Show();
                this.Hide();
            }
        }

        // Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // Exit
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Form Maximize
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        // Form Minimize
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblTenNV.Text = DangNhap.tenNV;
        }
    }
}

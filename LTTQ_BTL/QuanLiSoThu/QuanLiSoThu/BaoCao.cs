using Microsoft.Reporting.WinForms;
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
    public partial class BaoCao : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public BaoCao()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void BaoCao_Load(object sender, EventArgs e)
        {
            string sqlSLThu = "Select Count(*) From Thu";
            int slthu = (int)db.LayGtri(sqlSLThu);
            lblSLThu.Text = slthu.ToString();

            string sqlSLChuong = "Select Count(*) From Chuong";
            int slChuong = (int)db.LayGtri(sqlSLChuong);
            lblSLChuong.Text = slChuong.ToString();

            string sqlSLNhanVien = "Select Count(*) From NhanVien";
            int slNhanVien = (int)db.LayGtri(sqlSLNhanVien);
            lblSLNhanVien.Text = slNhanVien.ToString();

            string sqlSLTA = "Select Count(*) From ThucAn";
            int slThucAn = (int)db.LayGtri(sqlSLTA);
            lblSLTA.Text = slThucAn.ToString();
        }

        private void btnChiTietDS_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }
    }
}

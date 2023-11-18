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
    public partial class ThuSuKien : Form
    {
        ProccessDatabase db = new ProccessDatabase();
        public ThuSuKien()
        {
            InitializeComponent();
        }

        private void ThuSuKien_Load(object sender, EventArgs e)
        {
            dgvThuSK.DataSource = db.DocBang("Select * From Thu_SuKien");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockChainCSharp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void bThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông Báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
               e.Cancel = true;
        }

        private void bDangNhap_Click(object sender, EventArgs e)
        {
            FormMain f = new FormMain();
            this.Hide();
            f.ShowDialog(); //showdialog là thằng này phải sử lý song và tắt đi thì mới chạy câu lệnh dưới để show.
            this.Show();
        }
    }
}

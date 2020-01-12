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
    public partial class FormSignUp : Form
    {
        public FormSignUp()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void bDanhNhap_Click(object sender, EventArgs e)
        {
            FormLogin flg = new FormLogin();
            
            this.Hide();
            flg.Show();
           
        }

        private void bDangKy_Click(object sender, EventArgs e)
        {
            FormLogin flg = new FormLogin();
            this.Hide();
            flg.Show();
        }

        private void bThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

using BlockChainCSharp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockChainCSharp
{
    //public
        partial class FormLogin : Form
    {
        BlockChain blockChain;
        public FormLogin(BlockChain bc)
        {
            InitializeComponent();
            this.blockChain = bc;
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
            string filePath = @"./login.bat";

            string[] lines;
            
            string[] nhap = new string[2];
            nhap[0] = tbDangNhap.Text;
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(tbPassword.Text);
            byte[] outputBytesHash = sha256.ComputeHash(inputBytes);
            nhap[1] = Convert.ToBase64String(outputBytesHash);
            if (System.IO.File.Exists(filePath))
            {
                lines = System.IO.File.ReadAllLines(filePath);
                for (int i = 0; i < nhap.Length; i++)
                {
                    //Console.WriteLine("Line {0}: {1} ; length:{2}", i, lines[i],lines.Length);
                    if (lines[i] != nhap[i])
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác", "Thông báo");
                        return;
                    }

                }            
                FormMain formMain = new FormMain(blockChain);
                this.Hide();
                formMain.ShowDialog(); //showdialog là thằng này phải sử lý song và tắt đi thì mới chạy câu lệnh dưới để show.
                this.tbDangNhap.Text = "";
                this.tbPassword.Text = "";
                this.Show();

            }
            else
            {           
                MessageBox.Show("file ko tồn tại.");
            }

          
        }

        private void bDangky_Click(object sender, EventArgs e)
        {
            FormSignUp fSignUP = new FormSignUp(blockChain);
            this.Hide();
            fSignUP.ShowDialog();
            this.Show();

        }
    }
}

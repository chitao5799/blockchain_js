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
      partial   class FormSignUp : Form
    {
        BlockChain blockChain;
        public FormSignUp( BlockChain bc)
        {
            InitializeComponent();
            this.blockChain = bc;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
   
        private void bDangKy_Click(object sender, EventArgs e)
        {
            string fileLPath = @"./login.bat";
            string[] lines = new string[2];
            lines[0] = tbTenDN.Text;
            lines[1] = tbMatKhau.Text;
            //lines[2] = tbXacNhanMK.Text;
            if (tbTenDN.Text == "")
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Thông báo");
                return;
            }
            if (tbMatKhau.Text == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Thông báo");
                return;
            }
            if (lines[1] == tbXacNhanMK.Text)
            {
                List<UserBlock> userData;
                userData = blockChain.UserChain;
                if(blockChain.isValidUserData() == false)
                {
                    MessageBox.Show("dữ liệu hệ thống đã bị thay đổi, vui lòng chờ đội ngũ kỹ thuật khắc phục !!", "Thông báo");
                    return;
                }
                foreach(UserBlock userTemp in userData)
                {
                    if(userTemp.User.Username ==tbTenDN.Text)
                    {
                        MessageBox.Show("User đã tồn tại, mời bạn đăng ký tên khác!", "Thông báo");
                        return;
                    }
                }
                User user = new User(tbTenDN.Text,tbMatKhau.Text,1000,false);
                blockChain.createUser(user);

                MessageBox.Show("Đăng ký thành công.");
                SHA256 sha256 = SHA256.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(lines[1]);              
                byte[] outputBytesHash = sha256.ComputeHash(inputBytes);
                lines[1] = Convert.ToBase64String(outputBytesHash);
                System.IO.File.WriteAllLines(fileLPath, lines);             
                this.Close();
            }
            else
            {
                MessageBox.Show("Mật khẩu nhập không trùng ô xác nhận mật khẩu!","Thông báo");
      
            }
           
        }

        private void bThoat_Click(object sender, EventArgs e)
        {         
            this.Close();
        }
    }
}

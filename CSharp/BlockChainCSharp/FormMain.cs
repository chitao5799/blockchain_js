using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlockChainCSharp.Model;

namespace BlockChainCSharp
{
    //public
        partial class FormMain : Form
    {
        BlockChain blockChain;
        public FormMain(BlockChain bc)
        {
            InitializeComponent();
            this.blockChain = bc;
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abcd(object sender, PaintEventArgs e)
        {
            //FormLogin form2 = new FormLogin();
           
        
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInfo fnf = new FormInfo();
            fnf.ShowDialog();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //lvPendingTrans.View = View.Details;
            //lvPendingTrans.Columns.Add("Ngày");
            //lvPendingTrans.Columns.Add("Từ");
            //lvPendingTrans.Columns.Add("Tới");
            //lvPendingTrans.Columns.Add("Số Tiền");
            ListViewItem item1 = new ListViewItem();
            item1.Text = "item 1";
            item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "subitem1" });
            item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "subitem2" });

            ListViewItem item2 = new ListViewItem();
            item2.Text = "item 2";

            lvPendingTrans.Items.Add(item1);
            lvPendingTrans.Items.Add(item2);
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            List<UserBlock> userData;
            userData = blockChain.UserChain;           
            foreach(UserBlock blockUser in userData)//tìm các user là tổ chức từ thiện
            {
                if (blockUser.User.Is_charity == true)
                    cbCharity.Items.Add(blockUser.User.Username);
                       
            }
            
            
        }
    }
}

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
        String userNameLogined;
        public FormMain( BlockChain bc,String ulogined)
        {
            InitializeComponent();
            this.blockChain = bc;
            this.userNameLogined = ulogined;
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

            //ListViewItem item1 = new ListViewItem();
            //item1.Text = "item 1";
            //item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "subitem1" });
            //item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "subitem2" });

            //ListViewItem item2 = new ListViewItem();
            //item2.Text = "item 2";

            //lvPendingTrans.Items.Add(item1);
            //lvPendingTrans.Items.Add(item2);
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            List<UserBlock> userData;
            userData = blockChain.UserChain;
            cbCharity.Items.Clear();
            foreach(UserBlock blockUser in userData)//tìm các user là tổ chức từ thiện
            {
                if (blockUser.User.Is_charity == true)
                    cbCharity.Items.Add(blockUser.User.Username);
                       
            }
            
            
        }

        private void bGui_Click(object sender, EventArgs e)
        {
            UserBlock blockUserLogined=blockChain.UserChain[0] ;
            foreach (UserBlock blockUser in blockChain.UserChain)
            {
                if (blockUser.User.Username == userNameLogined)
                {
                    blockUserLogined = blockUser;
                    //Console.WriteLine("có tìm đc user đã login");
                    break;
                }
            }
            int blockSend = blockChain.UserChain.LastIndexOf(blockUserLogined);
            User userSend = blockChain.UserChain[blockSend].User;
            String fromAdress = userSend.Pub_key;


            UserBlock blockUserCharity = blockChain.UserChain[0];
            foreach (UserBlock blockUser in blockChain.UserChain)
            {
                if (blockUser.User.Username == cbCharity.Text)
                {
                    blockUserCharity = blockUser;
                    //Console.WriteLine("có tìm đc user charity");
                    break;
                }
            }
            int blockReceive = blockChain.UserChain.LastIndexOf(blockUserCharity);
            User userReceive = blockChain.UserChain[blockReceive].User;
            String toAdress = userReceive.Pub_key;

            int amount = Convert.ToInt32(tbTien.Text);
            //Console.WriteLine("amount nhập trong textbox:{0}", amount);
            Transaction tranc = new Transaction(fromAdress, toAdress, amount);
            tranc.signTransaction(userSend.Private_key);
            //tranc.signTransaction(userSend.Pub_key);
            blockChain.addTransaction(tranc);
            blockChain.MinePendingTransactions(fromAdress);

            userSend.Money += blockChain.getBalanceOfAdress(fromAdress);
            userReceive.Money += blockChain.getBalanceOfAdress(toAdress);
            Console.WriteLine("from adress hay public key của usersend:{0}", fromAdress);
            Console.WriteLine("private key của user send:{0}",userSend.Private_key);
            Console.WriteLine("public key của user charity:{0}",userReceive.Pub_key);
            //Console.WriteLine("to adress:{0}", toAdress);
            //Console.WriteLine("user send money:{0}", userSend.Money);
            //Console.WriteLine("user receive money:{0}", userReceive.Money);
            //Console.WriteLine("user send name:{0}", userSend.Username);

            //blockChain.createUser(userSend);
            //blockChain.createUser(userReceive);

            lvCharity.Items.Clear();
            Console.WriteLine("so luong chain:{0}", blockChain.Chain.Count);
            foreach(Block block in blockChain.Chain)
            {
                Console.WriteLine("VAO FOR NGOAI.");
                Console.WriteLine("so luong trans trong chain:{0}", block.transactions.Count);
                foreach(Transaction transaction in block.transactions)
                {
                    Console.WriteLine("VAO FOR TRONG.");
                    ListViewItem item = new ListViewItem();
                    //if(transaction.fromAdress==fromAdress || transaction.toAdress == fromAdress)//liệt kê các transaction liên quan tới user đã login
                    //{
                        item.Text =block.timestamp;
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text =transaction.fromAdress });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = transaction.toAdress });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text =Convert.ToString( transaction.amount) });
                        lvCharity.Items.Add(item);
                            //Console.WriteLine(" transaction form adress:{0}", transaction.fromAdress);
                            //Console.WriteLine(" transaction to adress:{0}", transaction.toAdress);
                            //Console.WriteLine(" transaction mount:{0}", transaction.amount);
                    //}
                   
                }
            }

        }

    }
}

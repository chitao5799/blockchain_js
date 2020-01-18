using BlockChainCSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockChainCSharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BlockChain blockChain = new BlockChain();
            Application.Run( new FormLogin(ref blockChain));

            //BlockChain blockChain = new BlockChain();
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //string publicPrivateKeyXML = rsa.ToXmlString(true);
            //string publicOnlyKeyXML = rsa.ToXmlString(false);
            //Transaction ts = new Transaction(publicOnlyKeyXML, "tuan mon", 100);
            //ts.signTransaction(publicOnlyKeyXML);
            //Console.WriteLine("mining ....");

            //blockChain.MinePendingTransactions(publicOnlyKeyXML);

        }
    }
   
}

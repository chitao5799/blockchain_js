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
            Application.Run(new FormLogin(blockChain));

            //BlockChain blockChain = new BlockChain();
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //string publicPrivateKeyXML = rsa.ToXmlString(true);
            //string publicOnlyKeyXML = rsa.ToXmlString(false);
            //Transaction ts = new Transaction(publicOnlyKeyXML, "tuan mon", 100);
            //ts.signTransaction(publicOnlyKeyXML);
            ////blockChain.addTransaction(ts);
            //Console.WriteLine("mining ....");

            //blockChain.MinePendingTransactions(publicOnlyKeyXML);

            /*
            //hash-verify-sign trong c#
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicPrivateKeyXML = rsa.ToXmlString(true);

            // Console.WriteLine("private key là:{0}", publicPrivateKeyXML);
            string publicOnlyKeyXML = rsa.ToXmlString(false);
            // Console.WriteLine("public key là:{0}", publicOnlyKeyXML);

            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes("hello world");
            //ASCIIEncoding myAscii = new ASCIIEncoding();
            //byte[] inputBytes = myAscii.GetBytes("hello world");
            Console.WriteLine("input byte là:{0}", inputBytes);
            // byte[] outputBytesHash = sha256.ComputeHash(inputBytes);
            SHA1Managed hash = new SHA1Managed();
            byte[] outputBytesHash = hash.ComputeHash(inputBytes);
            Console.WriteLine("output byte là:{0}", outputBytesHash);
            String hashed = Convert.ToBase64String(outputBytesHash);
            Console.WriteLine("hashed là:{0}", hashed);
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            // byte[] signedd = rsaCSP.SignHash(outputBytesHash, CryptoConfig.MapNameToOID("SHA1"));
            byte[] signedd = rsaCSP.SignHash(outputBytesHash, CryptoConfig.MapNameToOID(publicPrivateKeyXML));
            Console.WriteLine("đã sign:{0}", signedd);
            Console.WriteLine("verify là:{0}", rsaCSP.VerifyHash(outputBytesHash, CryptoConfig.MapNameToOID(publicOnlyKeyXML), signedd));
            Console.WriteLine("hash lại :{0}", Convert.ToBase64String(hash.ComputeHash(inputBytes)));*/


        }
    }
   
}

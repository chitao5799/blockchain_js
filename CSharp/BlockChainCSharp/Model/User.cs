using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainCSharp.Model
{
    class User
    {
        String username, password;
        double money;
        bool is_charity;
        String private_key, pub_key;
        Byte[] signature;

        public string Username { get => username; set => username = value; }
        public bool Is_charity { get => is_charity; set => is_charity = value; }

        public User(String username, String password, double money, bool is_charity)
        {
            this.Username = username;
            this.password = password;
            this.money = money;
            this.Is_charity = is_charity;
            this.createKeyPair();
        }
        public void createKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicPrivateKeyXML = rsa.ToXmlString(true);
            string publicOnlyKeyXML = rsa.ToXmlString(false);
            this.private_key = publicPrivateKeyXML;
            this.pub_key = publicOnlyKeyXML;
        }
        public byte[] CalculateBytesHash()
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.Username + this.password + this.money);
            SHA1Managed hash = new SHA1Managed();
            byte[] outputBytesHash = hash.ComputeHash(inputBytes);
            return outputBytesHash;
        }
        public String HashToString()
        {
            return Convert.ToBase64String(this.CalculateBytesHash());
        }
        public void signRegister(String publicKey)
        {
         
            byte[] hashed = this.CalculateBytesHash();
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            this.signature = rsaCSP.SignHash(hashed, CryptoConfig.MapNameToOID(publicKey));
        }
        public bool isValid()
        {

	        if(this.signature==null || this.signature.Length == 0) {
                throw new System.Exception("No signature in this block");

            }

            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            return rsaCSP.VerifyHash(this.CalculateBytesHash(), CryptoConfig.MapNameToOID(this.pub_key), this.signature);
        }

    }
}

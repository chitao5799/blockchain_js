using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BlockChainCSharp.Model
{
    class Transaction
    {
        public String fromAdress, toAdress;
        public int amount;
        Byte[] signature;

        public Transaction(String fromAdress,String toAdress,int amount)
        {
            this.fromAdress = fromAdress;
            this.toAdress = toAdress;
            this.amount = amount;
        }
        public byte[] CalculateBytesHash()
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.fromAdress+this.toAdress+Convert.ToString(this.amount));
            SHA1Managed hash = new SHA1Managed();
            byte[] outputBytesHash = hash.ComputeHash(inputBytes);
            return outputBytesHash;
        }
        public String HashToString()
        {
            return Convert.ToBase64String(this.CalculateBytesHash());
        }
        //public void signTransaction(String publicKey)
        //{
        //    if (publicKey != this.fromAdress)
        //    {
        //        throw new System.ArgumentException("you cannot sign transaction for other wallet");
        //    }
        //    byte[] hashed = this.CalculateBytesHash();
        //    RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
        //    this.signature = rsaCSP.SignHash(hashed, CryptoConfig.MapNameToOID(publicKey));

        //}
        public void signTransaction(String privateKey)
        {
            //if (publicKey != this.fromAdress)
            //{
            //    throw new System.ArgumentException("you cannot sign transaction for other wallet");
            //}
            byte[] hashed = this.CalculateBytesHash();
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            this.signature = rsaCSP.SignHash(hashed, CryptoConfig.MapNameToOID(privateKey));
            Console.WriteLine("private key đc truyen vao ham sign transaction:{0}",privateKey);
        }
        public bool isValid()
        {
            //if (this.fromAdress!=null)
            //{
            //    return true;
            //}

            if (this.signature==null || this.signature.Length == 0)
            {
                throw new System.Exception("No signature in this transaction");
            }
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            Console.WriteLine("public key hay fromaddress truyền vào hàm isvalid transaction la:{0}", this.fromAdress);
            bool isVerify=rsaCSP.VerifyHash(this.CalculateBytesHash(), CryptoConfig.MapNameToOID(this.fromAdress), this.signature);
            Console.WriteLine("kết quả verify là:{0}", isVerify);
            //return isVerify;
            return true;
        }
    }
}

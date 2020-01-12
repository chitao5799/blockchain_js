using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainCSharp.Model
{
    class UserBlock
    {
        User user;
        public String previousHash, hash;
        int nonce;
        
        public UserBlock(User user,String previousHash)
        {
            this.user = user;
            this.previousHash = previousHash;
            this.hash = this.calculateHash();
            this.nonce = 0;
        }
        public String calculateHash() {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.previousHash + this.user.ToString() + this.nonce);
            SHA1Managed hash = new SHA1Managed();
            byte[] outputBytesHash = hash.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytesHash);
        }
        public void mineBlock(int difficulty)
        {

            string leadingZeros = new string('0', difficulty);
            while (this.hash == null || this.hash.Substring(0, difficulty) != leadingZeros)
            {
                this.nonce++;

                this.hash = this.calculateHash();

             }

        }
        public bool hasValid ()
        {
            if (!this.user.isValid())
            {
                return false;      
            }
            return true;
        }

    }
}

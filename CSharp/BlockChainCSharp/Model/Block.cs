using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainCSharp.Model
{
    class Block
    {
        public String timestamp, previousHash;
        public String hash;
        public List<Transaction> transactions;
        int nonce = 0;
        public Block(String timestamp, List<Transaction> transactions,String previousHash)
        {
            this.timestamp = timestamp;
            this.transactions = transactions;
            this.previousHash = previousHash;
            this.hash = this.calculateHash();
            this.nonce = 0;
        }
        public String calculateHash()
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.previousHash + this.timestamp+this.transactions.ToString()+this.nonce);
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

            Console.WriteLine("Block is mined:{0} ", this.hash);
        }
        public bool hasValidTransactions()
        {

                foreach(Transaction tns in this.transactions)
                {
                    if (tns.isValid())
                        return false;
	            }

             return true;
        }
    }
}

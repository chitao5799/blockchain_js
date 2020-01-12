using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainCSharp.Model
{
    class BlockChain
    {
        int difficulty;
        int miningReward;
        List<Transaction> pendingTransactions;
        List<Block> chain;
        List<UserBlock> userChain;

        public BlockChain()
        {
            this.chain = new List<Block>();
            this.difficulty = 2;
            this.pendingTransactions = new List<Transaction>();
            this.miningReward = 100;
            this.userChain = new List<UserBlock>();

            this.chain.Add(this.createGenesisBlock());
            this.userChain.Add(createGenesisUser());
        }
        public Block createGenesisBlock()
        {
            List<Transaction> list = new List<Transaction>();
            list.Add(new Transaction("0", "Genesis Block", 0));
            return new Block("11/23/2019",list, "0");
        }
        public UserBlock createGenesisUser()
        {

            User charity = new User("charity", "1", 0, true);
            return new UserBlock(charity, "0");
        }
        public Block GetLatestBlock()
        {
            return chain[chain.Count - 1];
        }
        public UserBlock getLastUser()
        {
            return this.userChain[this.userChain.Count - 1];
        }
        public void MinePendingTransactions(String miningRewardAdress)
        {

            DateTime date = DateTime.Now;
            string curentTime = Convert.ToString(date.Day) + '/' + Convert.ToString(date.Month) + '/' + Convert.ToString(date.Year);

            Block block = new Block(curentTime, this.pendingTransactions,GetLatestBlock().hash);

            block.mineBlock(this.difficulty);
            this.chain.Add(block);

            this.pendingTransactions.Clear();
            this.pendingTransactions.Add(new Transaction(null, miningRewardAdress, this.miningReward));
        }
        public void addTransaction(Transaction transaction) 
        {
	        // cai nay de kiem tra khi tao 1 transaction , khac voi trans dc tao ra khi mine block
	        if(transaction.fromAdress==null || transaction.toAdress==null) 
            {
                throw new System.Exception("Transaction must include from and to address");
            }
	        if(!transaction.isValid()) 
            {
                throw new System.Exception("Cannot add invalid transaction");
            }
            this.pendingTransactions.Add(transaction);
        }
        public int getBalanceOfAdress(string public_key_address)
        {

            int balance = 0;

            foreach (Block block in this.chain)
            {
		        foreach(Transaction tran in block.transactions)
                {
                    if (tran.fromAdress == public_key_address) {
                        balance -= tran.amount;
                    }

                    if (tran.toAdress == public_key_address) {
                        balance += tran.amount;
                    }
                }
            }
            return balance;
        }
        public bool isChainValid() {
	        for(int i = 1; i< this.chain.Count; i++) {
                Block curentBlock = this.chain[i];

                Block previousBlock = this.chain[i - 1];

		        if(!curentBlock.hasValidTransactions()) {
                    return false;
		        }

		        if(curentBlock.hash !=curentBlock.calculateHash()) {
                    return false;
		        }
		        if(curentBlock.previousHash != previousBlock.hash) {
                    return false;
		        }
	        }
            return true;
        }
        public void createUser(User user)
        {

            UserBlock userBlock = new UserBlock(user,this.getLastUser().hash);
            userBlock.mineBlock(this.difficulty);
            this.userChain.Add(userBlock);
        }
        public bool isValidUserData() 
        {
	        for(int i = 1; i< this.userChain.Count; i++) 
            {
                UserBlock curentBlock = this.userChain[i];

                UserBlock previousBlock = this.userChain[i - 1];

		        if(!curentBlock.hasValid()) {
                    return false;
		        }

		        if(curentBlock.hash != curentBlock.calculateHash()) {
                    return false;
		        }
		        if(curentBlock.previousHash != previousBlock.hash) {
                    return false;
		        }
	        }
            return true;
        }

    }
}

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

        internal List<UserBlock> UserChain { get => userChain; set => userChain = value; }
        internal List<Block> Chain { get => chain; set => chain = value; }
        internal List<Transaction> PendingTransactions { get => pendingTransactions; set => pendingTransactions = value; }

        public BlockChain()
        {
            this.Chain = new List<Block>();
            this.difficulty = 2;
            this.PendingTransactions = new List<Transaction>();
            this.miningReward = 100;
            this.UserChain = new List<UserBlock>();

            this.Chain.Add(this.createGenesisBlock());
            this.UserChain.Add(createGenesisUser());
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
            return Chain[Chain.Count - 1];
        }
        public UserBlock getLastUser()
        {
            return this.UserChain[this.UserChain.Count - 1];
        }
        public void MinePendingTransactions(String miningRewardAdress)
        {

            DateTime date = DateTime.Now;
            string curentTime = Convert.ToString(date.Day) + '/' + Convert.ToString(date.Month) + '/' + Convert.ToString(date.Year);
            Console.WriteLine("so luong trans trong pending de cho vao block de mine:{0}", this.pendingTransactions.Count);
            Block block = new Block(curentTime, this.PendingTransactions,GetLatestBlock().hash);

            block.mineBlock(this.difficulty);
            Console.WriteLine("-----------------========transactions in block sau mine truoc add chain ---------------");
            foreach (Transaction i in block.transactions)
            {
                Console.WriteLine(" from adress:{0}", i.fromAdress);
                Console.WriteLine(" to adress:{0}", i.toAdress);
                Console.WriteLine(" mount:{0}", i.amount);
                Console.WriteLine(" ------------------------------");//ở đây vẫn hiện đúng  các transaction
            }
            Console.WriteLine("-----------------===========end   ----transactionsin block sau mine truoc add chain ---------------");
            this.Chain.Add(block);

            this.PendingTransactions.Clear();
            this.PendingTransactions.Add(new Transaction(null, miningRewardAdress, this.miningReward));
            Console.WriteLine("===========transactions in chain  after mine===================");
            foreach(Block b in this.Chain)
            {
               
                 foreach (Transaction i in b.transactions)
                    {
                        Console.WriteLine(" from adress:{0}", i.fromAdress);
                        Console.WriteLine(" to adress:{0}", i.toAdress);
                        Console.WriteLine(" mount:{0}", i.amount);
                        Console.WriteLine(" ------------------------------");
                    }
                Console.WriteLine(" -------------==============-----------------");//ở đây hiện sai các transaction
            }
           
            Console.WriteLine("=====================end   ----transactions in chain =============-");
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
            this.PendingTransactions.Add(transaction);
            Console.WriteLine("-----------------transactions in pending ---------------");
            foreach(Transaction i in this.pendingTransactions)
            {
                Console.WriteLine(" from adress:{0}", i.fromAdress);
                Console.WriteLine(" to adress:{0}", i.toAdress);
                Console.WriteLine(" mount:{0}", i.amount);
                Console.WriteLine(" ------------------------------");
            }
            Console.WriteLine("-----------------end   ----transactions in pending ---------------");
        }
        public int getBalanceOfAdress(string public_key_address)
        {

            int balance = 0;

            foreach (Block block in this.Chain)
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
	        for(int i = 1; i< this.Chain.Count; i++) {
                Block curentBlock = this.Chain[i];

                Block previousBlock = this.Chain[i - 1];

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
            this.UserChain.Add(userBlock);
        }
        public bool isValidUserData() 
        {
	        for(int i = 1; i< this.UserChain.Count; i++) 
            {
                UserBlock curentBlock = this.UserChain[i];

                UserBlock previousBlock = this.UserChain[i - 1];

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

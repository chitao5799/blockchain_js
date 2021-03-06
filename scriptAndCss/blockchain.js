const Block = require('../block.js');
const Transaction = require('../transaction.js');
const UserBlock = require('../user.block.js');
const User = require('../user.js');


class BlockChain {
    constructor() {
        this.chain = [this.createGenesisBlock()];
        this.difficulty = 2;
        this.pendingTransactions = [];
        this.miningReward = 100;
        this.userChain = [this.createGenesisUser()];
    }
    createGenesisBlock() {
        let block = new Block("11/23/2019", ["Genesis Block"], "0");
        return block;

    }
    createGenesisUser() {
        let charity = new User("charity", "1", 0, 1);
        let userBlock = new UserBlock(charity, "0");
        userBlock.mineBlock(2);

        return userBlock;
    }
    getLastBlock() {
        return this.chain[this.chain.length - 1];
    }
    getLastUser() {
        return this.userChain[this.userChain.length - 1]; // .slice(-1).pop();
    }
    minePendingTransactions(miningRewardAdress) {
        let date = new Date();
        let curentTime = date.getDate() + '/' + date.getMonth() + '/' + date.getFullYear() + '';
        let block = new Block(curentTime, this.pendingTransactions);
        block.previousHash = this.getLastBlock().hash;
        block.mineBlock(this.difficulty);
        this.chain.push(block);

        this.pendingTransactions = [new Transaction(null, miningRewardAdress, this.miningReward)];
    }
    addTransaction(transaction) {
        // cai nay de kiem tra khi tao 1 transaction , khac voi trans dc tao ra khi mine block
        if (!transaction.fromAdress || !transaction.toAdress) {
            throw new Error('Transaction must include from and to address');
        }

        if (!transaction.isValid()) {
            throw new Error('Cannot add invalid transaction');
        }
        this.pendingTransactions.push(transaction);
    }
    getBalanceOfAdress(address) {
        let balance = 0;

        for (let block of this.chain) {
            for (let tran of block.transactions) {
                if (tran.fromAdress === address) {
                    balance -= tran.amount;
                }

                if (tran.toAdress === address) {
                    balance += tran.amount;
                }
            }
        }

        return balance;
    }
    isChainValid() {
        for (let i = 1; i < this.chain.length; i++) {
            const curentBlock = this.chain[i];
            const previousBlock = this.chain[i - 1];

            if (!curentBlock.hasValidTransactions()) {
                return false;
            }

            if (curentBlock.hash !== curentBlock.calculateHash()) {
                return false;
            }
            if (curentBlock.previousHash !== previousBlock.hash) {
                return false;
            }
        }
        return true;
    }
    createUser(user) {
        let userBlock = new UserBlock(user);
        userBlock.previousHash = this.getLastUser().hash;
        userBlock.mineBlock(2);
        this.userChain.push(userBlock);
    }
    isValidUserData() {
        if (this.userChain.length > 2) {
            for (let i = 1; i < this.userChain.length; i++) {
                const curentBlock = this.userChain[i];
                const previousBlock = this.userChain[i - 1];

                if (!curentBlock.hasValid()) {
                    return false;
                }
                if (curentBlock.hash !== curentBlock.calculateHash()) {
                    return false;
                }
                if (curentBlock.previousHash !== previousBlock.hash) {
                    return false;
                }
            }
        }

        return true;
    }
}

global.BlockChain = BlockChain;
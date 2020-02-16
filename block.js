const SHA256 = require('crypto-js/sha256')
const Transaction = require('./transaction')

class Block {
    constructor(timestamp, transactions, previousHash) {
        this.timestamp = timestamp;
        this.transactions = transactions;
        this.previousHash = previousHash;
        this.hash = this.calculateHash();
        this.nonce = 0;
    }

    calculateHash() {
        return SHA256(this.previousHash + this.timestamp + JSON.stringify(this.transactions) + this.nonce).toString();
    }
    mineBlock(difficulty) {

        while (this.hash.substring(0, difficulty) !== Array(difficulty + 1).join("0")) {
            this.nonce++;
            this.hash = this.calculateHash();
        }

        console.log("Block is mined: ", this.hash);
    }
    hasValidTransactions() {
        //console.log('có vào valid tran of block');

        for (let tx of this.transactions) {
            var tr = Object.assign(new Transaction(), tx);
            if (!tr.isValid()) {
                return false;
            }
        }

        return true;
    }
}
module.exports = Block
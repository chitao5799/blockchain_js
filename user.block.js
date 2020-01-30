const SHA256 = require('crypto-js/sha256')

class UserBlock {
    constructor(user, previousHash) {
        this.user = user;
        this.previousHash = previousHash;
        this.hash = this.calculateHash();
        this.nonce = 0;
    }
    calculateHash() {

        return SHA256(this.previousHash + JSON.stringify(this.user) + this.nonce).toString();
    }
    mineBlock(difficulty) {

        while (this.hash.substring(0, difficulty) !== Array(difficulty + 1).join("0")) {
            this.nonce++;
            this.hash = this.calculateHash();
        }

    }
    hasValid() {

        if (!this.user.isValid()) {
            return false
        }

        return true
    }
}
module.exports = UserBlock
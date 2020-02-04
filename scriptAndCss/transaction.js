const SHA256 = require('crypto-js/sha256');
const EC = require('elliptic').ec;
const ec = new EC('secp256k1');

class Trans {
    constructor(fromAdress, toAdress, amount) {
        this.fromAdress = fromAdress;
        this.toAdress = toAdress;
        this.amount = amount;
    }
    calculateHash() {
        var encoded = SHA256(this.fromAdress + this.toAdress + this.amount).toString();
        return encoded;
    }
    signTransaction(signingKey) {
        if (signingKey.getPublic('hex') !== this.fromAdress) {
            throw new Error('you cannot sign transaction for other wallet');
        }
        const hashTx = this.calculateHash();
        const sig = signingKey.sign(hashTx, 'base64');
        this.signature = sig.toDER('hex');
    }
    isValid() {
        if (!this.fromAdress) {
            return false;
        }

        if (!this.signature || this.signature.length === 0) {
            throw new Error('No signature in this transaction');
        }

        const publicKey = ec.keyFromPublic(this.fromAdress, 'hex');
        return publicKey.verify(this.calculateHash(), this.signature);
    }

}
global.Transaction = Trans;
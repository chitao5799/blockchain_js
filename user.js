const SHA256 = require('crypto-js/sha256')
var prettyjson = require('prettyjson')
const EC = require('elliptic').ec
const ec = new EC('secp256k1')

class User {
    constructor(username, password, money, is_charity) {
        this.username = username;
        this.password = password;
        this.money = money;
        this.is_charity = is_charity;
        this.createKeyPair();
    }
    calculateHash() {
        return SHA256(this.username + this.password + this.money).toString()
    }
    createKeyPair() {
        const key = ec.genKeyPair(); //trả về KeyPair
        const privateKey = key.getPrivate('hex'); //trả về chuỗi string
        const userKey = ec.keyFromPrivate(privateKey); //trả về KeyPair
        this.private_key = privateKey;
        this.pub_key = userKey.getPublic('hex'); //trả về chuỗi tring dài gấp đôi chuỗi trên
    }
    signRegister(signingKey) {

        const hashTx = this.calculateHash();
        const sig = signingKey.sign(hashTx, 'base64'); //KeyPair.sign(hex-string)
        this.signature = sig.toDER('hex');
    }
    isValid() {

        if (!this.signature || this.signature.length === 0) {
            throw new Error('No signature in this block');
        }

        const publicKey = ec.keyFromPublic(this.pub_key, 'hex');
        return publicKey.verify(this.calculateHash(), this.signature);
    }

}
module.exports = User
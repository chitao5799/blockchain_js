const SHA256 = require('crypto-js/sha256')
var prettyjson = require('prettyjson')
const EC = require('elliptic').ec
const ec = new EC('secp256k1')

class User {
    constructor(username, password, money, is_charity) {
        this.username = username;
        this.password = SHA256(password).toString();
        this.money = money;
        this.is_charity = is_charity;
        this.hashed = this.calculateHash(); //them vao.
        this.createKeyPair();

    }
    calculateHash() {
        return SHA256(this.username + this.password + this.is_charity).toString() //+ this.money
    }
    createKeyPair() {
        const key = ec.genKeyPair(); //trả về KeyPair
        const privateKey = key.getPrivate('hex'); //trả về chuỗi string
        const userKey = ec.keyFromPrivate(privateKey); //trả về KeyPair
        this.private_key = privateKey;
        this.pub_key = userKey.getPublic('hex'); //trả về chuỗi tring dài gấp đôi chuỗi trên

        this.signRegister(userKey);
        // console.log('verify khi vua sign la:', this.isValid()); //true
    }
    signRegister(signingKey) {

        const hashTx = this.hashed;
        const sig = signingKey.sign(hashTx, 'base64'); //KeyPair.sign(hex-string)
        this.signature = sig.toDER('hex');
    }
    isValid() {

        if (!this.signature || this.signature.length === 0) {
            throw new Error('No signature in this block');
        }
        // var hashed = this.hashed;
        // console.log("hash la:", hashed);
        const publicKey = ec.keyFromPublic(this.pub_key, 'hex');
        // console.log("keypair la:", publicKey);
        // console.log("signature la:", this.signature);
        var Verified = publicKey.verify(this.hashed, this.signature);
        // console.log("kết quả verify:", Verified);
        return Verified;
    }

}
module.exports = User
const SHA256 = require('crypto-js/sha256')
const EC = require('elliptic').ec
const ec = new EC('secp256k1')

function Transaction(fromAdress, toAdress, amount) {
	this.fromAdress = fromAdress
	this.toAdress = toAdress
	this.amount = amount
}

Transaction.prototype.calculateHash = function() {
	var encoded=SHA256(this.fromAdress + this.toAdress + this.amount).toString();
	// console.log('sha256 transaction:',encoded);//chuỗi string
	return encoded;
}
Transaction.prototype.signTransaction = function(signingKey) {
	if(signingKey.getPublic('hex') !== this.fromAdress) {
		throw new Error('you cannot sign transaction for other wallet')
	}
	//signingKey.getPublic('hex') và this.fromAdress đều được tạo ra từ 1 kiểu - là 1 chuỗi string
			// console.log('signingKey.getPublic(hex)=',signingKey.getPublic('hex'));
			// console.log('this.fromAdress=',this.fromAdress);
	const hashTx = this.calculateHash()
	// console.log('hashTx=',hashTx);//chuỗi string sha256
	const sig = signingKey.sign(hashTx, 'base64')
	//			<KeyPair{ }>.sign(< an array or a hex-string>)
	// console.log('sig=',sig); // là Signature { }
	// console.log('sig.toDER(hex)=',sig.toDER('hex')); //một chuỗi string dài
	this.signature = sig.toDER('hex')
}

Transaction.prototype.isValid = function() {
	if(!this.fromAdress) {
		return true
	}

	if(!this.signature || this.signature.length === 0) {
		throw new Error('No signature in this transaction')
	}

	const publicKey = ec.keyFromPublic(this.fromAdress, 'hex') //trả về KeyPair{ }
			// console.log(' ec.keyFromPublic(this.fromAdress, hex)=', ec.keyFromPublic(this.fromAdress, 'hex'));
			// console.log('publicKey.verify(this.calculateHash(), this.signature)=',publicKey.verify(this.calculateHash(), this.signature));//true
	return publicKey.verify(this.calculateHash(), this.signature)//true
	// <KeyPair{ }>.verify(<chuỗi hash>,<chuỗi-đã-sign.toDER()>)
}

module.exports=Transaction;
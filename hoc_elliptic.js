/*
var EC = require('elliptic').ec;
var ec = new EC('secp256k1');

// Generate a key pair
var keyPair = ec.genKeyPair();
var privateKey = keyPair.getPrivate('hex');
var publicKey = keyPair.getPublic('hex');

// Calculate public key from private key
ec.keyFromPrivate(somePrivateKey, 'hex').getPublic('hex');
*/
const SHA256 = require('crypto-js/sha256')
var prettyjson = require('prettyjson')
const EC = require('elliptic').ec
const ec = new EC('secp256k1')

const key = ec.genKeyPair()//trả về KeyPair {}, gọi tạm là yyy
const privateKey = key.getPrivate('hex')//trả về chuỗi string
const publicKey=key.getPublic('hex'); //trả về chuỗi string là xxx, dài gấp đối privatekey
const userKey = ec.keyFromPrivate(privateKey)//trả về KeyPair,cũng là yyy
var pub_key = userKey.getPublic('hex')//trả về chuỗi tring, cũng là xxx
// const key_From_Public=ec.keyFromPublic(publicKey);//lỗi Object.<anonymous>

var hashString=SHA256('hello world').toString();
var signature=userKey.sign(hashString, 'base64');

// console.log('hash:',SHA256('hello world'));//trả về; { }
// console.log('hast to string:',SHA256('hello world').toString());//trả về chuỗi hash gồm các ký tự hệ 16
// console.log('sign:',signature); //trả về: Signature{ }
// console.log('sign to der:',signature.toDER());// trả về mảng: [ <các số nguyên>]

//varify: <KeyPair>.verify(<hash-string>,<signature(signed with hash-string And KeyPair)>);
console.log(key.verify(hashString,signature)); //true
console.log(key.verify(hashString,signature.toDER()));//true
console.log(userKey.verify(hashString,signature));//true
console.log(userKey.verify(hashString,signature.toDER()));//true
console.log(ec.genKeyPair().verify(hashString,signature));//false
//------
// console.log('key là:',key);
// console.log('key form private:',userKey);
// console.log('key là:',key);
// console.log('userkey là:',userKey);
// console.log('private_key là:',private_key);

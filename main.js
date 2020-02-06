var express = require('express');
var app = express();
var cookieParser = require('cookie-parser')
const SHA256 = require('crypto-js/sha256')
var http = require('http').createServer(app);
var io = require('socket.io')(http);

const Blockchain = require('./blockchain')
const Block = require('./block')
const Transaction = require('./transaction')
const User = require('./user')


var bodyParser = require('body-parser')

const EC = require('elliptic').ec
const ec = new EC('secp256k1')
const key = ec.genKeyPair()


const low = require('lowdb')
const FileSync = require('lowdb/adapters/FileSync')

const adapter = new FileSync('db.json')
const db = low(adapter)
db.defaults({ blockChain: [] }).write()
const adapter2 = new FileSync('dbBlock.json')
const dbBlock = low(adapter2)
dbBlock.defaults({ UserChain: [] }).write()

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))
app.use(cookieParser('MY_SECCRET'))
app.use(express.static(__dirname + '/scriptAndCss'));
app.use(express.static(__dirname + '/node_modules'));
app.use(express.static(__dirname));
app.set('view engine', 'ejs')
app.set('views', './views')

app.get('/thu', function(req, res) {
    res.render('thu.ejs');
});
// defination block chain
let blockChain = new Blockchain();
var Transactions = []; //lưu các trans chưa ký.
var pendingTransactionsTemporary = [];
var login_thanh_cong = false;
/*
const fs = require('fs')
try {
    if (fs.existsSync('./db.json')) {
        //file exists
        blockChain.chain = db.get('blockChain').value();
    }
} catch (err) {
    console.error(err)
}
try {
    if (fs.existsSync('./dbBlock.json')) {
        blockChain.userChain = dbBlock.get('UserChain').value();
    }
} catch (err) {
    console.error(err)
}*/


app.get('/', function(req, res) {
    res.render('login.ejs');
});

app.get('/login', function(req, res) {
    res.render('login');
});

app.post('/login', function(req, res) {

    let errs = []
    if (!req.body.username) {
        errs.push('tài khoản là bắt buộc')
    }
    if (!req.body.password) {
        errs.push('mật khẩu là bắt buộc')
    }

    let userLogin = blockChain.userChain.find(block => {
        return block.user.username === req.body.username
    })
    if (!userLogin || userLogin.user.password !== SHA256(req.body.password).toString()) {
        errs.push('sai tài khoản hoặc mật khẩu')
    }

    if (errs.length) {
        res.render('login', {
            title: 'Login',
            errs: errs
        });
        return;
    }

    res.cookie('username', userLogin.user.username, {
        signed: true
    })
    login_thanh_cong = true;
    res.render('index', {
        blockChain: blockChain.chain,
        user: userLogin.user,
        tienDaTuThien: 0
    })
    return
});
app.get('/index', function(req, res) {
    let blockFind = blockChain.userChain.find(block => {
        return block.user.username === req.signedCookies.username
    })
    let blockUser = blockChain.userChain.lastIndexOf(blockFind)

    let userLogin = blockChain.userChain[blockUser];
    userLogin.user.money = blockChain.getBalanceOfAdress(userLogin.user.pub_key) + 1000;
    let tienDaTuThien = 0;
    blockChain.chain.forEach(block => {
        block.transactions.forEach(transaction => {
            if (transaction.fromAdress === userLogin.user.pub_key) {
                tienDaTuThien += transaction.amount;
            }
        });

    });

    res.render('index', {
        blockChain: blockChain.chain,
        user: userLogin.user,
        tienDaTuThien: tienDaTuThien
    });
});
app.get('/register', function(req, res) {
    res.render('register');
});
app.get('/AllTransactions', function(req, res) {
    res.render('allTransaction.ejs');
});
app.post('/register', function(req, res) {
    let userData = blockChain.userChain
    let errs = []

    if (!blockChain.isValidUserData()) {
        errs.push('dữ liệu hệ thống đã bị thay đổi, vui lòng chờ đội ngũ kỹ thuật khắc phục !!')
    }

    if (!req.body.username) {
        errs.push('tài khoản là bắt buộc')
    }

    if (!req.body.password) {
        errs.push('mật khẩu là bắt buộc')
    }

    let userRegister = new User(req.body.username, req.body.password, 1000, 0)
    userData.forEach((userblock) => {
        if (userblock.user.username === userRegister.username) {
            errs.push('đã tồn tại tài khoản')
        }
    });


    if (errs.length) {
        res.render('register', {
            title: 'Register',
            errs: errs
        });
        return;

    }
    blockChain.createUser(userRegister);

    res.redirect('/login');

});

app.get('/charity', function(req, res) {
    var charitys = blockChain.userChain.filter(block => {
        if (block.user.is_charity == 1) {
            return block
        }
    })

    charitys = charitys.map(block => block.user)

    res.render('charity', {
        charitys: charitys
    })
});
app.get('/pendingTrans', function(req, res) {
    res.render('pendingTrans');
});


app.post('/charity', function(req, res) {

    let blockFind = blockChain.userChain.find(block => {
        return block.user.username === req.signedCookies.username
    })
    let blockSend = blockChain.userChain.lastIndexOf(blockFind)

    let userSend = blockChain.userChain[blockSend].user
    let fromAdress = userSend.pub_key //là user hiện tại trên trình duyệt đang ở trang /charity
    let toAdress = req.body.charity_pub_key //là user được chọn trong thẻ select
    let amount = parseInt(req.body.amount)

    let transaction = new Transaction(fromAdress, toAdress, amount)
    transaction.signTransaction(ec.keyFromPrivate(userSend.private_key))

    blockChain.addTransaction(transaction)


    let blockReceive = blockChain.userChain.find(block => {
        return block.user.pub_key === toAdress
    })

    let userReceive = blockChain.userChain[blockChain.userChain.lastIndexOf(blockReceive)].user

    userSend.money = blockChain.getBalanceOfAdress(fromAdress) + 1000;
    userReceive.money = parseInt(blockChain.getBalanceOfAdress(toAdress));

    // blockChain.createUser(userSend)
    // blockChain.createUser(userReceive)

    let tienDaTuThien = 0;
    blockChain.chain.forEach(block => {
        block.transactions.forEach(transaction => {
            if (transaction.fromAdress === userSend.pub_key) {
                tienDaTuThien += transaction.amount;
            }
        });

    });
    res.render('index', {
        blockChain: blockChain.chain,
        user: userSend,
        tienDaTuThien: tienDaTuThien

    })
});

var blockMinedByClient = false;
var numerical_order_userSendBlock = 0;
io.on('connection', function(socket) {
    if (login_thanh_cong == true) {
        login_thanh_cong = false;
        // console.log('login thành công server gửi chain.');
        socket.emit('send_data_when_login', { blockchain: blockChain.chain });
    }
    socket.on("getData", function() {
        socket.emit("sendData", { blockchain: blockChain.chain });
    });

    socket.on("userSendBlockJustMine", function(data) {
        numerical_order_userSendBlock++;
        blockMinedByClient = true;
        if (numerical_order_userSendBlock === 1) {
            // socket.broadcast.emit('server_send_block_just_mine',data.blockMined);
            io.sockets.emit('server_send_block_just_mine', data.blockMined);
            // console.log('server nhận block just mine:', data.blockMined);
            pendingTransactionsTemporary = [];
            blockChain.chain.push(data.blockMined);
            // console.log('ghi vào db.json');
            // console.log('block just mine:', data.blockMined);

            /**--------------Lỗi Khó Hiểu.
             * block được mined đầu tiên là 1 object vừa đào và ko hề sai khi thử hiện ra màn hình, nhưng ko hiểu vì sao 
             * khi ghi vào file db.json thì ghi cả các block được đào từ những lần chạy thử chương trình lúc trước.
             * từ block thứ 2 trở đi được mine thì lại ghi vào file bình thường.
             * 
             */
            db.get('blockChain').push(data.blockMined).write();

            let publicKey;
            for (var i = 0; i < blockChain.userChain.length; i++) {
                if (blockChain.userChain[i].user.username === data.userName) {
                    publicKey = blockChain.userChain[i].user.pub_key;
                    // console.log('publickey server tim dc của user mine:', publicKey); //
                    break;
                }

            }
            blockChain.pendingTransactions.push(new Transaction(null, publicKey, blockChain.miningReward));
        }

    });

    var interval_obj = setInterval(function() //cứ 10 phút server sẽ gửi pending trans cho các client
        {
            /*blockMinedByClient == fasle là trong 10 phút vừa rồi ko có user nào mine block nên các pending transaction
            trong 10 phút trước đó sẽ nối thêm vào mảng pendTrans trong 10 phút vừa mới đây.*/
            if (blockMinedByClient == false) {
                for (let i = 0; i < pendingTransactionsTemporary.length; i++)
                    blockChain.pendingTransactions.push(pendingTransactionsTemporary[i]);
            }

            pendingTransactionsTemporary = blockChain.pendingTransactions;
            blockChain.pendingTransactions = [];
            io.sockets.emit("serverSendPendTrans", pendingTransactionsTemporary);
            blockMinedByClient = false;
            numerical_order_userSendBlock = 0;
            // clearInterval(interval_obj);//
        }, 60000); //600000

    socket.on('userName_mine', function(data) {
        let publicKey;
        for (var i = 0; i < blockChain.userChain.length; i++) {
            if (blockChain.userChain[i].user.username === data) {
                publicKey = blockChain.userChain[i].user.pub_key;
                break;
            }

        }
        // console.log("public key la:", publicKey);
        socket.emit('userPublicKey', {
            pubKey: publicKey,
            latestBlock: blockChain.chain[blockChain.chain.length - 1]
        });
    });
});

http.listen(3000, function() {
    console.log('listening on *:3000');
});
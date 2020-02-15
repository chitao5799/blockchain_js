const low = require('lowdb');
const FileSync = require('lowdb/adapters/FileSync');

const adapter3 = new FileSync('./dbBlock.json');
const db3 = low(adapter3);
db3.defaults({ UserChain: [] }).write();

module.exports.auth = function(req, res, next) {
    //  console.log('cookie la:', req.signedCookies.username);
    if (!req.signedCookies.username || req.signedCookies.username == '') {
        //  console.log('vao if 1');
        res.redirect('login');
        return;
    }
    //  console.log('state:', db3.getState().UserChain);
    var blockFind = db3.read().__wrapped__.UserChain.find(block => {
        //   console.log('usernaem  la:', block.user.username);
        return block.user.username == req.signedCookies.username
    });
    //console.log('read:', db3.read().__wrapped__.UserChain);
    //console.log(db.get('UserChain').value());
    // console.log('la:', blockFind);
    if (!blockFind) {
        //console.log('vao if 2');
        res.redirect('login');
        return;
    }

    next();
}
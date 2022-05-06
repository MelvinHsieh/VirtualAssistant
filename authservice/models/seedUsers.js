var mongoose = require('mongoose');
var User = mongoose.model('User');
var bcrypt = require('bcryptjs')
var q = require('q');

function saveCallback(err) {
    if (err) {
        console.log('Fill testdata failed, reason: %s', err)
    }
}

function createUsers() {
    User.find({}).then(async function (data) {
        if (data.length == 0) {
            console.log('Creating targets testdata');

            new User({ username: "user1", password: 'user!1', roles: ['user'] }).save(saveCallback);
            new User({ username: "user2", password: 'user!2', roles: ['user'] }).save(saveCallback);
            new User({ username: "admin", password: 'admin!1', roles: ['admin'] }).save(saveCallback);
        } else {
            console.log('Skipping create user testdata, already present');
        }
        return;
    });
}

module.exports = function () {
    q.fcall(createUsers);
};
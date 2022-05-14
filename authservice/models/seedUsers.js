var mongoose = require('mongoose');
var User = mongoose.model('User');
var bcrypt = require('bcryptjs')
var q = require('q');
const ROLES = require('./roles')

function saveCallback(err) {
    if (err) {
        console.log('Fill testdata failed, reason: %s', err)
    }
}

function createUsers() {
    User.find({}).then(async function (data) {
        if (data.length == 0) {
            console.log('Creating targets testdata');

            new User({ username: "patient1", password: 'patient!1', roles: [ROLES.Patient] }).save(saveCallback);
            new User({ username: "patient2", password: 'patient!2', roles: [ROLES.Patient] }).save(saveCallback);
            new User({ username: "employee1", password: 'employee!1', roles: [ROLES.Employee] }).save(saveCallback);
            new User({ username: "admin1", password: 'admin!1', roles: [ROLES.Admin] }).save(saveCallback);
        } else {
            console.log('Skipping create user testdata, already present');
        }
        return;
    });
}

module.exports = function () {
    q.fcall(createUsers);
};
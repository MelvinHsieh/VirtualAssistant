var mongoose = require('mongoose');
var Interaction = mongoose.model('Interaction');
var q = require('q');

function saveCallback(err) {
    if (err) {
        console.log('Fill interaction testdata failed, reason: %s', err)
    }
}

function createInteractions() {
    Interaction.find({}).then(function (data) {
        if (data.length == 0) {
            console.log('Creating Interaction testdata');

            new Interaction({
                _id: "f2ee9d28-27d4-494c-848a-a3604458af56",
                from: "User",
                message: "Welke medicijnen moet ik nemen?",
                replies: [{
                    _id: "f2e66d28-27d4-494c-848a-a360445ddf56",
                    message: "Ik open het medicijnrooster!"
                }]
            }).save(saveCallback);

            console.log("Created seed data");
        } else {
            console.log('Skipping create interaction testdata, already present');
        }
        return;
    });
}

module.exports = function () {
    q.fcall(createInteractions);
};
var mongoose = require('mongoose');

var interactionSchema = new mongoose.Schema({
    // Schema including validation
    _id: { type: String, required: true, lowercase: true },
    from: { type: String, required: true, lowercase: false },
    message: { type: String, required: true, lowercase: false },
    replies: [{
        _id: { type: String, required: true, lowercase: true },
        message: { type: String, required: true }
    }]
},
    // settings:
    {
        toObject: { virtuals: true },
        toJSON: { virtuals: true }
    }
);

module.exports = mongoose.model('Interaction', interactionSchema)
var mongoose = require('mongoose');

var errorLogSchema = new mongoose.Schema({
    // Schema including validation
    _id: { type: String, required: true, lowercase: true },
    date: { type: Date, default: Date.now },
    service: { type: String, required: true, lowercase: false },
    error: { type: String, required: true, lowercase: false },
},
    // settings:
    {
        toObject: { virtuals: true },
        toJSON: { virtuals: true }
    }
);

module.exports = mongoose.model('ErrorLog', errorLogSchema)
var mongoose = require('mongoose');
const bcrypt = require('bcryptjs');
const ROLES = require('./roles')

const userSchema = mongoose.Schema({
    username: {
        type: String,
        required: true,
        unique: true
    },
    password: {
        type: String,
        required: true,
    },
    employeeId: {
        type: Number,
        required: false,
    },
    patientId: {
        type: Number,
        required: false
    },
    role: {type: String, enum: ROLES.roleList}
});

userSchema.pre(
    'save',
    async function (next) {
        const hash = await bcrypt.hash(this.password, 10);

        this.password = hash;
        next();
    }
)

userSchema.methods.isValidPassword = async function (password) {
    const user = this;
    const compare = await bcrypt.compare(password, user.password);

    return compare;
}


var User = mongoose.model('User', userSchema);

module.exports = User;


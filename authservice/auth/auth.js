const passport = require("passport");
const localStrategy = require('passport-local').Strategy;
const UserModel = require("../models/user");
const ROLES = require('../models/roles')

passport.use(
    'signup',
    new localStrategy(
        {
            usernameField: 'username',
            passwordField: 'password',
            passReqToCallback: true
        },
        async (req, username, password, done) => {
            try {
                if (req.body.role != null) {
                    var id = req.body.id; 
                    var user;
                    if (id) {
                        switch (req.body.role) {
                            case ROLES.Employee:
                                user = await UserModel.create({ username, password, employeeId: id, role: ROLES.Employee });
                                break;
                            case ROLES.Patient:
                                user = await UserModel.create({ username, password, patientId: id, role: ROLES.Patient });
                                break;
                            default:
                                break;
                            }
                    }
                }
                

                return done(null, user);
            } catch (error) {
                done(error);
            }
        }
    )
)

passport.use(
    'login',
    new localStrategy({
        usernameField: 'username',
        passwordField: 'password'
    },
        async (username, password, done) => {
            
            try {
                const user = await UserModel.findOne({ username });
                console.log(user)
                if (!user) {
                    return done(null, false, { message: 'User not found' });
                }

                const validate = await user.isValidPassword(password);

                if (!validate) {
                    return done(null, false, { message: 'Wrong password' });
                }

                return done(null, user, { message: 'Logged in succesfully' });
            } catch (error) {
                return done(error);
            }
        }
    )
)
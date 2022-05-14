const passport = require("passport");
const localStrategy = require('passport-local').Strategy;
const UserModel = require("../models/user");

passport.use(
    'signup',
    new localStrategy(
        {
            usernameField: 'username',
            passwordField: 'password'
        },
        async (username, password, done) => {
            try {
                const user = await UserModel.create({ username, password, roles: ['user'] });

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
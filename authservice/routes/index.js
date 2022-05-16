var express = require('express');
const passport = require('passport');
const jwt = require('jsonwebtoken');
var router = express.Router();

router.post('/signup', passport.authenticate('signup', {session: false}), async (req, res, next) => {
    res.json({
      message: 'Signup succesful',
      user: req.user
    });
  }
);

router.post('/login', async (req, res, next) => {
    const { body: user } = req;
  
    if(!user.username) {
      return res.status(422).json({
        errors: {
          username: "is required"
        }
      })
    }

    if(!user.password) {
      return res.status(422).json({
        errors: {
          password: "is required"
        }
      })
    }

    passport.authenticate('login', {session: false}, async (err, user, info) => {
      try {
        if(err) {
          return res.status(500).json('an error occured');
        }

        if(!user) {
          return res.status(500).json('The given credentials are incorrect');
        }
          
        req.logIn( user, {session: false}, async (error) => {
            if(error) return next(error);

          const body = {_id: user._id, username: user.username };
          const token = jwt.sign({ user: body, auth_roles: user.roles }, process.env.AUTH_SECRET,
            {
              //TODO EXTRACT
              audience: "VA_AuthAudience",
              issuer: "VA_AuthIssuer"
            })

            return res.json(token);
          }
        )
      } catch (error) {
        return res.status(500).json('An error occured');
      }
    })(req, res, next);
  }
)

module.exports = router;

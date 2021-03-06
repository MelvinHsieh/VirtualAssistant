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

          
          var body = { _id: user._id, username: user.username, role: user.role };

          if (user.employeeId) {
            body.employeeId = user.employeeId
          } 
          if (user.patientId) {
            body.patientId = user.patientId
          }

          const token = jwt.sign({ user: body, auth_role: user.role }, process.env.JWT_SECRET_TOKEN,
            {
              audience: process.env.JWT_AUDIENCE,
              issuer: process.env.JWT_ISSUER
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

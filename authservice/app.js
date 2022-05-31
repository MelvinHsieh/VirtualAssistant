var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
require('dotenv').config();

// Data Access Layer
const mongoose = require('mongoose');
mongoose.connect(`mongodb://${process.env.AUTH_DB_USER_ID}:${process.env.AUTH_DB_USER_PASSWORD}@${process.env.AUTH_DB_HOST_NAME}:${process.env.AUTH_DB_PORT}/`, { useNewUrlParser: true });

require('./auth/auth')

//Require models
require('./models/user');
require('./models/seedUsers')();

var indexRouter = require('./routes/index');

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500).json(err.message);
});

module.exports = app;
require('dotenv').config();

var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

const mongoose = require('mongoose');
mongoose.Promise = require('q').Promise;
mongoose.connect(process.env.MONGO_URL, { useNewUrlParser: true });

require('./models/interaction');
require('./models/interactionSeeder')();

let storeInteraction = require("./consumers/storeInteraction")

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res, next) {
  // Respond with JSON error object
  res.status(err.status || 500);
  res.json(err.message);
});

async function startConsumers() {

    storeInteraction()
    .then(() => {
      console.log('Consumers enabled in loggingservice.');
    })
    .catch((error) => {
      console.log(`Error: ${error}`)
    });
}

startConsumers();

module.exports = app;

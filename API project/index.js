const http = require("http"),
    logger = require("morgan"),
    express = require("express"),
    bodyParser = require("body-parser"),
    mongoose = require("mongoose"),
    dotenv = require("dotenv");

let app = express();

let port = 8080;
dotenv.config();

app.use(bodyParser.json());
app.use(require('./routes'));
app.use(logger("tiny"));

mongoose.connection.on('error', (err) => {
    console.log('Mongodb error ', err, '.');
    process.exit();
});
mongoose.connection.on('connected', () => {
    console.log('MongoDB successfully connected.');
});

app.listen(port, function(err) {
    console.log("Listening on port: " + port);
});

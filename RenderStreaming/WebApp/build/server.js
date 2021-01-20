"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var express = require("express");
var bodyParser = require("body-parser");
var path = require("path");
var fs = require("fs");
var signaling_1 = require("./signaling");
var log_1 = require("./log");
exports.createServer = function (config) {
    var app = express();
    // const signal = require('./signaling');
    app.use(bodyParser.urlencoded({ extended: true }));
    app.use(bodyParser.json());
    app.get('/protocol', function (req, res) { return res.json({ useWebSocket: config.websocket }); });
    app.use('/signaling', signaling_1.default);
    app.use(express.static(path.join(__dirname, '/../public/stylesheets')));
    app.use(express.static(path.join(__dirname, '/../public/scripts')));
    app.use('/images', express.static(path.join(__dirname, '/../public/images')));
    app.get('/', function (req, res) {
        var indexPagePath = path.join(__dirname, '/../index.html');
        fs.access(indexPagePath, function (err) {
            if (err) {
                log_1.log(log_1.LogLevel.warn, "Can't find file ' " + indexPagePath);
                res.status(404).send("Can't find file " + indexPagePath);
            }
            else {
                res.sendFile(indexPagePath);
            }
        });
    });
    return app;
};

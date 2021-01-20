"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var https = require("https");
var fs = require("fs");
var os = require("os");
var server_1 = require("./server");
var websocket_1 = require("./websocket");
var RenderStreaming = /** @class */ (function () {
    function RenderStreaming(options) {
        var _this = this;
        this.options = options;
        this.app = server_1.createServer(this.options);
        if (this.options.secure) {
            this.server = https.createServer({
                key: fs.readFileSync(options.keyfile),
                cert: fs.readFileSync(options.certfile),
            }, this.app).listen(this.options.port, function () {
                var port = _this.server.address().port;
                var addresses = _this.getIPAddress();
                for (var _i = 0, addresses_1 = addresses; _i < addresses_1.length; _i++) {
                    var address = addresses_1[_i];
                    console.log("https://" + address + ":" + port);
                }
            });
        }
        else {
            this.server = this.app.listen(this.options.port, function () {
                var port = _this.server.address().port;
                var addresses = _this.getIPAddress();
                for (var _i = 0, addresses_2 = addresses; _i < addresses_2.length; _i++) {
                    var address = addresses_2[_i];
                    console.log("http://" + address + ":" + port);
                }
            });
        }
        if (this.options.websocket) {
            console.log("start websocket signaling server ws://" + this.getIPAddress()[0]);
            //Start Websocket Signaling server
            new websocket_1.default(this.server);
        }
    }
    RenderStreaming.run = function (argv) {
        var program = require('commander');
        var readOptions = function () {
            if (Array.isArray(argv)) {
                program
                    .usage('[options] <apps...>')
                    .option('-p, --port <n>', 'Port to start the server on', process.env.PORT || 80)
                    .option('-s, --secure', 'Enable HTTPS (you need server.key and server.cert)', process.env.SECURE || false)
                    .option('-k, --keyfile <path>', 'https key file (default server.key)', process.env.KEYFILE || 'server.key')
                    .option('-c, --certfile <path>', 'https cert file (default server.cert)', process.env.CERTFILE || 'server.cert')
                    .option('-w, --websocket', 'Enable Websocket Signaling', process.env.WEBSOCKET || false)
                    .parse(argv);
                return {
                    port: program.port,
                    secure: program.secure,
                    keyfile: program.keyfile,
                    certfile: program.certfile,
                    websocket: program.websocket,
                };
            }
        };
        var options = readOptions();
        return new RenderStreaming(options);
    };
    RenderStreaming.prototype.getIPAddress = function () {
        var interfaces = os.networkInterfaces();
        var addresses = [];
        for (var k in interfaces) {
            for (var k2 in interfaces[k]) {
                var address = interfaces[k][k2];
                if (address.family === 'IPv4') {
                    addresses.push(address.address);
                }
            }
        }
        return addresses;
    };
    return RenderStreaming;
}());
exports.RenderStreaming = RenderStreaming;
RenderStreaming.run(process.argv);

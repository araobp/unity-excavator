"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var websocket = require("ws");
var uuid_1 = require("uuid");
var offer_1 = require("./class/offer");
var answer_1 = require("./class/answer");
var candidate_1 = require("./class/candidate");
// [{sessonId:[connectionId,...]}]
var clients = new Map();
// [{connectionId:[sessionId1, sessionId2]}]
var connectionPair = new Map();
// [{connectionId:Offer}]
var offers = new Map();
// [{connectionId:Answer}]
var answers = new Map();
// [{sessionId:[{connectionId:Candidate},...]}]
var candidates = new Map();
function getOrCreateConnectionIds(settion) {
    var connectionIds = null;
    if (!clients.has(settion)) {
        connectionIds = new Set();
        clients.set(settion, connectionIds);
    }
    connectionIds = clients.get(settion);
    return connectionIds;
}
var WSSignaling = /** @class */ (function () {
    function WSSignaling(server) {
        var _this = this;
        this.server = server;
        this.wss = new websocket.Server({ server: server });
        this.wss.on('connection', function (ws) {
            clients.set(ws, new Set());
            ws.onclose = function (_event) {
                clients.delete(ws);
            };
            ws.onmessage = function (event) {
                // JSON Schema expectation
                // type: connect, disconnect, offer, answer, candidate
                // from: from connection id
                // to: to connection id
                // data: any message data structure
                var msg = JSON.parse(event.data);
                if (!msg || !_this) {
                    return;
                }
                console.log(msg);
                switch (msg.type) {
                    case "connect":
                        _this.onConnect(ws);
                        break;
                    case "disconnect":
                        _this.onDisconnect(ws, msg.data);
                        break;
                    case "offer":
                        _this.onOffer(ws, msg.data);
                        break;
                    case "answer":
                        _this.onAnswer(ws, msg.data);
                        break;
                    case "candidate":
                        _this.onCandidate(ws, msg.data);
                        break;
                    default:
                        break;
                }
            };
        });
    }
    WSSignaling.prototype.onConnect = function (ws) {
        var connectionId = uuid_1.v4();
        var connectionIds = getOrCreateConnectionIds(ws);
        connectionIds.add(connectionId);
        ws.send(JSON.stringify({ type: "connect", connectionId: connectionId }));
    };
    WSSignaling.prototype.onDisconnect = function (ws, message) {
        var connectionIds = clients.get(ws);
        var connectionId = message.connectionId;
        connectionIds.delete(connectionId);
        connectionPair.delete(connectionId);
    };
    WSSignaling.prototype.onOffer = function (ws, message) {
        var connectionId = message.connectionId;
        var newOffer = new offer_1.default(message.sdp, Date.now());
        offers.set(connectionId, newOffer);
        connectionPair.set(connectionId, [ws, null]);
        clients.forEach(function (_v, k) {
            if (k == ws) {
                return;
            }
            k.send(JSON.stringify({ from: connectionId, to: "", type: "offer", data: newOffer }));
        });
    };
    WSSignaling.prototype.onAnswer = function (ws, message) {
        var connectionId = message.connectionId;
        var connectionIds = getOrCreateConnectionIds(ws);
        connectionIds.add(connectionId);
        var newAnswer = new answer_1.default(message.sdp, Date.now());
        answers.set(connectionId, newAnswer);
        var pair = connectionPair.get(connectionId);
        var otherSessionWs = pair[0];
        connectionPair.set(connectionId, [otherSessionWs, ws]);
        var mapCandidates = candidates.get(otherSessionWs);
        if (mapCandidates) {
            var arrayCandidates = mapCandidates.get(connectionId);
            for (var _i = 0, arrayCandidates_1 = arrayCandidates; _i < arrayCandidates_1.length; _i++) {
                var candidate = arrayCandidates_1[_i];
                candidate.datetime = Date.now();
            }
        }
        clients.forEach(function (_v, k) {
            if (k == ws) {
                return;
            }
            k.send(JSON.stringify({ from: connectionId, to: "", type: "answer", data: newAnswer }));
        });
    };
    WSSignaling.prototype.onCandidate = function (ws, message) {
        var connectionId = message.connectionId;
        if (!candidates.has(ws)) {
            candidates.set(ws, new Map());
        }
        var map = candidates.get(ws);
        if (!map.has(connectionId)) {
            map.set(connectionId, []);
        }
        var arr = map.get(connectionId);
        var candidate = new candidate_1.default(message.candidate, message.sdpMLineIndex, message.sdpMid, Date.now());
        arr.push(candidate);
        clients.forEach(function (_v, k) {
            if (k === ws) {
                return;
            }
            k.send(JSON.stringify({ from: connectionId, to: "", type: "candidate", data: candidate }));
        });
    };
    return WSSignaling;
}());
exports.default = WSSignaling;

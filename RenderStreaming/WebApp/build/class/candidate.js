"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Candidate = /** @class */ (function () {
    function Candidate(candidate, sdpMLineIndex, sdpMid, datetime) {
        this.candidate = candidate;
        this.sdpMLineIndex = sdpMLineIndex;
        this.sdpMid = sdpMid;
        this.datetime = datetime;
    }
    return Candidate;
}());
exports.default = Candidate;

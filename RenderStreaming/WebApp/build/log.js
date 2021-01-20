"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var isDebug = true;
var LogLevel;
(function (LogLevel) {
    LogLevel[LogLevel["info"] = 0] = "info";
    LogLevel[LogLevel["log"] = 1] = "log";
    LogLevel[LogLevel["warn"] = 2] = "warn";
    LogLevel[LogLevel["error"] = 3] = "error";
})(LogLevel = exports.LogLevel || (exports.LogLevel = {}));
function log(level) {
    var args = [];
    for (var _i = 1; _i < arguments.length; _i++) {
        args[_i - 1] = arguments[_i];
    }
    if (isDebug) {
        switch (level) {
            case LogLevel.log:
                console.log.apply(console, args);
                break;
            case LogLevel.info:
                console.info.apply(console, args);
                break;
            case LogLevel.warn:
                console.warn.apply(console, args);
                break;
            case LogLevel.error:
                console.error.apply(console, args);
                break;
        }
    }
}
exports.log = log;

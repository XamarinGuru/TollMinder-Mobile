cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/org.apache.cordova.geolocation/www/Coordinates.js",
        "id": "org.apache.cordova.geolocation.Coordinates",
        "clobbers": [
            "Coordinates"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.geolocation/www/PositionError.js",
        "id": "org.apache.cordova.geolocation.PositionError",
        "clobbers": [
            "PositionError"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.geolocation/www/Position.js",
        "id": "org.apache.cordova.geolocation.Position",
        "clobbers": [
            "Position"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.geolocation/www/geolocation.js",
        "id": "org.apache.cordova.geolocation.geolocation",
        "clobbers": [
            "navigator.geolocation"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesis.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesis",
        "merges": [
            "cordova.plugins.SpeechSynthesis"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisUtterance.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisUtterance",
        "merges": [
            "SpeechSynthesisUtterance"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisEvent.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisEvent",
        "merges": [
            "SpeechSynthesisEvent"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisVoice.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisVoice",
        "merges": [
            "SpeechSynthesisVoice"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisVoiceList.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisVoiceList",
        "clobbers": [
            "SpeechSynthesisVoiceList"
        ]
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "org.apache.cordova.geolocation": "0.3.11",
    "org.apache.cordova.speech.speechsynthesis": "0.1.0"
}
// BOTTOM OF METADATA
});
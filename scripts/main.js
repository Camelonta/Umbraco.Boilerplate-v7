var camelonta = (function () {

    // Init scripts
    var init = function () {

    }

    // Crossbrowser events
    var eventListener = function (target, type, callback) {
        var listenerMethod = target.addEventListener || target.attachEvent,
            eventName = target.addEventListener ? type : 'on' + type;

        listenerMethod(eventName, callback);
    }

    return {
        Init: init,
        EventListener: eventListener
    }
})();

// Run init scripts
camelonta.EventListener(window, 'load', function () { camelonta.Init(); });
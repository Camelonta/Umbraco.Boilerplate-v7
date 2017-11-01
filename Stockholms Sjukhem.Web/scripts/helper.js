Camelonta.Helper = (function () {

    // Debouncer for resize. Fire resize event resize has been still for 200ms. 
    var debouncer = function (func, timeout) {
        var timeoutID, timeout = timeout || 200;
        return function () {
            var scope = this, args = arguments;
            clearTimeout(timeoutID);
            timeoutID = setTimeout(function () {
                func.apply(scope, Array.prototype.slice.call(args));
            }, timeout);
        }
    }

    var getCurrentNodeId = function () {
        return $('body').data('current-node');
    }

    return {
        Debouncer: debouncer,
        GetCurrentNodeId: getCurrentNodeId
    };

})();
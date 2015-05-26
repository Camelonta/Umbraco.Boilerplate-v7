var Camelonta = (function () {

    // Init scripts
    var init = function () {
        Camelonta.Nav.Init();
        Camelonta.Faq.Init();
        Camelonta.Youtube.Init();

        // Global resize-event. Triggered if the resize-event has been still for 200ms (debounced)
        // L�gg all logik som h�nder vid resize h�r
        $(window).resize(Camelonta.Helper.Debouncer(function () {
            Camelonta.Nav.PlaceSubmenu();
        }));

    }

    return {
        Init: init
    }
})();

$(function() {
    Camelonta.Init();
})
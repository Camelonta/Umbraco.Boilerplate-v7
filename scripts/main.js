var Camelonta = (function () {

    // Init scripts
    var init = function () {
        Camelonta.Menu.Init();
        Camelonta.Faq.Init();
        Camelonta.Youtube.Init();
        Camelonta.Menu.PlaceSubmenu();

        // Resize-event. Triggered if the resize-event has been still for 200ms (debouncer)
        $(window).resize(Camelonta.Helper.Debouncer(function () {
            Camelonta.Menu.PlaceSubmenu();
            Camelonta.Menu.CheckIfMobileMenuShouldBeClosed();
        }));

    }

    return {
        Init: init
    }
})();

$(function() {
    Camelonta.Init();
})
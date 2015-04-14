var camelonta = (function () {

    // Init scripts
    var init = function () {
        camelonta.menu.Init();
        camelonta.faq.Init();
        camelonta.youtube.Init();
    }

    return {
        Init: init
    }
})();

$(function() {
    camelonta.Init();
})
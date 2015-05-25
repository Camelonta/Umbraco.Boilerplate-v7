Camelonta.Faq = (function () {

    var init = function () {
        // Show answers on question-click
        $(".question").click(function () {
            $(this).next(".answer").slideToggle();
        });
    }

    return {
        Init: init
    }
})();
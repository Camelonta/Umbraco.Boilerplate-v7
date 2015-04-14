camelonta.faq = (function () {

    // Init scripts
    var init = function () {
        initFaq();
    }

    // Init faq, hide answers and attach events
    var initFaq = function () {

        // Hide answers
        $(".answer").addClass("is-hidden");

        // Show answers on question-click
        $(".question").click(function() {
            $(this).next(".answer").toggleClass("is-hidden");
        });
    }

    return {
        Init: init
    }
})();
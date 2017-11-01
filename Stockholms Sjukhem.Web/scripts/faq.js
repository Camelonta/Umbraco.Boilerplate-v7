(function () {
    'use strict';

    // Show answers on question-click
    $(".question").click(function () {
        $(this).next(".answer").slideToggle();
    });

})();
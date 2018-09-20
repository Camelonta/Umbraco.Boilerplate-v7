(function () {

    $('.menu-button').click(function () {
        $.post('/umbraco/surface/partialsurface/navigationmenu', { nodeId: Camelonta.Helper.GetCurrentNodeId() }, function (data) {
            $('body').prepend($(data));

            $('.navigation-menu__close').click(function () {
                $('.navigation-menu').remove();
            });
        });
    });

})();

(function () {

    $('.search-button').click(function () {
        $.post('/umbraco/surface/partialsurface/searchmenu', { nodeId: Camelonta.Helper.GetCurrentNodeId() }, function (data) {
            $('body').prepend($(data));

            $('.search-menu__close').click(function () {
                $('.search-menu').remove();
            });
        });
    });

})();

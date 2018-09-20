(function () {

    $('.search-button').click(function () {
        $('.search-menu').addClass('search-menu--open');
    });

    $('.search-menu__close').click(function () {
        $('.search-menu').removeClass('search-menu--open');
    });

})();

(function () {

    $('.menu-button').click(function () {
        $('.navigation-menu').addClass('navigation-menu--open');
    });

    $('.navigation-menu__close').click(function () {
        $('.navigation-menu').removeClass('navigation-menu--open');
    });

})();

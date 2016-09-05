Camelonta.Nav = (function () {
    var menuOpen = false,
        menuOpenClass = 'mobile-menu-open',
        expandedClass = 'nav--is-expanded',
        loadingClass = 'nav--is-loading';

    // Get children from the surfacecontroller
    function openSubmenu(e) {
        e.preventDefault();
        var navSubExpander = $(e.target),
            li = $(e.target).closest('li'),
            id = li.data('id'),
            hasSubmenu = li.find('ul').length > 0;


        if (!hasSubmenu) {
            li.addClass(loadingClass);

            var parentUl = navSubExpander.closest('ul'),
                level = parentUl.data('level'),
                type = parentUl.data('type');

            $.post('/umbraco/surface/navigationsurface/getsubmenus', { id: id, currentNode: Camelonta.Helper.GetCurrentNodeId(), level: level, type: type }, function (data) {
                var ul = $(data);
                li.append(ul);

                ul.slideDown(200, function () {
                    // Remove display:block after the menu has slided
                    ul.css('display', '');
                });

                li.removeClass(loadingClass);
                li.addClass(expandedClass);
            });
        } else {
            var ul = li.find('ul').first(),
                isOpen = li.hasClass(expandedClass),
                actionClass = isOpen ? 'minimizing' : 'maximizing';

            // ActionClass is used to add the "active" style before the slide-animation is complete.
            li.addClass(actionClass);

            ul.slideToggle(200, function () {
                // Remove display after the menu has slided
                ul.css('display', '');
                li.toggleClass(expandedClass);
                li.removeClass(actionClass);
            });
        }
    }


    // Button to open mobile menu
    $(".mobile-menu-toggle").click(function () {

        menuOpen = !menuOpen; // Change the sate of menuOpen

        $('body').toggleClass(menuOpenClass);
    });

    $('nav').on('click', '.nav-sub-expander', function (e) {
        openSubmenu(e);
    });
})();
Camelonta.Nav = (function () {
    var menuOpenClass = 'mobile-menu-open',
        expandedClass = 'nav--is-expanded',
        loadingClass = 'nav--is-loading';

    function openSubmenu(e) {
        e.preventDefault();
        var navSubExpander = $(e.target),
            li = $(e.target).closest('li'),
            id = li.data('id'),
            hasSubmenu = li.find('ul').length > 0;

        if (hasSubmenu) {
            var ul = li.find('ul').first(),
    isOpen = li.hasClass(expandedClass),
    actionClass = isOpen ? 'nav--is-minimizing' : 'nav--is-maximizing';

            // ActionClass is used to add the "active" style before the slide-animation is complete.
            li.addClass(actionClass);

            ul.slideToggle(140, function () {
                // Remove display after the menu has slided
                ul.css('display', '');
                li.toggleClass(expandedClass);
                li.removeClass(actionClass);
            });
          
        } else {
            // If the clicked expander has submenus but they do not exist in DOM - fetch them via AJAX
            li.addClass(loadingClass);

            var parentUl = navSubExpander.closest('ul'),
                level = parentUl.data('level'), 
                type = parentUl.data('type'); // Type = main or side (nav)

            // Get children from the surfacecontroller
            $.post('/umbraco/surface/navigationsurface/getsubmenus', { id: id, currentNode: Camelonta.Helper.GetCurrentNodeId(), level: level, type: type }, function (data) {
                var ul = $(data);
                li.append(ul);

                ul.slideDown(140, function () {
                    // Remove display:block after the menu has slided
                    ul.css('display', '');
                });

                li.removeClass(loadingClass);
                li.addClass(expandedClass);
            });
        }
    }


    // Button to open mobile menu
    $(".mobile-menu-toggle").click(function () {
        $('body').toggleClass(menuOpenClass);
    });

    $('nav').on('click', '.nav-sub-expander', function (e) {
        openSubmenu(e);
    });
})();
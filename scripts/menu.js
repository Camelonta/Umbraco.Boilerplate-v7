Camelonta.Menu = (function () {
    var menuOpen = false,
        menuOpenClass = 'mobile-menu-open',
        mainNavigation;

    var init = function () {
        mainNavigation = $('#top-nav');

        $(".mobile-menu-toggle").click(function () {
            var body = $('body'),
                menuEnterClass = 'mobile-menu-enter',
                menuLeaveClass = 'mobile-menu-leave';


            // Change the sate of menuOpen
            menuOpen = !menuOpen;

            if (menuOpen) {
                body.addClass(menuEnterClass);
                mainNavigation.show();
            }
            else {
                body.addClass(menuLeaveClass);
            }

            setTimeout(function () {
                body.toggleClass(menuOpenClass);

                if (menuOpen) {
                    body.removeClass(menuEnterClass);
                } else {
                    mainNavigation.css('display','');
                    body.removeClass(menuLeaveClass);
                }
            }, 250);
        });

        $(".dyn-menu").on('click', '.toggle', function (e) {
            openSubmenu(e);
        });
    }

    var checkIfMobileMenuShouldBeClosed = function () {
        // If mobile menu is open and screen is larger than 768, close the menu
        if (menuOpen && Modernizr.mq('screen and (min-width:768px)')) {
            $('body').removeClass(menuOpenClass);
            mainNavigation.css('display', '');
            menuOpen = false;
        }
    };

    // Put the submenu to the correct location. Either let it be (desktop) or put it beneath the active link in main-navigation
    var placeSubmenu = function () {

        if (Modernizr.mq('screen and (max-width:767px)')) {
            // Append the submenus to the top navigation (mobile menu)
            var activeMenu = mainNavigation.find('li.active');
            $("#left-nav > ul").appendTo(activeMenu);
        } else {
            // All submenus in the top navigation (they exist if viewport went from small to large)
            var allSubmenus = $('#top-nav .dyn-menu > li > ul');

            if (allSubmenus.length > 0) {
                var leftNav = $('#left-nav'),
                    currentMenuItem = mainNavigation.find('li.current');

                var activeParent = currentMenuItem.parents('li.active').last();
                if (activeParent.length) {
                    // If we are on a sub sub page, we must find the top-level LI that is active
                    currentMenuItem = activeParent;
                }

                // Get the UL of the top-level menuitem
                var currentSubmenus = currentMenuItem.find('ul').first();

                if (leftNav.length > 0 && currentSubmenus.length > 0) {
                    // Append submenus to the left nav
                    currentSubmenus.appendTo(leftNav);
                }

                var allOtherSubmenusExceptCurrent = allSubmenus.not(currentSubmenus);

                // Remove submenus from top navigation (except for the current one) if screen went from small to large
                $.each(allOtherSubmenusExceptCurrent, function (i, el) {
                    var li = $(el).closest('li');
                    if (!li.hasClass('current'))
                        li.removeClass('active');
                });
                allOtherSubmenusExceptCurrent.remove();
            }
        }
    };


    function openSubmenu(e) {
        e.preventDefault();
        var li = $(e.target).closest('li'),
            id = li.data('id'),
            hasSubmenu = li.find('ul').length > 0;

        if (!hasSubmenu) {
            li.addClass('loading');
            $.post('/umbraco/surface/menusurface/getsubmenus', { id: id, currentNode: window.currentNode }, function (data) {
                var ul = $(data);
                li.append(ul);

                ul.slideDown(200, function () {
                    // Remove display:block after the menu has slided
                    ul.css('display', '');
                });

                li.removeClass('loading');
                li.addClass('active');
            });
        } else {
            var ul = li.find('ul').first(),
                isOpen = li.hasClass('active'),
                actionClass = isOpen ? 'minimizing' : 'maximizing';

            // ActionClass is used to add the "active" style before the slide-animation is complete.
            li.addClass(actionClass);

            ul.slideToggle(200, function () {
                // Remove display after the menu has slided
                ul.css('display', '');
                li.toggleClass('active');
                li.removeClass(actionClass);
            });
        }
    }

    return {
        Init: init,
        PlaceSubmenu: placeSubmenu,
        CheckIfMobileMenuShouldBeClosed: checkIfMobileMenuShouldBeClosed
    }
})();
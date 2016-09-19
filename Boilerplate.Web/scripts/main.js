var Camelonta = (function () {

    var replaceRightColumn = function () {
        var aside = $('aside');
        if (!aside.length)
            return;

        if (Modernizr.mq('screen and (max-width:800px)')) {
            aside.insertAfter('main').show();
        } else {
            aside.insertBefore('main');
        }
    };

    // Init scripts
    var init = function () {
        replaceRightColumn();

        // Global resize-event. Triggered if the resize-event has been still for 200ms (debounced)
        // Lägg all logik som händer vid resize här
        $(window).resize(Camelonta.Helper.Debouncer(function () {
            replaceRightColumn();
        }));

        // Hack for making CTA's work correctly when created in Tiny
        $('[class^="button"], [class^="icon"]').click(function (e) {
            var targetNode = e.target.nodeName;
            if (targetNode === 'SPAN') {
                var link = $(this).find('a'),
                    href = link.attr('href'),
                    target = link.attr('target');
                if (link.length && href !== "") {
                    if (target) {
                        window.open(href, target);
                    } else {
                        window.location.href = href;
                    }
                }
            }
        });
    }

    return {
        Init: init
    }
})();

$(function() {
    Camelonta.Init();
})
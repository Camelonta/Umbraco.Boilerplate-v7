if (navigator.cookieEnabled) {
    var cookieName = 'cookiesaccepted';

    if (!Cookies.get(cookieName)) {
        $.post('/umbraco/surface/partialsurface/cookiewarning', { nodeId: window.currentNode }, function (data) {
            var html = $(data);
            $('body').prepend(html);
            html.slideDown();

            // Click-event on Accept-button
            $('#cookie-warning .button').on('click', function () {
                Cookies.set(cookieName, true, { expires: 365 });
                html.slideUp();
            });
        });
    }
}
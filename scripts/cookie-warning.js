if (navigator.cookieEnabled) {
    var cookieName = 'cookiesaccepted';

    if (!Cookies.get(cookieName)) {
        $.post('/umbraco/surface/partialsurface/cookiewarning', { nodeId: Camelonta.Helper.GetCurrentNodeId() }, function (data) {

            if (data.length > 10) {
                var html = $(data);

                // Add element to DOM
                $.when($('body').prepend(html)).then(function () {
                    // When it's added, slide it down
                    html.slideDown();
                });
            } else {
                // Disclaimer does not contain any text. Disable it so it doesn't make any more requests
                Cookies.set(cookieName, true, { expires: 365 });
            }

            // Click-event on Accept-button
            $('#cookie-warning .accept').on('click', function () {
                Cookies.set(cookieName, true, { expires: 365 });
                html.slideUp();
            });
        });
    }
}
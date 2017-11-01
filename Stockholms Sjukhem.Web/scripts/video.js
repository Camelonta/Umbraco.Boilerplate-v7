(function () {
    'use strict';

    $(".video").click(function () {
        var videoId = $(this).data("video-id"),
            elId = 'video-' + videoId,
             url = "https://www.youtube.com/embed/" + videoId + "?modestbranding=1&autohide=1&showinfo=0&autoplay=1&rel=0&enablejsapi=1",
             iframe = '<iframe id="' + elId + '" src="' + url + '" allowfullscreen></iframe>';

        // Append this iframe to the DOM
        $(this).html(iframe);

        // If Triggerbees Youtube-logging is activated we must log manually (since this iframe is dynamically appended)
        if (typeof YT !== 'undefined' && typeof mtr_youtube !== 'undefined') {
            new YT.Player(elId, {
                events: {
                    'onStateChange': function (event) {
                        mtr_youtube.HandleEvent(event);
                    }
                }
            });
        }
    });
})();
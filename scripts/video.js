(function() {
    'use strict';
    
    $(".video").click(function () {
        var url = "https://www.youtube.com/embed/" + $(this).data("video-id") + "?modestbranding=1&autohide=1&showinfo=0&autoplay=1&rel=0";
        var iframe = '<iframe src="' + url + '" allowfullscreen></iframe>';
        $(this).html(iframe);
    });

})();
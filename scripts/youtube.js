camelonta.youtube = (function () {

    // Init scripts
    var init = function () {
        initYouTube();
    }

    // Init youtube player, only load youtube only on click
    var initYouTube = function () {

        $(".youtube-container").click(function () {
            var video = $(this).children("#youtube-start").first();
            var videoId = video.data("youtubeId");
            if (videoId) {
                var url = "http://www.youtube.com/embed/" + videoId + "?modestbranding=1&autohide=1&showinfo=0&autoplay=1&rel=0";
                var iframe = '<iframe src="' + url + '" allowfullscreen style="width:100%; height:100%; border:0"></iframe>';
                video.html(iframe);
            }
        });
    }

    return {
        Init: init
    }
})();

// Init youtube videos
camelonta.youtube.Init();
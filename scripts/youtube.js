camelonta.youtube = (function () {

    // Init scripts
    var init = function () {
        $(".youtube-preview").click(function () {
            var videoId = $(this).data("video");
            if (videoId) {
                var url = "http://www.youtube.com/embed/" + videoId + "?modestbranding=1&autohide=1&showinfo=0&autoplay=1&rel=0";
                var iframe = '<iframe src="' + url + '" allowfullscreen style="width:100%; height:100%; border:0"></iframe>';
                $(this).html(iframe);
            }
        });
    }

    return {
        Init: init
    }
})();

// Init youtube videos
camelonta.youtube.Init();
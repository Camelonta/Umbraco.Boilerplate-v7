camelonta.youtube = (function () {

    // Init scripts
    var init = function () {
        initYouTube();
    }

    // Init youtube player, only load youtube only on click
    var initYouTube = function () {
        var containers = document.getElementsByClassName("youtube-container");
        var i;
        for (i = 0; i < containers.length; i++) {

            camelonta.EventListener(containers[i], 'click', function () {
                if (event.target.children[0]) {
                    var videoId = event.target.children[0].dataset.youtubeId;
                    if (videoId) {
                        var url = "http://www.youtube.com/embed/" + videoId + "?modestbranding=1&autohide=1&showinfo=0&autoplay=1&rel=0";
                        var iframe = '<iframe src="' + url + '" allowfullscreen style="width:100%; height:100%; border:0"></iframe>';
                        event.target.innerHTML = iframe;
                    }
                }
            });
            
        }
    }

    return {
        Init: init
    }
})();

// Init youtube videos
camelonta.EventListener(window, 'load', function () { camelonta.youtube.Init(); });
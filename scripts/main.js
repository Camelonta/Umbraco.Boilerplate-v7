var boilerplate = boilerplate || (function () {

    // Init scripts
    var init = function () {
        initYouTube();
    }

    // Init youtube player, only load youtube only on click
    var initYouTube = function () {
        var containers = document.getElementsByClassName("youtube-container");
        var i;
        for (i = 0; i < containers.length; i++) {

            boilerplate.EventListener(containers[i], 'click', function () {
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

    // Crossbrowser events
    var eventListener = function (target, type, callback) {
        var listenerMethod = target.addEventListener || target.attachEvent,
            eventName = target.addEventListener ? type : 'on' + type;

        listenerMethod(eventName, callback);
    }

    return {
        Init: init,
        EventListener: eventListener
    }
})();

// Run init scripts
boilerplate.EventListener(window, 'load', function () { boilerplate.Init(); });
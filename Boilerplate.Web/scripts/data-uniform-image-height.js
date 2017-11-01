(function () {
    'use strict';

    $(window).load(function () {
        $('[data-uniform-image-height="1"]').each(function () {
            var $images = $(this).find('img');
            var imageHeights = $images.map(function () {
                return $(this).height();
            }).get();
            var largestHeight = Math.max.apply(null, imageHeights);
            $images.each(function () {
                this.setAttribute('style', 'height:' + largestHeight + 'px !important');
            });
        });
    });
})();
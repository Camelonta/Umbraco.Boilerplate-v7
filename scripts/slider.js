Camelonta.Slider = (function () {
    var init = function () {
        var swiper = [];
        $('.swiper-container').each(function (index) {

            var $el = $(this);

            swiper[index] = $el.swiper({
                centeredSlides: true,
                loop: true,
                autoplay: 10000,
                pagination: $el.find('.swiper-pagination')[0],
                paginationClickable: true
            });

            $el.find('.swiper-button-prev').on('click', function () {
                swiper[index].slidePrev();
            });

            $el.find('.swiper-button-next').on('click', function () {
                swiper[index].slideNext();
            });

        });
    }

    return {
        Init: init
    }
})();
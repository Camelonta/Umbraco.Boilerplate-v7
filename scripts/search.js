Camelonta.Search = (function () {
    var highlightDescription = function (searchTerm) {
        if (searchTerm) {
            $('.search-result').highlight(searchTerm);
        }
    }

    var autocomplete = function () {
        $('.search-form input[type="search"]').autoComplete({
            minChars: 1,
            source: function (term, suggest) {
                // Setup data to POST
                var data = {
                    searchTerm: term
                };

                $.post('/umbraco/surface/searchsurface/GetSearchSuggestions', data, function (response) {
                    var matches = response;
                    suggest(matches);
                });
            },
            onSelect: function (event, term, item) {
                // Trigger new search
                //$('.search-form input[type="search"').eq(0).trigger('click');// TODO: FIxa

                window.location = $('body').data('search-url') + '?q=' + term; // TODO: Sök utan att ladda om sidan
            }
        });
    }

    var init = function () {
        autocomplete();
        var searchMoreLink = $('#search-more-results');
        var searchTerm = $('#search-results').data('search-term');

        searchMoreLink.click(function (e) {
            // Set loading state
            searchMoreLink.addClass('loading');

            // Hide error if it's shown and this is another search
            $('#search-unexpected-error').addClass('hide'); 

            // Setup data to POST
            var data = {
                searchTerm: searchTerm,
                nextPage: searchMoreLink.data('next-page')
            };

            $.post('/umbraco/surface/searchsurface/getsearchresults', data, function (response) {
                // Print response
                $('#search-results').html(response);

                // Increase nextPage
                var nextPage = data.nextPage + 1;
                searchMoreLink.data('next-page', nextPage);

                var resultCount = parseInt($('#search-results-count').text());
                var totalResults = parseInt($('#search-total-results').text());
                if (resultCount >= totalResults) {
                    // Remove "more results"-link
                    $('#search-more-results').remove();
                }
                
                // Highlight when links is pressed
                highlightDescription(searchTerm)

                // Set loading state
                searchMoreLink.removeClass('loading');
            }).fail(function (errorResponse) {
                // Show error
                $('#search-unexpected-error').removeClass('hide');
                //console.error(errorResponse.responseText);

                // Set loading state
                searchMoreLink.removeClass('loading');
            });

            e.preventDefault();
        });

        // Highlight on load
        highlightDescription(searchTerm)
    }

    init();
})();
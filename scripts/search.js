(function () {
    'use strict';

    //var highlightDescription = function (searchTerm) {
    //    if (searchTerm) {
    //        $('.search-result').highlight(searchTerm);
    //    }
    //}

    var autocomplete = function () {
        var searchInput = $('.search-form input[type="search"]');
        searchInput.autoComplete({
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
                // TODO: posta formuläret. man får bara inte tag i formuläert på något sätt. ? 
                //console.log(event)
                //console.log(term)
                //console.log(item)
                //console.log($(this))
                //$(item).closest('form').submit();
            }
        });
    }

    autocomplete();

    var searchMoreLink = $('#search-more-results'),
        searchTerm = window.query,
        skip = window.skipAndTakeAmount;

    searchMoreLink.click(function (e) {
        e.preventDefault();

        // Set loading state
        searchMoreLink.addClass('loading');

        // Hide error if it's shown and this is another search
        $('#search-unexpected-error').addClass('hide');

        // Setup data to POST
        var data = {
            searchTerm: searchTerm,
            skip: skip,
            take: window.skipAndTakeAmount
        };

        $.post('/umbraco/surface/searchsurface/getsearchresults', data, function (response) {

            // Print search results
            $('#search-results').append(response.html);

            // Increase the skip-amount
            skip = skip + window.skipAndTakeAmount;

            // If there are no more search results - delete the button
            if (!response.moreResultsAvailable) {
                // Remove "more results"-link
                $('#search-more-results').remove();
            }

            // Update the amount of search results we have retireved
            $('#amount-of-taken-search-results').text(response.amountOfTakenResult);

            // Highlight when links is pressed
            //highlightDescription(searchTerm);

        }).fail(function () {
            // Show error
            $('#search-unexpected-error').removeClass('hide');

        }).always(function () {
            // Always remove loading state
            searchMoreLink.removeClass('loading');
        });
    });

    // Highlight on load
    //highlightDescription(searchTerm);

})();
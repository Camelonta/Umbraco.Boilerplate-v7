(function () {
    'use strict';

    var highlightDescription = function (searchTerm) {
        if (searchTerm) {
            $('.search-result').highlight(searchTerm);
        }
    }

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
                // Try to get the form wich the user used to search (no exact way to get this if there are multiple search forms)
                // Loop thourgh all search inputs
                for (var i = 0; i < searchInput.length; i++) {
                    // If this input has a value
                    if (searchInput[i].value) {
                        // Get the search form for this input
                        var formWithSearchValue = $(searchInput[i]).closest('form');
                        // Submit tjs
                        formWithSearchValue.submit();
                    }
                }
            }
        });
    }

    autocomplete();

    var searchMoreLink = $('.load-more-results'),
        loadingClass = 'load-more-results--is-loading',
        searchTerm = window.query,
        skip = window.skipAndTakeAmount;

    searchMoreLink.click(function (e) {
        e.preventDefault();

        // Set loading state
        searchMoreLink.addClass(loadingClass);

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
                searchMoreLink.remove();
            }

            // Update the amount of search results we have retireved
            $('#amount-of-taken-search-results').text(response.amountOfTakenResult);

            // Highlight when links is pressed
            highlightDescription(searchTerm);

        }).fail(function () {
            // Show error
            $('#search-unexpected-error').removeClass('hide');

        }).always(function () {
            // Always remove loading state
            searchMoreLink.removeClass(loadingClass);
        });
    });

    // Highlight on load
    highlightDescription(searchTerm);

})();
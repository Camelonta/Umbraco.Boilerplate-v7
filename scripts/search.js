Camelonta.Search = (function () {
    var highlightDescription = function (searchTerm) {
        $('.search-result').highlight(searchTerm);
    }

    var init = function () {
        $('#search-unexpected-error').addClass('hidden');

        var searchMoreLink = $('#search-more-results');
        var searchTerm = $('#search-more-results').data('search-term');

        searchMoreLink.click(function (e) {
            var data = {
                nodeId: Camelonta.Helper.GetCurrentNodeId(),
                searchTerm: searchTerm,
                nextPage: searchMoreLink.data('next-page')
            };

            $.post('/umbraco/surface/partialsurface/getsearchresults', data, function (response) {
                // Print response
                $('#search-results').html(response); // TODO: Testa med exception. Ev hantera
                $('#search-results').slideDown();

                // Increase next page
                searchMoreLink.data('next-page', data.nextPage + 1);

                // Remove
                var resultCount = $('#search-results-count').text();
                var totalResults = $('#search-total-results').text();
                if (resultCount >= totalResults) {
                    $('#search-more-results').remove();
                }
                
                // Highlight when links is pressed
                highlightDescription(searchTerm)
            }).fail(function (errorResponse) {
                //console.error(errorResponse);
                $('#search-unexpected-error').removeClass('hidden');
            });

            e.preventDefault();
        });

        // Highlight on load
        highlightDescription(searchTerm)
    }

    init();
})();
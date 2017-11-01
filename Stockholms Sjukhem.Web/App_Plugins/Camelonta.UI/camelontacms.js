angular.module('umbraco.services').config(['$httpProvider', function ($httpProvider) {

    $httpProvider.interceptors.push(function ($q) {
        return {
            'response': function (response) {
                // Load our welcome-text on the login-page
                if (response.config.url.indexOf('views/common/dialogs/login.html') > -1) {
                    loadLoginPage(response);
                }
                // Load camelonta-logo
                if (response.config.url.indexOf('views/components/application/umb-sections.html') > -1) {
                    loadCamelontaLogo(response);
                }
                // Load our content in help-section
                if (response.config.url.indexOf('views/common/overlays/help/help.html') > -1) {
                    loadHelpSection(response);
                }

                return response;
            }
        };
    });

    // Load our welcome-text on the login-page
    function loadLoginPage(response) {
        var html = $('<div />', { html: response.data });
        html.find("#login h1")
            .html("Welcome to <span class='cam-green'>Umbraco CMS</span> <span class='cam-basedon'>- Built and maintained by <span class='cam-orange'>Camelonta AB</span></span>")
            .css('opacity', 1);

        html.find("#login").prepend("<div class='camelonta-logo'><a href='/umbraco'><img src='../App_Plugins/Camelonta.UI/camelontacms.png'/></a></div>");

        response.data = html.html();
    }

    // Load camelonta-logo
    function loadCamelontaLogo(response) {
        var html = $('<div />', { html: response.data });
        html.find("#applications .sections").prepend("<li class='camelonta-logo'><a href='/umbraco'><img src='../App_Plugins/Camelonta.UI/camelontacms.png'/></a></li>")
        html.find("li.avatar").insertBefore(html.find("li.help"));
        response.data = html.html();
    }

    // Load our content in help-section
    function loadHelpSection(response) {
        var html = $('<div />', { html: response.data });
        html.find(".umb-control-group:first").prepend("<iframe height='240' src='../App_Plugins/Camelonta.UI/help.html'/>");
        response.data = html.html();
    }

}]);
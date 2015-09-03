(function () {

    // Globals
    var loaded;

    // Init
    function init() {
        loaded = {}
        setTimeout(loadLoginPage, 300);
        setTimeout(loadCamelontaLogo, 300);
        setTimeout(loadHelpLink, 400);
        setTimeout(loadUserLink, 300);
    }
    $(function () {
        init();
    });


    // Load camelonta-logo
    function loadCamelontaLogo() {

        if ($("#applications .sections").length) {

            $("#applications .sections").prepend("<li class='camelonta-logo'><a href='/umbraco'><img src='../App_Plugins/Camelonta.UI/camelontacms.png'/></a></li>");

            $("li.avatar").detach().insertBefore('li.help');

            loaded.camelontaLogoLoaded = true;
        }

        if (!loaded.camelontaLogoLoaded)
            setTimeout(loadCamelontaLogo, 300);
    }

    // Load click-event to help-section
    function loadHelpLink() {

        if ($("a.help").length) {

            $("a.help").click(function () {
                setTimeout(loadHelpSection, 300);
            });

            loaded.helpLinkLoaded = true;
        }

        if (!loaded.helpLinkLoaded)
            setTimeout(loadHelpLink, 300);
    }

    // Load our content in help-section
    function loadHelpSection() {

        if ($("div[ng-controller='Umbraco.Dialogs.HelpController']").length) {

            $("div[ng-controller='Umbraco.Dialogs.HelpController'] .tab-content > .umb-pane:first-child").prepend("<iframe height='240' src='../App_Plugins/Camelonta.UI/help.html'/>");

            loaded.helpSectionLoaded = true;
        }

        if (!loaded.helpSectionLoaded)
            setTimeout(loadHelpSection, 300);
    }

    // Load our welcome-text on the login-page
    function loadLoginPage() {

        if ($("#login h1").length) {

            $("#login h1")
                .html("Välkommen till <span class='cam-green'>Camelonta Web Pro</span> <span class='cam-basedon'>- en tjänst baserad på <span class='cam-orange'>Umbraco</span> CMS</span>")
                .css('opacity', 1);

            $("#login").prepend("<div class='camelonta-logo'><a href='/umbraco'><img src='../App_Plugins/Camelonta.UI/camelontacms.png'/></a></div>");

            loaded.loginPageLoaded = true;
        }

        // Check if we are not on loginpage and abort
        if ($("#leftcolumn #applications").length) {
            loaded.loginPageLoaded = true;
        }

        if (!loaded.loginPageLoaded)
            setTimeout(loadLoginPage, 300);
    }

    // Load click-event to user-profile-section
    function loadUserLink() {

        if ($("#avatar-img").length) {

            $("#avatar-img").click(function () {
                setTimeout(loadUserSection, 300);
            });

            loaded.userLinkLoaded = true;
        }

        if (!loaded.userLinkLoaded)
            setTimeout(loadUserLink, 300);
    }

    // Load click-event on logout-button in user-section
    function loadUserSection() {
        if ($("div[ng-controller='Umbraco.Dialogs.UserController']").length) {

            $("div[ng-controller='Umbraco.Dialogs.UserController'] .btn-toolbar button").click(function () {
                init();
            });

            loaded.userSectionLoaded = true;
        }

        if (!loaded.userSectionLoaded)
            setTimeout(loadUserSection, 300);
    }

})();
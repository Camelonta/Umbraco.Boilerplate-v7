/* Load logo */

setTimeout(loadCamelontaLogo, 10);

var camelontaLogoLoaded = false;

function loadCamelontaLogo() {
    var elems = window.top.document.getElementsByClassName('avatar');
    if (elems.length) {

        // TODO: Fade in the logo
        for (var i = 0; i < elems.length; i++) {
            var image = elems[i].children[0];
            image.src = "../../App_Plugins/Camelonta.UI/camelontacms.png";
            image.style.width = "30px";
            image.style.borderRadius = "16px";
        }

        // Load css     
        var fileref=document.createElement("link")
        fileref.setAttribute("rel", "stylesheet")
        fileref.setAttribute("type", "text/css")
        fileref.setAttribute("href", "/App_Plugins/Camelonta.UI/camelontacms.css")
        window.top.document.getElementsByTagName("head")[0].appendChild(fileref)

        camelontaLogoLoaded = true;
    }

    if (!camelontaLogoLoaded)
        setTimeout(loadCamelontaLogo, 10);
}

/* Load toolbar */

//var getQueryStringValueFromParent = function (name) {
//    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
//    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
//        results = regex.exec(window.top.location.search);
//    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
//};

//var hideToolbar = function() {
//    document.getElementById('preview-toolbar').style.display = 'none';
//}

//var id = getQueryStringValueFromParent("id");

//var toolbar = document.createElement("div");
//toolbar.id = "preview-toolbar";
//toolbar.style.background = "#f8f8f8";
//toolbar.style.borderBottom = "1px solid #d9d9d9";
//toolbar.style.padding = "10px";

//toolbar.innerHTML += "<p>Id: " + id + "</p>";
//toolbar.innerHTML += "<p><a href='/umbraco#/content/content/edit/" + id + "' target='_top'>Edit</a></p>";
//toolbar.innerHTML += "<p><a href='javascript:void(0)' onclick='hideToolbar()' target='_top'>Close toolbar</a></p>";

//$(function () {

//    var url = "/umbraco/backoffice/UmbracoTrees/ContentTree/GetTreeNode?id=" + id + "&application=content&tree=&isDialog=false";

//    $.get(url, function() {
//        alert("success");
//    });
//});

//document.body.insertBefore(toolbar, document.body.firstChild);
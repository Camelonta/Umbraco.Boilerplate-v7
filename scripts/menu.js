camelonta.menubar = (function () {

    var menuOpen = false;
    var mobileWidthBreakpoint = 480;
    var topNav = document.getElementById("topnav");
    var leftNav = document.getElementById("leftnav");
    var mobileMenuButton = document.getElementById("mobile-menu-button");

    var init = function () {
        checkMenuState();
        addEvent(window, "resize", checkMenuState);
        addEvent(mobileMenuButton, "click", openCloseMenu);
    }

    var openCloseMenu = function () {
        if (!menuOpen) {
            openMenu();
        } else {
            closeMenu();
        }
        menuOpen = !menuOpen;
    }

    var openMenu = function () {
        //console.log("Open menu...");
        topNav.classList.remove("is-hidden");
        topNav.classList.add("mobile");

        var x = topNav.clientHeight;
        topNav.classList.add("slide-in");
    }

    var closeMenu = function () {
        //console.log("Close menu...");

        topNav.classList.remove("slide-in");
        topNav.classList.add("is-hidden");
        topNav.classList.remove("mobile");
    }

    var checkMenuState = function () {

        var w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth;

        if (x < mobileWidthBreakpoint) {
            topNav.classList.add("is-hidden");
            if (leftNav != null)
                leftNav.classList.add("is-hidden");
            mobileMenuButton.classList.remove("is-hidden");
            //console.log("MOBILE");
        } else {
            topNav.classList.remove("is-hidden");
            if (leftNav != null)
                leftNav.classList.remove("is-hidden");
            mobileMenuButton.classList.add("is-hidden");
            //console.log("DESKTOP");
        }
    }

    // Ersätt med tobbes variant
    var addEvent = function (elem, type, eventHandle) {
        if (elem == null || typeof (elem) == 'undefined') return;
        if (elem.addEventListener) {
            elem.addEventListener(type, eventHandle, false);
        } else if (elem.attachEvent) {
            elem.attachEvent("on" + type, eventHandle);
        } else {
            elem["on" + type] = eventHandle;
        }
    };

    return {
        Init: init
    }
})();

// Init mobile menu
camMain.EventListener(window, 'load', function () { camMenu.Init(); });




//function detectswipe(el, func) {
//    swipe_det = new Object();
//    swipe_det.sX = 0;
//    swipe_det.sY = 0;
//    swipe_det.eX = 0;
//    swipe_det.eY = 0;
//    var min_x = 20;  //min x swipe for horizontal swipe
//    var max_x = 40;  //max x difference for vertical swipe
//    var min_y = 40;  //min y swipe for vertical swipe
//    var max_y = 50;  //max y difference for horizontal swipe
//    var direc = "";
//    ele = document.getElementById(el);
//    ele.addEventListener('touchstart', function (e) {
//        var t = e.touches[0];
//        swipe_det.sX = t.screenX;
//        swipe_det.sY = t.screenY;
//    }, false);
//    ele.addEventListener('touchmove', function (e) {
//        e.preventDefault();
//        var t = e.touches[0];
//        swipe_det.eX = t.screenX;
//        swipe_det.eY = t.screenY;
//    }, false);
//    ele.addEventListener('touchend', function (e) {
//        //horizontal detection
//        if ((((swipe_det.eX - min_x > swipe_det.sX) || (swipe_det.eX + min_x < swipe_det.sX)) && ((swipe_det.eY < swipe_det.sY + max_y) && (swipe_det.sY > swipe_det.eY - max_y)))) {
//            if (swipe_det.eX > swipe_det.sX) direc = "r";
//            else direc = "l";
//        }
//        //vertical detection
//        if ((((swipe_det.eY - min_y > swipe_det.sY) || (swipe_det.eY + min_y < swipe_det.sY)) && ((swipe_det.eX < swipe_det.sX + max_x) && (swipe_det.sX > swipe_det.eX - max_x)))) {
//            if (swipe_det.eY > swipe_det.sY) direc = "d";
//            else direc = "u";
//        }

//        if (direc != "") {
//            if (typeof func == 'function') func(el, direc);
//        }
//        direc = "";
//    }, false);
//}

//function myfunction(el, d) {
//    alert("you swiped on element with id '" + el + "' to " + d + " direction");
//}

//detectswipe('topnav', myfunction);
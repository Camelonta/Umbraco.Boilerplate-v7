angular.module("umbraco").controller("ActivityLog.DashController", function ($scope, $http) {

    var skipPerPage = 10;
    var allPages = [];
    $scope.currentPages = [];
    $scope.currentPage = 1;
    $scope.morePages = false;
    $scope.lessPages = false;
    $scope.resources = null;

    var setPageSpan = function () {
        var nMin = $scope.currentPage - 6 > 0 ? $scope.currentPage - 6 : 0;
        var nMax = nMin + 11 < allPages.length ? nMin + 11 : allPages.length;
        $scope.currentPages = allPages.slice(nMin, nMax);
        $scope.lessPages = nMin > 0;
        $scope.morePages = nMax < allPages.length;
    }

    $scope.getActivities = function (skip) {
        $http.get("backoffice/ActivityLog/ActivityLog/GetLog?skip=" + skip).success(function (data) {
            $scope.loglist = data;
        });
        $scope.currentPage = (skip / 10) + 1;
        setPageSpan();
    }

    $http.get("backoffice/ActivityLog/ActivityLog/GetTotalActivitiesAndResources").success(function (response) {
        $scope.resources = response.Resources;
        var numberOfPages = Math.ceil(response.NumberOfActivities / skipPerPage);
        for (var i = 0; i < numberOfPages; i++) {
            var page = {
                index: i + 1,
                skip: i * 10
            }
            allPages.push(page);
        }
        setPageSpan();
    });

    $scope.getActivities(0);

    $scope.isUmbracoFormLogItem = function (item) { 
        if ($scope.formItem(item) == null) {
            return false;
        }
        return true;
    }

    $scope.formItem = function (item) {
        var umbracoFormsPrefix = "UmbracoForm '";
        var isUmbracoFormLogItem = item.Message.indexOf(umbracoFormsPrefix) > -1;
        if (isUmbracoFormLogItem) {
            var endOfObject = item.Message.lastIndexOf("'");
            var objStr = item.Message.substring(umbracoFormsPrefix.length, endOfObject);
            var obj = JSON.parse(objStr);
            // TODO: Pass and print info about workflow
            if (obj.Event) {
                var eventStr = '';
                var words = obj.Event.split(/(?=[A-Z])/);
                for (var i = 0; i < words.length; i++) {
                    var word = words[i];
                    eventStr += word.toLowerCase();
                    if (i < words.length) {
                        eventStr += ' ';
                    }
                }
                obj.Event = eventStr; // TODO: New property instead of set this event property
            }
            return obj; // TODO: Add properties on ActivityViewModel instead?
        }
        return null;
    }
});
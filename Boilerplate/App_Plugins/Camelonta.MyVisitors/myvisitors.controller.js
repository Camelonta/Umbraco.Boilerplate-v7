angular.module("umbraco").controller("MyVisitors.DashController", function ($scope, $http, $window, $element) {


    // Set width of graf when we resize
    $scope.getElementWidth = function () {
        return $element.width();
    };

    $scope.width = $scope.getElementWidth();

    $scope.$watch($scope.getElementWidth, function (newValue, oldValue) {
        $scope.width = newValue / 2.1;
    }, true);

    $element.bind('resize', function () {
        $scope.$apply();
    });


    // Get data for list
    $scope.sites = [];
    GetSites();


    $scope.showHideSettings = function () {
        $scope.showSettings = !$scope.showSettings;
    };

    $scope.add = function () {
        $scope.sites.push({});
    };

    $scope.save = function (site) {

        if (!site.Id)
            site.Id = "";

        $http.post("backoffice/MyVisitors/Sites/SaveSite?Id="+ site.Id +"&name="+ site.Name +"&siteid="+ site.SiteId +"&sitekey="+ site.SiteKey).success(function (message) {
            GetSites();
        });

    };

    $scope.delete = function (site, index) {

        $http.delete("backoffice/MyVisitors/Sites/DeleteSite", { params: { Id: site.Id } }).success(function (message) {
            GetSites();
        });

    };


    function GetSites() {

        $http.get("backoffice/MyVisitors/Sites/GetSites").success(function (data) {

            $scope.showSettings = false;
            $scope.resources = data.resources;
            $scope.sites = data.sites;
            $scope.message = null;

            angular.forEach(data.sites, function (site) {

                $http.get("backoffice/MyVisitors/Visitors/GetVisitors", { params: { siteid: site.SiteId, sitekey: site.SiteKey } }).success(function (visitors) {

                    if (visitors && !visitors[0].error) {
                        site.visitors = visitors[0].dates[0].items;
                    } else {
                        var error = visitors ? visitors[0].error : "Invalid config?";
                        $scope.message = "Error: " + error;
                    }
                });

            });

        });
    }

});
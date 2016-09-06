console.log(app.requires)
//This is my Angular module that I want to inject/require
app.requires.push('chart.js');

app.controller("MyVisitors.DashController", function ($scope, $http, $window, $element) {


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
    $scope.someData = {
        labels: ['Apr', 'May', 'Jun'],
        datasets: [
          {
              data: [1, 7, 15, 19, 31, 40]
          }
        ]
    };

    $scope.someOptions = {
        segementStrokeWidth: 20,
        segmentStrokeColor: '#000'
    };

    $scope.save = function (site) {

        if (!site.Id)
            site.Id = "";

        $http.post("backoffice/MyVisitors/Sites/SaveSite?Id=" + site.Id + "&name=" + site.Name + "&siteid=" + site.SiteId + "&sitekey=" + site.SiteKey).success(function (message) {
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
                console.log(site)
                site.someData = {
                    labels: ['Apr', 'May', 'Jun'],
                    datasets: [
                      {
                          data: [1, 7, 15, 19, 31, 40]
                      }
                    ]
                };

                site.someOptions = {
                    segementStrokeWidth: 20,
                    segmentStrokeColor: '#000'
                };

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


    //$scope.load = function() {
    //  
    //}

});






//angular.module("umbraco").directive('myvisitors', function () {
//    return {
//        restrict: 'E',
//        template: '<canvas></canvas><div class="jschart-legend" ng-if="legend" ng-bind-html="legend | sanitize"></div>',
//        controller: ['$scope', '$element', '$http', '$rootScope', function ($scope, $element, $http, $rootScope) {
//            var canvas = $element.find('canvas').get(0);
//            console.log($element)
//            console.log(canvas)
//            var ctx = canvas.getContext("2d");

//            var options = {
//                scaleShowHorizontalLines: false,
//                scaleShowVerticalLines: false,
//                legendTemplate: "<div class=\"jschart-legend-line\"><% for (var i=0; i<datasets.length; i++){%><div><div class=\"jschart-legend-border\"><div class=\"jschart-legend-fill\" style=\"border:5px solid <%=datasets[i].strokeColor%>;overflow:hidden\"></div></div><%if(datasets[i].label){%><%=datasets[i].label%><%}%></div><%}%></div>"
//            };

//            var data = {
//                labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
//                datasets: []
//            };

//            data.datasets.push({
//                label: "besökare",
//                fillColor: "rgba(151,187,205,0.2)",
//                pointStrokeColor: "#fff",
//                pointHighlightFill: "#fff",
//                //pointHighlightStroke: dataset.strokeColor,
//                data: [12, 19, 3, 5, 2, 3]
//            });
//            // Activate chart
//            var myNewChart = new Chart(ctx).Line(data, options);

//            // Fill legend-div on scope
//            $scope.legend = myNewChart.generateLegend();
//        }]
//    }
//});
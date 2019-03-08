(function () {
    'use strict';
    angular.module("umbraco")
        .controller("Diplo.LinkCheckerController",
            function ($scope, $http, $filter, dialogService, notificationsService) {
                var baseApiUrl = "/Umbraco/BackOffice/Api/LinkChecker/";
                var checkLinksUrl = baseApiUrl + "CheckPage/";
                var getIdsToCheckUrl = baseApiUrl + "GetIdsToCheck/";
                var dialog;
                $scope.config = {};

                $http.get('/App_Plugins/Diplo.LinkChecker/config.js').success(function (data) {
                    $scope.config = data[0];
                    initialise();
                }).
                    error(function () {
                        $scope.config.timeout = 30;
                        $scope.config.onlyShowErrors = false;
                        $scope.config.checkInternalLinksOnly = false;
                        $scope.config.checkEntireDocument = false;
                        $scope.config.showSearchBox = false;
                        $scope.config.omitPortDuringChecks = false;
                        $scope.config.showErrors = "300,400,500";
                        console.log("Couldn't load config.js - dropping back to defaults...");
                        initialise();
                    });

                var initialise = function () {
                    console.log($scope.config);

                    $scope.buttonText = "Start Check";
                    $scope.pageCnt = $scope.linksCheckedCnt = $scope.linksOkCnt = $scope.linksErrorCnt = 0;
                    $scope.errorMessage = "";
                    $scope.httpCodes = {};

                    $http.get("/App_Plugins/Diplo.LinkChecker/HttpStatusCodes.js").
                        success(function (data, status, headers, config) {
                            $scope.httpCodes = data;
                        });

                    $scope.startCheck = function () {
                        $scope.showStartMessage = true;
                        $scope.startNodeName = null;
                        dialogService.contentPicker({
                            multiPicker: false,
                            callback: function (data) {
                                checkIds(data);
                            }
                        });
                    };
                };

                var checkIds = function (data) {
                    $scope.progress = 0;
                    $scope.checkedPages = [];
                    $scope.buttonText = "Checking ...";
                    $scope.startNodeName = data.name;
                    $scope.startNodeIcon = data.icon;
                    $scope.finished = false;
                    $scope.showStartMessage = false;
                    $scope.pageCnt = $scope.linksCheckedCnt = $scope.linksOkCnt = $scope.linksErrorCnt = 0;
                    $scope.finishMessage = "";
                    $scope.errorMessage = "";
                    $scope.statusGroup = {};

                    var cnt = 1;

                    $http.get(getIdsToCheckUrl + data.id).
                        success(function (data, status, headers, config) {
                            var dataLength = data.length;

                            if (dataLength < 1) {
                                $scope.finishMessage = "<p class=\"alert\">No pages (that have a template) where found to check!</p>";
                                $scope.buttonText = "Start Check";
                                return;
                            }

                            for (var i = 0; i < data.length; i++) {
                                $http({
                                    url: checkLinksUrl + data[i],
                                    method: "GET",
                                    params: {
                                        checkEntireDocument: $scope.config.checkEntireDocument,
                                        timeout: $scope.config.timeout,
                                        hideDuplicates: $scope.config.hideDuplicates,
                                        omitPortDuringChecks: $scope.config.omitPortDuringChecks,
                                        checkInternalLinksOnly: $scope.config.checkInternalLinksOnly,
                                        onlyShowErrors: $scope.config.onlyShowErrors,
                                        showErrors: $scope.config.showErrors
                                    }
                                }).
                                    success(function (data, status, headers, config) {
                                        $scope.linksCheckedCnt += data.LinksCount;
                                        $scope.linksErrorCnt += data.ErrorCount;
                                        $scope.linksOkCnt += data.SuccessCount;
                                        $scope.pageCnt++;

                                        if (data.CheckedLinks.length > 0) {
                                            for (var i = 0; i < data.CheckedLinks.length; i++) {
                                                var statusCode = data.CheckedLinks[i].StatusCode;

                                                statusCode = statusCode == null ? "N/A" : statusCode;

                                                if (statusCode in $scope.statusGroup) {
                                                    $scope.statusGroup[statusCode] = $scope.statusGroup[statusCode] + 1;
                                                } else {
                                                    $scope.statusGroup[statusCode] = 1;
                                                }
                                            }
                                        }

                                        $scope.checkedPages.push(data);
                                        $scope.progress = calculateProgress(cnt, dataLength);

                                        $scope.finished = (cnt++ === dataLength);

                                        if ($scope.finished) {
                                            $scope.finishMessage = "Checked <strong>" + $scope.linksCheckedCnt + "</strong> links and found <strong>" + $scope.linksErrorCnt + "</strong> errors.";
                                            if ($scope.linksErrorCnt === 0) {
                                                $scope.finishMessage += " <i class='icon-smiley-inverted'></i>";
                                            }

                                            notificationsService.success("Finished!", $scope.finishMessage);
                                            $scope.buttonText = "Start Check";
                                        }
                                    }).
                                    error(function (datax, status, headers, config) {
                                        $scope.pageCnt++;
                                        cnt++;
                                        $scope.errorMessage = status + ". " + datax.Message + "\n" + datax.ExceptionMessage;
                                        console.log(datax);
                                    });
                            }
                        }).
                        error(function (data, status, headers, config) {
                            $scope.errorMessage = "Fatal Error! " + status + ". Unable to retrieve pages from Umbraco to check!";
                            $scope.buttonText = "Start Check";
                            console.log(data);
                            cnt++;
                        });
                };

                var calculateProgress = function (currentPage, totalPages) {
                    return parseInt((currentPage / totalPages) * 100);
                };

                $scope.getStatusDescription = function (code) {
                    return $filter('filter')($scope.httpCodes, function (d) { return d.code == code; })[0];
                }

                $scope.openDetail = function (link, page) {
                    if (dialog) {
                        dialog.close();
                    }

                    link["page"] = page;
                    var dialogData = link;

                    dialog = dialogService.open({
                        template: '/App_Plugins/Diplo.LinkChecker/detail.html',
                        dialogData: dialogData, show: true, width: 800
                    });
                };

                $scope.openHelp = function (link, page) {
                    if (dialog) {
                        dialog.close();
                    }

                    dialog = dialogService.open({
                        template: '/App_Plugins/Diplo.LinkChecker/help.html',
                        show: true, width: 800
                    });
                };
            });
})();
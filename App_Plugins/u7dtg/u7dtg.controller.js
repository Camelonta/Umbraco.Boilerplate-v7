angular.module("umbraco")
    .controller("u7dtg.editorController",
    function ($scope, $timeout) {

        if (!$scope.model.value) {
            $scope.model.value = [

            ]
        }
        
        var dtgContentPicker = {
            alias: 'u7dtgContentPicker',
            label: '',
            description: '',
            view: 'contentpicker',
            config: {
                minNumber: 0,
                maxNumber: 0,
                multiPicker: '0'
            }
        };

        var dtgMediaPicker = {
            alias: 'u7dtgMediaPicker',
            label: '',
            description: '',
            view: 'mediapicker',
            config: {
                multiPicker: '0'
            }
        };

        var dtgDatePicker = {
            alias: 'u7dtgDatePicker',
            label: '',
            description: '',
            view: 'datepicker',
            config: {
                format : "YYYY-MM-DD",
                pickTime: false
            }
        };

        var dtgEditor = {
            alias: 'u7dtgRichtexteditor',
            label: '',
            description: '',
            view: 'rte',
            config: {
                editor: {
                    toolbar: ["code", "undo", "formats", "cut", "bold", "redo", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "link", "umbmediapicker", "umbmacro", "table", "umbembeddialog"],
                    stylesheets: [],
                    dimensions: { height: 400 }
                }
            }
        };

        var maxRows = parseInt($scope.model.config.rows.rows) || 0; 0;

        var propertiesEditorswatchers = [];
        var rowObject = {};
        var resetProertiesEditors = function () {
            $scope.contentpickers = {};
            $scope.mediapickers = {};
            $scope.datepickers = {};
            $scope.rtEditors = [];
            rowObject = {};
            $scope.propertiesOrder = [];

            // clean watchers before set again.
            for (var index = 0; index < propertiesEditorswatchers.length; ++index) {
                propertiesEditorswatchers[index]();
            }

            angular.forEach($scope.model.config.columns.columns, function (value, key) {
                rowObject[value.alias] = "";
                $scope.propertiesOrder.push(value.alias);
                var columnKey = key;
                var editorProperyAlias = value.alias;
                if (value.type == "rte") {
                    angular.forEach($scope.model.value, function (row, key) {
                        var rtEditor = angular.copy(dtgEditor);
                        rtEditor.alias = rtEditor.alias + $scope.model.alias + columnKey + key;
                        if (row[editorProperyAlias]) {
                            rtEditor.value = row[editorProperyAlias];
                        } else {
                            rtEditor.value = "";
                        }

                        $scope.rtEditors.push(rtEditor);
                    });
                }
                if (value.type == "mediapicker") {
                    angular.forEach($scope.model.value, function (row, key) {
                        var mediapicker = angular.copy(dtgMediaPicker);
                        mediapicker.alias = mediapicker.alias + columnKey + key;
                        if (row[editorProperyAlias]) {
                            mediapicker.value = row[editorProperyAlias];
                        } else {
                            mediapicker.value = "";
                        }

                        if (value.props.multiple) {
                            mediapicker.config.multiPicker = '1';
                        }

                        $scope.mediapickers["c" + columnKey + "r" + key] = mediapicker;

                        var pickerWatch = $scope.$watch('mediapickers["c' + columnKey + 'r' + key + '"].value', function (newVal, oldVal) {
                            if (newVal || newVal != oldVal) {
                                $scope.model.value[key][editorProperyAlias] = newVal;
                            }
                        });
                        propertiesEditorswatchers.push(pickerWatch)
                    });
                }

                if (value.type == "contentpicker") {
                    angular.forEach($scope.model.value, function (row, key) {
                        var contentpicker = angular.copy(dtgContentPicker);
                        contentpicker.alias = contentpicker.alias + columnKey + key;
                        if (row[editorProperyAlias]) {
                            contentpicker.value = row[editorProperyAlias];
                        } else {
                            contentpicker.value = "";
                        }

                        if (value.props.multiple) {
                            contentpicker.config.multiPicker = '1';
                        }

                        $scope.contentpickers["c" + columnKey + "r" + key] = contentpicker;

                        var pickerWatch = $scope.$watch('contentpickers["c' + columnKey + 'r' + key + '"].value', function (newVal, oldVal) {
                            if (newVal || newVal != oldVal) {
                                $scope.model.value[key][editorProperyAlias] = newVal;
                            }
                        });
                        propertiesEditorswatchers.push(pickerWatch)
                    });
                }

                if (value.type == "datepicker") {
                    angular.forEach($scope.model.value, function (row, key) {
                        var datepicker = angular.copy(dtgDatePicker);
                        datepicker.alias = datepicker.alias + $scope.model.alias + columnKey + key;
                        if (row[editorProperyAlias]) {
                            datepicker.value = row[editorProperyAlias];
                        } else {
                            datepicker.value = "";
                        }

                        if (value.props.time) {
                            datepicker.config.pickTime = true;
                            datepicker.config.format = "YYYY-MM-DD HH:mm:ss"
                        }

                        $scope.datepickers["c" + columnKey + "r" + key] = datepicker;

                        var pickerWatch = $scope.$watch('datepickers["c' + columnKey + 'r' + key + '"].value', function (newVal, oldVal) {
                            if (newVal || newVal != oldVal) {
                                $scope.model.value[key][editorProperyAlias] = newVal;
                            }
                        });
                        propertiesEditorswatchers.push(pickerWatch)
                    });
                }


            });
        }

        resetProertiesEditors();
        
        // Check for deleted columns
        angular.forEach($scope.model.value, function (row, key) {
            angular.forEach(row, function (value, alias) {
                if ($scope.propertiesOrder.indexOf(alias) == -1) {
                    delete row[alias];
                }
            });
        });

        $scope.addRow = function () {
            if (maxRows == 0 || $scope.model.value.length < maxRows) {
                $scope.model.value.push(angular.copy(rowObject));
                var newrowIndex = $scope.model.value.length - 1;
                var newRow = $scope.model.value[newrowIndex];

                angular.forEach($scope.model.config.columns.columns, function (value, key) {
                    var columnKey = key;
                    var editorProperyAlias = value.alias;
                    if (value.type == "rte") {
                        var rtEditor = angular.copy(dtgEditor);
                        rtEditor.alias = rtEditor.alias + $scope.model.alias + columnKey + newrowIndex;
                        rtEditor.value = "";
                        $scope.rtEditors.push(rtEditor);
                    }
                    if (value.type == "mediapicker") {

                            var mediapicker = angular.copy(dtgMediaPicker);
                            mediapicker.alias = mediapicker.alias + columnKey + newrowIndex;
                            mediapicker.value = "";

                            if (value.props.multiple) {
                                mediapicker.config.multiPicker = '1';
                            }

                            $scope.mediapickers["c" + columnKey + "r" + newrowIndex] = mediapicker;

                            var pickerWatch = $scope.$watch('mediapickers["c' + columnKey + 'r' + newrowIndex + '"].value', function (newVal, oldVal) {
                                if (newVal || newVal != oldVal) {
                                    $scope.model.value[newrowIndex][editorProperyAlias] = newVal;
                                }
                            });
                            propertiesEditorswatchers.push(pickerWatch)
                    }

                    if (value.type == "contentpicker") {
                            var contentpicker = angular.copy(dtgContentPicker);
                            contentpicker.alias = contentpicker.alias + columnKey + newrowIndex;
                            contentpicker.value = "";

                            if (value.props.multiple) {
                                contentpicker.config.multiPicker = '1';
                            }

                            $scope.contentpickers["c" + columnKey + "r" + newrowIndex] = contentpicker;

                            var pickerWatch = $scope.$watch('contentpickers["c' + columnKey + 'r' + newrowIndex + '"].value', function (newVal, oldVal) {
                                if (newVal || newVal != oldVal) {
                                    $scope.model.value[newrowIndex][editorProperyAlias] = newVal;
                                }
                            });
                            propertiesEditorswatchers.push(pickerWatch)
                    }

                    if (value.type == "datepicker") {
                            var datepicker = angular.copy(dtgDatePicker);
                            datepicker.alias = datepicker.alias + $scope.model.alias + columnKey + newrowIndex;
                            datepicker.value = "";

                            if (value.props.time) {
                                datepicker.config.pickTime = true;
                                datepicker.config.format = "YYYY-MM-DD HH:mm:ss"
                            }

                            $scope.datepickers["c" + columnKey + "r" + newrowIndex] = datepicker;

                            var pickerWatch = $scope.$watch('datepickers["c' + columnKey + 'r' + newrowIndex + '"].value', function (newVal, oldVal) {
                                if (newVal || newVal != oldVal) {
                                    $scope.model.value[newrowIndex][editorProperyAlias] = newVal;
                                }
                            });
                            propertiesEditorswatchers.push(pickerWatch)
                    }


                });

                //resetProertiesEditors();
            }
            else {
                alert("Max rows is - " + maxRows);
            }
        }

        $scope.removeRow = function (index) {
            $scope.model.value.splice(index, 1);
            //resetProertiesEditors();
        }

        $scope.sortableOptions = {
            axis: 'y',
            cursor: "move",
            handle: ".sortHandle",
            start: function (event, ui) {
                var curTH = ui.helper.closest("table").find("thead").find("tr");
                var itemTds = ui.item.children("td");
                curTH.find("th").each(function (ind, obj) {
                    itemTds.eq(ind).width($(obj).width());
                });
            },
            update: function (ev, ui) {
                //console.log("A");

                //$timeout(function () {
                    //resetProertiesEditors();
                //}, 500);
                //

                $timeout(function () {
                    $scope.rtEditors = [];
                    angular.forEach($scope.model.config.columns.columns, function (value, key) {
                        var columnKey = key;
                        var editorProperyAlias = value.alias;
                        if (value.type == "rte") {
                            angular.forEach($scope.model.value, function (row, key) {
                                var rtEditor = angular.copy(dtgEditor);
                                rtEditor.alias = rtEditor.alias + $scope.model.alias + columnKey + key;
                                if (row[editorProperyAlias]) {
                                    rtEditor.value = row[editorProperyAlias];
                                } else {
                                    rtEditor.value = "";
                                }

                                $scope.rtEditors.push(rtEditor);
                            });
                        }


                        if (value.type == "datepicker") {
                            angular.forEach($scope.model.value, function (row, key) {
                                var datepicker = angular.copy(dtgDatePicker);
                                datepicker.alias = datepicker.alias + $scope.model.alias + columnKey + key;
                                if (row[editorProperyAlias]) {
                                    datepicker.value = row[editorProperyAlias];
                                } else {
                                    datepicker.value = "";
                                }

                                if (value.props.time) {
                                    datepicker.config.pickTime = true;
                                    datepicker.config.format = "YYYY-MM-DD HH:mm:ss"
                                }

                                $scope.datepickers["c" + columnKey + "r" + key] = datepicker;

                                var pickerWatch = $scope.$watch('datepickers["c' + columnKey + 'r' + key + '"].value', function (newVal, oldVal) {
                                    if (newVal || newVal != oldVal) {
                                        $scope.model.value[key][editorProperyAlias] = newVal;
                                    }
                                });
                                propertiesEditorswatchers.push(pickerWatch)
                            });
                        }


                    });
                },0);
                
            }
        };

        //$scope.moveUp = function (index) {
        //    if (index != 0) {
        //        $scope.model.value[index] = $scope.model.value.splice(index - 1, 1, $scope.model.value[index])[0];
        //    }
        //    //resetProertiesEditors();
        //}
        //$scope.moveDown = function (index) {
        //    if (index != $scope.model.value.length - 1) {
        //        $scope.model.value[index] = $scope.model.value.splice(index + 1, 1, $scope.model.value[index])[0];
        //    }
        //    //resetProertiesEditors();
        //}

        $scope.selectedEditorIndex = null;
        $scope.selectedEditorRow = null;
        $scope.selectedEditorProperty = null;
        $scope.selectedEditorTitle = "";

        $scope.editorOpen = function (row, property) {

            var selectedEditorRowIndex = $scope.model.value.indexOf(row);
            var selectedEditorColumnIndex = $scope.propertiesOrder.indexOf(property);
            $scope.selectedEditorTitle = $scope.model.config.columns.columns[selectedEditorColumnIndex].title
            angular.forEach($scope.rtEditors, function (value, key) {
                if (value.alias == 'u7dtgRichtexteditor' + $scope.model.alias + selectedEditorColumnIndex + selectedEditorRowIndex) {
                    $scope.selectedEditorIndex = key;
                }
            });
            $scope.selectedEditorRow = row;
            $scope.selectedEditorProperty = property;
        }

      

        $scope.$on("formSubmitting", function (e, args) {
            resetProertiesEditors();
        });

       
});

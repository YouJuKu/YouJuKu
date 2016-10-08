//var app = angular.module('gridApp', ['ngGrid']);
//app.controller('gridController', function ($scope) {
//    $scope.gridData = [{ name: "Moroni", age: 50 },
//                     { name: "Tiancum", age: 43 },
//                     { name: "Jacob", age: 27 },
//                     { name: "Nephi", age: 29 },
//                     { name: "Enos", age: 34 }];
//    $scope.gridOptions = {
//        data: 'gridData',
//        enableCellSelection: true,
//        enableRowSelection: false,
//        enableCellEdit: true,
//        columnDefs: [{ field: 'name', displayName: 'Name', enableCellEdit: true },
//                     { field: 'age', displayName: 'Age', enableCellEdit: true }]
//    };
//});
var app = angular.module("gridApp", ["ngGrid"]);
app.controller("gridController",
    function($scope, $timeout, StatusesConstant) {
        $scope.statuses = StatusesConstant;
        $scope
            .cellInputEditableTemplate =
            '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
        $scope
            .cellSelectEditableTemplate =
            '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id as name for (id, name) in statuses" ng-blur="updateEntity(row)" />';

        $scope.list = [
            { name: "Fred", age: 45, status: 1 },
            { name: "Julie", age: 29, status: 2 },
            { name: "John", age: 67, status: 1 }
        ];

        $scope.gridOptions = {
            data: "list",
            enableRowSelection: false,
            enableCellEditOnFocus: true,
            multiSelect: false,
            columnDefs: [
                {
                    field: "name",
                    displayName: "Name",
                    enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellInputEditableTemplate
                },
                { field: "age", displayName: "Age", enableCellEdit: false },
                {
                    field: "status",
                    displayName: "Status",
                    enableCellEditOnFocus: true,
                    editableCellTemplate: $scope.cellSelectEditableTemplate,
                    cellFilter: "mapStatus"
                }
            ]
        };

        $scope.updateEntity = function(row) {
            if (!$scope.save) {
                $scope.save = { promise: null, pending: false, row: null };
            }
            $scope.save.row = row.rowIndex;
            if (!$scope.save.pending) {
                $scope.save.pending = true;
                $scope.save.promise = $timeout(function() {
                        // $scope.list[$scope.save.row].$update();
                        console.log("Here you'd save your record to the server, we're updating row: " +
                            $scope.save.row +
                            " to be: " +
                            $scope.list[$scope.save.row].name +
                            "," +
                            $scope.list[$scope.save.row].age +
                            "," +
                            $scope.list[$scope.save.row].status);
                        $scope.save.pending = false;
                    },
                    500);
            }
        };
    })
    .directive("ngBlur",
        function(scope, element, attributes) {
            element.bind("blur",
                function() {
                    scope.$apply(attributes.ngBlur);
                });
        })
    .filter("mapStatus", function (StatusesConstant) {
        return function (input) {
            if (StatusesConstant[input]) {
                return StatusesConstant[input];
            } else {
                return "unknown";
            }
        };
    })
    .factory("StatusesConstant", function () {
        return {
            1: "active",
            2: "inactive"
        };
    });

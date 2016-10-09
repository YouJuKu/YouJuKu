var app = angular.module("gridApp", ["ngGrid"]);
app.controller("gridController",
    function ($scope, $timeout, statuses, studentService) {
        $scope.statuses = statuses;
        $scope
            .cellInputEditableTemplate =
            '<input ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-blur="updateEntity(row)" />';
        $scope
            .cellSelectEditableTemplate =
            '<select ng-class="\'colt\' + col.index" ng-input="COL_FIELD" ng-model="COL_FIELD" ng-options="id as name for (id, name) in statuses" ng-blur="updateEntity(row)" />';

        var service = studentService.getStudents();
        service.then(function(d) {
            $scope.students = studentJson(d.data);
            },
            function(error) {
                console.log(error);
            });

        $scope.gridOptions = {
            data: "students",
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
                {
                    field: "address",
                    displayName: "Address",
                    enableCellEdit: true
                },
                {
                    field: "homephone",
                    displayName: "Home Phone",
                    enableCellEdit: true
                },
                {
                    field: "cellphone",
                    displayName: "Cell Phone",
                    enableCellEdit: true
                },
                {
                    field: "email",
                    displayName: "Email",
                    enableCellEdit: true
                },
                {
                    field: "age",
                    displayName: "Age",
                    enableCellEdit: true
                },
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
.filter("mapStatus", function (statuses) {
    return function (input) {
        if (statuses[input]) {
            return statuses[input];
        } else {
            return "unknown";
        }
    };
})
.factory("statuses", function () {
    return {
        false: "inactive",
        true: "active"
    };
});
app.service("studentService", function ($http) {
    this.getStudents = function () {
        return $http
        ({
            url: "/Students/GetStudents", 
            method: "GET"
        });
    };
});

function studentJson(data) {
    var array = [];
    for (var i = 0; i < data.length; i++) {
        array.push
        ({
            name: data[i].FirstName + " " + data[i].LastName, address: data[i].Address, homephone: data[i].HomePhone,
            cellphone: data[i].CellPhone, email: data[i].Email, age: data[i].Age, status: data[i].IsActive
        });
    }
    return array;
}
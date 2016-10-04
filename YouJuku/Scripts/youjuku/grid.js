var app = angular.module('gridApp', ['ngGrid']);
app.controller('gridController', function ($scope) {
    $scope.gridData = [{ name: "Moroni", age: 50 },
                     { name: "Tiancum", age: 43 },
                     { name: "Jacob", age: 27 },
                     { name: "Nephi", age: 29 },
                     { name: "Enos", age: 34 }];
    $scope.gridOptions = {
        data: 'gridData',
        enableCellSelection: true,
        enableRowSelection: false,
        enableCellEdit: true,
        columnDefs: [{ field: 'name', displayName: 'Name', enableCellEdit: true },
                     { field: 'age', displayName: 'Age', enableCellEdit: true }]
    };
});
'use strict';

(function() {
    var fields = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope) {
        var _this = this;
        this.modal = '';
        this.uploadStep = 1;

        var uploadBtn = document.querySelector('#upload-btn');
        var orderTable = document.querySelector('#order-data');

        function loadCSV(evt) {
            var files = evt.target.files;
            var f = files[0];

            var reader = new FileReader();

            reader.onload = function(file) {
                var csv = file.target.result;
                var rows = csv.split('\r\n');
                var columns = rows[0].split(',');
                var values = rows[1].split(',');
                _this.uploadStep = 2;

                for (var i = 0; i < fields.length; ++i) {
                    var row = orderTable.insertRow();
                    // insert field
                    var cell = row.insertCell();
                    cell.innerHTML = fields[i];

                    // insert corespondent
                    cell = row.insertCell();
                    var idx = columns.indexOf(fields[i]);

                    if (idx != -1) {
                        cell.innerHTML = columns[idx];

                        // insert value
                        cell = row.insertCell();
                        cell.innerHTML = values[idx];
                    } else {
                        cell.innerHTML = "";

                        // insert value
                        cell = row.insertCell();
                        cell.innerHTML = "";
                    }
                }

                $scope.$apply();
            }

            reader.readAsText(f);
        }



        uploadBtn.addEventListener('change', loadCSV);
    }
})();

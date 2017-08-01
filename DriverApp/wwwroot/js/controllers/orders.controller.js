'use strict';

(function() {
    var fields = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, Api) {
        var _this = this;
        var reader = new FileReader();
        var uploadBtn = document.querySelector('#upload-btn');
        var orderTable = document.querySelector('#tbody');

        this.modal = '';
        this.uploadStep = 1;
        this.order = {};



        /**   Methods   **/

        this.openOrderCreation = function() {
            this.modal = 'newOrder';
        }

        this.closeOrderCreation = function() {
            this.modal = '';
            uploadBtn.value = '';
            this.order = {};

            $timeout(function() {
                _this.uploadStep = 1;
                orderTable.innerHTML = '';
            }, 300);
        }

        this.saveOrder = function() {
            Api.sendNewOrder(this.order).then(function(res) {
                console.log(res);
                _this.uploadStep = 3;

                $timeout(function() {
                    _this.closeOrderCreation();
                }, 1500);

            }, function(res) {
                console.log('error', res);
            });
        }

        function loadCSV(evt) {
            var files = evt.target.files;
            var f = files[0];

            reader.readAsText(f);
            _this.uploadStep = 2;
            $scope.$apply();
        }



        /**   Events   **/

        uploadBtn.addEventListener('change', loadCSV);

        reader.onload = function(file) {
            var csv = file.target.result;
            var rows = csv.split('\r\n');
            var columns = rows[0].split(',');
            var values = rows[1].split(',');

            for (var i = 0; i < columns.length; ++i) {
                var row = orderTable.insertRow();

                // insert field
                var cell = row.insertCell();
                cell.innerHTML = columns[i];

                // insert value
                cell = row.insertCell();
                cell.innerHTML = values[i];

                // construct order object
                _this.order[columns[i]] = values[i];
            }
        }

    }
})();

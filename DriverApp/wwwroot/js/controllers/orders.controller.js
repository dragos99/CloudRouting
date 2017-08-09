'use strict';

(function() {
    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, Api, orders, drivers) {
        var _this = this;
        var uploadBtn = document.getElementById('upload-btn');

        this.modal = '';
        this.uploadStep = 1;
        this.orders = orders;
        this.drivers = drivers;
        this.tripDriver = '0';
        this.uploadFields = [];
        this.uploadValues = [];
        this.uploadOrders = [];
        this.selectedOrder = 0;


        /**   Methods   **/

        this.openOrderCreation = function() {
            this.modal = 'newOrder';
            this.order = {};
            this.uploadFields = [];
            this.uploadValues = [];
        }

        this.closeOrderCreation = function() {
            this.modal = '';
            uploadBtn.value = null;

            $timeout(function() {
                _this.uploadStep = 1;
            }, 300);
        }

        this.saveOrder = function() {
            this.uploadOrders = [];
            for (var i = 0; i < this.uploadValues.length; ++i) {
                var order = {};
                for (var j = 0; j < this.uploadValues[i].length; ++j) {
                    order[this.uploadFields[j]] = this.uploadValues[i][j];
                }
                this.uploadOrders.push(order);
            }

            Api.sendNewOrder(this.uploadOrders).then(function(res) {
                if (res.data.error) {
                    console.log(res.data.error);
                } else {
                    _this.uploadStep = 3;

                    $timeout(function() {
                        _this.closeOrderCreation();
                        scroll_down();

                        $timeout(function() {
                            for (var i = _this.uploadOrders.length - 1; i >= 0; --i) {
                                _this.uploadOrders[i].id = res.data.id--;
                                _this.orders.push(_this.uploadOrders[i]);
                            }
                        }, 500);
                    }, 1500);
                }
            });
        }

        $scope.loadCSV = function(evt) {
            var reader = new FileReader();
            var files = evt.target.files;
            var f = files[0];

            reader.onload = function(file) {
                var csv = file.target.result;
                var rows = csv.split('\r\n');
                _this.uploadFields = rows[0].split(',');
                rows.splice(0, 1);

                for (var i = 0; i < rows.length; ++i) {
                    _this.uploadValues.push(rows[i].split(','));
                }

                _this.selectedOrder = 0;

                _this.uploadStep = 2;
                $scope.$apply();
            }

            reader.readAsText(f);
        }


        this.generateTrip = function() {
            Api.triggerRouting(this.tripDriver).then(function(res) {
                console.log(res);
                _this.modal = '';
            });
        }

    }


    app.directive('loadCsv', function($timeout) {
        return {
            link: function(scope, elem, attrs) {
                elem.on('change', function(e) {
                    $timeout(function() {
                        scope.loadCSV(e);
                    });
                });
            }
        };
    });

})();



function scroll_down() {
    var el = jq("#orders-container");
    el.animate({scrollTop: el[0].scrollHeight}, 600);
}

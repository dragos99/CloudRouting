'use strict';

(function() {
    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, Api, orders, drivers) {
        var _this = this;
        var uploadBtn = document.getElementById('upload-btn');

        this.modal = '';
        this.uploadStep = 1;
        this.uploadOrders = [];
        this.orders = orders;
        this.drivers = drivers;
        this.tripDriver = '0';
        _this.uploadFields = [];
        _this.uploadValues = [];
        _this.selectedOrder = 0;


        /**   Methods   **/

        this.openOrderCreation = function() {
            this.modal = 'newOrder';
            _this.order = {};
            this.uploadOrders = [];
            _this.uploadFields = [];
            _this.uploadValues = [];
        }

        this.closeOrderCreation = function() {
            this.modal = '';
            uploadBtn.value = null;

            $timeout(function() {
                _this.uploadStep = 1;
            }, 300);
        }

        this.saveOrder = function() {
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
                    var order = {};
                    for (var j = 0; j < _this.uploadValues[i].length; ++j) {
                        order[_this.uploadFields[j]] = _this.uploadValues[i][j];
                    }
                    _this.uploadOrders.push(order);
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

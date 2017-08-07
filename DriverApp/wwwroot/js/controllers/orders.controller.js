'use strict';

(function() {
    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, Api, orders, drivers) {
        var _this = this;
        var uploadBtn = document.getElementById('upload-btn');
        _this.orderBody = document.getElementById('order-body');

        this.modal = '';
        this.uploadStep = 1;
        this.order = {};
        this.orders = orders;
        this.drivers = drivers;
        this.tripDriver = '0';


        /**   Methods   **/

        this.openOrderCreation = function() {
            this.modal = 'newOrder';
            _this.order = {};
        }

        this.closeOrderCreation = function() {
            this.modal = '';
            uploadBtn.value = null;

            $timeout(function() {
                _this.uploadStep = 1;
                _this.orderBody.innerHTML = '';
            }, 300);
        }

        this.saveOrder = function() {
            Api.sendNewOrder(this.order).then(function(res) {
                if (res.data.error) {
                    console.log(res.data.error);
                } else {
                    _this.uploadStep = 3;
                    _this.order.id = res.data.id;

                    $timeout(function() {
                        _this.closeOrderCreation();
                        anim();
                        $timeout(function() {
                            _this.orders.push(_this.order);
                        }, 1000);
                    }, 1500);
                }
            });
        }

        $scope.loadCSV = function(evt) {
            var reader = new FileReader();
            var files = evt.target.files;
            var f = files[0];
            var html = '';

            reader.onload = function(file) {
                var csv = file.target.result;
                var rows = csv.split('\r\n');
                var columns = rows[0].split(',');
                var values = rows[1].split(',');

                for (var i = 0; i < columns.length; ++i) {
                    html += '<tr><td>'+ columns[i] +'</td><td>'+ values[i] +'</td></tr>'
                    _this.order[columns[i]] = values[i];
                }

                _this.uploadStep = 2;
                $scope.$apply();
                _this.orderTable = document.getElementById('order-body');
                _this.orderTable.insertAdjacentHTML('beforeend', html);
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



function anim () {
    var el = jq("#orders-container");
    el.animate({scrollTop: el[0].scrollHeight}, 1000);
}

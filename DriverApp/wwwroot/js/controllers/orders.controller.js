'use strict';

(function() {
    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, Api, orders, $route) {
        var _this = this;
        var uploadBtn = document.querySelector('#upload-btn');
        var orderTable = document.querySelector('#tbody');

        this.modal = '';
        this.uploadStep = 1;
        this.order = {};
        this.orders = orders;
        

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
                orderTable.innerHTML = '';
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

            reader.onload = function(file) {
                var csv = file.target.result;
                var rows = csv.split('\r\n');
                var columns = rows[0].split(',');
                var values = rows[1].split(',');
                var html = '';

                for (var i = 0; i < columns.length; ++i) {
                    // construct table html
                    html += '<tr><td>'+ columns[i] +'</td><td>'+ values[i] +'</td></tr>'

                    // construct order object
                    _this.order[columns[i]] = values[i];
                }

                orderTable.insertAdjacentHTML('beforeend', html);
            }

            reader.readAsText(f);
            _this.uploadStep = 2;
            $scope.$apply();
        }



        /**   Events   **/

        //uploadBtn.onchange = this.loadCSV;
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

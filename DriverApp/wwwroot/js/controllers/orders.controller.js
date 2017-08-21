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
        this.assignStep = 1;
        this.orderSelection = true;
        this.mapShapes = [];

        var icon = {
            url: "img/marker-red.png", // url
            scaledSize: new google.maps.Size(24, 38), // scaled size
        }

        var selectedIcon = {
            url: "img/marker.green.png", // url
            scaledSize: new google.maps.Size(24, 38), // scaled size
        };


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


        this.openAssignModal = function() {
            this.modal = 'assignOrders';
            this.assignStep = 1;
            this.orderSelection = true;

            $timeout(function() {
                _this.initMap();
            }, 300);

        }

        this.retrySelection = function() {
            this.orders.forEach(function(order) {
                order.marker.setIcon(icon);
            });

            this.mapShapes.forEach(function(shape) {
                shape.setMap(null);
            });

            this.mapShapes = [];
            this.orderSelection = true;
        }


        this.initMap = function() {
        	var bucharest = {lat: 44.431430, lng: 26.104249};
        	this.map = new google.maps.Map(document.getElementById('assignOrdersMap'), {
        		zoom: 11,
        		center: bucharest
        	});

            this.orders.forEach(function(order) {
                var marker = new google.maps.Marker({
            		position: {lat: order.givenX, lng: order.givenY},
            		map: _this.map,
                    title: 'Order ' + order.id,
                    icon: icon
            	});

                order.marker = marker;
            });

            this.drawingManager = new google.maps.drawing.DrawingManager({
                drawingMode: google.maps.drawing.OverlayType.POLYGON,
                drawingControl: true,
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: ['polygon']
                },
                polygonOptions: {
                    strokeColor: '#147fbf',
                    fillColor: '#29b6ec'
                }
            });

            this.drawingManager.setMap(this.map);

            google.maps.event.addListener(this.drawingManager, 'overlaycomplete', function(polygon) {
                _this.orders.forEach(function(order) {
                    var coords = new google.maps.LatLng(order.givenX, order.givenY);
                    var inside = google.maps.geometry.poly.containsLocation(coords, polygon.overlay);
                    if (inside) {
                        order.marker.setIcon(selectedIcon);
                    }
                });

                _this.orderSelection = false;
                $scope.$apply();

                _this.mapShapes.push(polygon.overlay);
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

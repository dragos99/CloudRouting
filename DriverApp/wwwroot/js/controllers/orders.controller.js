'use strict';

(function() {
    var app = angular.module('app');
    app.controller('OrdersCtrl', OrdersController);

    function OrdersController($scope, $timeout, $route, Api, orders, drivers) {
        var _this = this;
        var uploadBtn = document.getElementById('upload-btn');

        this.modal = '';
        this.uploadStep = 1;
        this.orders = orders;
        this.drivers = drivers;
        this.uploadFields = [];
        this.uploadValues = [];
        this.uploadOrders = [];
        this.selectedOrder = 0;
        this.assignStep = 1;
        this.orderSelection = true;
        this.mapShapes = [];
        this.ordersSelected = 0;
        this.selectedDriver = '0';

        var icon = {
            url: "img/marker-red.png", // url
            scaledSize: new google.maps.Size(22, 43), // scaled size
        }

        var selectedIcon = {
            url: "img/marker-green.png", // url
            scaledSize: new google.maps.Size(22, 43), // scaled size
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
                        $route.reload();
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
            resetAssignModal();

            $timeout(function() {
                _this.initMap();
            }, 300);
        }

        this.closeAssignModal = function() {
            this.modal = '';
            this.selectedDriver = '0';
            resetAssignModal();
        }

        this.resetSelection = function() {
            this.orders.forEach(function(order) {
                if (order.marker) order.marker.setIcon(icon);
                order.selected = false;
            });

            this.mapShapes.forEach(function(shape) {
                shape.setMap(null);
            });

            this.mapShapes = [];
            this.orderSelection = true;
            this.ordersSelected = 0;
        }

        this.assignOrdersToDriver = function() {
            var data = [];
            this.orders.forEach(function(order) {
                if (order.selected) data.push(order.id);
            });
            Api.assignOrders(data, this.selectedDriver).then(function(res) {
                if (res.data === 'ok') {
					_this.assignStep = 3;
					$timeout(function() {
						_this.closeAssignModal();
						$timeout(function() {
							$route.reload();
						}, 50);
						
					}, 1500);
                }
            });
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
                        order.selected = true;
                        _this.ordersSelected++;
                    }
                });

                _this.orderSelection = false;
                $scope.$apply();

                _this.mapShapes.push(polygon.overlay);
            });
        }



        function resetAssignModal() {
            _this.assignStep = 1;
            _this.selectedDriver = '0';
            _this.resetSelection();
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

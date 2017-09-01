'use strict';

(function() {
    var app = angular.module('app');
    app.controller('TripsCtrl', TripsController);

    function TripsController($timeout) {
        var _this = this;
        this.modal = '';


        this.viewTrip = function() {
            this.modal = 'viewTrip';
            $timeout(initMap, 300);
        }

        this.closeViewTripModal = function() {
            this.modal = '';
        }

        function initMap() {
            var directionsService = new google.maps.DirectionsService;
            var directionsDisplay = new google.maps.DirectionsRenderer;
            var map = new google.maps.Map(document.getElementById('tripMap'), {
                zoom: 6,
                center: {lat: 41.85, lng: -87.65}
            });

            directionsDisplay.setMap(map);

            calculateAndDisplayRoute(directionsService, directionsDisplay);
        }

        function calculateAndDisplayRoute(directionsService, directionsDisplay) {
            var waypts = [];
            waypts.push({
                location: {lat: 45.443752, lng: -75.728965},
                stopover: true
            });

            directionsService.route({
                origin: 'montreal, quebec',
                destination: 'toronto, ont',
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: 'DRIVING'
            }, function (response, status) {
                if (status === 'OK') {
                    directionsDisplay.setDirections(response);
                } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }
    }

}());

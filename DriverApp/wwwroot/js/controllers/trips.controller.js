'use strict';

(function() {
    var app = angular.module('app');
    app.controller('TripsCtrl', TripsController);

    function TripsController($timeout, trips, Api) {
        var _this = this;
        this.modal = '';
        this.trips = trips;

        this.viewTrip = function(idx) {
            Api.getTripOrders(this.trips[idx].id).then(function(res) {
                _this.modal = 'viewTrip';
                $timeout(function() {
                    initMap(res.data);
                }, 300);
            });
        }

        this.closeViewTripModal = function() {
            this.modal = '';
        }

        function initMap(stops) {
            console.log(stops);
            var directionsService = new google.maps.DirectionsService;
            var directionsDisplay = new google.maps.DirectionsRenderer;
            var map = new google.maps.Map(document.getElementById('tripMap'), {
                zoom: 6,
                center: {lat: 41.85, lng: -87.65}
            });

            directionsDisplay.setMap(map);

            calculateAndDisplayRoute(directionsService, directionsDisplay, stops);
        }

        function calculateAndDisplayRoute(directionsService, directionsDisplay, stops) {
            var waypts = [];
            var lastStop = 0, destination;

            stops.forEach(function(stop) {
                if (stop.stopSequence > lastStop) {
                    lastStop = stop.stopSequence;
                    destination = {lat: stop.givenX, lng: stop.givenY};
                }
            });


            stops.forEach(function(stop) {
                if (stop.stopSequence == lastStop) return;
                waypts.push({
                    location: {lat: stop.givenX, lng: stop.givenY},
                    stopover: true
                });
            });


            directionsService.route({
                origin: 'ortec cee, bucharest, romania',
                destination: destination,
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

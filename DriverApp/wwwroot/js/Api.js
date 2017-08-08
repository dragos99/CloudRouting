'use strict';

(function () {
	var app = angular.module('app');
	app.service('Api', function ($http) {

		this.checkSession = function () {
			return $http.get('/api/manager');
		}

		this.authenticate = function (key) {
			return $http.post('/api/login/manager', { customerKey: key });
		}

		this.sendNewOrder = function(orders) {
			return $http.post('/api/manager/newOrders', {orders});
		}

		this.getOrders = function() {
			return $http.get('/api/routing/orders').then(function(res) {
				return res.data;
			});
		}

		this.getDrivers = function() {
			return $http.get('/api/manager/drivers').then(function(res) {
				return res.data;
			});
		}

		this.triggerRouting = function(driverId) {
			return $http.post('/api/routing/trigger', {driverId});
		}

	});
})();

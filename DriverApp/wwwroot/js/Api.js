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

		this.sendNewOrder = function(order) {
			return $http.post('/api/manager/newOrder', order);
		}

		this.getOrders = function() {
			return $http.get('/api/orders').then(function(res) {
				return res.data;
			});
		}

	});
})();

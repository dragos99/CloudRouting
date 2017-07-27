'use strict';

(function () {
	var app = angular.module('app');
	app.service('Api', function ($http) {

		this.authenticate = function (key) {
			return $http.post('/api/login/manager', key);
		}

	});
})();
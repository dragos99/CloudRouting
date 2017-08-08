'use strict';

(function () {
	var app = angular.module("app");
	app.service("Auth", function (Api, $location, $route) {
		var _this = this;
		this.key = "";
		this.authenticated = false;
		this.user = null;

		this.authenticate = function () {
			Api.authenticate(this.key).then(function(res) {
				_this.auth(true);
			}, function(res) {
				console.log('error', res.statusText);
			});
		}

		this.auth = function(value) {
			if (value) {
				if ($location.path() == '/orders' || $location.path() == '/trips') {
					$route.reload();
				} else {
					$location.path('/orders');
				}
			} else {
				$location.path('/');
			}
			this.authenticated = value;
		}
	});
})();

'use strict';

(function () {
	var app = angular.module("app");
	app.service("Auth", function (Api, $location) {
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
				$location.path('/orders');
			} else {
				$location.path('/');
			}
			this.authenticated = value;
		}
	});
})();

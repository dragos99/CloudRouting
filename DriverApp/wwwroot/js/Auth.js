'use strict';

(function () {
	var app = angular.module("app");
	app.service("Auth", function (Api) {
		this.authenticated = false;

		this.authenticate = function (key) {
			Api.authenticate(key).then(function () {
				
			}, function () {

			});
		}
	});
})();
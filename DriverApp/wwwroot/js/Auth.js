'use strict';

(function () {
	var app = angular.module("app");
	app.service("Auth", function (Api) {
		var _this = this;
		this.key = "";
		this.authenticated = false;
		this.user = null;

		this.authenticate = function () {
			console.log(this.key);
			Api.authenticate(this.key).then(function (res) {
				_this.authenticated = true;
			}, function (res) {
				console.log("error", res.statusText);
			});
		}
	});
})();
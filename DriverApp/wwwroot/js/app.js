(function () {

	var app = angular.module("app", ["ngRoute", "ngAnimate"]);

	app.config(function ($httpProvider, $routeProvider, $locationProvider) {
		$httpProvider.defaults.withCredentials = true;

		$locationProvider.html5Mode(true);
		$routeProvider
			.when("/", {
				templateUrl: "views/orders.html"
			})
			.when("/drivers", {
				templateUrl: "views/drivers.html"
			})
			.when("/orders", {
				templateUrl: "views/orders.html"
			})
			.when("/trips", {
				templateUrl: "views/trips.html"
			});
	});

	app.run(function ($rootScope, Api, Auth) {
		$rootScope.Auth = Auth;

		Api.checkSession().then(function () {
			Auth.authenticated = true;
		});
	});

})();

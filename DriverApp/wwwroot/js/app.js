(function () {

	var app = angular.module("app", ["ngRoute", "ngAnimate"]);

	app.config(function ($httpProvider, $routeProvider, $locationProvider) {
		$httpProvider.defaults.withCredentials = true;

		$locationProvider.html5Mode(true);
		$routeProvider
			.when("/", {
				templateUrl: "views/orders.html",
				ordersList: function(Api) {
					return Api.getOrders();
				}
			})
			.when("/drivers", {
				templateUrl: "views/drivers.html"
			})
			.when("/orders", {
				templateUrl: "views/orders.html",
				controller: 'OrdersCtrl',
				controllerAs: 'vm',
				resolve: {
					orders: function(Api) {
						return Api.getOrders();
					}
				}
			})
			.when("/trips", {
				templateUrl: "views/trips.html"
			});
	});

	app.run(function ($rootScope, $location, Api, Auth) {
		$rootScope.Auth = Auth;

		Api.checkSession().then(function () {
			console.log('Session available');
			$location.path = '/orders';
			Auth.authenticated = true;
		}, function() {
			console.log('Not authenticated');
		});
	});

})();

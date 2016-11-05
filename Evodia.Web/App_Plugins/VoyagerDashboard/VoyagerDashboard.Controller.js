angular.module("umbraco").controller("VoyagerDashboardController", function($scope, $routeParams, voyagerFactory) {

    $scope.Sync = function($event) {
        voyagerFactory.sync();

        $event.preventDefault();
    };

});

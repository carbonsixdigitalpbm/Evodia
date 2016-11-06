angular.module("umbraco").controller("VoyagerDashboardController", function($scope, $routeParams, voyagerFactory) {
    $scope.Statistics = voyagerFactory.getStatistics();

    $scope.Sync = function($event) {
        voyagerFactory.clearStatistic();
        voyagerFactory.sync();

        $event.preventDefault();
    };

});

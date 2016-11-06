angular.module("umbraco").controller("VoyagerDashboardController", function($scope, $routeParams, voyagerFactory) {
    voyagerFactory.clearStatistic();
    $scope.Statistics = voyagerFactory.getStatistics();

    $scope.Sync = function($event) {
        voyagerFactory.clearStatistic();
        voyagerFactory.sync();

        $event.preventDefault();
    };

});

angular.module("umbraco.resources").service("voyagerFactory", function($http, notificationsService) {
    var url = "/umbraco/surface/voyager/";
    var voyagerStatistics = [];

    var successCallBack = function(data) {
        if (data.status === "OK") {
            angular.copy(data.data, voyagerStatistics);

            notificationsService.success("Success", "Voyager listing has been updated.");
        } else {
            notificationsService.error("Error", data.status);
        }
    };

    var errorCallback = function(data) {
        notificationsService.error("Error", data.status);
    };

    this.sync = function() {
        $http.post(url + "sync/").success(successCallBack).error(errorCallback);
    };

    this.clearStatistic = function() {
        voyagerStatistics.length = 0;
    };

    this.getStatistics = function() {
        return voyagerStatistics;
    };
});

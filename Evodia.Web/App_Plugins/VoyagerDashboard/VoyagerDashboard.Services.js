angular.module("umbraco.resources").service("voyagerFactory", function($http, notificationsService) {
    var url = "/umbraco/surface/voyager/";
    var voyagerResult = [];

    var successCallBack = function(data) {
        if (data.status === "OK") {
            angular.copy(data.data, voyagerResult);

            console.log(data.data);
        } else {
            notificationsService.error("Error", data.status);
        }
    };

    var errorCallback = function(data) {
        notificationsService.error("Error", data.status);
    };

    this.sync = function () {
        $http.post(url + "sync/").success(successCallBack).error(errorCallback);
    };

});

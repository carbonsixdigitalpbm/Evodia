var Voyager = (function() {
    "use strict";

    var $jobsTarget,
        $keywords = $(".js-keywords"),
        $keywordsOnly = $(".js-keywords-only");

    var settings = {
        pageSize: 20,
        pageNumber: 0,
        controllerUrl: ""
    };

    var busyLoading = false;

    //var _bindUIActions = function () {

    //    $selects.on('change', function () {
    //        $eventsTarget.html("");
    //        settings.pageNumber = 0;
    //        _loadMore();
    //    });

    //    $loadMoreButton.on('click', function () {
    //        _loadMore();
    //    });
    //};

    var _getJobs = function(keywords, keywordsOnly) {
        console.log("Title: " + keywords);
        console.log("Title: " + keywordsOnly);

        $.ajax({
            type: "POST",
            url: settings.controllerUrl,
            data: "offset=" + settings.pageNumber + "&size=" + settings.pageSize + "&keywords=" + keywords + "&titleOnly=" + keywordsOnly,
            cache: false,
            success: function (result) {
                var $moreBlocks = $(result);

                $jobsTarget.append($moreBlocks);
            },
            complete: function () {
                busyloading = false;
            },
            error: function (result) {
                console.log('Jobs failed to load: ');
                console.log(result);
            }
        });
        settings.pageNumber++;
    }

    var _init = function(jobControllerUrl, target) {
        settings.controllerUrl = jobControllerUrl;
        $jobsTarget = $(target);

        _getJobs($keywords.val(), $keywordsOnly.prop("checked"));
    }

    return {
        init: _init,
        getJobs: _getJobs
    }

})();
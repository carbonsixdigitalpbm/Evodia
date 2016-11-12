var Voyager = (function() {
    "use strict";

    var $jobsTarget,
        $keywords = $(".js-keywords"),
        $keywordsOnly = $(".js-keywords-only"),
        $jobTypeCheckboxes = $(".js-jobtype"),
        $searchButton = $(".js-search");

    var settings = {
        pageSize: 20,
        pageNumber: 0,
        controllerUrl: "",
        jobTypes: []
    };

    var busyLoading = false;

    var _loadJobs = function () {
        busyLoading = true;
        $jobsTarget.html("");
        settings.pageNumber = 0;

        _getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes);
    };

    var _bindUIActions = function () {

        $searchButton.click(function (e) {
            _loadJobs();

            e.preventDefault();
        });

        $jobTypeCheckboxes.click(function () {
            settings.jobTypes = [];

            $(".js-jobtype:checkbox:checked").each(function (i) {
                settings.jobTypes[i] = $(this).attr("id");
            });

            _loadJobs();
        });
    };

    var _getJobs = function(keywords, keywordsOnly, jobTypes) {
        console.log("Keywords: " + keywords);
        console.log("Keywords only: " + keywordsOnly);
        console.log("Job types: " + jobTypes);
        $.ajax({
            type: "POST",
            url: settings.controllerUrl,
            data: "offset=" + settings.pageNumber +
                    "&size=" + settings.pageSize +
                    "&keywords=" + keywords +
                    "&titleOnly=" + keywordsOnly +
                    "&type=" + settings.jobTypes,
            cache: false,
            success: function (result) {
                var $moreBlocks = $(result);

                $jobsTarget.append($moreBlocks);
            },
            complete: function () {
                busyLoading = false;
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

        _bindUIActions();
        _getJobs($keywords.val(), $keywordsOnly.prop("checked"));
    }

    return {
        init: _init,
        getJobs: _getJobs
    }

})();
var Voyager = (function() {
    "use strict";

    var $jobsTarget,
        $navTarget,
        $countTarget = $(".js-count"),
        $keywords = $(".js-keywords"),
        $keywordsOnly = $(".js-keywords-only"),
        $jobTypeCheckboxes = $(".js-jobtype"),
        $filterSelects = $(".js-filter-select"),
        $locationSelect = $(".js-location"),
        $salarySelect = $(".js-salary"),
        $sectorTypeCheckboxes = $(".js-sector"),
        $searchButton = $(".js-search");

    var settings = {
        pageSize: 5,
        pageNumber: 0,
        controllerUrl: "",
        jobTypes: [],
        sectors: []
    };

    var busyLoading;

    var _loadJobs = function () {
        busyLoading = true;
        $jobsTarget.html("");
        settings.pageNumber = 0;

        _getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
    };

    var _bindUIActions = function () {

        $filterSelects.on('change', function () {
            _loadJobs();
        });

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

        $sectorTypeCheckboxes.click(function () {
            settings.sectors = [];

            $(".js-sector:checkbox:checked").each(function (i) {
                settings.sectors[i] = $(this).attr("id");
            });

            _loadJobs();
        });
    };

    var _getJobs = function (keywords, keywordsOnly, jobTypes, location, sectors, salary) {
        console.log("#####################");
        console.log("## SEARCH SETTINGS ##");
        console.log("#####################");
        console.log("Keywords: " + keywords);
        console.log("Keywords only: " + keywordsOnly);
        console.log("Job types: " + jobTypes);
        console.log("Location: " + location);
        console.log("Sectors: " + sectors);
        console.log("Salary: " + salary);
        console.log("");
        console.log("");

        $.ajax({
            type: "POST",
            url: settings.controllerUrl,
            dataType: "json",
            data: "offset=" + settings.pageNumber +
                    "&size=" + settings.pageSize +
                    "&keywords=" + keywords +
                    "&titleOnly=" + keywordsOnly +
                    "&type=" + settings.jobTypes +
                    "&location=" + location + 
                    "&sector=" + sectors +
                    "&salary=" + salary,
            cache: false,
            success: function (result) {
                console.log(result.jobs);
                console.log(result.navigation);
                console.log(result.count);

                var $jobs = $(result.jobs),
                    $nav = $(result.navigation);

                $jobsTarget.append($jobs);
                $navTarget.html($nav);
                var label = result.count === 1 ? " job" : " jobs";

                $countTarget.html(result.count + label);
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

    var _init = function(jobControllerUrl, jobTarget, navTarget) {
        settings.controllerUrl = jobControllerUrl;
        $jobsTarget = $(jobTarget);
        $navTarget = $(navTarget);

        _bindUIActions();
        _getJobs($keywords.val(), $keywordsOnly.prop("checked"), "", "", "", "");
    }

    return {
        init: _init,
        getJobs: _getJobs
    }

})();
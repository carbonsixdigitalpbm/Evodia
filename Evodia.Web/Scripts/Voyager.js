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
        $searchButton = $(".js-search"),
        $pagingButtons = $(".js-page");

    var settings = {
        pageSize: 5,
        pageNumber: 0,
        controllerUrl: "",
        jobTypes: [],
        sectors: []
    };

    var busyLoading;

    var _loadJobs = function() {
        busyLoading = true;
        $jobsTarget.html("");

        _getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
    };

    var _getPrevalues = function() {
        settings.jobTypes = [];

        $(".js-jobtype:checkbox:checked").each(function(i) {
            settings.jobTypes[i] = $(this).attr("id");
        });

        settings.sectors = [];

        $(".js-sector:checkbox:checked").each(function(i) {
            settings.sectors[i] = $(this).attr("id");
        });
    };

    var _resetPageNumber = function() {
        settings.pageNumber = 0;
    };

    var _bindUIActions = function() {

        $filterSelects.on('change', function() {
            _resetPageNumber();
            _loadJobs();
        });

        $searchButton.click(function(e) {
            _resetPageNumber();
            _loadJobs();

            e.preventDefault();
        });

        $jobTypeCheckboxes.click(function() {
            _resetPageNumber();
            _getPrevalues();
            _loadJobs();
        });

        $sectorTypeCheckboxes.click(function() {
            _resetPageNumber();
            _getPrevalues();
            _loadJobs();
        });

        $navTarget.on("click", ".js-page", function(e) {
            var pageNumber = $(this).data("page");

            settings.pageNumber = pageNumber;
            _loadJobs();
            e.preventDefault();
        });
    };

    var _getJobs = function(keywords, keywordsOnly, jobTypes, location, sectors, salary) {
        console.log("#####################");
        console.log("## SEARCH SETTINGS ##");
        console.log("#####################");
        console.log("Keywords: " + keywords);
        console.log("Keywords only: " + keywordsOnly);
        console.log("Job types: " + jobTypes);
        console.log("Location: " + location);
        console.log("Sectors: " + sectors);
        console.log("Salary: " + salary);
        console.log("Page: " + settings.pageNumber);
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
            success: function(result) {
                var $jobs = $(result.jobs),
                    $nav = $(result.navigation);

                $jobsTarget.append($jobs);
                $navTarget.html($nav);
                var label = result.count === 1 ? " job" : " jobs";

                $countTarget.html(result.count + label);
            },
            complete: function() {
                busyLoading = false;
            },
            error: function(result) {
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

        _getPrevalues();
        _bindUIActions();
        _getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
    }

    return {
        init: _init,
        getJobs: _getJobs
    }

})();

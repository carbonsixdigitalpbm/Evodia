var Voyager = (function($) {
    "use strict";

    var $jobsTarget = $("#jobs-target"),
        $navTarget = $("#pagination-target"),
        $countTarget = $(".js-count"),
        $keywords = $(".js-keywords"),
        $keywordsOnly = $(".js-keywords-only"),
        $jobTypeCheckboxes = $(".js-jobtype"),
        $filterSelects = $(".js-filter-select"),
        $locationSelect = $(".js-location"),
        $salarySelect = $(".js-salary"),
        $sectorTypeCheckboxes = $(".js-sector"),
        $searchButton = $(".js-search"),
        $page = $('html');

    var settings = {
        pageSize: 5,
        pageNumber: 0,
        controllerUrl: "",
        jobTypes: [],
        sectors: []
    };

    var busyLoading;

    var _loadJobs = function() {
        if (!busyLoading) {
            busyLoading = true;

            _toggleLoadingClass();
            //_logRequestParemeters($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
            _getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
        }
    };

    var _toggleLoadingClass = function() {
        $page.toggleClass("is-loading");
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

    var _pushHistoryState = function() {
        var queryString = {},
            item;

        var queryStringParams = {
            page: settings.pageNumber,
            titleonly: $keywordsOnly.prop("checked"),
            type: settings.jobTypes.join(),
            location: $locationSelect.val(),
            sector: settings.sectors.join(),
            salary: $salarySelect.val()
        };

        for (item in queryStringParams) {
            if (queryStringParams.hasOwnProperty(item)) {
                var isNotZero = queryStringParams[item] !== 0;
                var isNotEmptyString = queryStringParams[item] !== "";
                var isNotFalse = queryStringParams[item] !== false;

                if (isNotZero && isNotEmptyString && isNotFalse) {
                    queryString[item] = queryStringParams[item];
                }
            }
        }

        var link = $.param(queryString) !== "" ? "?" + $.param(queryString) : "/jobs/";

        history.pushState(null, "Jobs | Evodia", link);
    };

    var _animateBackToTop = function() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);

    };

    var _bindUIActions = function(isLarge) {

        $filterSelects.on('change', function() {
            _resetPageNumber();
            if (isLarge) {
                _loadJobs();
            }
        });

        $searchButton.click(function(e) {
            _resetPageNumber();
            _loadJobs();
            if (!isLarge) {
                $.magnificPopup.instance.close();
                _animateBackToTop();
            }

            e.preventDefault();
        });

        $jobTypeCheckboxes.click(function() {
            _resetPageNumber();
            _getPrevalues();
            if (isLarge) {
                _loadJobs();
            }
        });

        $sectorTypeCheckboxes.click(function() {
            _resetPageNumber();
            _getPrevalues();
            if (isLarge) {
                _loadJobs();
            }
        });

        $navTarget.on("click", ".js-page", function(e) {
            var pageNumber = $(this).data("page");

            settings.pageNumber = pageNumber;
            _loadJobs();
            _animateBackToTop();
            e.preventDefault();
        });
    };

    var _logRequestParemeters = function(keywords, keywordsOnly, jobTypes, location, sectors, salary) {
        console.log("## Search settings ##");
        console.log("Keywords: " + keywords + ", keywords only: " + keywordsOnly);
        console.log("Job types: " + jobTypes);
        console.log("Location: " + location);
        console.log("Sectors: " + sectors);
        console.log("Salary: " + salary);
        console.log("Page: " + settings.pageNumber);
        console.log("## End of settings ##");
    };

    var _getJobs = function(keywords, keywordsOnly, jobTypes, location, sectors, salary) {

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
                    $nav = $(result.navigation),
                    label = result.count === 1 ? " job" : " jobs";

                $jobsTarget.html($jobs);
                $navTarget.html($nav);
                $countTarget.html(result.count + label);
            },
            complete: function() {
                busyLoading = false;
                _pushHistoryState();
                _toggleLoadingClass();
            },
            error: function(result) {
                console.log('Jobs failed to load: ');
                console.log(result);
            }
        });
    };

    var breakpoints = [{
        context: ['small-max', 'small', 'medium'],
        call_for_each_context: false,
        match: function() {
            _bindUIActions(false);
        },
    }, {
        context: ['large', 'x-large', 'xx-large'],
        call_for_each_context: false,
        match: function() {
            _bindUIActions(true);
        }
    }];

    var _init = function() {
        settings.controllerUrl = "/umbraco/Surface/JobsSurface/GetFilteredJobs";
        _getPrevalues();
        MQ.init(breakpoints);
        //_getJobs($keywords.val(), $keywordsOnly.prop("checked"), settings.jobTypes, $locationSelect.val(), settings.sectors, $salarySelect.val());
    };

    return {
        init: _init,
        getJobs: _getJobs
    };

})(jQuery);

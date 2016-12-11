var CompactHeader = (function ($) {
	'use strict';

	var $page = $('.o-page'),
		$header = $('.js-header'),
		headerHeight = $header.height();

	//set scrolling variables
	var scrolling = false,
		previousTop = 0,
		currentTop = 0,
		scrollDelta = 10,
		scrollOffset = 50;

	var hasCover = $('.c-hero,.c-cover').length > 0 ? true : false;

	var _setElOffsets = function() {
		if( !hasCover ) {
			$page.css('paddingTop', headerHeight );
		}
	};

	var _checkSimpleNavigation = function(currentTop) {
		//there's no secondary nav or secondary nav is below primary nav

	    if( previousTop - currentTop && currentTop < scrollOffset ) {
	    	//if scrolling up...
	    	$header.removeClass('c-header--compact');
	    } else if( currentTop - previousTop > scrollDelta && currentTop > scrollOffset ) {
	    	//if scrolling down...
	    	$header.addClass('c-header--compact');
	    }
	};

	var _autoHideHeader = function() {
		var currentTop = $(window).scrollTop();

		_checkSimpleNavigation(currentTop);

	   	previousTop = currentTop;
		scrolling = false;
	};


	var _init = function( isCompact ) {

		$header.addClass('is-fixed');
		_setElOffsets();
		if( hasCover ) {
			$header.addClass('is-transparent');
		}

		if( isCompact ) {

			$(window).on('scroll', function(){

				if( !scrolling ) {
					scrolling = true;
					if( !window.requestAnimationFrame ) {
						setTimeout(_autoHideHeader, 250);
					} else {
						requestAnimationFrame(_autoHideHeader);
					}
				}
			});

			$(window).on('resize', function(){
				headerHeight = $header.height();
				_setElOffsets();
			});

		}

	};

	return {
		init: _init
	};

})(jQuery);

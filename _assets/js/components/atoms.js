var Atoms = (function ($) {
	'use strict';

	var s = new Snap('#atomsSvg'),
	    path = s.select('.js-atoms-line1'),
	    star = s.select('.js-atoms-atom6-line1'),
		animation;

	var anim1,
		anim2,
		anim3,
		anim4,
		anim5,
		anim6,
		anim7,
		anim8,
		anim9,
		anim10;

	var path1 = s.select('.js-atoms-line1'),
		path2 = s.select('.js-atoms-line2'),
		path3 = s.select('.js-atoms-line3'),
		path4 = s.select('.js-atoms-line4'),
		path5 = s.select('.js-atoms-line5'),
		path6 = s.select('.js-atoms-line6'),
		path7 = s.select('.js-atoms-line7'),
		path8 = s.select('.js-atoms-line8');

	var atom1 = s.select('.js-atoms-atom1-line2'),
		atom2 = s.select('.js-atoms-atom2-line8'),
		atom3 = s.select('.js-atoms-atom3-line7'),
		atom4 = s.select('.js-atoms-atom4-line5'),
		atom5 = s.select('.js-atoms-atom5-line6'),
		atom6 = s.select('.js-atoms-atom6-line1'),
		atom7 = s.select('.js-atoms-atom7-line3'),
		atom8 = s.select('.js-atoms-atom8-line2'),
		atom9 = s.select('.js-atoms-atom9-line6'),
		atom10 = s.select('.js-atoms-atom10-line3');

		// path 1
		anim1 = function () {
			atom6.transform('t0,0');
			animateAlongPath(path1, atom6, 0, 30000, mina.linear, anim1);
		};

		//path 2
		anim2 = function () {
			atom1.transform('t0,0');
			animateAlongPath(path2, atom1, 0, 20000, mina.linear, anim2);
		};
		anim3 = function () {
		  atom1.transform('t0,0');
			animateAlongPath(path2, atom8, 1000, 20000, mina.linear, anim3);
		};

		//path 3
		anim4 = function () {
		  atom7.transform('t0,0');
			animateAlongPath(path3, atom7, 0, 20000, mina.linear, anim4);
		};
		anim5 = function () {
		  atom10.transform('t0,0');
			animateAlongPath(path3, atom10, 1000, 20000, mina.linear, anim5);
		};

		//path 5
		anim6 = function () {
		  atom4.transform('t0,0');
			animateAlongPath(path5, atom4, 0, 20000, mina.linear, anim6);
		};

		//path 6
		anim7 = function () {
		  atom5.transform('t0,0');
			animateAlongPath(path6, atom5, 0, 20000, mina.linear, anim7);
		};
		anim8 = function () {
		  atom5.transform('t0,0');
			animateAlongPath(path6, atom9, 1000, 20000, mina.linear, anim8);
		};

		//path 7
		anim9 = function () {
		  atom3.transform('t0,0');
			animateAlongPath(path7, atom3, 0, 40000, mina.linear, anim9);
		};

		//path 8
		anim10 = function () {
		  atom2.transform('t0,0');
			animateAlongPath(path8, atom2, 0, 20000, mina.linear, anim10);
		};


/**/


/*
	animation = function () {
	  star.transform('t0,0');
		animateAlongPath(path, star, 0, 5000, mina.easeout, animation);
	};
*/

	// Snap.svg helper method to make an element trace a defined path
	// http://michaeltempest.com/the-missing-snap-svg-function/
	var animateAlongPath = function (path, el, start, duration, easing, callback) {
		var len = Snap.path.getTotalLength(path),
			elBB =  el.getBBox(),
			elCenter = {
				x: elBB.x + (elBB.width / 2),
				y: elBB.y + (elBB.height / 2),
			};

		Snap.animate(start, len, function (value) {
			var movePoint = Snap.path.getPointAtLength(path, value);
				el.transform('t'+ (movePoint.x - elCenter.x) + ',' + (movePoint.y - elCenter.y));
			}, duration, easing, function () {
			if (callback) callback(path);
		});
	};

	var _init = function( ) {
//		animation();

		anim1();
		anim2();
//		anim3(); positioned weird
		anim4();
		anim5();
		anim6();
		anim7();
		anim8();
		anim9();
		anim10();
	};

	return {
		init: _init
	};

})(jQuery);


/*
var s = new Snap('.snap'),
    path = s.select('.path'),
    star = s.select('.star'),
    animation,
    animateAlongPath;

animation = function () {
  star.transform('t0,0');
	animateAlongPath(path, star, 0, 5000, mina.easeinout, animation);
};

animateAlongPath = function (path, el, start, duration, easing, callback) {
  var len = Snap.path.getTotalLength(path),
      elBB =  el.getBBox(),
      elCenter = {
        x: elBB.x + (elBB.width / 2),
        y: elBB.y + (elBB.height / 2),
      };

    Snap
      .animate(start, len, function (value) {
      var movePoint = Snap.path.getPointAtLength(path, value);
      el.transform('t'+ (movePoint.x - elCenter.x) + ',' + (movePoint.y - elCenter.y));
    }, duration, easing, function () {
      if (callback) callback(path);
    });
};

animation();

*/

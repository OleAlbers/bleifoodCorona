// helper - validate data attr
function validateDataAttr($attr) {
	"use strict";
	return $attr !== undefined;

}




//window.onload = loadScript;


jQuery(document).ready(function () {
	"use strict";
	// set nav position
	jQuery(".navbar-nav").each(function () {
		var $this = jQuery(this);
		$this.css($this.data("type"), $this.data("pos"));
	})

	// set logo position
	jQuery(".navbar-brand, .small-brand").each(function () {
		var $this = jQuery(this);

		$this.css({
			"position": "absolute",
			"left": "50%",
			"margin-left": (-$this.data("width") / 2),
			"top": $this.data("top")
		});

	})

	// set background image
	jQuery(".full-width-photo, .section > .inner").each(function () {
		var $this = jQuery(this);

		var image = "none";

		if (validateDataAttr($this.data("image"))) {
			image = "url(" + $this.data("image") + ")";
		}

		$this.css("background-image", image).css("background-attachment", $this.data("scroll")).css("padding-top", $this.data("topspace") + "px").css("padding-bottom", $this.data("bottomspace") + "px");
	})

	// init rounded image
	jQuery(".roundedImg").each(function () {
		var $this = jQuery(this);
		var imgpath = $this.find("img").attr("src");
		$this.css("background-image", "url(" + imgpath + ")");

		var $sizeImg = $this.data("size");
		if (validateDataAttr($sizeImg)) {
			var size = $sizeImg;

			$this.css({
				width: size,
				height: size
			});
		}
	})




	// Direction-aware hover effect
	jQuery('.galleryContainer > li .inner ').each(function () {
		jQuery(this).hoverdir({
			speed: 300,
			easing: 'ease',
			hoverDelay: 25,
			inverse: false
		});
	});


	// enable HTML5 placeholder behavior for browsers that aren’t trying hard enough yet
	//jQuery('input, textarea').placeholder();


});


jQuery(window).load(function () {
	"use strict";



})

function scrollToTop(i) {
	"use strict";
	if (i == 'show') {
		if (jQuery(this).scrollTop() != 0) {
			jQuery('#toTop').fadeIn();
		} else {
			jQuery('#toTop').fadeOut();
		}
	}
	if (i == 'click') {
		jQuery('#toTop').click(function () {
			jQuery('body,html').animate({scrollTop: 0}, 600);
			return false;
		});
	}
}

// scroll top button
jQuery(document).ready(function () {
	"use strict";
	scrollToTop('click');
});


jQuery(window).scroll(function () {
	"use strict";
	scrollToTop('show');
});


// sticky menu + scroll to section init

function setScrollOffset() {
	"use strict";
	var windowsize = jQuery(window).width();
	if (windowsize < 985) {
		return 0;
	}
	// return 248;
	return 85;
}

function menuSticky(i) {
	"use strict";
	if (i == 'click') {

	/*	jQuery(".full-sticky-menu").sticky({
			topSpacing: 0
		});
*//*
		jQuery('.nav.navbar-nav li a').click(function () {


			// if mobile and menu open - hide it after click
			var $togglebtn = jQuery(".navbar-toggle")

			if (!($togglebtn.hasClass("collapsed")) && ($togglebtn.is(":visible"))) {
				jQuery(".navbar-toggle").trigger("click");
			}

			var $this = jQuery(this);

			var content = $this.attr('href');

			var myUrl = content.match(/^#([^\/]+)$/i);

			if (jQuery(content).length > 0) {
				if (myUrl) {

					var goPosition = jQuery(content).offset().top - setScrollOffset()-parseInt(jQuery('.sticky-wrapper').height());

					jQuery('html,body').stop().animate({ scrollTop: goPosition}, 1000, 'easeInOutExpo', function () {
						$this.closest("li").addClass("active");
					});


				} else {
					window.location = content;
				}

				return false;
			}
		});
        */
	}

	if (i == 'scroll') {
		var menuEl, mainMenu = jQuery(".full-sticky-menu .nav.navbar-nav"), mainMenuHeight = mainMenu.outerHeight() + 500;
		var menuElements = mainMenu.find('a');

		var scrollElements = menuElements.map(function () {

			var content = jQuery(this).attr("href");
			var myUrl = content.match(/^#([^\/]+)$/i);

			if (myUrl) {

				var item = jQuery(jQuery(this).attr("href"));
				if (item.length) {
					return item;
				}

			}
		});

		var fromTop = jQuery(window).scrollTop() + mainMenuHeight;

		var currentEl = scrollElements.map(function () {
			if (jQuery(this).offset().top < fromTop) {
				return this;
			}
		});

		currentEl = currentEl[currentEl.length - 1];
		var id = currentEl && currentEl.length ? currentEl[0].id : "";
		if (menuEl !== id) {
			menuElements.parent().removeClass("active").end().filter("[href=#" + id + "]").parent().addClass("active");
		}
	}
}

jQuery(window).load(function () {
	"use strict";
	menuSticky('click');
});

jQuery(window).scroll(function () {
	"use strict";
	menuSticky('scroll');
});


jQuery(document).ready(function () {
	"use strict";
	/* rotate text */
	if (jQuery().arctext) {
		jQuery(".curved").each(function () {
			var $this = jQuery(this);

			var radius = $this.data("radius");
			var direction = $this.data("direction");

			if ((validateDataAttr(radius)) && (validateDataAttr(direction))) {
				jQuery($this).arctext({
					radius: radius,
					dir: direction,
					// 1: curve is down,
					// -1: curve is up
					rotate: true
					// if true each letter should be rotated.
				})
			}
		})
	}

	// ini tooltips
	jQuery("[data-toggle='tooltip']").tooltip();

	// progress bar animation

	if(jQuery().appear) {
		jQuery('.progress').appear(function () {
			var $this = jQuery(this);
			$this.each(function () {
				var $innerbar = $this.find(".progress-bar");
				var percentage = $innerbar.attr("data-percentage");

				$innerbar.addClass("animating").css("width", percentage + "%");

				$innerbar.on('transitionend webkitTransitionEnd oTransitionEnd otransitionend MSTransitionEnd', function () {
									$innerbar.find(".pull-right").fadeIn(900);
								});

			});
		}, {accY: -100});
	}

	// easy pie chart
	if(jQuery().appear) {
		jQuery('.pie-chart').appear(function () {
			jQuery(this).easyPieChart({
				barColor: jQuery(this).attr("data-color"),
				trackColor: "transparent",
				scaleColor:	false,
				lineCap:	"square",
				animate:1500,
				lineWidth:24,
				size:	155
			});
		}, {accY: -100});
	}


	// preloader
	if(jQuery().queryLoader2) {
		jQuery("body.preloader").queryLoader2({
		  backgroundColor: "#fff",	 //Background color of the loader (in hex).
		  barColor: "#584A41",	 //Background color of the bar (in hex).
		  barHeight: 3,	 //Height of the bar in pixels. Default: 1
		  deepSearch: true,	 //Set to true to find ALL images with the selected elements. If you don't want queryLoader to look in the children, set to false. Default: true.
		  percentage: true,	 //Set to true to enable percentage visualising. Default is false.
		  completeAnimation: "fade", //Set the animation type at the end. Options: "grow" or "fade". Default is "fade".
		  onComplete: function(){
		    jQuery("#ct_preloader").fadeOut(600);
		  }
		});
	}


})


// demo required - block clicks in hash links
/*
 jQuery(document).ready(function () {
 "use strict";
 jQuery("a[href='#']").click(function () {
 return false;
 });
 })
 */




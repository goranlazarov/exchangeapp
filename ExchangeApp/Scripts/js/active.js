/*********************************************************************************

	Template Name: Korde - Tax Service HTML Template  
	Template URI: https://themeforest.net/user/hastech
	Description: Korde is a beautifula and unique tax service html template.
	Author: HasTech
	Author URI: https://hastech.company/
	Version: 1.0

	Note: This is active js. Plugins activation code here.

**********************************************************************************/


/*===============================================================================
			[ INDEX ]
=================================================================================

	Scroll Up Activation
	Slider Activation
	Banner Slider
	Banner Slider Style 2
	Banner Slider Style 3
	Banner Content Slider
	Testimonial Content
	Testimonial Author
	Testimonial Slider Style 2 Active
	Testimonial Slider Style 3 Active
		Blog Slider
	Wowjs active
	CounterUp Active
	Mobile menu
	Videopopup
	Instafeed
	Calender
	Video Player
	Selectpicker
	Instafeed active
	Fake Loader
	
	=================================================================================
			[ END INDEX ]
================================================================================*/

(function($) {
	'use strict';

	
	///* Scroll Up Activation */
	//$.scrollUp({
	//    scrollText: '<i class="fa fa-angle-up"></i>',
	//    easingType: 'linear',
	//    scrollSpeed: 900,
	//    animation: 'slide'
	//});




	
	/* Slider Activation */
		// Banner Slider
		$('.banner-slider-active').slick({
			arrows: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			fade: true,
			cssEase: 'ease-out',
			easing: 'ease-out',
			speed: 600,
			adaptiveHeight: true,
			prevArrow: '<button class="cr-slick-arrow cr-slick-prev"><i class="fa fa-angle-left"></i></button>',
			nextArrow: '<button class="cr-slick-arrow cr-slick-next"><i class="fa fa-angle-right"></i></button>'
		});


		// Banner Slider Style 2
		$('.banner--style2-slider-active').slick({
			arrows: false,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			fade: true,
			cssEase: 'ease-out',
			easing: 'ease-out',
			speed: 600,
			dots: true,
			adaptiveHeight: true,
		});


		// Banner Slider Style 3
		$('.banner-slider-active--with-pagination').slick({
			arrows: false,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			fade: true,
			cssEase: 'ease-out',
			easing: 'ease-out',
			speed: 600,
			dots: true,
			adaptiveHeight: true,
		});

		// Banner Content Slider
		$('.banner-contnet-slider').slick({
			arrows: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			cssEase: 'ease-out',
			easing: 'ease-out',
			speed: 600,
			adaptiveHeight: true,
			prevArrow: '<button class="cr-slick-arrow cr-slick-prev"><i class="fa fa-angle-left"></i></button>',
			nextArrow: '<button class="cr-slick-arrow cr-slick-next"><i class="fa fa-angle-right"></i></button>'
		});

		// Testimonial Content
		$('.testimonial-content-slider-active').slick({
			adaptiveHeight: true,
			autoplay: true,
			autoplaySpeed: 8000, 
			arrows: false,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slidesToScroll: 1,
			asNavFor: '.testimonial-author-slider-active'
		});

		// Testimonial Author
		$('.testimonial-author-slider-active').slick({
			centerMode: true,
			autoplay: true,
			autoplaySpeed: 8000,
			arrows: false,
			focusOnSelect: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 3,
			slideToScroll: 1,
			asNavFor: '.testimonial-content-slider-active',
			responsive: [
				{
					breakpoint: 577,
					settings: {
						slidesToShow: 1,
						slidesToScroll: 1
					}
				}
			]
		});

		// Testimonial Slider Style 2 Active
		$('.testimonial-slider-active').slick({
			centerMode: true,
			autoplay: true,
			autoplaySpeed: 8000,
			arrows: false,
			focusOnSelect: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 3,
			slideToScroll: 1,
			responsive: [
				{
					breakpoint: 992,
					settings: {
						slidesToShow: 2,
						slidesToScroll: 1
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesToShow: 1,
						slidesToScroll: 1
					}
				}
			]
		});

		// Testimonial Slider Style 3 Active
		$('.testimonial-slider-style3-active').slick({
			centerMode: true,
			autoplay: true,
			autoplaySpeed: 8000,
			arrows: false,
			focusOnSelect: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 2,
			slideToScroll: 1,
			responsive: [
				{
					breakpoint: 992,
					settings: {
						slidesToShow: 2,
						slidesToScroll: 1
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesToShow: 1,
						slidesToScroll: 1
					}
				}
			]
		});

		// Blog Slider
		$('.blog--slider-active .blog__thumb').slick({
			arrows: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			prevArrow: '<button class="cr-slick-arrow cr-slick-prev"><i class="fa fa-angle-left"></i></button>',
			nextArrow: '<button class="cr-slick-arrow cr-slick-next"><i class="fa fa-angle-right"></i></button>'
		});


		// Blog Slider
		$('.pg-blog-thumb-gallery-active').slick({
			arrows: true,
			infinite: true,
			pauseOnHover: true,
			slidesToShow: 1,
			slideToScroll: 1,
			prevArrow: '<button class="cr-slick-arrow cr-slick-prev"><i class="fa fa-angle-left"></i></button>',
			nextArrow: '<button class="cr-slick-arrow cr-slick-next"><i class="fa fa-angle-right"></i></button>'
		});



	/* Wowjs active */
	new WOW().init();





	/* CounterUp Active */
	$('.counter').counterUp({
		delay: 10,
		time: 1000
	});



	/* Mobile menu */
	$('nav.main-navigation').meanmenu({
		meanMenuClose: '<i class="flaticon-clear-button"></i>',
		meanMenuCloseSize: '18px',
		meanScreenWidth: '991',
		meanExpandableChildren: true,
		meanMenuContainer: '.mobile-menu',
		onePage: true
	});



	/* Videopopup */
	$('.video-popup-trigger').YouTubePopUp();





    /* Calender */
    $('#my-calendar').zabuto_calendar({
        cell_border: false,
        today: true,
        show_days: true,
        weekstartson: 0,
        nav_icon: {
            prev: '<i class="fa fa-angle-left"></i>',
            next: '<i class="fa fa-angle-right"></i>'
        }
    });




    /* Video Player */
    plyr.setup('.video-player');




    /* Selectpicker */
	$('select').selectpicker();
	
	
	
	/* Instafeed active */
	if($('#sidebar-instagram-feed').length){

		var userFeed = new Instafeed({
			get: 'user',
			userId: 6665768655,
			accessToken: '6665768655.1677ed0.313e6c96807c45d8900b4f680650dee5',
			target: 'sidebar-instagram-feed',
			resolution: 'thumbnail',
			limit: 6,
			template: '<li><a href="{{link}}" target="_new"> <img src="{{image}}" /><ul class="likes-comments"><li><i class="fa fa-heart-o"></i><span>{{likes}}</span></li><li><i class="fa fa-comments-o"></i><span>{{comments}}</span></li></ul></a></li>',
		});
		userFeed.run();

	}



	/* Fake Loader */
	$('.fakeloader').fakeLoader({
		timeToHide: 1200,
		bgColor: 'rgba(225,225,225,0.95)',
		spinner: 'spinner2'
	});

    


})(jQuery);

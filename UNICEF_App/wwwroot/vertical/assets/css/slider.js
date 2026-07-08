jQuery(document).ready(function($){
	//on mobile - open/close primary navigation clicking/tapping the menu icon
	$('.cd-primary-nav').on('click', function(event){
		if($(event.target).is('.cd-primary-nav')) $(this).children('ul').toggleClass('is-visible');
	});

	//upload videos if not on mobile
	uploadVideo($('.cd-hero-slider'));

	//change visible slide
	$('.cd-slider-nav li').on('click', function(event){
		event.preventDefault();
		var selectedItem = $(this);		
		if(!selectedItem.hasClass('selected')) {
			// if it's not already selected
			var selectedPosition = selectedItem.index(),
				activePosition = $('.cd-hero-slider .selected').index();
			if( activePosition < selectedPosition) {
				nextSlide($('.cd-hero-slider'), $('.cd-slider-nav'), selectedPosition);
			} else {
				prevSlide($('.cd-hero-slider'), $('.cd-slider-nav'), selectedPosition);
			}

			updateNavigationMarker(selectedPosition+1);
		}
	});

	function nextSlide(container, pagination, n){
		var visibleSlide = container.find('.selected'),
			navigationDot = pagination.find('.selected');
		
		visibleSlide.removeClass('selected from-left from-right').addClass('is-moving').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
			visibleSlide.removeClass('is-moving');
		});
		
		container.children('li').eq(n).addClass('selected from-right').prevAll().addClass('move-left');
		navigationDot.removeClass('selected')
		pagination.find('li').eq(n).addClass('selected');

		checkVideo(visibleSlide, container, n);
	}

	function prevSlide(container, pagination, n){
		var visibleSlide = container.find('.selected'),
			navigationDot = pagination.find('.selected');
		
		visibleSlide.removeClass('selected from-left from-right').addClass('is-moving').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function(){
			visibleSlide.removeClass('is-moving');
		});

		container.children('li').eq(n).addClass('selected from-left').removeClass('move-left').nextAll().removeClass('move-left');
		navigationDot.removeClass('selected');
		pagination.find('li').eq(n).addClass('selected');

		checkVideo(visibleSlide, container, n);
	}

	function uploadVideo(container) {
		container.find('.cd-bg-video-wrapper').each(function(){
			var videoWrapper = $(this);
			if( videoWrapper.is(':visible') ) {
				// if visible - we are not on a mobile device 
				var	videoUrl = videoWrapper.data('video'),
					video = $('<video loop><source src="'+videoUrl+'.mp4?token2=1432242975_9f000e6f57ccb7a41611bd6ecd0a79ca&aksessionid=0c574dd13641c95a&ns=4" type="video/mp4" /><source src="'+videoUrl+'.webm" type="video/webm" /></video>');
				video.appendTo(videoWrapper);
			}
		});
	}

	function checkVideo(hiddenSlide, container, n) {
		//check if a video outside the viewport is playing - if yes, pause it
		if( hiddenSlide.find('video').length > 0 ) hiddenSlide.find('video').get(0).pause();

		//check if the select slide contains a video element - if yes, play the video
		if( container.children('li').eq(n).find('video').length > 0 ) container.children('li').eq(n).find('video').get(0).play();
	}

	function updateNavigationMarker(n) {
		$('.cd-marker').removeClassPrefix('item').addClass('item-'+n);
	}

	$.fn.removeClassPrefix = function(prefix) {
		//remove all classes starting with 'prefix'
	    this.each(function(i, el) {
	        var classes = el.className.split(" ").filter(function(c) {
	            return c.lastIndexOf(prefix, 0) !== 0;
	        });
	        el.className = $.trim(classes.join(" "));
	    });
	    return this;
	};
});




  // 3D STUFF HERE //
jQuery(document).ready(function($) {
  //check media query
  var mediaQuery = window.getComputedStyle(document.querySelector('.cd-background-wrapper'), '::before').getPropertyValue('content').replace(/"/g, ''),
    //define store some initial variables
    halfWindowH = $(window).height() * 0.5,
    halfWindowW = $(window).width() * 0.5,
    //define a max rotation value (X and Y axises)
    maxRotationY = 18,
    maxRotationX = 18,
    aspectRatio;

  //detect if hero <img> has been loaded and evaluate its aspect-ratio
  $('.cd-floating-background').find('img').eq(0).load(function() {
    aspectRatio = $(this).width() / $(this).height();
    if (mediaQuery == 'web' && $('html').hasClass('preserve-3d')) initBackground();
  }).each(function() {
    //check if image was previously load - if yes, trigger load event
    if (this.complete) $(this).load();
  });

  //detect mouse movement
  $('.cd-background-wrapper').on('mousemove', function(event) {
    if (mediaQuery == 'web' && $('html').hasClass('preserve-3d')) {
      window.requestAnimationFrame(function() {
        moveBackground(event);
      });
    }
  });

  //on resize - adjust .cd-background-wrapper and .cd-floating-background dimentions and position
  $(window).on('resize', function() {
    mediaQuery = window.getComputedStyle(document.querySelector('.cd-background-wrapper'), '::before').getPropertyValue('content').replace(/"/g, '');
    if (mediaQuery == 'web' && $('html').hasClass('preserve-3d')) {
      window.requestAnimationFrame(function() {
        halfWindowH = $(window).height() * 0.5,
          halfWindowW = $(window).width() * 0.5;
        initBackground();
      });
    } else {
      $('.cd-background-wrapper').attr('style', '');
      $('.cd-floating-background').attr('style', '').removeClass('is-absolute');
    }
  });

  function initBackground() {
    var wrapperHeight = Math.ceil(halfWindowW * 2 / aspectRatio),
      proportions = (maxRotationY > maxRotationX) ? 1.1 / (Math.sin(Math.PI / 2 - maxRotationY * Math.PI / 180)) : 1.1 / (Math.sin(Math.PI / 2 - maxRotationX * Math.PI / 180)),
      newImageWidth = Math.ceil(halfWindowW * 2 * proportions),
      newImageHeight = Math.ceil(newImageWidth / aspectRatio),
      newLeft = halfWindowW - newImageWidth / 2,
      newTop = (wrapperHeight - newImageHeight) / 2;

    //set an height for the .cd-background-wrapper
    $('.cd-background-wrapper').css({
      'height': wrapperHeight,
    });
    //set dimentions and position of the .cd-background-wrapper		
    $('.cd-floating-background').addClass('is-absolute').css({
      'left': newLeft,
      'top': newTop,
      'width': newImageWidth,
    });
  }

  function moveBackground(event) {
    var rotateY = ((-event.pageX + halfWindowW) / halfWindowW) * maxRotationY,
      rotateX = ((event.pageY - halfWindowH) / halfWindowH) * maxRotationX;

    if (rotateY > maxRotationY) rotateY = maxRotationY;
    if (rotateY < -maxRotationY) rotateY = -maxRotationY;
    if (rotateX > maxRotationX) rotateX = maxRotationX;
    if (rotateX < -maxRotationX) rotateX = -maxRotationX;

    $('.cd-floating-background').css({
      '-moz-transform': 'rotateX(' + rotateX + 'deg' + ') rotateY(' + rotateY + 'deg' + ') translateZ(0)',
      '-webkit-transform': 'rotateX(' + rotateX + 'deg' + ') rotateY(' + rotateY + 'deg' + ') translateZ(0)',
      '-ms-transform': 'rotateX(' + rotateX + 'deg' + ') rotateY(' + rotateY + 'deg' + ') translateZ(0)',
      '-o-transform': 'rotateX(' + rotateX + 'deg' + ') rotateY(' + rotateY + 'deg' + ') translateZ(0)',
      'transform': 'rotateX(' + rotateX + 'deg' + ') rotateY(' + rotateY + 'deg' + ') translateZ(0)',
    });
  }
});

/* 	Detect "transform-style: preserve-3d" support, or update csstransforms3d for IE10 ? #762
	https://github.com/Modernizr/Modernizr/issues/762 */
(function getPerspective() {
  var element = document.createElement('p'),
    html = document.getElementsByTagName('html')[0],
    body = document.getElementsByTagName('body')[0],
    propertys = {
      'webkitTransformStyle': '-webkit-transform-style',
      'MozTransformStyle': '-moz-transform-style',
      'msTransformStyle': '-ms-transform-style',
      'transformStyle': 'transform-style'
    };

  body.insertBefore(element, null);

  for (var i in propertys) {
    if (element.style[i] !== undefined) {
      element.style[i] = "preserve-3d";
    }
  }

  var st = window.getComputedStyle(element, null),
    transform = st.getPropertyValue("-webkit-transform-style") ||
    st.getPropertyValue("-moz-transform-style") ||
    st.getPropertyValue("-ms-transform-style") ||
    st.getPropertyValue("transform-style");

  if (transform !== 'preserve-3d') {
    html.className += ' no-preserve-3d';
  } else {
    html.className += ' preserve-3d';
  }
  document.body.removeChild(element);

})();
$.fn.equalHeights = function(){
	var max_height = 0;
	$(this).each(function(){
		max_height = Math.max($(this).height(), max_height);
	});
	$(this).each(function(){
		$(this).height(max_height);
	});
};

$(document).ready(function(e) {
	
	devWidth = $(window).width();
	
    
	window.onload = function(){
		
		bannerHeight = $('#banner').height()
		$('#banner .banner-navs .prev-nav').height(bannerHeight)
		$('#banner .banner-navs .next-nav').height(bannerHeight)
	
		if (devWidth >= 751){
			
			$('.round-edge').equalHeights();
			lineHeight = $(".icons img.hi").height();
			$('.round-edge .icons').css("lineHeight", lineHeight + "px");
			bannerImage = ($("#banner .property-img").height()) + 14 ;
			$('#banner .property-details').css("height", bannerImage + "px");
		}
	}
	
});

$( window ).resize(function() {
	bannerImage = ($("#banner .property-img").height()) + 14 ;
	$('#banner .property-details').css("height", bannerImage + "px");
	bannerHeight = $('#banner').height()
	$('#banner .banner-navs .prev-nav').height(bannerHeight)
	$('#banner .banner-navs .next-nav').height(bannerHeight)
});
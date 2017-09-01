// https://developers.google.com/youtube/iframe_api_reference

var player2;

// this function gets called when API is ready to use
function onYouTubePlayerAPIReady() {
    // create the global player from the specific iframe (#video)
    player2 = new YT.Player('youtube', {
        events: {
            // call this function when player is ready to use
            'onReady': onPlayerReady
        }
    });
}

function onPlayerReady(event) {

}

// Inject YouTube API script
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/player_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);


/**
 * Email validate field.
 *
 * @since Saatchi 1.0 
 */

function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) {
        return true;
    } else {
        return false;
    }
}

/**
 * Add class if not set.
 *
 * @since Saatchi 1.0 
 */

function addValidationClass(_this, status) {
    var errorClass = 'email-error',
        validClass = 'email-valid';

    if (!_this.hasClass(errorClass) && status == 'error') {
        _this.addClass(errorClass).removeClass(validClass);
    }

    if (!_this.hasClass(validClass) && status == 'valid') {
        _this.addClass(validClass).removeClass(errorClass);
    }
}

jQuery(document).ready(function($, Cookies) {

    /* Country Flags Tooltip */
    $('[data-toggle="tooltip"]').tooltip();

    if ($('.page-template-template-search').length > 0 || $('.search-results').length > 0 || $('.search-no-results').length > 0) {
        var searchEl = $('#s'),
            placeholder = searchEl.attr('placeholder');
        searchEl.attr('value', ''); /*Reset on page load*/
        searchEl.on('focus', function() {
            $(this).attr('placeholder', '');
        }).on('blur', function() {
            $(this).attr('placeholder', placeholder);
        });
    }

    // Hide newsletter popup and save cookie for 1 week
    if ($('.home').length > 0 ) {
        if (Cookies && Cookies.get('close-newsletter') !== "hidden") {
            var tweetBox = $('.widget_saatchi_widget_recent_tweets');
            tweetBox.hide();
            $('.mail-list-container .close-btn').on('click', function() {
                $('.mail-list-container').animate({
                    opacity: 0
                }, 350, 'swing', function() {
                    $(this).remove();
                    Cookies.set('close-newsletter', 'hidden', {
                        expires: 7
                    });
                });
                tweetBox.show();
            });
        }
        var instaTime = $('.jr-insta-time');
        var instaBy = $('.jr-insta-username');
        var instaText = instaBy.text().replace('by ','');
        instaBy.html('<div class="instagram-icon"><i class="fa fa-instagram"></i></div><p><a href="http://instagram.com/'+instaText+'" target="_blank"><strong>@'+instaText+'</strong></a> | <span>'+instaTime.text()+'</span></p>');
        instaTime.hide();
    }


    // Find all YouTube videos
    var vids = $("body").find("iframe[src*='www.youtube.com']");

    // Figure out and save aspect ratio for each video
    vids.each(function() {
        //console.log( $( this ).attr( 'src' ) );
        $(this).wrap("<p class='videoWrapper'></p>");
    });

    // Single page slider
    if ($('.single').length > 0) {
        $('.flexslider').flexslider({
            animation: "fade",
            animationSpeed: 1000,
            nextText: "",
            prevText: "",
            slideshowSpeed: 3500,
            useCSS: false,
            easing: "linear",
            pauseOnHover: true
        });
    }

    // Froogaloop
    // https://s3-us-west-2.amazonaws.com/s.cdpn.io/3/froogaloop.js
    // https://github.com/vimeo/player-api/blob/master/javascript/froogaloop.js
    // http://a.vimeocdn.com/js/froogaloop2.min.js

    var iframe = $('#vimeo')[0];
    var player = $f(iframe);

    // Vimeo and YouTube in slider
    $('.play').on('click', function() {
        if ($(this).hasClass('youtube')) {
            //console.log( 'youtube opened' );
            var url = "//www.youtube.com/embed" + $(this).prev().text() + "?enablejsapi=1&rel=0&showinfo=0";
            var iframeHeight = $('.youtube-player iframe').height();
            $('.youtube-player').css('margin-top', -(iframeHeight / 2));

            $('#youtube').attr('src', url);

            $('.slider-overlay').animate({
                zIndex: 11
            }, 500, function() {
                $('.slider-overlay').fadeIn(300);
                $('.slider-overlay .youtube-player').css('display', 'inline-block');
                player2.playVideo();
            });
        }

        if ($(this).hasClass('vimeo')) {
            //console.log( 'vimeo opened' );
            var url = "//player.vimeo.com/video" + $(this).prev().text() + "?api=1";
            var iframeHeight = $('.vimeo-player iframe').height();
            $('.vimeo-player').css('margin-top', -(iframeHeight / 2));

            $('#vimeo').attr('src', url);

            $('.slider-overlay').animate({
                zIndex: 11
            }, 500, function() {
                $('.slider-overlay').fadeIn(300);
                $('.slider-overlay .vimeo-player').css('display', 'inline-block');
                player.api("play");
            });
        }
    });

    $('.close').on('click', function() {
        if ($('.youtube-player').is(':visible')) {
            //console.log( 'youtube closed' );
            $('.slider-overlay, .slider-overlay .youtube-player').fadeOut(250);

            $('.slider-overlay').animate({
                zIndex: -1
            }, 500, function() {
                player2.pauseVideo();
                $('#youtube').attr('src', '');
            });
        }

        if ($('.vimeo-player').is(':visible')) {
            //console.log( 'vimeo closed' );
            $('.slider-overlay, .slider-overlay .vimeo-player').fadeOut(250);

            $('.slider-overlay').animate({
                zIndex: -1
            }, 500, function() {
                player.api("pause");
                $('#vimeo').attr('src', '');
            });
        }
    });


    /** 
     * Newsletter Signup
     */
    function fadeandSetCookie() {
        $('.mail-list-container, .sign-up-big').fadeOut(300);
        $('.widget_saatchi_widget_recent_tweets').show();
        Cookies.set('close-newsletter', 'hidden', {
            expires: 7
        });
    }
    var explode = function() {
        $('.feedback').fadeOut(300);
        hideLoading();
    };

    var successAction = function() {
        fadeandSetCookie();
    };

    var hideSpeed = 250,
        timeout = 2000;

    function errorMessage(element, message, hideSpeed, timeout) {
        element.text(message).parent().fadeIn(hideSpeed);
        setTimeout(explode, timeout);
    }

    function successMessage(element, message, hideSpeed, timeout) {
        element.text(message).parent().fadeIn(hideSpeed);
    }

    function showLoading() {
        $('.feedback-overlay').fadeIn(250);
    }

    function hideLoading() {
        $('.feedback-overlay').fadeOut(250);
    }

    // Validate Newsletter E-mail field
    $('.sign-up').on('submit', function(e) {
        showLoading();

        var ajaxurl = global.ajaxurl,
            feedback = $('.feedback span'),
            _this = $(this).parent().find('input[type="email"]'),
            sEmail = _this.val();

        if ($.trim(sEmail).length == 0) {
            var message = 'Please enter a valid email address';
            errorMessage(feedback, message, hideSpeed, timeout);

            addValidationClass(_this, 'error');
            e.preventDefault();
        } else if (validateEmail(sEmail)) {
            addValidationClass(_this, 'valid');
            e.preventDefault();

            var formData = $(this).serialize();

            $.ajax({
                url: ajaxurl,
                type: 'post',
                data: {
                    action: 'saatchi_newsletter_signup',
                    values: formData
                },
                beforeSend: function() {
                    //console.log( 'before sending..' );   
                },
                success: function(message) {

                    if (message == 'User already exists') {
                        $('.found-overlay').fadeIn(250, function() {
                            setTimeout(fadeandSetCookie, 1500);
                        });

                    } else {
                        Cookies.set('success-newsletter', 'hidden', {
                            expires: 7
                        });
                        $('.success-overlay').fadeIn(250);
                        setTimeout(successAction, 3000);
                    }
                }
            });
        } else {
            var message = 'Invalid email address';
            errorMessage(feedback, message, hideSpeed, timeout);

            addValidationClass(_this, 'error');
            e.preventDefault();
        }
    });


    var ajaxurl = global.ajaxurl,
        pagenumber = 1;

    /* Global Pagination */
    var maxnumpages = $('.load-more-btn').attr('data-total');

    if (maxnumpages == 1 || maxnumpages == 0) {
        $('.load-more-btn').hide();
    }

    $('.load-more-btn').on('click', function(e) {
        e.preventDefault();
        $(this).hide();
        if ( $( '.error404' ).length > 0 ) {
            var class_name = "error404";
        } else if ( $( '.single' ).length > 0 ) {
            var class_name = "single";
        } else if ( $( '.archive' ).length > 0 || $( '.page-template-template-search' ).length > 0 || $( '.search-results' ).length > 0 ) {
            var class_name = "archive";
        } else if ( $( '.home' ).length > 0 ) {
            var class_name = "home";
        } else {
            var class_name = "";
        }

        $.ajax({
            url: ajaxurl,
            type: 'post',
            data: {
                action: 'ajax_pagination',
                query_vars: global.query_vars,
                page: ++pagenumber,
                post_meta: ($('.page-template-template-search').length > 0) ? "popular-posts" : "",
                post_class: class_name
            },
            beforeSend: function() {
                $('.loader').append('<div id="loader"><div class="feedback-overlay"></div></div>');
                $('#loader .feedback-overlay').show();
            },
            success: function(html) {
                // console.log( 'pagenumber is: ' + pagenumber );
                if ((pagenumber == $('.load-more-btn').attr('data-total')) || $('.load-more-btn').attr('data-total') == 1) {
                    $('.load-more-btn').remove();
                }
                $('#loader').remove();
                $('.load-more-btn').show();
                $('.append-post-container').append(html);
            }
        });

    });


    /** 
     * Responsive implementation
     */

    if ($(window).width() < 992) {
        $('.right-sidebar').appendTo('.single article');

        if ($('.view-portfolio').length > 0) {
            $('.view-portfolio').appendTo('.single article');
        }
    }


    $(window).resize(function() {
        if ($(window).width() < 992) {
            if (!$('body').hasClass('mobile')) {
                $('.right-sidebar').appendTo('.single article');

                if ($('.view-portfolio').length > 0) {
                    $('.view-portfolio').appendTo('.single article');
                }

                $('body').addClass('mobile');
            }
        } else {
            $('body').removeClass('mobile');

            $('.right-sidebar').appendTo('.post-wrapper');
        }
    });


    /** 
     * Go to top button
     */

    var offset = 300,
        offset_opacity = 1200,
        scroll_top_duration = 700,
        $back_to_top = $('.go-top');

    $(window).scroll(function() {
        ($(this).scrollTop() > offset) ? $back_to_top.addClass('cd-is-visible'): $back_to_top.removeClass('cd-is-visible cd-fade-out');
        if ($(this).scrollTop() > offset_opacity) {
            $back_to_top.addClass('cd-fade-out');
        }
    });

    $back_to_top.on('click', function(event) {
        event.preventDefault();
        $('body, html').animate({
            scrollTop: 0,
        }, scroll_top_duration);
    });

    /**
     * Adds .attribution class to <p> tags containing the text '(Image: *'
     */

    $("p:contains('(Image:')").attr('class', 'attribution');

    /**
     * Adds .attribution class to <p> tags containing the text '(Video: *'
     */

    $("p:contains('(Video:')").attr('class', 'attribution');

    /**
     * Show social list when user scrolls page
     */

    $(window).scroll(function() {
        $('.social-list').fadeIn('slow');   
    });

    /**
     * append social controls to end of article
     */
     $('.social-list').clone().appendTo('article');

});

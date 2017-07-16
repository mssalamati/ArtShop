/*Author : soroosh salamati*/
var fullwith = '100%';
var view_in_room_active = false;
var locker = false;
var paperdiv = $('.rag-print');
var framediv = $('.select-frame');
$(document).ready(function () {
    fullwith = $('#mainmage').width();
});

function changetab(tab) {
    if (tab == 0) {
        $('#orginalTabTitle').addClass('active');
        $('#printTabTitle').removeClass('active');
        $('#original').removeClass('hidden');
        $('#prints').addClass('hidden');
        $('.view-in-a-room').show();
        resetprint();
        removeFrame();

    } else if (tab == 1) {
        $('#orginalTabTitle').removeClass('active');
        $('#printTabTitle').addClass('active');
        $('#original').addClass('hidden');
        $('#prints').removeClass('hidden');
        $('.view-in-a-room').hide();
        resetprint();
        removeFrame();
    }
}
function view_in_room() {
    if (locker) return;
    locker = true;
    if (view_in_room_active) {
        $('#bodmain').removeClass('artwithpic');
        $('.art-detail-description').css('min-height', '943px');
        $('#viewroombtnicon').addClass('icn-view-in-a-room');
        $('#viewroombtnicon').removeClass('fa fa-arrow-left');
        $('#viewroombtntxt').html('View in a Room');
        $('#mainmage').css('box-shadow', '');
        $('#mainmage').animate({
            width: fullwith,
            marginTop: "0"
        }, 750, function () {
            locker = false;
        });
        view_in_room_active = false;
    } else {
        $('#bodmain').addClass('artwithpic');
        $('.art-detail-description').css('min-height', '770px');
        $('#viewroombtnicon').removeClass('icn-view-in-a-room');
        $('#viewroombtnicon').addClass('fa fa-arrow-left');
        $('#viewroombtntxt').html('Back to Artwork');
        $('#mainmage').css('box-shadow', 'rgba(0, 0, 0, 0.1) 2px 2px 3px');
        $('#mainmage').animate({
            width: "314px",
            marginTop: "144px"
        }, 750, function () {
            locker = false;
        });
        view_in_room_active = true;
    }
}
$('#selectMaterial').change(function () {
    var val = this.value;
    if (val == null || val == '') {
        resetprint();
    } else {
        paperdiv.removeClass('hidden');
    }
});
$('#rag-selectSize').change(function () {
    var val = this.value;
    removeFrame();
    if (val == null || val == '')
        print_sate(false, 0, 0);
    else {
        var nwidth = $(this).find(':selected').data('width');
        var nheight = $(this).find(':selected').data('height');
        var nframe = $(this).find(':selected').data('frames');
        framediv.removeClass('hidden');
        selectframeSet(nframe);
        print_sate(true, nwidth, nheight);
    }
});
function selectframeSet(data) {
    $('#selectFrame').find('option').remove().end().append('<option value="">Choose from List</option>').val('');
    if (data != null)
        $.each((data), function (key, value) {
            $('#selectFrame').append('<option value="' + value.val + '" data-color="' + value.color + '" data-size="' + value.size + '">' + value.desc + '</option>');
        });
}
function resetprint() {
    $('#selectMaterial').val("");
    $('#rag-selectSize').val("");
    paperdiv.addClass('hidden');
    framediv.addClass('hidden');
    $('#bodmain').removeClass('artwithpic');
    $('#bodmain').removeClass('artwhitprint');
    $('#mainmage').css('box-shadow', '');
    $('#mainmage').css('background', '');
    $('#mainmage').css('border', '');
    $('#mainmage').css('padding', '');
    $('#mainmage').css('display', '');
    $('#mainmage').animate({
        width: fullwith,
        marginTop: 0
    }, 750);
}
function print_sate(isenable, paper_width, paper_height) {
    if (isenable) {
        $('#bodmain').removeClass('artwithpic');
        $('#bodmain').addClass('artwhitprint');
        $('.art-detail-description').css('min-height', 'auto');
        $('#mainmage').css('box-shadow', 'rgba(0, 0, 0, 0.1) 2px 2px 3px');
        $('#mainmage').css('background', 'rgb(255, 255, 255)');
        $('#mainmage').css('border', '1px solid rgb(235, 235, 235)');
        $('#mainmage').css('display', 'inline-block');
        $('#mainmage').css('padding', '15px');
        var top = 367 - Math.ceil(paper_height * 8.23);
        var w = paper_width * 8.23;
        $('#mainmage').animate({
            width: w,
            marginTop: top
        }, 750);
    } else {
        resetprint();
    }
}
$('#selectFrame').change(function () {
    var val = this.value;
    if (val == null || val == '')
        removeFrame();
    else {
        var color = $(this).find(':selected').data('color');
        var size = $(this).find(':selected').data('size');
        setFrame(color, size);
    }
});
function setFrame(color, size) {
    var mframe = $('#mframe');
    c = $(".art-detail-image img");
    mframe.show();
    mframe.css(2 === size ? {
        backgroundColor: color,
        height: c.height() + 54,
        width: c.width() + 53,
        left: c.offset().left - 11,
        top: c.offset().top - 11
    } : {
        backgroundColor: color,
        height: c.height() + 48,
        width: c.width() + 47,
        left: c.offset().left - 8,
        top: c.offset().top - 8
    });
}
function removeFrame() {
    $('#selectFrame').val('');
    $('#mframe').hide();
}
function initfullscreen () {
    $(window).width() < 450 || $("[data-orbit-slide] img, .fullscreen").each(function (b, c) {
        var d, e = $(this);
        d = "A" == c.tagName ? $(c).attr("href") : $(c).attr("src"), e.qtip({
            content: {
                text: '<img src="' + d + '">',
                title: {
                    text: '<img src="/Content/Images/sa-logo.svg">',
                    button: "Close"
                }
            },
            show: {
                event: "click",
                solo: !0,
                modal: !0,
                effect: function (b) {
                    $(this).fadeIn(750), $("#livechat-full, #livechat-compact-container, #livechat-eye-catcher").css("z-index", "0")
                }
            },
            hide: {
                target: $("body"),
                event: "click",
                fixed: !0,
                leave: !1
            },
            style: {
                classes: "ui-tooltip-light ui-tooltip-fullscreen",
                tip: {
                    corner: !1
                }
            },
            position: {
                my: "center",
                at: "center",
                target: $(document),
                effect: !1
            },
            events: {
                render: function (b, c) {
                    $(this).find(".ui-tooltip-icon[title]").addClass("so-quick-tip");
                },
                show: function (b, c) {
                    $(".content, #footer").hide(), e.qtip("option", {
                        "position.my": "top center",
                        "position.at": "top center"
                    }), c.elements.overlay.addClass("white")
                },
                hide: function (b, c) {
                    $(".content, #footer").show(), c.elements.overlay.removeClass("white")
                }
            }
        })
    }).bind("click", function (a) {
        return a.preventDefault(), !1
    });
}
initfullscreen();
//soroosh

Saatchi.Art = function (a) {
    "use strict";
    return {
        artist: null,
        art: null,
        safe: !1,
        original: !1,
        prints: !1,
        additionalImages: [],
        dropzone: null,
        product: {},
        imgHostUri: "saimg-a.akamaihd.net",
        init: function () {
            this.Layout.init(), (this.original || this.prints) && (this.Products.init(), this.AddToCart.init(), a(window).width() > 601 && this.ViewRoom.init()), this.original && this.Offer.init(), this.Favorite.init(), this.Fullscreen.init(), this.Render.init(), this.Comment.init(), this.MultipleItems.init(), this.UserBadges.init(), this.ProductDetailsToDataLayer.init()
        },
        ProductDetailsToDataLayer: {
            init: function () {
                var b = !0,
                    c = a(".art-detail__recognition__single-badge-wrap").text().indexOf("Other Art Fair") >= 0 ? !0 : !1;
                a(".tab-prints, #viewPrints a").on("click", function () {
                    b = !1, Saatchi.Art.ProductDetailsToDataLayer.dataLayerPush(b, c)
                }), a(".tab-original").on("click", function () {
                    b = !0, Saatchi.Art.ProductDetailsToDataLayer.dataLayerPush(b, c)
                }), (window.location.hash || !Saatchi.Art.original && Saatchi.Art.prints || window.location.search.indexOf("prints=1") > -1) && (b = !1), Saatchi.Art.ProductDetailsToDataLayer.dataLayerPush(b, c)
            },
            dataLayerPush: function (a, b) {
                dataLayer.push({
                    ecommerce: {
                        detail: {
                            actionField: {},
                            products: [{
                                name: Saatchi.Art.product.title || "unknown",
                                id: Saatchi.Art.product.sku,
                                price: Saatchi.Art.product.price,
                                brand: b ? "toaf" : "saatchi",
                                category: Saatchi.Art.product.category,
                                variant: a ? "original" : "print"
                            }]
                        }
                    }
                })
            }
        },
        UserBadges: {
            badgesSelector: ".art-detail__recognition__single-badge-wrap",
            recognitionSectionSelector: "#art-detail__recognition",
            init: function () {
                var b = a(this.badgesSelector),
                    c = a(this.recognitionSectionSelector),
                    d = a("html, body");
                b.click(function () {
                    d.animate({
                        scrollTop: c.offset().top + "px"
                    }, 500)
                })
            }
        },
        Layout: {
            detailImage: a(".art-detail-image"),
            detailDescription: a(".art-detail-description"),
            additionalImages: a(".additional-images"),
            init: function () {
                a(".tab-prints, #viewPrints a").on("click", function () {
                    a(".category-size, .packaging-info").addClass("hidden")
                }), a(".tab-original").on("click", function () {
                    a(".category-size, .packaging-info").removeClass("hidden")
                })
            },
            setDetailHeight: function (b) {
                a(window).width() < 1024 || (b ? Saatchi.Art.Layout.detailDescription.css({
                    minHeight: b
                }) : Saatchi.Art.Layout.detailImage.height() >= Saatchi.Art.Layout.detailDescription.height() ? Saatchi.Art.Layout.detailDescription.animate({
                    minHeight: Saatchi.Art.Layout.detailImage.height()
                }, {
                    duration: 0,
                    specialEasing: {
                        height: "swing"
                    }
                }) : Saatchi.Art.Layout.additionalImages.height() >= Saatchi.Art.Layout.detailDescription.height() ? (Saatchi.Art.Layout.detailDescription.animate({
                    minHeight: Saatchi.Art.Layout.additionalImages.height()
                }, {
                    duration: 0,
                    specialEasing: {
                        height: "swing"
                    }
                }), Saatchi.Art.Layout.detailImage.animate({
                    minHeight: Saatchi.Art.Layout.additionalImages.height()
                }, {
                    duration: 0,
                    specialEasing: {
                        height: "swing"
                    }
                })) : Saatchi.Art.Layout.detailDescription.css("minHeight", ""))
            }
        },
        Comment: {
            validator: null,
            processing: !1,
            init: function () {
                a("#commentForm").on("submit", function (b) {
                    return b.preventDefault(), Saatchi.Art.Comment.validate(a(this)), !1
                }), a("body").on("submit", ".delete-comment", function (b) {
                    b.preventDefault(), Saatchi.Art.Comment.removeComment(a(this))
                })
            },
            validate: function (b) {
                var c = b.find('input[type="submit"]');
                c.val("Posting...").addClass("disabled").attr("disabled", !0), Saatchi.Art.Comment.processing || (Saatchi.Art.Comment.processing = !0, a.ajax({
                    url: "/comment/art-validate",
                    data: b.serialize(),
                    success: function (a) {
                        a.result ? ("undefined" != typeof _bt && _bt.track("commented", {
                            model: "art"
                        }), Saatchi.Art.Comment.addComment(b)) : (Saatchi.Dialog.error("We're sorry, but you cannot leave an empty comment."), c.val("Post").removeClass("disabled").attr("disabled", !1), Saatchi.Art.Comment.processing = !1)
                    }
                }))
            },
            addComment: function (b) {
                var c = b.find('input[type="submit"]');
                a.ajax({
                    url: b.attr("action"),
                    data: b.serialize(),
                    success: function (b) {
                        if (b.result) {
                            var d = b.content,
                                e = a("#commentCount"),
                                f = parseInt(e.text().replace(/\(|\)/g, ""));
                            return e.text("(" + (f + 1) + ")"), a(d).insertBefore("#leaveComment"), a('[name="comment"]').val(""), c.val("Post").removeClass("disabled").attr("disabled", !1), void (Saatchi.Art.Comment.processing = !1)
                        }
                        return b.requiresLogin === !0 ? (Saatchi.Signup.init(), Saatchi.Signup.source = "comment", void Saatchi.Signup.openAccountRequired(null, "You must login to comment on art.")) : (Saatchi.Dialog.error("We're sorry, but there was an error posting."), c.val("Post").removeClass("disabled").attr("disabled", !1), void (Saatchi.Art.Comment.processing = !1))
                    }
                })
            },
            removeComment: function (b) {
                b.find('[type="submit"]').attr("disabled", !0).addClass("disabled").val("Deleting..."), a.post(b.attr("action"), b.serialize(), function () {
                    var c = a("#commentCount"),
                        d = parseInt(c.text().replace(/\(|\)/g, ""));
                    c.text("(" + (d - 1) + ")"), b.closest(".comment").remove(), Saatchi.Dialog.success("Comment has been deleted.")
                })
            },
            cancel: function () {
                a('[name="comment"]').val("")
            }
        },
        Products: {
            init: function () {
                var b = a("#productTabs").find("a") || "",
                    c = a(b).attr("href") || "";
                window.location.hash || !Saatchi.Art.original && Saatchi.Art.prints || window.location.search.indexOf("prints=1") > -1 ? (Saatchi.Art.Products.updateProductDisplay("prints"), a(document).scrollTop(0)) : Saatchi.Art.Products.updateProductDisplay("original"), a("#productTabs, #viewPrints").find("a").on("click", function (b) {
                    return b.preventDefault(), a(this).parent().hasClass("active") ? !1 : void Saatchi.Art.Products.updateProductDisplay(a(this).attr("href").replace("#", ""))
                }), c && c.toLowerCase().indexOf("http") >= 0 && a("#productTabs, #viewPrints").find("a").unbind("click")
            },
            updateProductDisplay: function (b) {
                var c = "original" === b ? "prints" : "original",
                    d = a(".view-in-a-room"),
                    e = a(".art-detail__recognition--small");
                if (a(window).width() > 601 && Saatchi.Art.ViewRoom.destroy(), "prints" == b) {
                    d.addClass("hidden");
                    var f = a("#selectMaterial").val();
                    "" !== f && e.hide(), a(window).width() > 601 && f && "" !== f && (Saatchi.Art.ViewRoom.active = !1, Saatchi.Art.ViewRoom.Prints.type = f.replace("_print", ""), Saatchi.Art.ViewRoom.Prints.build()), Saatchi.Art.Layout.setDetailHeight("auto")
                } else e.show(), d.removeClass("hidden"), Saatchi.Art.Layout.setDetailHeight();
                a("#" + b).removeClass("hidden"), a(".tab-" + b).parent().addClass("active"), a("#" + c).addClass("hidden"), a(".tab-" + c).parent().removeClass("active")
            }
        },
        AddToCart: {
            sku: null,
            init: function () {
                a("#addOriginal, .so-art-buy").on("click", function (b) {
                    if (b.preventDefault(), a(this).hasClass("disabled")) return !1;
                    "undefined" != typeof _bt && _bt.track("added_to_cart", {
                        model: "art"
                    });
                    var c = a(this).attr("href");
                    if (a(this).attr("disabled", !0).html("Adding to Cart..."), void 0 === c) return !1;
                    var d = c.lastIndexOf("/") + 1;
                    Saatchi.Art.AddToCart.sku = c.substr(d), Saatchi.GlobalAddToCart.pushToDataLayer({
                        title: Saatchi.Art.product.title,
                        sku: Saatchi.Art.AddToCart.sku,
                        category: Saatchi.Art.product.category,
                        original: "addOriginal" === a(this).attr("id") ? "original" : "print"
                    }), Saatchi.GoogleTracking.trackEvent("commerce", "addToCart", Saatchi.Art.AddToCart.sku, function () {
                        a.ajax({
                            beforeSend: function () { },
                            data: {
                                artist: Saatchi.Art.artist,
                                art: Saatchi.Art.art,
                                safe: Saatchi.Art.safe
                            },
                            timeout: 8e3,
                            url: "/art/process-product",
                            complete: function () {
                                a.ajax({
                                    url: c,
                                    success: function (a) {
                                        Saatchi.Art.AddToCart.addToCartCallback(a)
                                    },
                                    error: function (b) {
                                        Saatchi.Dialog.error("Error while trying to add to cart. Please try again later. (" + b.status + ")"), a("#addOriginal, .so-art-buy").attr("disabled", !1).html("Add to Cart")
                                    }
                                })
                            }
                        })
                    })
                }), Saatchi.Art.AddToCart.Prints.init()
            },
            Prints: {
                init: function () {
                    var b = a(".art-detail__recognition--small");
                    a("#selectMaterial").on("change", function () {
                        var c, d = "-",
                            e = "#",
                            f = a(this).val();
                        if ("" !== f ? b.hide() : b.show(), a(".so-more-tip").removeClass("hidden"), "photo_print" === f) c = a("#photo-selectSize"), a("#photoProductDetails, .photo-print").removeClass("hidden"), a("#ragProductDetails, #canvasProductDetails, .rag-print, .canvas-print").addClass("hidden"), Saatchi.Art.AddToCart.Prints.showFrameOptions(c.find("option:selected").data());
                        else if ("rag_print" === f) c = a("#rag-selectSize"), a("#ragProductDetails, .rag-print").removeClass("hidden"), a("#photoProductDetails, #canvasProductDetails, .photo-print, .canvas-print").addClass("hidden"), Saatchi.Art.AddToCart.Prints.showFrameOptions(c.find("option:selected").data());
                        else {
                            if ("canvas_print" !== f) return a(".select-frame").addClass("hidden"), a("#selectFrame").select2("val", ""), a("#photoProductDetails, #ragProductDetails, #canvasProductDetails, .photo-print, .rag-print, .canvas-print, #materialDetails").addClass("hidden"), Saatchi.Art.ViewRoom.destroy(), Saatchi.Art.AddToCart.Prints.updateSku(e), void Saatchi.Art.AddToCart.Prints.updatePrice(d);
                            c = a("#canvas-selectSize"), a("#canvasProductDetails, .canvas-print").removeClass("hidden"), a("#photoProductDetails, #ragProductDetails, .photo-print, .rag-print").addClass("hidden"), a(".select-frame").addClass("hidden"), a("#selectFrame").select2("val", "")
                        }
                        d = c.find("option:selected").data().price, e = c.val(), Saatchi.Art.AddToCart.Prints.updateSku(e), Saatchi.Art.AddToCart.Prints.updatePrice(d)
                    }), a(".product-select").on("change", function (b) {
                        var c = a(this).find("option:selected").data(),
                            d = c.price;
                        Saatchi.Art.AddToCart.Prints.updatePrice(d), Saatchi.Art.AddToCart.Prints.updateSku(a(this).val()), "canvas_print" !== a("#selectMaterial").val() && Saatchi.Art.AddToCart.Prints.showFrameOptions(c)
                    }), a("#wrap-option").on("change", function () {
                        Saatchi.Art.AddToCart.Prints.updateWrapSku(a(this).val())
                    })
                },
                showFrameOptions: function (b) {
                    var c = "",
                        d = b.frame,
                        e = b.price;
                    if (a("#selectFrame").select2("val", ""), "" !== d) {
                        c += '<option value data-total_price="' + e + '" data-description="">Choose from List</option>';
                        for (var f = 0; f < d.length; f++) /[-!€C£A$$%^&*() _+|~=`{}\[\]:";'<>?,.\/]/.test(d[f].frame_price.charAt(0)) ? d[f].frame_price = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(d[f].frame_price.substring(1))) : d[f].frame_price = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(d[f].frame_price)), c += tmpl("tmpl-frameOptions", d[f]);
                        for (var f = 0; f < d.length; f++) {
                            if (/[-!€C£A$$%^&*()_+|~=`{ }\[\]: ";'<>?,.\/]/.test(d[f].frame_price.charAt(0))) {
                                var g = d[f].frame_price.substring(1);
                                "A" === g || "C" === g ? d[f].frame_price = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(d[f].frame_price.substring(2))) : d[f].frame_price = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(g))
                            } else d[f].frame_price = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(d[f].frame_price));
                            c += tmpl("tmpl-frameOptions", d[f])
                        }
                        a("#selectFrame").html(c), a("#selectFrame").parent(".select-frame").removeClass("hidden"), a(".no-frame").addClass("hidden")
                    } else a(".no-frame").removeClass("hidden"), a(".select-frame").addClass("hidden");
                    a("#selectFrame").on("change", function (b) {
                        var c = a(this).find("option:selected").data(),
                            d = c.description,
                            e = c.total_price,
                            f = Saatchi.Art.AddToCart.Prints.updateCurrencyAndConvert(Number(e));
                        a(".frame-details").removeClass("hide"), a("#frameDescription").html(d.replace(/\|/g, "<br /><br />")), Saatchi.Art.AddToCart.Prints.updatePrice(f), Saatchi.Art.AddToCart.Prints.updateFrameSku(a(this).val()), "" === d && (a(".frame-details").addClass("hide"), Saatchi.Art.AddToCart.Prints.updatePrice(f))
                    })
                },
                updatePrice: function (b) {
                    "-" === b ? a(".so-art-buy").addClass("disabled") : a(".so-art-buy").removeClass("disabled"), a(".art-detail-ribbon").find(".so-subtotal-price").text(b)
                },
                updateCurrencyAndConvert: function (a) {
                    var b, c = Saatchi.Cookie.read("currency"),
                        d = Saatchi.Cookie.read("currency_rate") || 1;
                    "USD" === c && (d = 1);
                    var e = {
                        USDAUD: "A$",
                        USDEUR: "€",
                        USDCAD: "C$",
                        USDGBP: "£",
                        USD: "$"
                    },
                        f = e[c] ? e[c] : "$";
                    return window.INTERNATIONALIZATION_TEST ? b = Math.round(parseFloat(d * a, 10)).toFixed(2) : (b = Math.round(parseFloat(1 * a, 10)).toFixed(2), f = "$"), f + b
                },
                updateSku: function (b) {
                    var c = b;
                    "#" !== b && (c = "/cart/add/" + b), a(".so-art-buy").attr("href", c)
                },
                updateFrameSku: function (b) {
                    var c = a(".so-art-buy");
                    "" == b ? c.attr("href", c.attr("href").replace(/-F[0-9]/, "")) : c.attr("href", c.attr("href").replace(/-F[0-9]/, "") + "-" + b)
                },
                updateWrapSku: function (b) {
                    var c = a(".so-art-buy");
                    c.attr("href", c.attr("href").replace(/-W[0-9][0-9]/, "") + "-" + b)
                }
            },
            addToCartCallback: function (b) {
                if (void 0 !== b && b.result) Saatchi.GoogleTracking.trackEvent("commerce", "successAddToCart", Saatchi.Art.AddToCart.sku, function () {
                    window.location.pathname = "/cart"
                }), Saatchi.GoogleTracking.trackPageview("/checkout/cart");
                else {
                    var c = void 0 !== b && b.message ? b.message : "Unknown exception from Yves/Zed";
                    Saatchi.GoogleTracking.trackEvent("commerce", "failAddToCart", c, function () {
                        void 0 !== b && b.message ? Saatchi.Dialog.error(b.message) : Saatchi.Dialog.error("Error while trying to add to cart. Please try again later. (YZ)"), a("#addOriginal, .so-art-buy").attr("disabled", !1).html("Add to Cart")
                    })
                }
            }
        },
        Offer: {
            link: a("#makeOffer, .make-an-offer-trigger"),
            init: function () {
                Saatchi.Art.Offer.link.on("click", function (a) {
                    return Saatchi.Signup.isLoggedIn ? void 0 : (a.preventDefault(), Saatchi.Signup.source = "art-offer", Saatchi.Signup.openAccountRequired(Saatchi.Art.Offer.link.attr("href"), "You must have an account to make an offer."), !1)
                })
            }
        },
        Favorite: {
            btn: a(".sa-add-to-favorites"),
            init: function () {
                var b = Saatchi.Art.Favorite.btn.find("span").text();
                Saatchi.Art.Favorite.btn.on("click", function (c) {
                    if (c.preventDefault(), !Saatchi.Signup.isLoggedIn) return Saatchi.Signup.source = "favorite", Saatchi.Signup.openAccountRequired(null, "You must have an account to favorite art."), !1;
                    "undefined" != typeof _bt && _bt.track("favorited", {
                        model: "art"
                    });
                    var d = a(this),
                        e = d.data("id"),
                        f = d.data("user"),
                        g = d.data("favorite"),
                        h = {
                            id: f,
                            id_user_art: e,
                            favorite: g
                        },
                        i = d.find("i.fa-heart"),
                        j = d.find(".round"),
                        k = d.find("span"),
                        l = a("#favoriteCount");
                    return Saatchi.Favorite.update(h) || (i.hasClass("is-favorite") ? (b = "Add to Favorites", i.removeClass("is-favorite").removeAttr("style"), j.removeAttr("style"), k.text(b), d.data("favorite", !1).attr("title", b), l.text(parseInt(l.text()) - 1)) : (b = "Added To Favorites", i.addClass("is-favorite").removeAttr("style"), j.removeAttr("style"), k.text(b), d.data("favorite", !0).attr("title", b), l.text(parseInt(l.text()) + 1)), i.removeClass("fa-heart-o")), !1
                }), Saatchi.Art.Favorite.btn.on("mouseenter", function (b) {
                    b.preventDefault(), Saatchi.Art.Favorite.isFavorite() && (a(this).find(".is-favorite").addClass("fa-heart-o"), a(this).find("span").text("Remove Favorite"))
                }).on("mouseleave", function () {
                    Saatchi.Art.Favorite.isFavorite() && (a(this).find(".is-favorite").removeClass("fa-heart-o"), a(this).find("span").text(b))
                })
            },
            getLabel: function () {
                return Saatchi.Art.Favorite.btn.find("span").text()
            },
            isFavorite: function () {
                return Saatchi.Art.Favorite.btn.data("favorite")
            }
        },
        ViewRoom: {
            active: !1,
            animation: !1,
            initialDetailHeight: null,
            unitOfMeasure: "",
            type: null,
            init: function () {
                var a = "";
                if ("CENTIMETER" === Saatchi.Art.ViewRoom.unitOfMeasure && (a = "_cm"), Saatchi.Art.original || Saatchi.Art.prints) {
                    var b = new Image;
                    b.src = Saatchi.Loader.getCDNPrefix() + "/content/view_in_room_couch_plant" + a + ".jpg"
                }
                if (Saatchi.Art.original) {
                    Saatchi.Art.ViewRoom.Original.init();
                    for (var c = ["couch" + a, "empty", "plant" + a, "oversized_couch" + a, "oversized_couch_plant" + a, "oversized_empty", "oversized_plant" + a], d = [], e = 0; e < c.length; e++) d[e] = new Image, d[e].src = Saatchi.Loader.getCDNPrefix() + "/content/view_in_room_" + c[e] + ".jpg"
                }
                Saatchi.Art.prints && Saatchi.Art.ViewRoom.Prints.init(), Saatchi.Art.ViewRoom.initialDetailHeight = Saatchi.Art.Layout.detailImage.height()
            },
            Original: {
                data: [],
                init: function () {
                    var b = ["Sculpture", "Installation", "Video"]; -1 === a.inArray(Saatchi.Art.ViewRoom.Original.data.type, b) && a(".view-in-a-room").on("click", function (a) {
                        a.preventDefault(), "undefined" != typeof _bt && _bt.track("view_in_room", {
                            model: "art"
                        }), Saatchi.Art.Layout.detailImage.css("height", "100vh"), Saatchi.Art.ViewRoom.animation !== !0 && (Saatchi.Art.ViewRoom.animation = !0, Saatchi.Art.ViewRoom.active ? Saatchi.Art.ViewRoom.destroy() : Saatchi.Art.ViewRoom.Original.build())
                    })
                },
                build: function () {
                    var b = Saatchi.Art.ViewRoom.Original.data,
                        c = b.height > 60 || b.width > 90,
                        d = c ? 5.27 : 6.53,
                        e = c ? 5.16 : 6.09,
                        f = Math.ceil(b.width * d),
                        g = Math.ceil(b.height * e),
                        h = a(".art-detail-image"),
                        i = h.find("img"),
                        j = "";
                    "CENTIMETER" === Saatchi.Art.ViewRoom.unitOfMeasure && (j = "_cm"), a(window).width() < 1024 && h.height(790), Saatchi.Art.ViewRoom.type = "original", i.stop(!0, !0), a(".art-detail-image div:first-child").width(660);
                    var k = 0,
                        l = c ? 425 : 352;
                    l > g ? k = l - g : 654 >= g && (k = Math.floor((654 - g) / 2)), h.before('<div id="fakeFade" style="width:100%;height:100%;background:#fff;position:absolute;margin: -20px 0 0 -20px;"></div>'), i.css({
                        boxShadow: "2px 2px 3px rgba(0, 0, 0, 0.1)"
                    }), i.animate({
                        width: f,
                        height: "auto",
                        marginTop: k
                    }, {
                        duration: 750,
                        specialEasing: {
                            width: "swing"
                        },
                        start: function () {
                            a(".art-detail-body").css({
                                backgroundImage: "url(/content/view_in_room_" + Saatchi.Art.ViewRoom.Original.setRoom(b.width, b.height, c) + j + ".jpg)",
                                backgroundRepeat: "no-repeat",
                                backgroundColor: "transparent"
                            }), a("#fakeFade").fadeOut(750), Saatchi.Art.ViewRoom.Original.setButtonState(), Saatchi.Art.Layout.setDetailHeight(770)
                        },
                        done: function () {
                            Saatchi.Art.ViewRoom.active = !0, Saatchi.Art.ViewRoom.animation = !1;
                            var b = !1,
                                c = i.height() - g;
                            (c > 45 && 60 > c || -45 > c && c > -60) && (i.height() > g ? b = k - (i.height() - g) : i.height() < g && (b = k + (g - i.height()))), b && b > 0 && i.animate({
                                marginTop: b
                            }), a("#fakeFade").remove()
                        }
                    })
                },
                setButtonState: function (b) {
                    var c = a(".art-image-actions .view-in-a-room");
                    Saatchi.Art.ViewRoom.active || b ? (c.find("span").text("View in a Room"), c.find("i").attr("class", "icn-view-in-a-room")) : (c.find("span").text("Back to Artwork"), c.find("i").attr("class", "fa fa-arrow-left"))
                },
                setRoom: function (a, b, c) {
                    var d = "empty",
                        e = c ? 80 : 57,
                        f = c ? 77 : 78;
                    return e > b && f > a && (d = "couch_plant"), e > b && a > f && (d = "couch"), b >= e && f >= a && (d = "plant"), c ? "oversized_" + d : d
                }
            },
            Prints: {
                frame: !1,
                init: function () {
                    a("#selectMaterial").on("change", function () {
                        a("#frame, #mat").remove(), Saatchi.Art.ViewRoom.type = a(this).val().replace("_print", ""), "" === Saatchi.Art.ViewRoom.type ? Saatchi.Art.ViewRoom.destroy() : Saatchi.Art.ViewRoom.Prints.build()
                    }), a(".product-select").on("change", function () {
                        a("#frame, #mat").remove(), Saatchi.Art.ViewRoom.Prints.build()
                    }), a("#selectFrame").on("change", function () {
                        Saatchi.Art.ViewRoom.Prints.frame = a(this).find("option:selected").data(), a("#frame, #mat").remove(), "" !== Saatchi.Art.ViewRoom.Prints.frame.description ? Saatchi.Art.ViewRoom.Prints.addFrame() : Saatchi.Art.ViewRoom.Prints.build()
                    }), a("#wrap-option").on("change", function () {
                        var b = "#fff";
                        "W39" === a(this).val() && (b = "#000"), a(".art-detail-image img").css("borderColor", b)
                    }), a(window).on("resize", function () {
                        "" !== a("#selectFrame").val() && Saatchi.Art.ViewRoom.active && "original" !== Saatchi.Art.ViewRoom.type && (a("#frame, #mat").remove(), Saatchi.Art.ViewRoom.Prints.addFrame())
                    }), a(document).on("click", "#mat", function (b) {
                        b.preventDefault(), a(".art-detail-image img").trigger("click")
                    })
                },
                build: function () {
                    var b = a("#selectMaterial").val().replace("_print", ""),
                        c = a("#" + b + "-selectSize").find("option:selected").data(),
                        d = "canvas" !== b ? 8.23 : 6.53,
                        e = "canvas" !== b ? 8.23 : 6.48,
                        f = Math.ceil(c.width * d),
                        g = Math.ceil(c.height * e),
                        h = a(".art-detail-image"),
                        i = h.find("img"),
                        j = {
                            boxShadow: "2px 2px 3px rgba(0, 0, 0, 0.1)",
                            background: "",
                            border: "",
                            padding: "",
                            borderRight: "",
                            borderTop: "",
                            borderRadius: ""
                        },
                        k = "";
                    "CENTIMETER" === Saatchi.Art.ViewRoom.unitOfMeasure && (k = "_cm"), Saatchi.Art.ViewRoom.type = b, a(window).width() < 1024 && h.height(790), i.stop(!0, !0), Saatchi.Art.ViewRoom.active || h.before('<div id="fakeFade" style="width:100%;height:100%;background:#fff;position:absolute;margin: -20px 0 0 -20px;"></div>'), a(".art-detail-image div:first-child").width(660);
                    var l = 367 - g;
                    if (i.css(j), "canvas" != b) j = {
                        background: "#FFF",
                        border: "1px solid #EBEBEB",
                        padding: "15px"
                    };
                    else {
                        var m = a("#wrap-option").val(),
                            n = "W39" === m ? "#000" : "#fff";
                        j = {
                            borderRight: "5px solid " + n,
                            borderBottom: "5px solid " + n,
                            borderRadius: "0 5px 0 5px"
                        }
                    }
                    i.css(j), i.animate({
                        width: f,
                        height: "auto",
                        marginTop: l
                    }, {
                        duration: 750,
                        specialEasing: {
                            width: "swing"
                        },
                        start: function () {
                            a(".art-detail-body").css({
                                backgroundImage: "url(/content/view_in_room_couch_plant" + k + ".jpg)",
                                backgroundRepeat: "no-repeat",
                                backgroundColor: "transparent"
                            }), a("#fakeFade").fadeOut(750), Saatchi.Art.Layout.setDetailHeight("auto"), i.css(j)
                        },
                        done: function () {
                            Saatchi.Art.ViewRoom.active = !0, Saatchi.Art.ViewRoom.animation = !1, "" !== a("#selectFrame").val() && "canvas" !== b ? Saatchi.Art.ViewRoom.Prints.addFrame() : a("#frame, #mat").remove(), a("#fakeFade").remove()
                        }
                    })
                },
                addFrame: function () {
                    var b, c = a(".art-detail-image img"),
                        d = {
                            1: {
                                color: "black",
                                size: 1.25,
                                mat: !0
                            },
                            2: {
                                color: "white",
                                size: 1.25,
                                mat: !0
                            },
                            3: {
                                color: "black",
                                size: 1.25,
                                mat: !1
                            },
                            4: {
                                color: "white",
                                size: 1.25,
                                mat: !1
                            },
                            5: {
                                color: "black",
                                size: 2,
                                mat: !0
                            },
                            6: {
                                color: "white",
                                size: 2,
                                mat: !0
                            },
                            7: {
                                color: "black",
                                size: 2,
                                mat: !1
                            },
                            8: {
                                color: "white",
                                size: 2,
                                mat: !1
                            }
                        },
                        e = d[Saatchi.Art.ViewRoom.Prints.frame.framing_type_id];
                    e && (c.parent().css("zIndex", 2), c.css("boxShadow", "inset 1px 1px 5px rgba(0, 0, 0, 0.1)"), a("body").append('<div id="frame"></div>'), b = 2 === e.size ? {
                        height: c.height() + 54,
                        width: c.width() + 53,
                        left: c.offset().left - 11,
                        top: c.offset().top - 11
                    } : {
                        height: c.height() + 48,
                        width: c.width() + 47,
                        left: c.offset().left - 8,
                        top: c.offset().top - 8
                    }, a("#frame").css({
                        backgroundColor: e.color,
                        boxShadow: "2px 2px 3px rgba(0, 0, 0, 0.1)",
                        position: "absolute",
                        zIndex: 1
                    }).css(b), e.mat && (a("#frame").before('<div id="mat"></div>'), a("#mat").css({
                        height: c.height() + 2,
                        width: c.width() + 2,
                        position: "absolute",
                        left: c.offset().left + 14,
                        top: c.offset().top + 14,
                        borderLeft: "2px solid #CCC",
                        zIndex: 3,
                        borderTop: "2px solid #CCC",
                        borderBottom: "2px solid #EFE",
                        borderRight: "2px solid #EFE"
                    })))
                }
            },
            destroy: function () {
                var b = a(".art-detail-image img");
                a("#frame, #mat").remove(), a(".art-detail-image div:first-child").width(""), b.css({
                    boxShadow: "",
                    background: "",
                    border: "",
                    padding: "",
                    borderRight: "",
                    borderTop: "",
                    borderRadius: ""
                }), a(".art-detail-body").removeAttr("style"), b.animate({
                    width: b.data().width,
                    height: "auto",
                    marginTop: 0
                }, {
                    duration: 750,
                    specialEasing: {
                        width: "swing"
                    },
                    start: function () {
                        Saatchi.Art.ViewRoom.Original.setButtonState(!0), a(window).width() < 1024 && a(".art-detail-image").animate({
                            height: Saatchi.Art.ViewRoom.initialDetailHeight
                        }, 0, function () {
                            a(".art-detail-image").height("")
                        })
                    },
                    done: function () {
                        b.removeAttr("style"), Saatchi.Art.ViewRoom.active = !1, Saatchi.Art.ViewRoom.animation = !1, Saatchi.Art.Layout.setDetailHeight(Saatchi.Art.ViewRoom.initialDetailHeight), a(".art-detail-image").css("height", b.height())
                    }
                })
            }
        },
        Fullscreen: {
            image: null,
            protect: !0,
            init: function () {
                a(window).width() < 450 || a("[data-orbit-slide] img, .fullscreen").each(function (b, c) {
                    var d, e = a(this);
                    d = "A" == c.tagName ? a(c).attr("href") : a(c).attr("src").replace("-7.", "-8."), e.qtip({
                        content: {
                            text: (Saatchi.Art.Fullscreen.protect ? '<div class="protect"></div>' : "") + '<img src="' + d + '">',
                            title: {
                                text: '<img src="https://dfcdths9j2gip.cloudfront.net/images/saatchiart-logo@1x.png">',
                                button: "Close"
                            }
                        },
                        show: {
                            event: "click",
                            solo: !0,
                            modal: !0,
                            effect: function (b) {
                                a(this).fadeIn(750), a("#livechat-full, #livechat-compact-container, #livechat-eye-catcher").css("z-index", "0")
                            }
                        },
                        hide: {
                            target: a("body"),
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
                            target: a(document),
                            effect: !1
                        },
                        events: {
                            render: function (b, c) {
                                a(this).find(".ui-tooltip-icon[title]").addClass("so-quick-tip"), Saatchi.Tips.init(), Saatchi.GoogleTracking.trackEvent("Art Detail Page", "View Zoom Layout of Artwork")
                            },
                            show: function (b, c) {
                                a(".content, #footer").hide(), e.qtip("option", {
                                    "position.my": "top center",
                                    "position.at": "top center"
                                }), c.elements.overlay.addClass("white")
                            },
                            hide: function (b, c) {
                                a(".content, #footer").show(), c.elements.overlay.removeClass("white")
                            }
                        }
                    })
                }).bind("click", function (a) {
                    return a.preventDefault(), !1
                })
            }
        },
        Render: {
            init: function () {
                this.artistArt(), this.relatedArt()
            },
            artistArt: function () {
                var b = a("#artistArt");
                a.ajax("/art/artist-art", {
                    type: "GET",
                    data: {
                        artist: Saatchi.Art.artist,
                        art: Saatchi.Art.art,
                        safe: Saatchi.Art.safe
                    },
                    timeout: 5e3,
                    success: function (a) {
                        a.data && a.data.length > 0 ? b.toggleClass("hidden").find(".item-list").append(a.data) : b.remove()
                    },
                    error: function () {
                        b.remove()
                    }
                })
            },
            relatedArt: function () {
                var b = a("#relatedArt");
                a.ajax("/art/related-art", {
                    type: "GET",
                    data: {
                        artist: Saatchi.Art.artist,
                        art: Saatchi.Art.art,
                        safe: Saatchi.Art.safe
                    },
                    timeout: 5e3,
                    success: function (a) {
                        a.data && a.data.length > 0 ? b.toggleClass("hidden").find(".item-list").append(a.data) : b.remove()
                    },
                    error: function () {
                        b.remove()
                    }
                })
            }
        },
        MultipleItems: {
            init: function () {
                document.getElementById("preview-template") && (this.initDropzone(), this.initSortable()), this.showForm(), a(".view-in-a-room, #viewPrints a, .tab-prints").on("click", function () {
                    Saatchi.Art.MultipleItems.disable()
                }), a(".tab-original").on("click", function () {
                    Saatchi.Art.MultipleItems.enable()
                }), a("#selectMaterial").on("change", function () {
                    Saatchi.Art.MultipleItems.disable()
                }), a("#save-images").on("click", function (a) {
                    Saatchi.Art.MultipleItems.submit()
                }), a("#save-description").on("click", function (b) {
                    b.preventDefault();
                    var c = a("#itemId").val(),
                        d = a("#caption").val();
                    Saatchi.Art.MultipleItems.setDescription(c, d)
                })
            },
            initDropzone: function () {
                Saatchi.Art.dropzone = new Dropzone("#myDropzone", {
                    paramName: "file",
                    maxFilesize: 50,
                    clickable: "#upload-to-s3",
                    maxFiles: 5,
                    previewTemplate: document.querySelector("#preview-template").innerHTML,
                    addRemoveLinks: !0,
                    init: function () {
                        var b = this;
                        b.on("sending", function (b) {
                            a("#save-images").attr("disabled", !0)
                        }), b.on("removedfile", function (b) {
                            a("#multiple-item-form").trigger("reset").hide("slow"), a(".multiple-item-form .form").hide("slow")
                        }), b.on("maxfilesexceeded", function (a) {
                            this.removeFile(a)
                        }), a.each(Saatchi.Art.additionalImages, function (a, c) {
                            var d = {
                                name: c.prefix,
                                prefix: c.prefix,
                                description: c.extra_fields.description
                            },
                                e = "//" + Saatchi.Art.imgHostUri + d.prefix + "22.jpg";
                            b.emit("addedfile", d), b.emit("success", d, {
                                s3key: c.s3.key,
                                prefix: c.prefix
                            }), b.emit("thumbnail", d, e)
                        })
                    },
                    accept: function (b, c) {
                        a(b.previewElement).attr("staging", 1), c()
                    },
                    success: function (b, c) {
                        a(b.previewElement).attr("id", "s3-" + c.s3key), a(b.previewElement).attr("prefix", c.prefix), a(b.previewElement).attr("data-text", b.description), a(b.previewElement).click()
                    },
                    complete: function (b, c) {
                        0 === this.getUploadingFiles().length && 0 === this.getQueuedFiles().length && a("#save-images").removeAttr("disabled")
                    },
                    error: function (a, b) {
                        this.removeFile(a), b.hasOwnProperty("error") ? Saatchi.Dialog.error(b.error) : Saatchi.Dialog.error(b)
                    }
                })
            },
            initSortable: function () {
                Sortable.create(document.getElementById("myDropzone"), {
                    animation: 150,
                    handle: ".dz-preview",
                    draggable: ".dz-preview"
                })
            },
            showForm: function () {
                a("#openMultipleUploadForm").on("click", function () {
                    a(".multiple-item-form .form").hide(), a(".multiple-item-form").show("slow"), a("html, body").animate({
                        scrollTop: a(".multiple-item-form").offset().top
                    }, 1e3)
                }), Saatchi.Art.MultipleItems.editForm()
            },
            editForm: function () {
                a(document).on("click", ".dz-preview", function (b) {
                    b.preventDefault();
                    var c = a(this).attr("id"),
                        d = a(this).attr("data-text");
                    a("#multiple-item-form").trigger("reset").show("slow"), a(".multiple-item-form .form").show("slow"), a(".dz-preview").removeClass("active"), a(this).addClass("active"), a("#itemId").val(c), a("#caption").val(d), a("#caption").focus(), a("#caption-label label").text("Edit caption:")
                })
            },
            deleteItem: function () { },
            disable: function () {
                a("#theArtwork").attr("style", "height: 100vh;"), a("li.flexcontainer").attr("style", "z-index: 2; margin: 100%; opacity: 0;"), a(".additional-images li").removeClass("active"), a("#artworkThumb").addClass("active"), a(".additional-images").is(":visible") ? a(".additional-images").hide() : (a(".tab-prints").parent().hasClass("active") ? a(".additional-images").hide() : a(".additional-images").toggle(), a("#theArtwork").attr("style", "z-index: 4; margin: 0%; opacity: 1;").addClass("active"), a(".flexcontainer").removeClass("active"), a(".additional-images li").on("click", function () {
                    if (!a(this).hasClass("active")) {
                        a(this).addClass("active").siblings().removeClass("active");
                        var b = a(this).attr("data-orbit-link");
                        a(".art-detail-image").find("[data-orbit-slide='" + b + "']").attr("style", "z-index: 4; margin: 0%; opacity: 1;").addClass("active"), a("#theArtwork").attr("style", "z-index: 2; margin: 100%; opacity: 0;").removeClass("active")
                    }
                })), a(".multiple-item-form").hide()
            },
            enable: function () {
                a(".art-detail-image li").attr("style", ""), a(".additional-images").show()
            },
            setDescription: function (b, c) {
                a("#" + b).attr("data-text", c), Saatchi.Dialog.success("Thank you! Description was successfully saved.")
            },
            submit: function () {
                var b = {
                    id_user_art: Saatchi.Art.id_user_art,
                    extra_images: [],
                    extra_images_length: 0
                };
                a("#myDropzone .dz-preview").each(function (c, d) {
                    b.extra_images.push({
                        s3key: a(d).attr("id"),
                        description: a(d).attr("data-text"),
                        prefix: a(d).attr("prefix"),
                        is_staging: a(d).attr("staging")
                    })
                }), b.extra_images_length = b.extra_images.length, a.ajax({
                    type: "POST",
                    dataType: "json",
                    url: a("#multiple-item-form").attr("action"),
                    data: b,
                    beforeSend: function (b) {
                        a("#save-images-loader").css("display", "block"), Saatchi.Dialog.success("Please wait. Saving additional images.")
                    },
                    success: function (a) {
                        Saatchi.Dialog.success("Congrats! Additional images successfully submitted."), setTimeout(function () {
                            location.reload()
                        }, 200)
                    },
                    error: function (b, c, d) {
                        500 == b.status ? Saatchi.Dialog.error("We're sorry, but there was an error on our side processing your request.") : Saatchi.Dialog.error(b.responseJSON.error), a("#save-images-loader").css("display", "none")
                    }
                })
            }
        }
    }
}(jQuery);
Saatchi.StickyNav = function (a) {
    "use strict";
    var b, c, d, e;
    return {
        triggerElSelector: "#addOriginal",
        navSelector: ".art-detail__sticky-nav",
        navActiveClass: "art-detail__sticky-nav--visible",
        printsTabSelector: ".tab-prints",
        originalTabSelector: ".tab-original",
        init: function () {
            c = a(this.triggerElSelector), d = a(this.navSelector);
            var b = a(this.printsTabSelector).parent(),
                f = a(this.originalTabSelector).parent();
            e = b.hasClass("active"), b.click(function () {
                e = !0
            }), f.click(function () {
                e = !1
            }), this.cacheDimensions(), a(document).ready(this.onScroll.bind(this)), window.addEventListener("resize", this.cacheDimensions), window.addEventListener("scroll", this.onScroll.bind(this))
        },
        onScroll: function (a) {
            var c = !e && window.scrollY >= b ? "addClass" : "removeClass";
            d[c](this.navActiveClass)
        },
        cacheDimensions: function () {
            b = c.offset().top + c.height()
        }
    }
}(jQuery);
﻿using ArtShop.Models;
using ArtShop.Util;
using DataLayer;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Extentions;
using Microsoft.AspNet.Identity;

namespace ArtShop.Controllers
{
    public class CardController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Route("checkout")]
        public ActionResult Index()
        {
            var cart = CartManager.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        public ActionResult getinfo()
        {
            return View();
        }
  
        [Authorize]
        public ActionResult Pay()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var cart = CartManager.GetCart(this.HttpContext);
            int amount = (int)cart.GetTotal();

            Order o = new Order() { user_id = userId,OrderDetails = new List<OrderDetail>() };
            var orderId = cart.CreateOrder(o);
            var neworder = db.Orders.Find(orderId);

            System.Net.ServicePointManager.Expect100Continue = false;
            ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient zp = new ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;
            int Status = zp.PaymentRequest("test",
                amount,
                profile.FirstName + " " + profile.LastName,
                user.Email,
                user.PhoneNumber,
                "http://artiscovery.com/card/Verify",
                out Authority);

            long longAuth = 0;
            long.TryParse(Authority, out longAuth);
            neworder.TransactionDetail.Number = longAuth.ToString();
            db.SaveChanges();
            if (Status == 100)
            {
                Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                return View();
            }
            else
            {
                return Content("error: " + Status);
            }
        }

        public ActionResult verify()
        {
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {
                    long longAuth = 0;
                    long.TryParse(Request.QueryString["Authority"], out longAuth);
                    var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == longAuth.ToString());
                    if (tran != null)
                    {
                        decimal Amount = tran.amount;
                        long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient zp =
                            new ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient();
                        int Status = zp.PaymentVerification("e21e6fde-e5f6-11e6-bdac-000c295eb8fc", Request.QueryString["Authority"].ToString(), (int)Amount, out RefID);
                        tran.TransactionNumber = RefID.ToString();
                        tran.Description = Status.ToString();
                        if (Status == 100)
                        {
                            tran.Payed = true;
                            db.SaveChanges();
                            ViewBag.text = "پرداخت با موفقیت انجام شد";
                            ViewBag.refid = RefID.ToString();
                            CartManager.GetCart(this.HttpContext).EmptyCart();
                            return View();
                        }
                        else
                        {
                            db.SaveChanges();
                            ViewBag.text = "پرداخت نا موفق";
                            ViewBag.refid = RefID.ToString();
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.text = "پرداخت نا موفق";
                        return View();
                    }
                }
                else
                {
                    long longAuth = 0;
                    long.TryParse(Request.QueryString["Authority"], out longAuth);
                    var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == longAuth.ToString());
                    if (tran != null)
                    {
                        tran.Description = Request.QueryString["Status"].ToString();
                        db.SaveChanges();
                    }
                    ViewBag.text = "پرداخت نا موفق";
                    return View();
                }
            }
            else
            {
                ViewBag.text = "پرداخت نا موفق";
                return View();
            }
        }



        public ActionResult AddToCart(int id, Ordertype type)
        {
            var addedAlbum = db.Products.Find(id);
            var cart = CartManager.GetCart(this.HttpContext);
            cart.AddToCart(addedAlbum, type);
            return Redirect("/checkout");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = CartManager.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string Pname = db.ShoppingCarts
                .Single(item => item.Id == id).Product.Title;

            // Remove from cart
            decimal itemCount = cart.RemoveAllFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(Pname) +
                    " با موفقیت از سبد خرید حذف شد ",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        [HttpPost]
        public ActionResult changecnt(int id, int cnt)
        {
            // Remove the item from the cart
            var cart = CartManager.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string Pname = db.ShoppingCarts
                .Single(item => item.Id == id).Product.Current().Title;

            // Remove from cart
            decimal itemCount = cart.newcnt(id, cnt);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(Pname) +
                    " تغییر کرد ",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = CartManager.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
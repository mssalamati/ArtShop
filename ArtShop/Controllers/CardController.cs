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
using Postal;
using paypal = PayPal.Api;

namespace ArtShop.Controllers
{
    public class CardController : BaseController
    {
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

        [Authorize]
        public ActionResult getinfo(int c = 0)
        {
            var user = db.UserProfiles.Find(User.Identity.GetUserId());
            ViewBag.name = user.FirstName;
            ViewBag.lastname = user.LastName;
            if (user.billingInfo != null)
            {
                ViewBag.Street = user.billingInfo.Street + " " + user.billingInfo.Unit;
                ViewBag.City = user.billingInfo.City;
                ViewBag.Country = user.billingInfo.CountryId;
                ViewBag.Region = user.billingInfo.Region;
                ViewBag.ZipCode = user.billingInfo.ZipCode;
                ViewBag.PhoneNumber = user.billingInfo.PhoneNumber;
            }
            if (c != 0)
            {
                ViewBag.Country = c;
            }
            var cart = CartManager.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Pay(checkInfoViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var cart = CartManager.GetCart(this.HttpContext);
            var cartItems = cart.GetCartItems();
            var setting = db.SettingValues.FirstOrDefault();
            decimal orderTotal = 0;
            Order o = new Order()
            {
                user_id = userId,
                OrderDetails = new List<OrderDetail>(),
                ReceiverName = model.firstname + " " + model.lastname,
                Address = model.address,
                CountryCode = model.country,
                City = model.city,
                PhoneNumber = model.PhoneNumber,
            };
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity,
                    type = item.type
                };
                orderTotal += (item.Quantity * item.Product.Price);
                o.OrderDetails.Add(orderDetail);
            }
            o.TotalPrice = (double)orderTotal;
            o.TransactionDetail = new TransactionDetail()
            {
                amount = orderTotal,
                Method = model.paymentMethod,
                currencyRate = setting.IRRialRate
            };
            db.Orders.Add(o);
            db.SaveChanges();

            if (model.paymentMethod == PaymentMethod.zarinpall)
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient zp = new ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient();
                string Authority;
                int Status = zp.PaymentRequest("test", (int)(orderTotal * setting.IRRialRate), profile.FirstName + " " + profile.LastName, user.Email, user.PhoneNumber, "http://artiscovery.com/card/Verify", out Authority);
                long longAuth = 0;
                long.TryParse(Authority, out longAuth);
                o.TransactionDetail.Number = longAuth.ToString();
                db.SaveChanges();
                if (Status == 100)
                {
                    Response.RedirectPermanent("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                    return View();
                }
                else
                {
                    return Content("error: " + Status);
                }
            }
            else
            {
                try
                {
                    var apiContext = getPaypalApiContect();

                    var items = cartItems.Select(x => new paypal.Item()
                    {
                        name = x.Product.Title,
                        currency = "USD",
                        price = ((int)x.Product.Price).ToString(),
                        quantity = ((int)x.Quantity).ToString(),
                        sku = "sku"
                    }).ToList();

                    var payment = paypal.Payment.Create(apiContext, new paypal.Payment
                    {
                        intent = "sale",
                        payer = new paypal.Payer
                        {
                            payment_method = "paypal"
                        },
                        transactions = new List<paypal.Transaction>
                        {
                        new paypal.Transaction
                        {
                            description = "Artiscovery shopping store",
                            invoice_number = o.Id.ToString(),
                            amount = new paypal.Amount
                            {
                                currency = "USD",
                                total = orderTotal.ToString(),
                                details = new paypal.Details
                                {
                                    tax = "0",
                                    shipping = "0",
                                    subtotal = orderTotal.ToString()
                                }
                            },
                            item_list = new paypal.ItemList
                            {
                                items = items
                            }

                        }
                    },
                        redirect_urls = new paypal.RedirectUrls
                        {
                            return_url = "https://artiscovery.com/card/PaypalReturn",
                            cancel_url = "https://artiscovery.com/card/PaypalCancel"
                        }
                    });
                    //paypal refrence id
                    o.TransactionDetail.Number = payment.id;
                    db.SaveChanges();
                    var approveurl = payment.links.FirstOrDefault(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));
                    return RedirectPermanent(approveurl.href);
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }
            }
        }



        public ActionResult verify()
        {
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                long longAuth = 0;
                long.TryParse(Request.QueryString["Authority"], out longAuth);
                var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == longAuth.ToString());
                var order = db.Orders.FirstOrDefault(x => x.TransactionDetailId == tran.Id);
                var orderId = order.Id;
                if (tran != null)
                {
                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        decimal Amount = tran.amount * tran.currencyRate;
                        long RefID;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient zp = new ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient();
                        int Status = zp.PaymentVerification("e21e6fde-e5f6-11e6-bdac-000c295eb8fc", Request.QueryString["Authority"].ToString(), (int)Amount, out RefID);
                        tran.TransactionNumber = RefID.ToString();
                        tran.Description = Status.ToString();
                        if (Status == 100)
                        {
                            tran.Payed = true;
                            foreach (var item in order.OrderDetails)
                            {
                                item.Product.user.Account += (item.UnitPrice * item.Quantity) * (decimal)((100d - 10d) / 100d);
                                item.Product.AllEntity--;
                            }
                            db.SaveChanges();
                            SendOrderDetail(order);
                            SendInvoice(order);
                            CartManager.GetCart(this.HttpContext).EmptyCart();
                            return RedirectToActionPermanent("paymentReport", new { id = orderId });
                        }
                        else
                        {
                            db.SaveChanges();
                            return RedirectToActionPermanent("paymentReport", new { id = orderId });
                        }
                    }
                    else
                    {
                        tran.Description = Request.QueryString["Status"].ToString();
                        db.SaveChanges();
                        return RedirectToActionPermanent("paymentReport", new { id = orderId });
                    }
                }
                else
                {
                    return Content("not valid address");
                }
            }
            else
            {
                return Content("not valid address");
            }
        }

        public ActionResult PaypalReturn(string payerId, string paymentId)
        {
            var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == paymentId);
            //var apiContext = getPaypalApiContect();
            //var paymentExecution = new paypal.PaymentExecution() { payer_id = payerId };
            //var payment = new paypal.Payment();
            //var executedpayment = payment.Execute(apiContext, paymentExecution);
            var order = db.Orders.FirstOrDefault(x => x.TransactionDetailId == tran.Id);
            var orderId = order.Id;
            tran.Payed = true;
            foreach (var item in order.OrderDetails)
                item.Product.user.Account += (item.UnitPrice * item.Quantity) * (decimal)((100d - 10d) / 100d);
            db.SaveChanges();
            SendOrderDetail(order);
            SendInvoice(order);
            CartManager.GetCart(this.HttpContext).EmptyCart();
            return RedirectToActionPermanent("paymentReport", new { id = orderId });
        }

        public ActionResult PaypalCancel(string payerId, string paymentId)
        {
            var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == paymentId);
            var apiContext = getPaypalApiContect();
            var paymentExecution = new paypal.PaymentExecution() { payer_id = payerId };
            var payment = new paypal.Payment();
            var executedpayment = payment.Execute(apiContext, paymentExecution);
            var order = db.Orders.FirstOrDefault(x => x.TransactionDetailId == tran.Id);
            var orderId = order.Id;
            return RedirectToActionPermanent("paymentReport", new { id = orderId });
        }

        [Authorize]
        public ActionResult paymentReport(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var order = profile.Orders.SingleOrDefault(x => x.Id == id);
            return View(order);
        }



        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        [NoCache]
        public ActionResult AddToCart(int id, Ordertype type)
        {
            var addedAlbum = db.Products.Find(id);
            var cart = CartManager.GetCart(this.HttpContext);
            var basket = cart.GetCountProduct(id);
            if (basket + 1 > addedAlbum.AllEntity)
                return RedirectPermanent("/checkout");
            cart.AddToCart(addedAlbum, type);
            return RedirectPermanent("/checkout");
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

        private void SendInvoice(Order order)
        {

            dynamic email = new Email("Invoice");
            email.To = User.Identity.GetUserName();
            email.Subject = "Invoice";
            email.orderid = order.Id;
            email.fullname = order.ReceiverName;
            email.address = order.Address;
            email.shipaddress = order.Address;
            email.orderdate = order.BuyDate.ToString();
            email.products = order.OrderDetails.ToList();
            email.subtotal = order.TotalPrice;
            email.total = order.TotalPrice;
            email.Send();

        }
        private void SendOrderDetail(Order order)
        {
            foreach (var item in order.OrderDetails.GroupBy(a => a.Product.user))
            {
                var user = item.Key;
                dynamic email = new Email("Order");
                email.To = user.ApplicationUserDetail.Email;
                email.Subject = "Order";
                email.orderid = order.Id;
                email.ReceiverName = order.ReceiverName;
                email.orderdate = order.BuyDate.ToString();
                email.products = item.Select(x => new productemailviewmodel()
                {
                    title = x.Product.Title,
                    photo = x.Product.Sqphoto.Path,
                    quantity = x.Quantity,
                    unitPrice = x.UnitPrice,
                    package = x.Product.Packaging
                }).ToList();

                email.subtotal = order.TotalPrice;
                email.total = order.TotalPrice;
                email.Send();
            }
        }

        public paypal.APIContext getPaypalApiContect()
        {
            var config = paypal.ConfigManager.Instance.GetProperties();
            var accessToken = new paypal.OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new paypal.APIContext(accessToken);
            return apiContext;
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}
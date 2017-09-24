using ArtShop.Models;
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
        public ActionResult getinfo()
        {
            var user = db.UserProfiles.Find(User.Identity.GetUserId());
            ViewBag.name = user.FirstName;
            ViewBag.lastname = user.LastName;
            if (user.billingInfo != null)
            {
                ViewBag.Street = user.billingInfo.Street;
                ViewBag.City = user.billingInfo.City;
                ViewBag.Country = user.billingInfo.country.Current().Name;
                ViewBag.Region = user.billingInfo.Region;
                ViewBag.ZipCode = user.billingInfo.ZipCode;
                ViewBag.PhoneNumber = user.billingInfo.PhoneNumber;
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
            int amount = (int)cart.GetTotal();
            decimal orderTotal = 0;
            var cartItems = cart.GetCartItems();
            var setting = db.SettingValues.FirstOrDefault();

            Order o = new Order()
            {
                user_id = userId,
                OrderDetails = new List<OrderDetail>(),
                ReceiverName = model.firstname + " " + model.lastname,
                Address = model.address,
                Country = model.country,
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
            o.TransactionDetail = new TransactionDetail() { amount = orderTotal, Method = model.paymentMethod, currencyRate = setting.IRRialRate };
            db.Orders.Add(o);
            db.SaveChanges();

            System.Net.ServicePointManager.Expect100Continue = false;
            ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient zp = new ZPServiceReference.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;
            int Status = zp.PaymentRequest("test",
                (int)(orderTotal * setting.IRRialRate),
                profile.FirstName + " " + profile.LastName,
                user.Email,
                user.PhoneNumber,
                "http://artiscovery.com/card/Verify",
                out Authority);

            long longAuth = 0;
            long.TryParse(Authority, out longAuth);
            o.TransactionDetail.Number = longAuth.ToString();
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
                long longAuth = 0;
                long.TryParse(Request.QueryString["Authority"], out longAuth);
                var tran = db.TransactionDetails.FirstOrDefault(x => x.Number == longAuth.ToString());
                var orderId = db.Orders.FirstOrDefault(x => x.TransactionDetailId == tran.Id).Id;
                if (tran != null)
                {

                    if (Request.QueryString["Status"].ToString().Equals("OK"))
                    {
                        decimal Amount = tran.amount * tran.currencyRate;
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
                            CartManager.GetCart(this.HttpContext).EmptyCart();
                            return RedirectToAction("paymentReport", new { id = orderId });
                        }
                        else
                        {
                            db.SaveChanges();
                            return RedirectToAction("paymentReport", new { id = orderId });
                        }
                    }
                    else
                    {
                        tran.Description = Request.QueryString["Status"].ToString();
                        db.SaveChanges();
                        return RedirectToAction("paymentReport", new { id = orderId });
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

        [Authorize]
        public ActionResult paymentReport(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var order = profile.Orders.SingleOrDefault(x => x.Id == id);
            if (order.TransactionDetail.Payed)
                SendInvoice(order);
            return View(order);
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
            foreach (var item in order.OrderDetails.GroupBy(a => a.Product.user_id))
            {
                foreach (var orderDetail in item)
                {
                    dynamic email = new Email("Order");
                    email.To = orderDetail.Product.user.ApplicationUserDetail.Email;
                    email.Subject = "Order";
                    email.orderid = order.Id;
                    email.fullname = order.ReceiverName;
                    email.orderdate = order.BuyDate.ToString();
                    email.products = order.OrderDetails.ToList();
                    email.subtotal = order.TotalPrice;
                    email.total = order.TotalPrice;
                    email.Send();
                }
            }
        }

    }
}
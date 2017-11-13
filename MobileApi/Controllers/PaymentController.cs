using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using paypal = PayPal.Api;
using DataLayer;
using DataLayer.Enitities;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Net.Http;
using MobileApi.Models;

namespace MobileApi.Controllers
{
 
    public class PaymentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        internal JsonMediaTypeFormatter formatter = MJsonMaker();
        private static Func<JsonMediaTypeFormatter> MJsonMaker = () =>
        {
            var formatter = new JsonMediaTypeFormatter();
            var json = formatter.SerializerSettings;
            json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            json.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.Culture = new CultureInfo("it-IT");
            return formatter;
        };

        [HttpGet]
        public ActionResult Pay(checkInfoViewModel model)
        {
            var userId = model.userId;
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var cart = CartManager.GetCart(userId);
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
                int Status = zp.PaymentRequest("test", (int)(orderTotal * setting.IRRialRate), profile.FirstName + " " + profile.LastName, user.Email, user.PhoneNumber, "http://artiscovery.com/card/Mobileverify", out Authority);
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
                    var payment = CreatePayment(cartItems, orderTotal, o.Id.ToString());
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

        //paypal segment
        private paypal.APIContext getPaypalApiContect()
        {
            var config = paypal.ConfigManager.Instance.GetProperties();
            var accessToken = new paypal.OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new paypal.APIContext(accessToken);
            return apiContext;
        }

        private paypal.Payment CreatePayment(List<ShoppingCart> cartItems, decimal orderTotal, string invoice_number)
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
            var transactionList = new List<paypal.Transaction>();
            var amount = new paypal.Amount
            {
                currency = "USD",
                total = orderTotal.ToString(),
                details = new paypal.Details
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = orderTotal.ToString()
                }
            };
            transactionList.Add(new paypal.Transaction()
            {
                description = "Artiscovery shopping store",
                invoice_number = invoice_number,
                amount = amount,
                item_list = new paypal.ItemList { items = items }
            });
            var payer = new paypal.Payer() { payment_method = "paypal" };
            return paypal.Payment.Create(apiContext, new paypal.Payment
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = new paypal.RedirectUrls
                {
                    return_url = "https://artiscovery.com/card/MobilePaypalReturn",
                    cancel_url = "https://artiscovery.com/card/MobilePaypalCancel"
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
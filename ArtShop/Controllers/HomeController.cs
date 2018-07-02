using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Extentions;
using ArtShop.Models;
using ArtShop.Helper;
using System.Globalization;
using System.Configuration;
using DataLayer.Enitities;
using DataLayer;
using Microsoft.AspNet.Identity;
using ArtShop.Util;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SimpleMvcSitemap;
using SimpleMvcSitemap.Translations;
using SimpleMvcSitemap.Images;

namespace ArtShop.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //var articles = db.Articles.Include("Translations").ToList();
            //foreach (var item in articles)
            //{
            //    var translation = item.Translations.Single(a => a.languageId == "en");
            //    translation.Description = translation.Description.Replace("https://artiscovery.com", "/en-us");
            //    db.SaveChanges();
            //}

            //foreach (var item in articles)
            //{
            //    var translation = item.Translations.Single(a => a.languageId == "fa");
            //    translation.Description = translation.Description.Replace("اثرهنری", "اثر هنری");
            //    db.SaveChanges();
            //}

            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            HomeIndexViewModel model = new HomeIndexViewModel();
            var count = db.sliderImages.Count();
            if (count != 0)
            {
                
                var sl = db.sliderImages.Include("Translations").ToList();
                model.SliderItems = new List<Slide>();
                foreach (var item in sl)
                {
                    Slide slideItem = new Slide();
                    slideItem.Slider_Image = ConfigurationManager.AppSettings["FileUrl"] + "/" + item.path;
                    slideItem.slider_H1 = item.Current().H1;
                    slideItem.slider_H2 = item.Current().H2;
                    slideItem.slider_Button_Text = item.Current().ButtonText;
                    slideItem.slider_Button_Url = item.ButtonURL;
                    slideItem.slider_text_color = item.TextColor;
                    slideItem.slider_Button_color = item.ButtonColor;
                    slideItem.slider_Button_text_color = item.ButtonTextColor;
                    slideItem.slider_P = item.Current().P1;
                                  
                    model.SliderItems.Add(slideItem);
                }
                model.FirstPageSections = db.FirstPageSections.Include("Translations").ToList();
            }         

            return View(model);
        }

        //[Route("Blog")]
        //public ActionResult Blog()
        //{
        //    return Redirect("http://blog.artiscovery.com/");
        //}

        //[OutputCache(Duration = 360)]
        public ActionResult _SelectedCurators(FirstPageSection model)
        {
            try
            {
                var p1 = db.Products.Find(int.Parse(model.param1));
                ViewBag.pic1 = p1.Sqphoto.Path;
                ViewBag.url1 = CultureHelper.GetCurrentCulture() + "/artwork/" + p1.category.Current().Name + "/" + p1.GenerateSlug();

                var p2 = db.Products.Find(int.Parse(model.param2));
                ViewBag.pic2 = p2.Sqphoto.Path;
                ViewBag.url2 = CultureHelper.GetCurrentCulture() + "/artwork/" + p2.category.Current().Name + "/" + p2.GenerateSlug();
                var p3 = db.Products.Find(int.Parse(model.param3));
                ViewBag.pic3 = p3.Sqphoto.Path;
                ViewBag.url3 = CultureHelper.GetCurrentCulture() + "/artwork/" + p3.category.Current().Name + "/" + p3.GenerateSlug();
            }
            catch
            {

            }
            return PartialView(model);
        }
        //[OutputCache(Duration = 360)]
        public ActionResult _SalebyPrice(FirstPageSection model)
        {
            var p1 = db.Pricethresholds.Find(int.Parse(model.param1)) ?? new Pricethreshold();
            var p2 = db.Pricethresholds.Find(int.Parse(model.param2)) ?? new Pricethreshold();
            var p3 = db.Pricethresholds.Find(int.Parse(model.param3)) ?? new Pricethreshold();
            var p4 = db.Pricethresholds.Find(int.Parse(model.param4)) ?? new Pricethreshold();
            var p5 = db.Pricethresholds.Find(int.Parse(model.param5)) ?? new Pricethreshold();
            ViewBag.p1 = p1;
            ViewBag.p2 = p2;
            ViewBag.p3 = p3;
            ViewBag.p4 = p4;
            ViewBag.p5 = p5;
            var filter1 = db.Products.Where(x => (!p1.min.HasValue || p1.min.Value < x.Price) && (!p1.max.HasValue || p1.max.Value > x.Price));
            var filter2 = db.Products.Where(x => (!p2.min.HasValue || p2.min.Value <= x.Price) && (!p2.max.HasValue || p2.max.Value > x.Price));
            var filter3 = db.Products.Where(x => (!p3.min.HasValue || p3.min.Value <= x.Price) && (!p3.max.HasValue || p3.max.Value > x.Price));
            var filter4 = db.Products.Where(x => (!p4.min.HasValue || p4.min.Value <= x.Price) && (!p4.max.HasValue || p4.max.Value > x.Price));
            var filter5 = db.Products.Where(x => (!p5.min.HasValue || p5.min.Value <= x.Price) && (!p5.max.HasValue || p5.max.Value > x.Price));
            var count1 = filter1.Count();
            var count2 = filter2.Count();
            var count3 = filter3.Count();
            var count4 = filter4.Count();
            var count5 = filter5.Count();
            var r1 = new Random().Next(1, Math.Max(count1, 1));
            var r2 = new Random().Next(1, Math.Max(count2, 1));
            var r3 = new Random().Next(1, Math.Max(count3, 1));
            var r4 = new Random().Next(1, Math.Max(count4, 1));
            var r5 = new Random().Next(1, Math.Max(count5, 1));
            ViewBag.pic1 = count1 == 0 ? "" : filter1.OrderBy(x => x.CreateDate).Skip(r1 - 1).First().Widephoto.Path;
            ViewBag.pic2 = count2 == 0 ? "" : filter2.OrderBy(x => x.CreateDate).Skip(r2 - 1).First().Sqphoto.Path;
            ViewBag.pic3 = count3 == 0 ? "" : filter3.OrderBy(x => x.CreateDate).Skip(r3 - 1).First().Sqphoto.Path;
            ViewBag.pic4 = count4 == 0 ? "" : filter4.OrderBy(x => x.CreateDate).Skip(r4 - 1).First().Sqphoto.Path;
            ViewBag.pic5 = count5 == 0 ? "" : filter5.OrderBy(x => x.CreateDate).Skip(r5 - 1).First().Sqphoto.Path;
            // text1 , text2 , link , pic
            return PartialView(model);
        }
        //[OutputCache(Duration = 360)]
        public ActionResult _RecentlySold(FirstPageSection model)
        {
            var recently = db.Orders
                .SelectMany(x => x.OrderDetails).Select(x => x.Product).Take(10).ToList();
            return PartialView(recently);
        }
        //[OutputCache(Duration = 360)]
        public ActionResult _SalebyStyle(FirstPageSection model)
        {
            var p1 = db.Styles.Find(int.Parse(model.param1)) ?? new Style();
            var p2 = db.Styles.Find(int.Parse(model.param2)) ?? new Style();
            var p3 = db.Styles.Find(int.Parse(model.param3)) ?? new Style();
            var p4 = db.Styles.Find(int.Parse(model.param4)) ?? new Style();
            var p5 = db.Styles.Find(int.Parse(model.param5)) ?? new Style();
            ViewBag.p1 = p1;
            ViewBag.p2 = p2;
            ViewBag.p3 = p3;
            ViewBag.p4 = p4;
            ViewBag.p5 = p5;
            var filter1 = db.Products.Where(x => x.Styles.Any(y => y.Id == p1.Id));
            var filter2 = db.Products.Where(x => x.Styles.Any(y => y.Id == p2.Id));
            var filter3 = db.Products.Where(x => x.Styles.Any(y => y.Id == p3.Id));
            var filter4 = db.Products.Where(x => x.Styles.Any(y => y.Id == p4.Id));
            var filter5 = db.Products.Where(x => x.Styles.Any(y => y.Id == p5.Id));
            var count1 = filter1.Count();
            var count2 = filter2.Count();
            var count3 = filter3.Count();
            var count4 = filter4.Count();
            var count5 = filter5.Count();
            var r1 = new Random().Next(0, count1);
            var r2 = new Random().Next(0, count2);
            var r3 = new Random().Next(0, count3);
            var r4 = new Random().Next(0, count4);
            var r5 = new Random().Next(0, count5);
            ViewBag.pic1 = count1 == 0 ? "" : filter1.OrderBy(x => x.CreateDate).Skip(r1).First().Widephoto.Path;
            ViewBag.pic2 = count2 == 0 ? "" : filter2.OrderBy(x => x.CreateDate).Skip(r2).First().Sqphoto.Path;
            ViewBag.pic3 = count3 == 0 ? "" : filter3.OrderBy(x => x.CreateDate).Skip(r3).First().Sqphoto.Path;
            ViewBag.pic4 = count4 == 0 ? "" : filter4.OrderBy(x => x.CreateDate).Skip(r4).First().Sqphoto.Path;
            ViewBag.pic5 = count5 == 0 ? "" : filter5.OrderBy(x => x.CreateDate).Skip(r5).First().Sqphoto.Path;
            // text1 , text2 , link , pic
            return PartialView(model);
        }
        //[OutputCache(Duration = 360)]
        public ActionResult _SalebyCategory(FirstPageSection model)
        {
            var p1 = db.Categories.Find(int.Parse(model.param1)) ?? new Category();
            var p2 = db.Categories.Find(int.Parse(model.param2)) ?? new Category();
            var p3 = db.Categories.Find(int.Parse(model.param3)) ?? new Category();
            var p4 = db.Categories.Find(int.Parse(model.param4)) ?? new Category();
            var p5 = db.Categories.Find(int.Parse(model.param5)) ?? new Category();
            ViewBag.p1 = p1;
            ViewBag.p2 = p2;
            ViewBag.p3 = p3;
            ViewBag.p4 = p4;
            ViewBag.p5 = p5;
            var filter1 = db.Products.Where(x => x.categoryId == p1.Id);
            var filter2 = db.Products.Where(x => x.categoryId == p2.Id);
            var filter3 = db.Products.Where(x => x.categoryId == p3.Id);
            var filter4 = db.Products.Where(x => x.categoryId == p4.Id);
            var filter5 = db.Products.Where(x => x.categoryId == p5.Id);
            var count1 = filter1.Count();
            var count2 = filter2.Count();
            var count3 = filter3.Count();
            var count4 = filter4.Count();
            var count5 = filter5.Count();
            var r1 = new Random().Next(0, count1);
            var r2 = new Random().Next(0, count2);
            var r3 = new Random().Next(0, count3);
            var r4 = new Random().Next(0, count4);
            var r5 = new Random().Next(0, count5);
            ViewBag.pic1 = count1 == 0 ? "" : filter1.OrderBy(x => x.CreateDate).Skip(r1).First().Widephoto.Path;
            ViewBag.pic2 = count2 == 0 ? "" : filter2.OrderBy(x => x.CreateDate).Skip(r2).First().Sqphoto.Path;
            ViewBag.pic3 = count3 == 0 ? "" : filter3.OrderBy(x => x.CreateDate).Skip(r3).First().Sqphoto.Path;
            ViewBag.pic4 = count4 == 0 ? "" : filter4.OrderBy(x => x.CreateDate).Skip(r4).First().Sqphoto.Path;
            ViewBag.pic5 = count5 == 0 ? "" : filter5.OrderBy(x => x.CreateDate).Skip(r5).First().Sqphoto.Path;
            // text1 , text2 , link , pic
            return PartialView(model);
        }

        public ActionResult Header()
        {
            var userId = User.Identity.GetUserId();
            var cart = CartManager.GetCart(this.HttpContext);
            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.FirstName + userProfile.LastName))
            {
                ViewBag.fullName = userProfile.FirstName + " " + userProfile.LastName;
                ViewBag.Title = userProfile.FirstName + " " + userProfile.LastName;
            }
            else
            {
                ViewBag.fullName = User.Identity.GetUserName();
                ViewBag.Title = User.Identity.GetUserName();
            }
            ViewBag.card = cart.GetCartItems().Count;
            return PartialView("_Header", CashManager.Instance.Header);
        }

        public ActionResult Footer()
        {

            return PartialView("_footer", CashManager.Instance.Footer);
        }

        public ActionResult AddSubscriber(string email)
        {
            var client = new RestSharp.RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers");
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"email\":\"" + email + "\", \"name\": \" \", \"fields\": {\"company\": \"Artiscovery\"}}", RestSharp.ParameterType.RequestBody);
            RestSharp.IRestResponse response = client.Execute(request);

            return Content("done");
        }

        public ActionResult SetCulture(string culture,string requestUrl)
        {
            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            string url = requestUrl;// this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query ?? "";

            string url2 = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query ?? "";

            if (url.ToLower().Contains("en-us") || url.ToLower().Contains("fa"))
            {
                Regex re = new Regex("^/\\w{2,3}(-\\w{2})?");
                url = re.Replace(url, "/" + culture.ToLower());
            }
            else
                url += culture;


            culture = CultureHelper.GetImplementedCulture(culture);
            RouteData.Values["culture"] = culture;  // set culture

            return RedirectPermanent(url);
        }

        public ActionResult RelatedArtwork(int id)
        {
            var result = db.Products.FirstOrDefault(x => x.Id == id);

            return PartialView(result);
        }


        public ActionResult cashmanager(string id)
        {
            if (id == "soroosh1313")
                CashManager.resete();
            return Content("ok");
        }

        [Route("robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("Disallow: /resources");
            stringBuilder.AppendLine("Disallow: /Account/");
            stringBuilder.AppendLine("Disallow: /upload/");
            stringBuilder.AppendLine("allow: /fa/");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapXml", null, this.Request.Url.Scheme).TrimEnd('/'));
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapfaXml", null, this.Request.Url.Scheme).TrimEnd('/'));

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml", Name = "GetSitemapXml"), OutputCache(Duration = 86400)]
        public ActionResult SitemapXml()
        {

            List<SitemapNode> nodes = new List<SitemapNode>
        {
            new SitemapNode(Url.Action("Index", "Home", new { Culture ="en-us" }))
        {
            ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },


       new SitemapNode(Url.Action("about", "pages", new { Culture ="en-us" }))
    {
        ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
                   new SitemapNode(Url.Action("privacypolicy", "pages", new { Culture ="en-us" }))
    {
  ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
       new SitemapNode(Url.Action("terms", "pages", new { Culture ="en-us" }))
    {
  ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
        new SitemapNode(Url.Action("copyrightpolicy", "pages", new { Culture ="en-us" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
         new SitemapNode(Url.Action("contactus", "pages", new { Culture ="en-us" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },
                  new SitemapNode(Url.Action("whysell", "pages", new { Culture ="en-us" }))
    {
       ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },
                  new SitemapNode(Url.Action("category", "support", new { Culture ="en-us" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        }

        };

            foreach (var item in db.SupportCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Category", "Support", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }


            foreach (var item in db.SupportSubCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("SubCategory", "Support", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Articles.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Article", "Support", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Products.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action(item.category.Current().Name, "Artwork", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));

        }

        [Route("sitemapfa.xml", Name = "GetSitemapfaXml"), OutputCache(Duration = 86400)]
        public ActionResult SitemapfaXml()
        {

            List<SitemapNode> nodes = new List<SitemapNode>
        {
            new SitemapNode(Url.Action("Index", "Home", new { Culture ="fa" }))
        {
            ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },


       new SitemapNode(Url.Action("about", "pages", new { Culture ="fa" }))
    {
        ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
                   new SitemapNode(Url.Action("privacypolicy", "pages", new { Culture ="fa" }))
    {
  ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
       new SitemapNode(Url.Action("terms", "pages", new { Culture ="fa" }))
    {
  ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
        new SitemapNode(Url.Action("copyrightpolicy", "pages", new { Culture ="fa" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
    },
         new SitemapNode(Url.Action("contactus", "pages", new { Culture ="fa" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },
                  new SitemapNode(Url.Action("whysell", "pages", new { Culture ="fa" }))
    {
       ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        },
                  new SitemapNode(Url.Action("category", "support", new { Culture ="fa" }))
    {
    ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        }

        };

            foreach (var item in db.SupportCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Category", "Support", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }


            foreach (var item in db.SupportSubCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("SubCategory", "Support", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Articles.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Article", "Support", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Products.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action(item.category.Current().Name, "Artwork", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }


    }
}
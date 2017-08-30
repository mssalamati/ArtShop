using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using ArtShop.Util;
using Microsoft.AspNet.Identity;
using ArtShop.Models;
using DataLayer.Enitities;

namespace ArtShop.Controllers
{
    public class ProductsController : BaseController
    {
        [Route("search/{category}/{style}/{subject}/{medium}/{price}/{page?}")]
        [Route("search")]
        public ActionResult Search(int category = 0, int style = 0, int subject = 0, int medium = 0, int price = 0, int page = 1)
        {
            int pageSize = 18;
            var manager = CashManager.Instance;
            var category_cash = manager.Categories.SingleOrDefault(x => x.id == category);
            ViewBag.categoryName = category_cash != null ? category_cash.name : Resources.SearchRes.All_Categories;
            ViewBag.categoryId = category;
            var style_cash = manager.Styles.Any(x => x.Key == style);
            ViewBag.styleName = style_cash ? manager.Styles[style] : Resources.SearchRes.All_Styles;
            ViewBag.styleId = style;
            var subject_cash = manager.Subjects.Any(x => x.Key == subject);
            ViewBag.subjectName = subject_cash ? manager.Subjects[subject] : Resources.SearchRes.All_Subjects;
            ViewBag.subjectId = subject;
            var medium_cash = manager.Mediums.Any(x => x.Key == medium);
            ViewBag.mediumName = medium_cash ? manager.Mediums[medium] : Resources.SearchRes.All_Mediums;
            ViewBag.mediumId = medium;
            var price_cash = manager.Pricethresholds.SingleOrDefault(x => x.Id == price);
            ViewBag.priceName = price_cash != null ? price_cash.Name : Resources.SearchRes.All_Prices;
            ViewBag.priceId = price;

            var p = db.Products.OrderBy(x => x.CreateDate).AsQueryable();
            p = p.Where(x => category == 0 || x.categoryId == category).AsQueryable();
            p = p.Where(x => style == 0 || x.Styles.FirstOrDefault(y => y.Id == style) != null);
            p = p.Where(x => subject == 0 || x.subjectId == subject).AsQueryable();
            p = p.Where(x => medium == 0 || x.Mediums.FirstOrDefault(y => y.Id == medium) != null);
            if (price_cash != null && price_cash.max.HasValue)
                p = p.Where(x => x.Price < price_cash.max.Value);
            if (price_cash != null && price_cash.min.HasValue)
                p = p.Where(x => x.Price > price_cash.min.Value);

            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;

            p = p.Skip((page - 1) * pageSize).Take(pageSize);

            var res = p.ToList();
            return View(res);
        }

        public ActionResult single(int id)
        {
            var p = db.Products.Find(id);

            bool mine = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var profile = user.userDetail;
                mine = profile.Products.Any(x => x.Id == id);
            }
            ViewBag.mine = mine;

            return View(p);
        }

        [HttpGet]
        public ActionResult remove(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var profile = user.userDetail;
                var p = profile.Products.SingleOrDefault(x => x.Id == id);
                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    return RedirectToAction("artworks", "profile", new { });
                }
            }
            return Content("error");
        }

        public ActionResult AddOrRemoveFavorit(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { IsAuthenticated = false, isInMyFavList = false }, JsonRequestBehavior.AllowGet);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            if (profile.Favorits.Any(x => x.productId == id))
                profile.Favorits.Remove(profile.Favorits.First(x => x.productId == id));
            else
                profile.Favorits.Add(new DataLayer.Enitities.Favorit() { productId = id });
            db.SaveChanges();
            bool isInMylist = profile.Favorits.Any(x => x.productId == id);
            return Json(new { IsAuthenticated = true, isInMyFavList = isInMylist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToCollection(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { IsAuthenticated = false }, JsonRequestBehavior.AllowGet);
            var p = db.Products.Find(id);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            ViewBag.collections = profile.Collections.ToList();
            ViewBag.productid = id;
            return PartialView(p);
        }
        [HttpPost]
        public ActionResult AddToCollection(SearchCollectionViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            var p = db.Products.Find(model.productId);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            if (model.collectionId == 0)
            {
                Collection newCollection = new Collection() { Title = model.collectionName };
                newCollection.Artworks = new List<CollectionProduct>();
                newCollection.Artworks.Add(new CollectionProduct() { productId = model.productId });
                profile.Collections.Add(newCollection);
            }
            else
            {
                var oldCollection = profile.Collections.SingleOrDefault(x => x.Id == model.collectionId);
                if (oldCollection != null && !oldCollection.Artworks.Any(x => x.productId == model.productId))
                    oldCollection.Artworks.Add(new CollectionProduct() { productId = model.productId });
            }
            try
            {
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var p = db.Products.Include("photo").Include("productshippingDetail").Single(x => x.Id == id);
            bool mine = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var profile = user.userDetail;
                mine = profile.Products.Any(x => x.Id == id);
            }
            if (!mine)
                return HttpNotFound();
            if (p.productshippingDetail == null)
                p.productshippingDetail = new ProductshippingDetail();
            return View(p);
        }

        public ActionResult EditPackag(int id)
        {
            var p = db.Products.Include("photo").Single(x => x.Id == id);
            bool mine = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var profile = user.userDetail;
                mine = profile.Products.Any(x => x.Id == id);
            }
            if (!mine)
                return HttpNotFound();

            return View(p);
        }
    }
}
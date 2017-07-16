using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using ArtShop.Util;

namespace ArtShop.Controllers
{
    public class ProductsController : BaseController
    {
        [Route("search/{category}/{style}/{subject}/{medium}/{price}")]
        public ActionResult Search(int category = 0, int style = 0, int subject = 0, int medium = 0, int price = 0)
        {
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
            ViewBag.priceId = price;

            var p = db.Products.ToList();
            return View(p);
        }

        public ActionResult single(int id)
        {
            var p = db.Products.Find(id);
            return View(p);
        }
    }
}
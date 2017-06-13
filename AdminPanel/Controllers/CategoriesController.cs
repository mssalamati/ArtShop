using AdminPanel.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class CategoriesController : BaseController
    {
        // GET: Categories
        public ActionResult Index()
        {
            var data = db.Categories;
            return View(data.ToList());
        }

        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }

        [HttpPost]
        public ActionResult Add(CategoryViewModel model)
        {
            return PartialView();
        }

        public ActionResult Edit(int id)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            return PartialView();
        }
    }
}
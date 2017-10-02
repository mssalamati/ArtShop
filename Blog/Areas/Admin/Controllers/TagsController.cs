using Blog.Areas.Admin.Models.ViewModel;
using Blog.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class TagsController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.Tags
                 .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
                 .OrderByDescending(x => x.Name)
                 .Skip(skip).Take(take);
            count = db.Tags.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Tag c = new Tag() { Name = model.Name };

            c.Translations = new List<TagTranslation>();
            foreach (var item in model.Translations)
                c.Translations.Add(new TagTranslation() { languageId = item.languageId, Name = item.Name });
            db.Tags.Add(c);
            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, ex.ToString());
                return PartialView(model);
            }
        }


        public ActionResult Edit(int id)
        {
            var finder = db.Tags.Find(id);
            ViewBag.language = db.Languages.ToList();
            TagViewModel cvm = new TagViewModel() { Id = finder.Id, Translations = new List<TagTranslationViewModel>(), Name = finder.Name };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new TagTranslationViewModel() { languageId = item.languageId, Name = item.Name });

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(TagViewModel model)
        {
            var finder = db.Tags.Find(model.Id);

            finder.Name = model.Name;

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                {
                    finder.Translations.Add(new TagTranslation() { languageId = item.languageId, Name = item.Name });
                }
            }

            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            TagViewModel cvm = new TagViewModel() { Id = finder.Id, Translations = new List<TagTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new TagTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.Tags.Find(id);
            db.Tags.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
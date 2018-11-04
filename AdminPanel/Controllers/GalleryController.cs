using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Manager")]
    public class GalleryController : BaseController
    {
        // GET: Gallery
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Galleries
                .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Title.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.StartTimestamp).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());    
        }


        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();

            return View(new GalleryViewModel());
        }
        [HttpPost]
        public ActionResult Add(GalleryViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.caregories = db.SupportCategories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.articles = db.Articles.ToList();

            if (!ModelState.IsValid)
                return View(model);

            Gallery newGallery = new Gallery();


            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');
            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);


            newGallery.DefaultTitle = model.Title;
            newGallery.Translations = new List<GalleryTranslation>();
            newGallery.Translations.Add(new GalleryTranslation() { languageId = model.languageId, Title = model.Title, Content = model.Content});
            newGallery.DefaultTitle = newGallery.Translations.FirstOrDefault().Title;
            newGallery.Active = model.Active;
            db.Galleries.Add(newGallery);
            try { db.SaveChanges(); }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError(string.Empty, ve.PropertyName + " " +
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName) + " " +
                        ve.ErrorMessage);
                return View(model);
            }
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id, string language = "en")
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
       
            ViewBag.language = db.Languages.ToList();
            var model = db.Galleries.Find(id);
            //if (model.AuthorProfileId != userId)
            //    return HttpNotFound();
            GalleryTranslation modelTranslation;
            var curr = model.Translations.SingleOrDefault(x => x.languageId == language);
            if (curr == null)
            {
                var newcur = new GalleryTranslation() { languageId = language };
                model.Translations.Add(newcur);
                db.SaveChanges();
                modelTranslation = newcur;
            }
            else
                modelTranslation = curr;
            GalleryViewModel result = new GalleryViewModel(model, language);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(GalleryViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.language = db.Languages.ToList();

            var gallery = db.Galleries.Find(model.Id);
            
            var translation = gallery.Translations.Single(x => x.languageId == model.languageId);
            translation.Title = model.Title;
            translation.Content = model.Content;
            try { db.SaveChanges(); }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError(string.Empty, ve.PropertyName + " " +
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName) + " " +
                        ve.ErrorMessage);
                return View();
            }
            ViewBag.message = "تغییرات با موفقیت انجام شد";
            return View(new GalleryViewModel(gallery, model.languageId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gallery model)
        {
            var finder = db.Galleries.Find(model.Id);

            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');
            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);

            finder.StartTimestamp = start;
            finder.EndTimestamp = end;
            finder.Active = model.Active;
            finder.DefaultTitle = model.DefaultTitle;


            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Title = item.Title;
                    curr.Content = item.Content;
                }
                else
                    finder.Translations.Add(new GalleryTranslation() { languageId = item.languageId, Title = item.Title, Content = item.Content });
            }

            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {                
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            Gallery cvm = new Gallery() { Id = finder.Id, StartTimestamp = finder.StartTimestamp, DefaultTitle = finder.DefaultTitle, EndTimestamp = finder.EndTimestamp, Active = finder.Active, Translations = new List<GalleryTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new GalleryTranslation() { languageId = item.languageId, Title = item.Title, Content = item.Content });
            return View(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.Galleries.Find(id);
            finder.Translations.Clear();            
            db.Galleries.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public class imagesviewmodel
        {
            public string Url { get; set; }
        }

        public void uploadnow(HttpPostedFileWrapper upload)
        {
            var userId = User.Identity.GetUserId();
            var folder = Server.MapPath("~/Uploads/cms/" + userId);
            FileHelper.CreateFolderIfNeeded(folder);
            if (upload != null)
            {
                string ImageName = upload.FileName;
                string path = Path.Combine(folder, ImageName);
                upload.SaveAs(path);
            }
        }

        public ActionResult uploadPartial()
        {
            var userId = User.Identity.GetUserId();
            var appData = Server.MapPath("~/Uploads/cms/" + userId);
            FileHelper.CreateFolderIfNeeded(appData);
            var images = Directory.GetFiles(appData).Select(x => new imagesviewmodel
            {
                Url = Url.Content("https://file.artiscovery.com/Uploads/cms/" + userId + "/" + Path.GetFileName(x))
            });
            return View(images);
        }
    }
}
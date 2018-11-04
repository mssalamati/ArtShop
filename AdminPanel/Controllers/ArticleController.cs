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
    public class ArticleController : BaseController
    {
        // GET: Article
        public ActionResult Index(int page = 1, string search = "")
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = user.adminDetail.Articles
                 .Where(x => string.IsNullOrEmpty(search) || x.Title.Contains(search))
                 .OrderByDescending(x => x.PostedOn)
                 .Skip(skip).Take(take);
            count = user.adminDetail.Articles.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }

        public ActionResult Add()
        {
            ViewBag.caregories = db.SupportCategories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.articles = db.Articles.ToList();

            return View(new ArticleViewModel());
        }
        [HttpPost]
        public ActionResult Add(ArticleViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.caregories = db.SupportCategories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.articles = db.Articles.ToList();

            if (!ModelState.IsValid)
                return View(model);

            Article newPost = new Article();
            string tempFolderName = "Upload/articles";

            if (model.Thumbnail != null)
            {
                var Thumbresult = ImageHelper.Saveimage(Server, model.Thumbnail, tempFolderName, ImageHelper.saveImageMode.Squre);
                if (!Thumbresult.ResultStatus)
                {
                    ModelState.AddModelError(string.Empty, Thumbresult.Error);
                    return View(model);
                }
                newPost.Thumbnail = Thumbresult.FullPath;
            }
            newPost.isHandbook = model.isHandbook;
            newPost.SupportSubCategory = db.SupportSubCategories.Find(model.SubCategory);
            newPost.SupportCategory = db.SupportCategories.Find(model.Category);
            if (model.ReletedArticles != null)
                newPost.ReletedArticles = db.Articles.Where(x => model.ReletedArticles.Any(y => y == x.Id)).ToList();

            newPost.Title = model.TitleDef;
            newPost.Translations = new List<ArticleTranslation>();
            newPost.Translations.Add(new ArticleTranslation() { languageId = model.languageId, Title = model.Title, Description = model.Description, ShortDescription = model.ShortDescription });
            newPost.Author = "Artiscovery Team";
            user.adminDetail.Articles.Add(newPost);
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
            ViewBag.articles = db.Articles.Where(x=>x.Id != id).ToList();
            ViewBag.categories = db.SupportCategories.ToList();
            ViewBag.language = db.Languages.ToList();
            var model = db.Articles.Find(id);
            //if (model.AuthorProfileId != userId)
            //    return HttpNotFound();
            ArticleTranslation modelTranslation;
            var curr = model.Translations.SingleOrDefault(x => x.languageId == language);
            if (curr == null)
            {
                var newcur = new ArticleTranslation() { languageId = language };
                model.Translations.Add(newcur);
                db.SaveChanges();
                modelTranslation = newcur;
            }
            else
                modelTranslation = curr;
            ArticleViewModel result = new ArticleViewModel(model, language);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            ViewBag.articles = db.Articles.ToList();
            ViewBag.categories = db.SupportCategories.ToList();
            ViewBag.language = db.Languages.ToList();

            var post = db.Articles.Find(model.Id);

            //if (post.AuthorProfileId != userId)
            //    return HttpNotFound();
            if (model.Thumbnail != null)
            {
                string tempFolderName = "Upload/articles";
                if (model.Thumbnail != null)
                {

                    var Thumbresult = ImageHelper.Saveimage(Server, model.Thumbnail, tempFolderName, ImageHelper.saveImageMode.Squre);
                    if (!Thumbresult.ResultStatus)
                    {
                        ModelState.AddModelError(string.Empty, Thumbresult.Error);
                        return View(model.FillPicture(post));
                    }
                    post.Thumbnail = Thumbresult.FullPath;
                }
            }
            post.isHandbook = model.isHandbook;
            post.SupportCategory = db.SupportCategories.Find(model.Category);
            post.SupportSubCategory = db.SupportSubCategories.Find(model.SubCategory);
            post.Title = model.TitleDef;
            post.ReletedArticles.Clear();
            if (model.ReletedArticles != null)
                post.ReletedArticles = db.Articles.Where(x => model.ReletedArticles.Any(y => y == x.Id)).ToList();
            var translation = post.Translations.Single(x => x.languageId == model.languageId);
            translation.Title = model.Title;
            translation.ShortDescription = model.ShortDescription;
            translation.Description = model.Description;
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
            return View(new ArticleViewModel(post, model.languageId));
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

        public ActionResult LoadSubCategories(int id)
        {
            return Json(db.SupportSubCategories.Where(x => x.supportCategory.Id == id).Select(x => new { x.Id, x.Name }).ToList(),
                JsonRequestBehavior.AllowGet);
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

        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var post = db.Articles.Find(id);
            //if (post.AuthorProfileId != userId)
            //    return HttpNotFound();
            db.Articles.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
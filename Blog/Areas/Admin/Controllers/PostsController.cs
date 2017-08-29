using Blog.Areas.Admin.Models.ViewModel;
using Blog.Interfaces;
using Blog.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Blog.Areas.Admin.Controllers
{
    public class PostsController : BaseController
    {
        // GET: Admin/Posts
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.Posts
                 .Where(x => string.IsNullOrEmpty(search) || x.Title.Contains(search))
                 .OrderBy(x => x.PostedOn)
                 .Skip(skip).Take(take);
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }

        public ActionResult Add()
        {
            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add(PostViewModel model)
        {
            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            if (!ModelState.IsValid)
                return View(model);
            Post newPost = new Post();
            string tempFolderName = "Upload/posts";
            var Thumbresult = ImageHelper.Saveimage(Server, model.Thumbnail, tempFolderName, ImageHelper.saveImageMode.Not);
            if (!Thumbresult.ResultStatus)
            {
                ModelState.AddModelError(string.Empty, Thumbresult.Error);
                return View();
            }
            newPost.HeaderPhotos = new List<HeaderPhoto>();
            foreach (var item in model.HeaderPhotos)
            {
                var res = ImageHelper.Saveimage(Server, item, tempFolderName, ImageHelper.saveImageMode.wide);
                if (!res.ResultStatus)
                {
                    ModelState.AddModelError(string.Empty, res.Error);
                    return View();
                }
                newPost.HeaderPhotos.Add(new HeaderPhoto() { Path = res.FullPath });
            }
            newPost.Category = db.Categories.Find(model.Category);
            newPost.Links = model.Links.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Link() { URL = x }).ToList();
            newPost.Tags = db.Tags.Where(x => model.Tags.Any(y => y == x.Id)).ToList();
            newPost.Thumbnail = Thumbresult.FullPath;
            newPost.Title = model.TitleDef;
            newPost.Translations = new List<PostTranslation>();
            newPost.Translations.Add(new PostTranslation() { languageId = model.languageId, Title = model.Title, Description = model.Description });
            db.Posts.Add(newPost);
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
            return RedirectToAction("index");
        }

        public ActionResult Edit(int id, string language = "en")
        {
            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            var model = db.Posts.Find(id);
            PostTranslation modelTranslation;
            var curr = model.Translations.SingleOrDefault(x => x.languageId == language);
            if (curr == null)
            {
                var newcur = new PostTranslation() { languageId = language };
                model.Translations.Add(newcur);
                db.SaveChanges();
                modelTranslation = newcur;
            }
            else
                modelTranslation = curr;
            PostViewModel result = new PostViewModel(model, language);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(PostViewModel model)
        {
            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            if (!ModelState.IsValid)
                return View(model);
            var post = db.Posts.Find(model.Id);

            //frome here
            //string tempFolderName = "Upload/posts";
            //var Thumbresult = ImageHelper.Saveimage(Server, model.Thumbnail, tempFolderName, ImageHelper.saveImageMode.Not);
            //if (!Thumbresult.ResultStatus)
            //{
            //    ModelState.AddModelError(string.Empty, Thumbresult.Error);
            //    return View(model);
            //}
            //newPost.HeaderPhotos = new List<HeaderPhoto>();
            //foreach (var item in model.HeaderPhotos)
            //{
            //    var res = ImageHelper.Saveimage(Server, item, tempFolderName, ImageHelper.saveImageMode.wide);
            //    if (!res.ResultStatus)
            //    {
            //        ModelState.AddModelError(string.Empty, res.Error);
            //        return View(model);
            //    }
            //    newPost.HeaderPhotos.Add(new HeaderPhoto() { Path = res.FullPath });
            //}
            //newPost.Category = db.Categories.Find(model.Category);
            //newPost.Links = model.Links.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Link() { URL = x }).ToList();
            //newPost.Tags = db.Tags.Where(x => model.Tags.Any(y => y == x.Id)).ToList();
            //newPost.Thumbnail = Thumbresult.FullPath;
            //newPost.Title = model.TitleDef;
            //newPost.Translations = new List<PostTranslation>();
            //newPost.Translations.Add(new PostTranslation() { languageId = model.languageId, Title = model.Title, Description = model.Description });
            //db.Posts.Add(newPost);
            //try { db.SaveChanges(); }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //        foreach (var ve in eve.ValidationErrors)
            //            ModelState.AddModelError(string.Empty, ve.PropertyName + " " +
            //            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName) + " " +
            //            ve.ErrorMessage);
            //    return View(model);
            //}

            ViewBag.message = "تغییرات با موفقیت انجام شد";
            return View(new PostViewModel(post, model.languageId));
        }

        public ActionResult Delete()
        {
            return View();
        }

    }
}
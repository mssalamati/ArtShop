﻿using Blog.Areas.Admin.Models.ViewModel;
using Blog.Interfaces;
using Blog.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using Microsoft.AspNet.Identity;

namespace Blog.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class PostsController : BaseController
    {
        // GET: Admin/Posts
        public ActionResult Index(int page = 1, string search = "")
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = user.userDetail.Posts
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
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            if (!ModelState.IsValid)
                return View(model);
            if (model.Thumbnail == null || model.HeaderPhotos == null)
            {
                ModelState.AddModelError(string.Empty, "Thumbnail or HeaderPhotos is empty");
                return View(model);
            }
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
            newPost.postType = model.postType;
            newPost.Translations = new List<PostTranslation>();
            newPost.Translations.Add(new PostTranslation() { languageId = model.languageId, Title = model.Title, Description = model.Description });
            newPost.Author = user.userDetail.FirstName + user.userDetail.LastName;
            user.userDetail.Posts.Add(newPost);
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
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            var model = db.Posts.Find(id);
            if (model.AuthorProfileId != userId)
                return HttpNotFound();
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
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            ViewBag.caregories = db.Categories.ToList();
            ViewBag.language = db.Languages.ToList();
            ViewBag.tags = db.Tags.ToList();
            var post = db.Posts.Find(model.Id);

            if (post.AuthorProfileId != userId)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(model.FillPicture(post));
            string tempFolderName = "Upload/posts";
            if (model.Thumbnail != null)
            {
                var Thumbresult = ImageHelper.Saveimage(Server, model.Thumbnail, tempFolderName, ImageHelper.saveImageMode.Not);
                if (!Thumbresult.ResultStatus)
                {
                    ModelState.AddModelError(string.Empty, Thumbresult.Error);
                    return View(model.FillPicture(post));
                }
                post.Thumbnail = Thumbresult.FullPath;
            }
            if (model.HeaderPhotos != null && model.HeaderPhotos.Count > 0)
                foreach (var item in model.HeaderPhotos)
                    if (item != null)
                    {
                        var res = ImageHelper.Saveimage(Server, item, tempFolderName, ImageHelper.saveImageMode.wide);
                        if (!res.ResultStatus)
                        {
                            ModelState.AddModelError(string.Empty, res.Error);
                            return View(model.FillPicture(post));
                        }
                        post.HeaderPhotos.Add(new HeaderPhoto() { Path = res.FullPath });
                    }
            post.Category = db.Categories.Find(model.Category);
            db.Links.RemoveRange(post.Links);
            post.Links = model.Links.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Link() { URL = x }).ToList();
            post.Tags.Clear();
            post.Tags = db.Tags.Where(x => model.Tags.Any(y => y == x.Id)).ToList();
            post.Title = model.TitleDef;
            post.postType = model.postType;
            var translation = post.Translations.Single(x => x.languageId == model.languageId);
            translation.Title = model.Title;
            translation.Description = model.Description;
            try { db.SaveChanges(); }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        ModelState.AddModelError(string.Empty, ve.PropertyName + " " +
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName) + " " +
                        ve.ErrorMessage);
                return View(model.FillPicture(post));
            }
            ViewBag.message = "تغییرات با موفقیت انجام شد";
            return View(new PostViewModel(post, model.languageId));
        }

        public ActionResult RemovePhoto(int postId, int photoId)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var post = db.Posts.Find(postId);
            if (post.AuthorProfileId != userId)
                return HttpNotFound();
            if (post != null)
            {
                var photo = post.HeaderPhotos.SingleOrDefault(x => x.Id == photoId);
                if (photo != null)
                {
                    db.HeaderPhotos.Remove(photo);
                    db.SaveChanges();
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false, message = "can not remove photo" }, JsonRequestBehavior.AllowGet);
        }

        public class imagesviewmodel
        {
            public string Url { get; set; }
        }

        public void uploadnow(HttpPostedFileWrapper upload)
        {
            var userId = User.Identity.GetUserId();
            var folder = Server.MapPath("~/Upload/cms/" + userId);
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
            var appData = Server.MapPath("~/Upload/cms/" + userId);
            FileHelper.CreateFolderIfNeeded(appData);
            var images = Directory.GetFiles(appData).Select(x => new imagesviewmodel
            {
                Url = Url.Content("/Upload/cms/" + userId + "/" + Path.GetFileName(x))
            });
            return View(images);
        }

        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var post = db.Posts.Find(id);
            if (post.AuthorProfileId != userId)
                return HttpNotFound();
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}